import { Trening } from "./Trening.js";
import { Teretana } from "./Teretana.js";
import { Fitness } from "./Fitness.js";

var listaTreninga= [];
var listaTeretana= [];
fetch("https://localhost:5001/Trening/VratiTreninge")
.then( p=> {
    p.json().then(treninzi =>{
        treninzi.forEach(trening => {
            var t= new Trening(trening.id, trening.duzina, trening.tip);
            listaTreninga.push(t);
        });
        fetch("https://localhost:5001/Teretana/VratiTeretane")
        .then( p=> {
            p.json().then(teretane =>{
                teretane.forEach(teretana => {
                    var t= new Teretana(teretana.id, teretana.naziv, teretana.cenaPoSatu);
                    listaTeretana.push(t);
                });
            var f= new Fitness(listaTreninga, listaTeretana);
            f.crtaj(document.body);
            })
        })          
    })
})

