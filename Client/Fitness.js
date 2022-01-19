import { Listing } from "./Listing.js";
import { Ormarici } from "./Ormarici.js";

export class Fitness{
    constructor(listaTreninga, listaTeretana){
        this.listaTreninga= listaTreninga;
        this.listaTeretana= listaTeretana;
        this.container= null;
    }

    crtaj(host){
        this.container= document.createElement("div");
        this.container.className= "MainContainer";
        host.appendChild(this.container);

        let containerInfo= document.createElement("div");
        containerInfo.className= "Info";
        this.container.appendChild(containerInfo);
        this.crtajInfo(containerInfo);

        let containerTable= document.createElement("div");
        containerTable.className= "containerTable";
        this.container.appendChild(containerTable);
        this.crtajTable(containerTable);
        
        let containerOrmarici= document.createElement("div");
        containerOrmarici.className= "ormariciKontejner";
        containerTable.appendChild(containerOrmarici);

    }

    

    crtajTable(host){
        var tabela= document.createElement("table");
        tabela.className= "table";
        host.appendChild(tabela);

        var tabelaHead= document.createElement("thead");
        tabela.appendChild(tabelaHead);

        var tabelaRed= document.createElement("tr");
        tabelaHead.appendChild(tabelaRed);

        var tabelaBody= document.createElement("tbody");
        tabelaBody.className= "tableBody";
        tabela.appendChild(tabelaBody);

        let heder;
        var zaglavlje= ["Broj kartice", "Ime", "Prezime", "Cena po satu", "Tip treninga", "Duzina treninga", "Ormaric", "Termin", "Ukupna cena"];
        zaglavlje.forEach(el => {
            heder= document.createElement("th");
            heder.innerHTML= el;
            tabelaRed.appendChild(heder);
        })
    }


    prikaziTreninge(){
        let izabranTrening= this.container.querySelector("select");
        var treningId= izabranTrening.options[izabranTrening.selectedIndex].value;

        let izabranaTeretana= this.container.querySelector("input[type='radio']:checked");
        if(izabranaTeretana === null){
            alert("Izaberite teretanu!");
            return;
        }
        var teretanaId= izabranaTeretana.value;
        
        this.vratiKlijente(treningId, teretanaId);
    }

    vratiKlijente(treningId, teretanaId){
        fetch("https://localhost:5001/Klijent/VratiKlijente/"+treningId+"/"+teretanaId,
        {
            method:"GET"
        }).then(s =>{
            if(s.ok){
                var teloTabele= this.resetujTabelu();
                s.json().then(data =>{
                    data.forEach(t =>{
                        let red= new Listing(t.brKartice, t.ime, t.prezime, t.cenaPoSatu, t.tipTreninga, t.duzinaTreninga, t.ormaric, t.termin, t.ukupnaCena);
                        red.crtaj(teloTabele);
                    })
                    fetch("https://localhost:5001/Teretana/VratiTeretanu/"+teretanaId,
        {
            method: "GET"
        }).then(s=>{
            if(s.ok)
            {
                s.json().then(data=>{
                    let brOrmarica= data.brojOrmarica;
                    let zauzeti= data.zauzetiOrmarici;
                    var teloOrmarica= this.resetujOrmarice();
                    let ormarici= new Ormarici(brOrmarica, zauzeti);
                    ormarici.crtajOrmarice(teloOrmarica);
                    //////
                })
            }
            else throw s;
        }).catch(err =>
            err.text().then(errMsg=>
                alert(errMsg)));
                })
            }
            else throw s;
        }).catch(err=>
            err.text().then(errMsg=>
                alert(errMsg)));
    }


    resetujTabelu(){
        var teloTabele= this.container.querySelector(".tableBody");
        var roditelj= teloTabele.parentNode;
        roditelj.removeChild(teloTabele);
        teloTabele= document.createElement("tbody");
        teloTabele.className= "tableBody";
        roditelj.appendChild(teloTabele);
        return teloTabele;
    }

    resetujOrmarice(){
        var ormkont= this.container.querySelector(".ormariciKontejner");
        var roditelj= ormkont.parentNode;
        roditelj.removeChild(ormkont);
        ormkont= document.createElement("div");
        ormkont.className= "ormariciKontejner";
        roditelj.appendChild(ormkont);
        return ormkont;
    }


