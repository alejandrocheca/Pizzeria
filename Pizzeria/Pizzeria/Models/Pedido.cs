using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class Pedido
    {
        [BindNever]
        public int OrderId { get; set; }

        public List<DetallePedido> OrderLines { get; set; }

        [Required(ErrorMessage = "Por favor, escriba su nombre")]
        [Display(Name = "Nombre")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Por favor, escriba su apellido")]
        [Display(Name = "Apellido")]
        [StringLength(50)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Por favor, escriba su direccion")]
        [StringLength(100)]
        [Display(Name = "Direccion 1")]
        public string Direccion1 { get; set; }

        [Display(Name = "Direccion 2")]
        public string Direccion2 { get; set; }

        [Required(ErrorMessage = "Por favor, escriba su codigo postal")]
        [Display(Name = "Codigo postal")]
        [StringLength(10, MinimumLength = 4)]
        public string CodPostal { get; set; }

        [Required(ErrorMessage = "Por favor, escriba su ciudad")]
        [StringLength(50)]
        public string Ciudad { get; set; }

        [StringLength(10)]
        public string ComAutonoma { get; set; }

        [Required(ErrorMessage = "Por favor, escriba su pais")]
        [StringLength(50)]
        public string Pais { get; set; }

        [Required(ErrorMessage = "Por favor, escriba su numero de telefono")]
        [StringLength(25)]
        [DataType(DataType.NumTelefono)]
        [Display(Name = "Telefono")]
        public string NumTelefono { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Email)]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
            ErrorMessage = "El correo no esta en el formato correcto")]
        public string Email { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        [DisplayName("Pedido Total")]
        [Precision(18, 2)]
        public decimal PEdidoTotal { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        [DisplayName("Fecha pedido")]
        public DateTime PedidoRealizado { get; set; }

        //[BindNever]
        //[ScaffoldColumn(false)]
        //public string OrderStatus { get; set; }

        public string IdUsuario { get; set; }

        public IdentityUser Usuario { get; set; }

    }
}