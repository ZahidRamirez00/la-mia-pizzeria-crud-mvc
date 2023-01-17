using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace La_Mia_Pizzeria_1.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100, ErrorMessage = "Il campo titolo non può contenere più di 100 caratteri")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Required]
        [Url]
        [StringLength(300, ErrorMessage = "Il campo titolo non può contenere più di 300 caratteri")]
        public string Image { get; set; }
        
        [Required]
        [Range(1, 100)]
        public double Prezzo { get; set; }

        public Pizza() { }
        public Pizza(string name, string description, string image, double prezzo)
        {
            Name = name;
            Description = description;
            Image = image;
            Prezzo = prezzo;    
        }
    }
}
