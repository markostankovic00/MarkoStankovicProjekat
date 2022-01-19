using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Projekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeretanaController : ControllerBase
    {
        public FitnessContext Context { get; set; }

        public TeretanaController(FitnessContext context)
        {
            Context= context;
        }

        [Route("DodajTeretanu")]
        [HttpPost]
        public async Task<ActionResult> Dodaj([FromBody] Teretana teretana)
        {
            if(string.IsNullOrWhiteSpace(teretana.Naziv) || teretana.Naziv.Length>50)
            {
                return BadRequest("Unet je nevalidan naziv teretane!");
            }
            if(teretana.CenaPoSatu <= 0)
            {
                return BadRequest("Uneta je nevalidna cena teretane!");
            }
            try
            {
                Context.Teretane.Add(teretana);
                await Context.SaveChangesAsync();
                return Ok($"Uspesno je dodata teretana! ID: {teretana.ID}");
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }
        [Route("VratiTeretane")]
        [HttpGet]
        public async Task<ActionResult> Vrati()
        {
            try
            {
                var teretane= await Context.Teretane.Select(p => new{p.ID, p.Naziv, p.CenaPoSatu}).ToListAsync();
                return Ok(teretane);
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }

        [Route("VratiTeretanu/{idTeretane}")]
        [HttpGet]
        public async Task<ActionResult> VratiTeretanu(int idTeretane)
        {
            try
            {
                var teretana= await Context.Teretane.Where(p => p.ID==idTeretane).FirstOrDefaultAsync();
                if(teretana==null)
                {
                    return BadRequest("Ne postoji teretana sa unetim ID-om!");
                }
                return Ok(teretana);
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }

        [Route("ZakaziTrening/{brKartice}/{idTreninga}/{idTeretane}/{ormaric}/{termin}")]
        [HttpPost]
        public async Task<ActionResult> Zakazi(int brKartice, int idTreninga, int idTeretane, int ormaric, DateTime termin)
        {
            if(brKartice < 100 || brKartice > 999)
            {
                return BadRequest("Unet je nevalidan broj kartice!");
            }
            if(idTreninga <= 0)
            {
                return BadRequest("Unet je nevalida ID treninga!");
            }
            if(idTeretane <= 0)
            {
                return BadRequest("Unet je nevalida ID teretane!");
            }
            if(ormaric<1 || ormaric>25)
            {
                return BadRequest("Unet je nevalidan ormaric!");
            }

            try
            {
                
                var klijent= await Context.Klijenti.Where(p => p.brKartice==brKartice).FirstOrDefaultAsync();
                if(klijent==null)
                {
                    return BadRequest("Ne postoji klijent sa unetim brojem kartice!");
                }
                var trening= await Context.Treninzi.Where(p => p.ID==idTreninga).FirstOrDefaultAsync();
                if(trening==null)
                {
                    return BadRequest("Ne postoji trening sa unetim ID-om!");
                }
                var teretana= await Context.Teretane.Where(p => p.ID==idTeretane).FirstOrDefaultAsync();
                if(teretana==null)
                {
                    return BadRequest("Ne postoji teretana sa unetim ID-om");
                }
                if(teretana.ZauzetiOrmarici[ormaric-1]=='z')
                {
                    return BadRequest("Trazeni ormaric je zauzet!");
                }
                Spoj postoji= await Context.KlijentiTreninzi.Where(p=>
                p.Klijent==klijent && p.Teretana==teretana && p.Trening==trening && p.Termin==termin).FirstOrDefaultAsync();
                if(postoji != null)
                {
                    return BadRequest("Vec ste zakazali specificirani trening!");
                }
                string tmp= teretana.ZauzetiOrmarici;
                teretana.ZauzetiOrmarici=tmp.Substring(0, ormaric-1) + 'z' + tmp.Substring(ormaric, teretana.BrojOrmarica-ormaric);
                Context.Teretane.Update(teretana);
                Spoj s= new Spoj
                {
                    Klijent= klijent,
                    Trening= trening,
                    Teretana= teretana,
                    Ormaric= ormaric,
                    Termin= termin,
                    Cena= trening.Duzina * teretana.CenaPoSatu
                };
                Context.KlijentiTreninzi.Add(s);
                await Context.SaveChangesAsync();


                var listingKlijent= await Context.KlijentiTreninzi
                    .Include(p => p.Klijent)
                    .Include(p => p.Trening)
                    .Include(p => p.Teretana)
                    .Where(p => p.Klijent.brKartice == brKartice && p.Teretana.ID == idTeretane)
                    .Select(p => 
                    new
                    {
                        brKartice= p.Klijent.brKartice,
                        ime= p.Klijent.Ime,
                        prezime= p.Klijent.Prezime,
                        cenaPoSatu= p.Teretana.CenaPoSatu,
                        tipTreninga= p.Trening.Tip,
                        duzinaTreninga= p.Trening.Duzina,
                        ormaric= p.Ormaric,
                        termin= p.Termin,
                        ukupnaCena= p.Cena
                    }).ToListAsync();
                return Ok(listingKlijent);
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }


        [Route("OtkaziTrening/{brKartice}/{idTreninga}/{idTeretane}/{termin}")]
        [HttpDelete]
        public async Task<ActionResult> Otkazi(int brKartice, int idTreninga, int idTeretane, DateTime termin)
        {
            if(idTreninga <= 0)
            {
                return BadRequest("Unet je nevalidan ID treninga!");
            }
            if(idTeretane <= 0)
            {
                return BadRequest("Unet je nevalidan ID teretane");
            }
            if(brKartice<100 || brKartice>999)
            {
                return BadRequest("Unet je nevalidan broj kartice!");
            }

            try
            {
                Spoj s= await Context.KlijentiTreninzi.Where(p => 
                    p.Klijent.brKartice==brKartice && p.Trening.ID==idTreninga && p.Teretana.ID==idTeretane && p.Termin==termin).FirstOrDefaultAsync();
                if(s == null)
                {
                    return BadRequest("Ne postoji specificirani zakazani trening!");
                }
                int ormaric= s.Ormaric;
                Teretana teretana= await Context.Teretane.Where(p => p.ID==idTeretane).FirstOrDefaultAsync();
                string tmp= teretana.ZauzetiOrmarici;
                teretana.ZauzetiOrmarici=tmp.Substring(0, ormaric-1) + 's' + tmp.Substring(ormaric, teretana.BrojOrmarica-ormaric);
                Context.Teretane.Update(teretana);
                Context.KlijentiTreninzi.Remove(s);
                await Context.SaveChangesAsync();

                var listingKlijent= await Context.KlijentiTreninzi
                    .Include(p => p.Klijent)
                    .Include(p => p.Trening)
                    .Include(p => p.Teretana)
                    .Where(p => p.Klijent.brKartice == brKartice && p.Teretana.ID == idTeretane)
                    .Select(p => 
                    new
                    {
                        brKartice= p.Klijent.brKartice,
                        ime= p.Klijent.Ime,
                        prezime= p.Klijent.Prezime,
                        cenaPoSatu= p.Teretana.CenaPoSatu,
                        tipTreninga= p.Trening.Tip,
                        duzinaTreninga= p.Trening.Duzina,
                        ormaric= p.Ormaric,
                        termin= p.Termin,
                        ukupnaCena= p.Cena
                    }).ToListAsync();
                return Ok(listingKlijent);
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }


        [Route("ZameniOrmaric/{brKartice}/{idTreninga}/{idTeretane}/{ormaric}/{termin}")]
        [HttpPut]
        public async Task<ActionResult> Zameni(int brKartice, int idTreninga, int idTeretane, int ormaric, DateTime termin)
        {
            if(idTreninga <= 0)
            {
                return BadRequest("Unet je nevalidan ID treninga!");
            }
            if(idTeretane <= 0)
            {
                return BadRequest("Unet je nevalidan ID teretane");
            }
            if(brKartice<100 || brKartice>999)
            {
                return BadRequest("Unet je nevalidan broj kartice!");
            }
            if(ormaric<1 || ormaric>25)
            {
                return BadRequest("Unet je nevalidan ormaric!");
            }
            try
            {
                Spoj s= await Context.KlijentiTreninzi.Where(p=>
                p.Klijent.brKartice==brKartice && p.Trening.ID==idTreninga && p.Teretana.ID==idTeretane && p.Termin==termin).FirstOrDefaultAsync();
                if(s==null)
                {
                    return BadRequest("Ne postoji specificirani zakazani trening!");
                }
                Teretana t= await Context.Teretane.Where(p=> p.ID==idTeretane).FirstOrDefaultAsync();
                if(t.ZauzetiOrmarici[ormaric] == 'z')
                {
                    return BadRequest("Zahtevani ormaric je vec zauzet!");
                }
                if(ormaric == s.Ormaric)
                {
                    return BadRequest("Molimo izaberite razlicit ormaric za zamenu!");
                }
                string tmp= t.ZauzetiOrmarici;
                t.ZauzetiOrmarici=tmp.Substring(0, ormaric-1) + 'z' + tmp.Substring(ormaric, t.BrojOrmarica-ormaric);
                tmp= t.ZauzetiOrmarici;
                t.ZauzetiOrmarici= tmp.Substring(0, s.Ormaric-1) + 's' + tmp.Substring(s.Ormaric, t.BrojOrmarica-s.Ormaric);
                Context.Teretane.Update(t);
                s.Ormaric= ormaric;
                Context.KlijentiTreninzi.Update(s);
                await Context.SaveChangesAsync();

                var listingKlijent= await Context.KlijentiTreninzi
                    .Include(p => p.Klijent)
                    .Include(p => p.Trening)
                    .Include(p => p.Teretana)
                    .Where(p => p.Klijent.brKartice == brKartice && p.Teretana.ID == idTeretane)
                    .Select(p => 
                    new
                    {
                        brKartice= p.Klijent.brKartice,
                        ime= p.Klijent.Ime,
                        prezime= p.Klijent.Prezime,
                        cenaPoSatu= p.Teretana.CenaPoSatu,
                        tipTreninga= p.Trening.Tip,
                        duzinaTreninga= p.Trening.Duzina,
                        ormaric= p.Ormaric,
                        termin= p.Termin,
                        ukupnaCena= p.Cena
                    }).ToListAsync();
                return Ok(listingKlijent);
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }
    }
}