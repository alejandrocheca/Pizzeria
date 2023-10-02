using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pizzeria.Models
{
    public class Ingredientes
    {
        public Ingredientes()
        {
            PizzaIngredientes = new HashSet<PizzaIngredientes>();
        }

        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "El campo Nombre sólo debe incluir letras y números.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<PizzaIngredientes> PizzaIngredientes { get; set; }

    }
}