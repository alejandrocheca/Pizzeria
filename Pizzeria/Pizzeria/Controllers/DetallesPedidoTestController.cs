using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pizzeria.Controllers
{
    public class DetallesPedidoTestController : Controller {
        private readonly AppDbContext _context;

        public OrderDetailsTestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: OrderDetailsTest
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.DetallesPedido.Include(o => o.Order);
            return View(await appDbContext.ToListAsync());
        }

        // GET: OrderDetailsTest/Detalles/5
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallesPedido
                .Include(o => o.Order)
                .SingleOrDefaultAsync(m => m.DetallePedidoId == id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            return View(detallePedido);
        }

        // GET: OrderDetailsTest/Create
        public IActionResult Create()
        {
            ViewData["PedidoId"] = new SelectList(_context.Orders, "PedidoId", "Direccion1");
            return View();
        }

        // POST: OrderDetailsTest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetallePedidoId,PedidoId,TartaId,Cantidad,Precio")] DetallePedido detallePedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detallePedido);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["PedidoId"] = new SelectList(_context.Orders, "PedidoId", "Direccion1", detallePedido.PedidoId);
            return View(detallePedido);
        }

        // GET: OrderDetailsTest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallesPedido.SingleOrDefaultAsync(m => m.DetallePedidoId == id);
            if (detallePedido == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Orders, "PedidoId", "Direccion1", detallePedido.PedidoId);
            return View(detallePedido);
        }

        // POST: OrderDetailsTest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DetallePedidoId,PedidoId,TartaId,Cantidad,Precio")] DetallePedido detallePedido)
        {
            if (id != detallePedido.DetallePedidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detallePedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(detallePedido.DetallePedidoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["PedidoId"] = new SelectList(_context.Orders, "PedidoId", "Direccion1", detallePedido.PedidoId);
            return View(detallePedido);
        }

        // GET: OrderDetailsTest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallesPedido
                .Include(o => o.Order)
                .SingleOrDefaultAsync(m => m.DetallePedidoId == id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            return View(detallePedido);
        }

        // POST: OrderDetailsTest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detallePedido = await _context.DetallesPedido.SingleOrDefaultAsync(m => m.DetallePedidoId == id);
            _context.DetallesPedido.Remove(detallePedido);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderDetailExists(int id)
        {
            return _context.DetallesPedido.Any(e => e.DetallePedidoId == id);
        }
    }
}
    