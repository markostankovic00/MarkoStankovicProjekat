using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Teretana
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Naziv { get; set; }

        [Required]
        [RegularExpression( @"^[1-9]\d*$")]
        public int CenaPoSatu { get; set; }


        [Required]
        [Range(10, 25)]
        public int BrojOrmarica { get; set; }


        [Required]
        [MaxLength(25)]
        [MinLength(10)]
        public string ZauzetiOrmarici { get; set;}
        
        [JsonIgnore]
        public virtual List<Spoj> KlijentiTreninzi { get; set; }
    }
}