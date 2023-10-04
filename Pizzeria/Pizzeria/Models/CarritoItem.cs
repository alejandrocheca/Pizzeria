using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class CartaCarritoItem
    {
        public int CartaCarritoItemId { get; set; }
        public Pizzas Pizza { get; set; }
        public int Cantidad { get; set; }
        public string CartaCarritoId { get; set; }
    }
}