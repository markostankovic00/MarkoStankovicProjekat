export class Slot{
    constructor(br, zauzet){
        this.br= br;
        this.zauzet= zauzet;
        this.slotContainer= null;
    }

    vratiBoju(){
        if(this.zauzet=='z')
            return "red";
        else
            return "green";
    }
    crtajSlot(host){
        this.slotContainer= document.createElement("div");
        this.slotContainer.className= "slot";
        this.slotContainer.innerHTML= this.br;
        this.slotContainer.style.backgroundColor= this.vratiBoju();
        host.appendChild(this.slotContainer);
    }
}