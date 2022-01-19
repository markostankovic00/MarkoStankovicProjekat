import { Slot } from "./Slot.js"

export class Ormarici{
    constructor(brSlotova, zauzeti){
        this.brSlotova= brSlotova;
        this.zauzeti= zauzeti;
        this.kontejner= null;

    }
    crtajOrmarice(host){
        let slot;
        for(let i=1; i<=this.brSlotova; i++){
            slot= new Slot(i, this.zauzeti[i-1]);
            slot.crtajSlot(host);

        }
    }
}