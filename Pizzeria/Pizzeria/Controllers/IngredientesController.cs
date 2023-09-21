using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Models;
using Microsoft.AspNetCore.Authorization;
using Pizzeria.Data;

namespace Pizzeria.Controllers
{
    public class IngredientesController : Controller
    {
        [Authorize(Roles = "Admin")]
        public class IngredientsController : Controller
        {
            private readonly AppDbContext _context;

            public IngredientsController(AppDbContext context)
            {
                _context = context;
            }

            // GET: Ingredients
            public async Task<IActionResult> Index()
            {
                return View(await _context.Ingredients.ToListAsync());
            }

            // GET: Ingredients/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var ingredientes = await _context.Ingredients
                    .SingleOrDefaultAsync(m => m.Id == id);
                if (ingredientes == null)
                {
                    return NotFound();
                }

                return View(ingredientes);
            }

            // GET: Ingredients/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Ingredients/Create

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("Id,Name")] Ingredientes ingredientes)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(ingredientes);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(ingredientes);
            }

            // GET: Ingredients/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var ingredientes = await _context.Ingredients.SingleOrDefaultAsync(m => m.Id == id);
                if (ingredientes == null)
                {
                    return NotFound();
                }
                return View(ingredientes);
            }

            // POST: Ingredients/Edit/5

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Ingredientes ingredientes)
            {
                if (id != ingredientes.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(ingredientes);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!IngredientsExists(ingredientes.Id))
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
                return View(ingredientes);
            }

            // GET: Ingredients/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var ingredientes = await _context.Ingredients
                    .SingleOrDefaultAsync(m => m.Id == id);
                if (ingredientes == null)
                {
                    return NotFound();
                }

                return View(ingredientes);
            }

            // POST: Ingredients/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var ingredientes = await _context.Ingredients.SingleOrDefaultAsync(m => m.Id == id);
                _context.Ingredients.Remove(ingredientes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            private bool IngredientsExists(int id)
            {
                return _context.Ingredients.Any(e => e.Id == id);
            }
        }
    }
}