    zakaziTrening(){
        var brKartice= this.container.querySelector(".tbxBrKartice").value;
        if(brKartice === null || brKartice === undefined || brKartice==="" || brKartice<100 || brKartice>999)
        {
            alert("Unet je nevalidan broj kartice!");
            return;
        }
        var ormaric= this.container.querySelector(".tbxOrmaric").value;
        if(ormaric === null || ormaric === undefined || ormaric==="" || ormaric<1 || ormaric>25)
        {
            alert("Unet je nevalidan broj ormarica!");
            return;
        }
        var termin= this.container.querySelector(".tbxTermin").value;
        if(termin === null || termin === undefined || termin==="")
        {
            alert("Molimo unesite termin!");
            return;
        }
        let izabranTrening= this.container.querySelector("select");
        var treningId= izabranTrening.options[izabranTrening.selectedIndex].value;
        let izabranaTeretana= this.container.querySelector("input[type='radio']:checked");
        if(izabranaTeretana === null){
            alert("Izaberite teretanu!");
            return;
        }
        var teretanaId= izabranaTeretana.value;
        this.pokreniZakazivanje(brKartice, treningId, teretanaId, ormaric, termin);
    }

    pokreniZakazivanje(brKartice, treningId, teretanaId, ormaric, termin)
    {
        fetch("https://localhost:5001/Teretana/ZakaziTrening/"+brKartice+"/"+treningId+"/"+teretanaId+"/"+ormaric+"/"+termin,
        {
            method:"POST"
        }).then(s =>{
            if(s.ok){
                var teloTabele= this.resetujTabelu();
                s.json().then(data=>{
                    data.forEach(t=> {
                        let red= new Listing(t.brKartice, t.ime, t.prezime, t.cenaPoSatu, t.tipTreninga, t.duzinaTreninga, t.ormaric, t.termin, t.ukupnaCena);
                        red.crtaj(teloTabele);
                    })
                    fetch("https://localhost:5001/Teretana/VratiTeretanu/"+teretanaId,
        {
            method: "GET"
        }).then(s=>{
            if(s.ok)
            {
                s.json().then(data=>{
                    let brOrmarica= data.brojOrmarica;
                    let zauzeti= data.zauzetiOrmarici;
                    var teloOrmarica= this.resetujOrmarice();
                    let ormarici= new Ormarici(brOrmarica, zauzeti);
                    ormarici.crtajOrmarice(teloOrmarica);
                    //////
                })
            }
            else throw s;
        }).catch(err =>
            err.text().then(errMsg=>
                alert(errMsg)));
                })
            }
            else throw s;
        }).catch(err=>
            err.text().then(errMsg=>
                alert(errMsg)));
    }

    otkaziTrening()
    {
        var brKartice= this.container.querySelector(".tbxBrKartice").value;
        if(brKartice === null || brKartice === undefined || brKartice==="" || brKartice<100 || brKartice>999)
        {
            alert("Unet je nevalidan broj kartice!");
            return;
        }
        var termin= this.container.querySelector(".tbxTermin").value;
        if(termin === null || termin === undefined || termin==="")
        {
            alert("Molimo unesite zeljeni termin!");
            return;
        }
        let izabranTrening= this.container.querySelector("select");
        var treningId= izabranTrening.options[izabranTrening.selectedIndex].value;
        let izabranaTeretana= this.container.querySelector("input[type='radio']:checked");
        if(izabranaTeretana === null){
            alert("Izaberite teretanu!");
            return;
        }
        var teretanaId= izabranaTeretana.value;
        this.pokreniOtkazivanje(brKartice, treningId, teretanaId, termin);
    }

