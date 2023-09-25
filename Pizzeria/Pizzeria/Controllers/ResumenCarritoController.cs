using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizzeria.Repositories;
using Pizzeria.Models;
using Pizzeria.ViewModels;
using Pizzeria.Data;

namespace Pizzeria.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly AppDbContext _context;
        private readonly Carrito _carrito;

        public ShoppingCartController(IPizzaRepository pizzaRepository,
            Carrito carrito, AppDbContext context)
        {
            _pizzaRepository = pizzaRepository;
            _carrito = carrito;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _carrito.GetShoppingCartItemsAsync();
            _carrito.CarritoItems = items;

            var shoppingCartViewModel = new CarritoViewModel
            {
                Carrito = _carrito,
                TotalCarrito = _carrito.GetTotalCarrito()
            };

            return View(shoppingCartViewModel);
        }

        public async Task<IActionResult> AddACarrito(int pizzaId)
        {
            var pizzaSeleccionada = await _pizzaRepository.GetByIdAsync(pizzaId);

            if (pizzaSeleccionada != null)
            {
                await _carrito.AddToCartAsync(pizzaSeleccionada, 1);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveDeCarrito(int pizzaId)
        {
            var pizzaSeleccionada = await _pizzaRepository.GetByIdAsync(pizzaId);

            if (pizzaSeleccionada != null)
            {
                await _carrito.RemoveFromCartAsync(pizzaSeleccionada);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCart()
        {
            await _carrito.ClearCartAsync();

            return RedirectToAction("Index");
        }

    }
}