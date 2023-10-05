using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class CarritoViewModel
    {
        public CarritoCarta CarritoCarta { get; set; }
        public decimal CarritoTotal { get; set; }
    }
}