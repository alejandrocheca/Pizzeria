using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Database
{
    public class Pizza
    {
        [Key]
        public String IdPizza { get; set; }
        [Required]
        public String Nombre { get; set; }
        [Required]
        public String Foto { get; set; }
        [Required]
        public decimal Precio { get; set; }

        public ICollection<Ingredientes> Ingrediente { get; set; }
        public ICollection<Comentarios> Comentario { get; set; }

        public Pizza()
        {
            Ingrediente = new LinkedList<Ingredientes>();
            Comentario = new LinkedList<Comentarios>();
        }

        public void AddIngredientPrice(decimal PrecioIngrediente)
        {
            Precio = Precio + PrecioIngrediente;
        }
    }
}
