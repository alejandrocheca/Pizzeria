using Microsoft.AspNetCore.Mvc;
using Pizzeria.Models;
using Pizzeria.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Components
{
    public class ResumenCarrito : ViewComponent
    {
        private readonly Carrito _carrito;
        public ResumenCarrito(Carrito carrito)
        {
            _carrito = carrito;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _carrito.GetShoppingCartItemsAsync();
            _carrito.CarritoItems = items;

            var carritoCartViewModel = new CarritoViewModel
            {
                Carrito = _carrito,
                ShoppingCartTotal = _carrito.GetShoppingCartTotal()
            };
            return View(carritoCartViewModel);
        }
    }
}