using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Models
{
    public class Klijent
    {
        [Key]
        public int ID {get; set;}

        [Required]
        [Range(100, 999)]
        public int brKartice { get; set; }

         [Required]
        [MaxLength(30)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(30)]
        public string Prezime { get; set; }

        public virtual List<Spoj> KlijentTrening { get; set; }
    }

}