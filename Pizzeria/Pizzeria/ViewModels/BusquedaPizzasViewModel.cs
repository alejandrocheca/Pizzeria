using Pizzeria.Models;
using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class BusquedaPizzasViewModel
    {
        [Required]
        [DisplayName("Serach")]
        public string BusquedaTexto { get; set; }

        //public IEnumerable<string> SearchListExamples { get; set; }

        public IEnumerable<Pizzas> PizzaLista { get; set; }

    }
}