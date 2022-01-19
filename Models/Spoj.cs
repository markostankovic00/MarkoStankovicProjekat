using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;

namespace Models
{
    public class Spoj
    {
        [Key]
        public int ID { get; set; }

        [RegularExpression( @"^[1-9]\d*$")]
        [Required]
        public int Cena { get; set; }

        [Range(1, 25)]
        [Required]
        public int Ormaric { get; set; }

        [Required]
        public DateTime Termin { get; set;}

        public virtual Teretana Teretana { get; set; }

        [JsonIgnore]
        public virtual Klijent Klijent { get; set; }

        public virtual Trening Trening { get; set; }
    }
}