using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Nombre Usuario")]
        public string NombreUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [ScaffoldColumn(false)]
        public string ReturnUrl { get; set; }
    }
}