using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Pizzeria.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pizzeria.Models
{
    public class Resenas
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "El campo Título sólo debe incluir letras y números.")]
        [DataType(DataType.Text)]
        [Required]
        public string Titulo { get; set; }

        [StringLength(500, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Descripcion { get; set; }

        [Range(1, 5)]
        public int Grado { get; set; }

        public DateTime Fecha { get; set; }

        [DisplayName("Selecciona Pizza")]
        public int PizzaId { get; set; }

        public virtual Pizzas Pizza { get; set; }

        public string UsuarioId { get; set; }

        public IdentityUser Usuario { get; set; }

    }
}