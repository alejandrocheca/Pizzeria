using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Data;
using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _appDbContext;

        private ShoppingCart(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public string CarritoId { get; set; }

        public List<CarritoItem> CarritoItem { get; set; }


        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<AppDbContext>();
            string cartaId = session.GetString("CartaItem") ?? Guid.NewGuid().ToString();

            session.SetString("CartaItem", cartaId);

            return new ShoppingCart(context) { CarritoId = cartaId };
        }

        public async Task AddToCartAsync(Pizzas pizza, int importe)
        {
            var carritoItem =
                    await _appDbContext.CarritoItem.SingleOrDefaultAsync(
                        s => s.Pizza.Id == pizza.Id && s.CarritoId == CarritoId);

            if (carritoItem == null)
            {
                carritoItem = new CarritoItem
                {
                    CarritoId = CarritoId,
                    Pizza = pizza,
                    Importe = 1
                };

                _appDbContext.CarritoItem.Add(carritoItem);
            }
            else
            {
                carritoItem.Importe++;
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveFromCartAsync(Pizzas pizza)
        {
            var carritoItem =
                    await _appDbContext.CarritoItem.SingleOrDefaultAsync(
                        s => s.Pizza.Id == pizza.Id && s.CarritoId == CarritoId);

            var localAmount = 0;

            if (carritoItem != null)
            {
                if (carritoItem.Importe > 1)
                {
                    carritoItem.Importe--;
                    localAmount = carritoItem.Importe;
                }
                else
                {
                    _appDbContext.CarritoItem.Remove(carritoItem);
                }
            }

            await _appDbContext.SaveChangesAsync();

            return localAmount;
        }

        public async Task<List<CarritoItem>> GetShoppingCartItemsAsync()
        {
            return CarritoItem ??
                   (CarritoItem = await
                       _appDbContext.CarritoItem.Where(c => c.CarritoId == CarritoId)
                           .Include(s => s.Pizza)
                           .ToListAsync());
        }

        public async Task ClearCartAsync()
        {
            var cartaItem = _appDbContextCarritoItem
                .CarritoItem
                .Where(carta => carta.CarritoId == CarritoId);

            _appDbContext.CarritoItem.RemoveRange(cartaItem);

            await _appDbContext.SaveChangesAsync();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _appDbContext.CarritoItem.Where(c => c.CarritoId == CarritoId)
                .Select(c => c.Pizza.Precio * c.Importe).Sum();
            return total;
        }

    }
}