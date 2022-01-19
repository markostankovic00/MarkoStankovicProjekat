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
    public class TreningController : ControllerBase
    {
        public FitnessContext Context { get; set; }

        public TreningController(FitnessContext context)
        {
            Context= context;
        }

        [Route("DodajTrening")]
        [HttpPost]
        public async Task<ActionResult> Dodaj([FromBody] Trening trening)
        {
            if(trening.Duzina < 1 || trening.Duzina > 24)
            {
                return BadRequest("Uneta je nevalidno trajanje treninga!");
            }
            if(string.IsNullOrWhiteSpace(trening.Tip) || trening.Tip.Length > 30)
            {
                return BadRequest("Unet je nevalidan tip treninga!");
            }
            try
            {
                 Context.Treninzi.Add(trening);
                 await Context.SaveChangesAsync();
                 return Ok($"Trening je uspesno dodat! ID: {trening.ID}");
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }

        [Route("VratiTreninge")]
        [HttpGet]
        public async Task<ActionResult> Vrati()
        {
            try
            {
                 var treninzi= await Context.Treninzi.Select(p => new { p.ID, p.Duzina, p.Tip}).ToListAsync();
                 return Ok(treninzi);
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }


        /*[Route("UkloniTrening/{id}")]
        [HttpDelete]
        public async Task<ActionResult> Ukloni(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Unet je nevalidan ID!");
            }
            try
            {
                var trening=await Context.Treninzi.FindAsync(id);
                if(trening != null)
                {
                    Context.Treninzi.Remove(trening);
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno uklonjen trening!");
                }
                else
                {
                    return BadRequest("Ne postoji trening sa unetim ID-om!");
                }
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }

        [Route("PromeniTrening/{id}/{duzina}/{tip}")]
        [HttpPut]
        public async Task<ActionResult> Promeni(int id, int duzina, string tip)
        {
            if(id <= 0)
            {
                return BadRequest("Unet je nevalidan ID!");
            }
            if(duzina <1 || duzina>24)
            {
                return BadRequest("Uneto je nevalidno trajanje treninga!");
            }
            if(string.IsNullOrWhiteSpace(tip) || tip.Length>30)
            {
                return BadRequest("Unet je nevalidan tip treninga!");
            }
            try
            {
                var trening= await Context.Treninzi.FindAsync(id);
                if(trening != null)
                {
                    trening.Duzina= duzina;
                    trening.Tip= tip;
                    await Context.SaveChangesAsync();
                    return Ok($"Uspešno izmenjen trening! ID: {trening.ID}");
                }
                else
                {
                    return BadRequest("Ne postoji trening sa unetim ID-om!");
                }
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }*/
    }
}