    pokreniOtkazivanje(brKartice, treningId, teretanaId, termin)
    {
        fetch("https://localhost:5001/Teretana/OtkaziTrening/"+brKartice+"/"+treningId+"/"+teretanaId+"/"+termin,
        {
            method: "DELETE"
        }).then(s=>{
            if(s.ok){
                var teloTabele= this.resetujTabelu();
                s.json().then(data=>{
                    data.forEach(t=> {
                        let red= new Listing(t.brKartice, t.ime, t.prezime, t.cenaPoSatu, t.tipTreninga, t.duzinaTreninga, t.ormaric, t.termin, t.ukupnaCena);
                        red.crtaj(teloTabele);
                    })
                    fetch("https://localhost:5001/Teretana/VratiTeretanu/"+teretanaId,
        {
            method: "GET"
        }).then(s=>{
            if(s.ok)
            {
                s.json().then(data=>{
                    let brOrmarica= data.brojOrmarica;
                    let zauzeti= data.zauzetiOrmarici;
                    var teloOrmarica= this.resetujOrmarice();
                    let ormarici= new Ormarici(brOrmarica, zauzeti);
                    ormarici.crtajOrmarice(teloOrmarica);
                    //////
                })
            }
            else throw s;
        }).catch(err =>
            err.text().then(errMsg=>
                alert(errMsg)));
                })
                alert("Uspesno je otkazan trening!");
            }
            else throw s;
        }).catch(err=>
            err.text().then(errMsg=>
                alert(errMsg)));
    }


    zameniTrening()
    {
        var brKartice= this.container.querySelector(".tbxBrKartice").value;
        if(brKartice === null || brKartice === undefined || brKartice==="" || brKartice<100 || brKartice>999)
        {
            alert("Unet je nevalidan broj kartice!");
            return;
        }
        var termin= this.container.querySelector(".tbxTermin").value;
        if(termin === null || termin === undefined || termin==="")
        {
            alert("Molimo unesite zeljeni termin!");
            return;
        }
        var ormaric= this.container.querySelector(".tbxOrmaric").value;
        if(ormaric === null || ormaric === undefined || ormaric==="" || ormaric<1 || ormaric>25)
        {
            alert("Unet je nevalidan ormaric!");
        }
        let izabranTrening= this.container.querySelector("select");
        var treningId= izabranTrening.options[izabranTrening.selectedIndex].value;
        let izabranaTeretana= this.container.querySelector("input[type='radio']:checked");
        if(izabranaTeretana === null){
            alert("Izaberite teretanu!");
            return;
        }
        var teretanaId= izabranaTeretana.value;
        this.pokreniZamenu(brKartice, treningId, teretanaId, ormaric, termin);
    }

    pokreniZamenu(brKartice, treningId, teretanaId, ormaric, termin)
    {
        fetch("https://localhost:5001/Teretana/ZameniOrmaric/"+brKartice+"/"+treningId+"/"+teretanaId+"/"+ormaric+"/"+termin,
        {
            method: "PUT"
        }).then(s =>{
            if(s.ok)
            {
                var teloTabele= this.resetujTabelu();
                s.json().then(data=>{
                    data.forEach(t=> {
                        let red= new Listing(t.brKartice, t.ime, t.prezime, t.cenaPoSatu, t.tipTreninga, t.duzinaTreninga, t.ormaric, t.termin, t.ukupnaCena);
                        red.crtaj(teloTabele);
                    })
                    fetch("https://localhost:5001/Teretana/VratiTeretanu/"+teretanaId,
        {
            method: "GET"
        }).then(s=>{
            if(s.ok)
            {
                s.json().then(data=>{
                    let brOrmarica= data.brojOrmarica;
                    let zauzeti= data.zauzetiOrmarici;
                    var teloOrmarica= this.resetujOrmarice();
                    let ormarici= new Ormarici(brOrmarica, zauzeti);
                    ormarici.crtajOrmarice(teloOrmarica);
                    //////
                })
            }
            else throw s;
        }).catch(err =>
            err.text().then(errMsg=>
                alert(errMsg)));
                })
            }
            else throw s;
        }).catch(err =>
            err.text().then(errMsg=>
                alert(errMsg)));
    }


