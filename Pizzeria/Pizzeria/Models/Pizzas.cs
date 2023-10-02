using Microsoft.EntityFrameworkCore;
using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class Pizzas
    {
        public Pizzas()
        {
            PizzaIngredientes = new HashSet<PizzaIngredientes>();
            Reviews = new HashSet<Reviews>();
        }

        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "El campo Nombre sólo debe incluir letras y números.")]
        [DataType(DataType.Text)]
        [Required]
        public string Nombre { get; set; }

        [Range(0, 1000)]
        [DataType(DataType.Currency)]
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [StringLength(255, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Descripcion { get; set; }

        [DataType(DataType.ImagenesUrl)]
        public string ImagenesUrl { get; set; }

        public bool IsPizzaOfTheWeek { get; set; }

        [DisplayName("Selecciona Categoria")]
        public int CategoriasId { get; set; }

        public virtual Categorias Categoria { get; set; }

        public virtual ICollection<PizzaIngredientes> PizzaIngredientes { get; set; }
        public virtual ICollection<Reviews> Reviews { get; set; }

    }
}