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
    public class KlijentController : ControllerBase
    {
        public FitnessContext Context { get; set; }

        public KlijentController(FitnessContext context)
        {
            Context= context;
        }

        [Route("VratiKlijente/{treningID}/{teretanaID}")]
        [HttpGet]
        public async Task<ActionResult> Vrati(int treningID, int teretanaID)
        {
            if(treningID <= 0)
            {
                return BadRequest("Unet je nevalidan ID treninga!");
            }
            if(teretanaID <= 0)
            {
                return BadRequest("Unet je nevalidan ID teretane!");
            }
            try
            {
                var klijenti= Context.KlijentiTreninzi
                    .Include(p => p.Klijent)
                    .Include(p => p.Teretana)
                    .Include(p => p.Trening)
                    .Where(p => p.Teretana.ID==teretanaID && p.Trening.ID==treningID);

                var klijent= await klijenti.ToListAsync();

                return Ok
                (
                    klijent.Select(p =>
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
                    }).ToList()
                );
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }

        [Route("DodajKlijenta")]
        [HttpPost]
        public async Task<ActionResult> Dodaj([FromBody] Klijent klijent)
        {
            if(klijent.brKartice < 100 || klijent.brKartice > 999)
            {
                return BadRequest("Unet je nevalidan broj kartice!");
            }
            if(string.IsNullOrWhiteSpace(klijent.Ime) || klijent.Ime.Length > 30)
            {
                return BadRequest("Uneto je nevalidno ime klijenta!");
            }
            if(string.IsNullOrWhiteSpace(klijent.Prezime) || klijent.Prezime.Length > 30)
            {
                return BadRequest("Uneto je nevalidno prezime klijenta!");
            }
            try
            {
                 Context.Klijenti.Add(klijent);
                 await Context.SaveChangesAsync();
                 return Ok($"Klijent je uspesno dodat! ID je: {klijent.ID}");
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }

        /*[Route("UkloniKlijenta/{id}")]
        [HttpDelete]
        public async Task<ActionResult> Ukloni(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Nevalidan ID!");
            }
            try
            {
                 var klijent= await Context.Klijenti.FindAsync(id);
                 if(klijent != null)
                 {
                     Context.Klijenti.Remove(klijent);
                     await Context.SaveChangesAsync();
                     return Ok($"Uspešno izbrisan klijent!");
                 }
                 else
                 {
                     return BadRequest("Ne postoji klijent sa unetim ID-om!");
                 }
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }*/

        /*[Route("PromeniKlijenta/{brKartice}/{ime}/{prezime}")]
        [HttpPut]
        public async Task<ActionResult> Promeni(int brKartice, string ime, string prezime)
        {
            if(brKartice<100 || brKartice>999)
            {
                return BadRequest("Nevalidan brKartice");
            }
            if(string.IsNullOrWhiteSpace(ime) || ime.Length > 30)
            {
                return BadRequest("Uneto je nevalidno ime klijenta!");
            }
            if(string.IsNullOrWhiteSpace(prezime) || prezime.Length > 30)
            {
                return BadRequest("Uneto je nevalidno prezime klijenta!");
            }
            try
            {
                var klijent= Context.Klijenti.Where(p => p.brKartice == brKartice).FirstOrDefault();
                if(klijent != null)
                {
                    klijent.Ime= ime;
                    klijent.Prezime= prezime;
                    await Context.SaveChangesAsync();
                    return Ok($"Uspešna izmena podataka o klijentu! ID: {klijent.ID}");
                }
                else
                {
                    return BadRequest("Ne postoji klijent sa unetim brojem kartice!");
                }
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }*/


        
    }
}