    crtajInfo(host){

        //labelaTrening
        let l= document.createElement("label");
        l.className= "labelTrening";
        l.innerHTML= "Trening:";
        host.appendChild(l);

        //selekcija tipa treninga
        let sel= document.createElement("select");
        sel.className= "selectTrening";
        host.appendChild(sel);
        let opcija;
        this.listaTreninga.forEach(trening => {
            opcija= document.createElement("option");
            opcija.innerHTML=trening.tip;
            opcija.value= trening.id;
            sel.appendChild(opcija);
        });

        //labela teretana
        l= document.createElement("label");
        l.className= "labelTeretana";
        l.innerHTML= "Teretana:";
        host.appendChild(l);

        //div za oba sektora radio dugmice
        let rbox= document.createElement("div");
        rbox.className= "rbox";
        host.appendChild(rbox);

        //div za prvu polovinu radio dugmica
        let rbox1= document.createElement("div");
        rbox1.className= "rbox1";
        rbox.appendChild(rbox1);
        //div za drugu polovinu radio dugmica
        let rbox2= document.createElement("div");
        rbox2.className= "rbox2";
        rbox.appendChild(rbox2);

        let d; //koristim za div-ove

        //dodavanje radio dugmica u odgovarajuce divove
        let rbutton;
        this.listaTeretana.forEach((teretana, index) => {
            rbutton= document.createElement("input");
            rbutton.type= "radio";
            rbutton.value= teretana.id;
            rbutton.name="teretane";
            l= document.createElement("label");
            l.innerHTML= teretana.naziv;
            d= document.createElement("div");
            d.appendChild(rbutton);
            d.appendChild(l);
            if(index % 2 == 0){
                rbox1.appendChild(d);
            }
            else{
                rbox2.appendChild(d);
            }
        })

        //dugme Prikazi
        let btnPrikazi= document.createElement("button");
        btnPrikazi.className= "btnPrikazi";
        btnPrikazi.innerHTML= "Prikaži";
        btnPrikazi.onclick=(ev)=>this.prikaziTreninge();
        host.appendChild(btnPrikazi);

        //div za labelu i textbox Broj kartice
        d= document.createElement("div");
        host.appendChild(d);

        //labela Broj kartice
        l= document.createElement("label");
        l.className= "labelBrKartice";
        l.innerHTML= "Broj kartice:";
        d.appendChild(l);

        //textbox BrKartice
        var tbx= document.createElement("input");
        tbx.type= "number";
        tbx.className= "tbxBrKartice";
        d.appendChild(tbx);
        
        //div za labelu i textbox za termin
        d= document.createElement("div");
        host.appendChild(d);

        //labela termin
        l= document.createElement("label");
        l.className= "labelTermin";
        l.innerHTML= "Termin:";
        d.appendChild(l);

        //textbox termin
        tbx= document.createElement("input");
        tbx.type= "date";
        tbx.className= "tbxTermin";
        d.appendChild(tbx);


        //dugme otkazi
        let btnOtkazi= document.createElement("button");
        btnOtkazi.className= "btnOtkazi";
        btnOtkazi.innerHTML= "Otkaži";
        btnOtkazi.onclick=(ev)=>this.otkaziTrening();
        host.appendChild(btnOtkazi);

        //div za labelu i textbox za ormaric
        d= document.createElement("div");
        host.appendChild(d);

        //labela ormaric
        l= document.createElement("label");
        l.className= "labelOrmaric";
        l.innerHTML= "Ormaric:";
        d.appendChild(l);

        //textbox ormaric
        tbx= document.createElement("input");
        tbx.type= "number";
        tbx.className= "tbxOrmaric";
        d.appendChild(tbx);

        //dugme zakazi
        let btnZakazi= document.createElement("button");
        btnZakazi.className= "btnZakazi";
        btnZakazi.innerHTML= "Zakaži";
        btnZakazi.onclick=(ev)=>this.zakaziTrening();
        host.appendChild(btnZakazi);

        //dugme zameni ormaric
        let btnZameni= document.createElement("button");
        btnZameni.className= "btnZameni";
        btnZameni.innerHTML= "Zameni ormaric";
        btnZameni.onclick=(ev)=>this.zameniTrening();
        host.appendChild(btnZameni);

    }

    /*vratiZauzete(teretanaId)
    {
        fetch("https://localhost:5001/Teretana/VratiTeretanu/"+teretanaId,
        {
            method: "GET"
        }).then(s=>{
            if(s.ok)
            {
                s.json().then(data=>{
                    console.log(data.zauzetiOrmarici);
                    return data.zauzetiOrmarici;
                })
            }
            else throw s;
        }).catch(err =>
            err.text().then(errMsg=>
                alert(errMsg)));
    }*/

    
}