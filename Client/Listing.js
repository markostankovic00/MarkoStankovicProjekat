export class Listing{
    constructor(brKartice, ime, prezime, cenaPoSatu, tipTreninga, duzinaTreninga, ormaric, termin, ukupnaCena){
        this.brKartice= brKartice;
        this.ime= ime;
        this.prezime= prezime;
        this.cenaPoSatu= cenaPoSatu;
        this.tipTreninga= tipTreninga;
        this.duzinaTreninga= duzinaTreninga;
        this.ormaric= ormaric;
        this.termin= termin;
        this.ukupnaCena= ukupnaCena;
    }

    crtaj(host){
        var redTabele= document.createElement("tr");
        host.appendChild(redTabele);


        //ELEMENTI REDA TABELE
        var el= document.createElement("td");
        el.innerHTML= this.brKartice;
        redTabele.appendChild(el);

        el= document.createElement("td");
        el.innerHTML= this.ime;
        redTabele.appendChild(el);

        el= document.createElement("td");
        el.innerHTML= this.prezime;
        redTabele.appendChild(el);

        el= document.createElement("td");
        el.innerHTML= this.cenaPoSatu;
        redTabele.appendChild(el);

        el= document.createElement("td");
        el.innerHTML= this.tipTreninga;
        redTabele.appendChild(el);

        el= document.createElement("td");
        el.innerHTML= this.duzinaTreninga;
        redTabele.appendChild(el);

        el= document.createElement("td");
        el.innerHTML= this.ormaric;
        redTabele.appendChild(el);

        el= document.createElement("td");
        el.innerHTML= this.termin;
        redTabele.appendChild(el);

        el= document.createElement("td");
        el.innerHTML= this.ukupnaCena;
        redTabele.appendChild(el);

    }
}