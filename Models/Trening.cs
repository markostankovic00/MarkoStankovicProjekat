using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Trening
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Range(1, 24)]
        public int Duzina { get; set; }

        [Required]
        [MaxLength(30)]
        public string Tip { get; set; }

        [JsonIgnore]
        public virtual List<Spoj> TreningKlijent { get; set; }
    }
}