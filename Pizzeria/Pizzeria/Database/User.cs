using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Database
{
    public class User
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Contrasena { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
