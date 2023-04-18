using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Aggiungi il titolo")]
        [StringLength(150, ErrorMessage = "Il titolo può contenere 150 caratteri")]
        public string Title { get; set; } = string.Empty;

        public IEnumerable<Pizza>? Pizzas { get; set; } 
    }
}
