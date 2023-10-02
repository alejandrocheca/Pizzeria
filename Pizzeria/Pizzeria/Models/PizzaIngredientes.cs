using Pizzeria.Database;
using System.ComponentModel;

namespace Pizzeria.Models
{
    public class PizzaIngredientes
    {
        public int Id { get; set; }

        [DisplayName("Selecciona Pizza")]
        public int PizzaId { get; set; }

        [DisplayName("Selecciona Ingrediente")]
        public int IngredientId { get; set; }

        public virtual Ingredientes Ingrediente { get; set; }
        public virtual Pizzas Pizza { get; set; }
    }
}