using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Database
{
    public class Comentarios
    {
        [Key]
        public String IdComentario { set; get; }
        [Required]
        public String Texto { set; get; }
        [Required]
        public int Ratio { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public String Usuario { get; set; }
        [Required]
        public String Pizza { get; set; }

    }
}