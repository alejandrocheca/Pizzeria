using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Models;
using Pizzeria.Repositories;
using Microsoft.AspNetCore.Authorization;
using Pizzeria.ViewModels;
using Pizzeria.Data;

namespace Pizzeria.Controllers
{
    public class PizzaIngredientesController : Controller
    {
        private readonly AppDbContext _context;

        public PizzaIngredientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PizzaIngredientes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PizzaIngredientes.Include(p => p.Ingredient).Include(p => p.Pizza);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PizzaIngredientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaIngredientes = await _context.PizzaIngredientes
                .Include(p => p.Ingredient)
                .Include(p => p.Pizza)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pizzaIngredientes == null)
            {
                return NotFound();
            }

            return View(pizzaIngredientes);
        }

        // GET: PizzaIngredientes/Create
        public IActionResult Create()
        {
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name");
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Name");
            return View();
        }

        // POST: PizzaIngredientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PizzaId,IngredientId")] PizzaIngredientes pizzaIngredientes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pizzaIngredientes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name", pizzaIngredientes.IngredientId);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Name", pizzaIngredientes.PizzaId);
            return View(pizzaIngredientes);
        }

        // GET: PizzaIngredientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaIngredientes = await _context.PizzaIngredientes.SingleOrDefaultAsync(m => m.Id == id);
            if (pizzaIngredientes == null)
            {
                return NotFound();
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name", pizzaIngredientes.IngredientId);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Name", pizzaIngredientes.PizzaId);
            return View(pizzaIngredientes);
        }

        // POST: PizzaIngredientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PizzaId,IngredientId")] PizzaIngredientes pizzaIngredientes)
        {
            if (id != pizzaIngredientes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizzaIngredientes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaIngredientesExists(pizzaIngredientes.Id))
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
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name", pizzaIngredientes.IngredientId);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Name", pizzaIngredientes.PizzaId);
            return View(pizzaIngredientes);
        }

        // GET: PizzaIngredientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaIngredientes = await _context.PizzaIngredientes
                .Include(p => p.Ingredient)
                .Include(p => p.Pizza)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pizzaIngredientes == null)
            {
                return NotFound();
            }

            return View(pizzaIngredientes);
        }

        // POST: PizzaIngredientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pizzaIngredientes = await _context.PizzaIngredientes.SingleOrDefaultAsync(m => m.Id == id);
            _context.PizzaIngredientes.Remove(pizzaIngredientes);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PizzaIngredientesExists(int id)
        {
            return _context.PizzaIngredientes.Any(e => e.Id == id);
        }
    }
}