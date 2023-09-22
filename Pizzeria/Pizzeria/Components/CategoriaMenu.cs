using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Pizzeria.Models;
using Pizzeria.Repositories;
using Pizzeria.ViewModels;
using Pizzeria.Data;
using Pizzeria.Database;

namespace OnlinePizzaWebApplication.Controllers {
    [Authorize(Roles = "Admin")]
    public class PizzasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPizzaRepository _pizzaRepo;
        private readonly ICategoriasRepository _categoryRepo;
        public PizzasController(AppDbContext context, IPizzaRepository pizzaRepo, ICategoriasRepository categoryRepo)
        {
            _context = context;
            _pizzaRepo = pizzaRepo;
            _categoryRepo = categoryRepo;
        }

        // GET: Pizzas
        public async Task<IActionResult> Index()
        {
            return View(await _pizzaRepo.GetAllIncludedAsync());
        }

        // GET: Pizzas
        [AllowAnonymous]
        public async Task<IActionResult> ListAll()
        {
            var model = new SearchPizzasViewModel()
            {
                PizzaList = await _pizzaRepo.GetAllIncludedAsync(),
                SearchText = null
            };

            return View(model);
        }

        private async Task<List<Pizzas>> GetPizzaSearchList(string userInput)
        {
            var userInputTrimmed = userInput?.ToLower()?.Trim();

            if (string.IsNullOrWhiteSpace(userInputTrimmed))
            {
                return await _context.Pizzas.Include(p => p.Categoria)
                    .Select(p => p).OrderBy(p => p.Nombre)
                    .ToListAsync();
            }
            else
            {
                return await _context.Pizzas.Include(p => p.Categoria)
                    .Where(p => p
                    .Nombre.ToLower().Contains(userInputTrimmed))
                    .Select(p => p).OrderBy(p => p.Nombre)
                    .ToListAsync();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AjaxSearchList(string searchString)
        {
            var pizzaList = await GetPizzaSearchList(searchString);

            return PartialView(pizzaList);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListAll([Bind("SearchText")] SearchPizzasViewModel model)
        {
            var pizzas = await _pizzaRepo.GetAllIncludedAsync();
            if (model.SearchText == null || model.SearchText == string.Empty)
            {
                model.PizzaList = pizzas;
                return View(model);
            }

            var input = model.SearchText.Trim();
            if (input == string.Empty || input == null)
            {
                model.PizzaList = pizzas;
                return View(model);
            }
            var searchString = input.ToLower();

            if (string.IsNullOrEmpty(searchString))
            {
                model.PizzaList = pizzas;
            }
            else
            {
                var pizzaList = await _context.Pizzas.Include(x => x.Categoria).Include(x => x.Reviews).Include(x => x.PizzaIngredientes).OrderBy(x => x.Nombre)
                     .Where(p =>
                     p.Nombre.ToLower().Contains(searchString)
                  || p.Precio.ToString("c").ToLower().Contains(searchString)
                  || p.Categoria.Nombre.ToLower().Contains(searchString)
                  || p.PizzaIngredientes.Select(x => x.Ingrediente.Nombre.ToLower()).Contains(searchString))
                    .ToListAsync();

                if (pizzaList.Any())
                {
                    model.PizzaList = pizzaList;
                }
                else
                {
                    model.PizzaList = new List<Pizzas>();
                }

            }
            return View(model);
        }

        // GET: Pizzas
        [AllowAnonymous]
        public async Task<IActionResult> ListaCategoria(string nombreCategoria)
        {
            bool categoryExtist = _context.Categories.Any(c => c.Nombre == nombreCategoria);
            if (!categoryExtist)
            {
                return NotFound();
            }

            var categoria = await _context.Categories.FirstOrDefaultAsync(c => c.Nombre == nombreCategoria);

            if (categoria == null)
            {
                return NotFound();
            }

            bool anyPizzas = await _context.Pizzas.AnyAsync(x => x.Categoria == categoria);
            if (!anyPizzas)
            {
                return NotFound($"No hay Pizzas donde se ha buscado en la categoria: {nombreCategoria}");
            }

            var pizzas = _context.Pizzas.Where(x => x.Categoria == categoria)
                .Include(x => x.Categoria).Include(x => x.Reviews);

            ViewBag.CurrentCategory = categoria.Nombre;
            return View(pizzas);
        }

        // GET: Pizzas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzas = await _pizzaRepo.GetByIdIncludedAsync(id);

            if (pizzas == null)
            {
                return NotFound();
            }

            return View(pizzas);
        }

        // GET: Pizzas/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> DisplayDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzas = await _pizzaRepo.GetByIdIncludedAsync(id);

            var listaIngredientes = await _context.PizzaIngredientes.Where(x => x.PizzaId == id).Select(x => x.Ingrediente.Nombre).ToListAsync();
            ViewBag.PizzaIngredientes = listaIngredientes;

            //var listOfReviews = await _context.Reviews.Where(x => x.PizzaId == id).Select(x => x).ToListAsync();
            //ViewBag.Reviews = listOfReviews;
            double score;
            if (_context.Reviews.Any(x => x.PizzaId == id))
            {
                var review = _context.Reviews.Where(x => x.PizzaId == id);
                score = review.Average(x => x.Grade);
                score = Math.Round(score, 2);
            }
            else
            {
                score = 0;
            }
            ViewBag.AverageReviewScore = score;

            if (pizzas == null)
            {
                return NotFound();
            }

            return View(pizzas);
        }

        // GET: Pizzas
        [AllowAnonymous]
        public async Task<IActionResult> SearchPizzas()
        {
            var model = new SearchPizzasViewModel()
            {
                PizzaList = await _pizzaRepo.GetAllIncludedAsync(),
                SearchText = null
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchPizzas([Bind("SearchText")] SearchPizzasViewModel model)
        {
            var pizzas = await _pizzaRepo.GetAllIncludedAsync();
            var search = model.SearchText.ToLower();

            if (string.IsNullOrEmpty(search))
            {
                model.PizzaList = pizzas;
            }
            else
            {
                var pizzaList = await _context.Pizzas.Include(x => x.Categoria).Include(x => x.Reviews).Include(x => x.PizzaIngredientes).OrderBy(x => x.Nombre)
                    .Where(p =>
                     p.Nombre.ToLower().Contains(search)
                  || p.Precio.ToString("c").ToLower().Contains(search)
                  || p.Categoria.Nombre.ToLower().Contains(search)
                  || p.PizzaIngredientes.Select(x => x.Ingrediente.Nombre.ToLower()).Contains(search)).ToListAsync();

                if (pizzaList.Any())
                {
                    model.PizzaList = pizzaList;
                }
                else
                {
                    model.PizzaList = new List<Pizzas>();
                }

            }
            return View(model);
        }

        // GET: Pizzas/Create
        public IActionResult Create()
        {
            ViewData["CategoriasId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Nombre");
            return View();
        }

        // POST: Pizzas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Descripcion,ImageUrl,PizzaDeLaSemana,CategoriasId")] Pizzas pizzas)
        {
            if (ModelState.IsValid)
            {
                _pizzaRepo.Add(pizzas);
                await _pizzaRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoriasId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Nombre", pizzas.CategoriasId);
            return View(pizzas);
        }

        // GET: Pizzas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzas = await _pizzaRepo.GetByIdAsync(id);

            if (pizzas == null)
            {
                return NotFound();
            }
            ViewData["CategoriasId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Nombre", pizzas.CategoriasId);
            return View(pizzas);
        }

        // POST: Pizzas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Descripcion,ImageUrl,PizzaDeLaSemana,CategoriasId")] Pizzas pizzas)
        {
            if (id != pizzas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _pizzaRepo.Update(pizzas);
                    await _pizzaRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzasExists(pizzas.Id))
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
            ViewData["CategoriasId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Nombre", pizzas.CategoriasId);
            return View(pizzas);
        }

        // GET: Pizzas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzas = await _pizzaRepo.GetByIdIncludedAsync(id);

            if (pizzas == null)
            {
                return NotFound();
            }

            return View(pizzas);
        }

        // POST: Pizzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pizzas = await _pizzaRepo.GetByIdAsync(id);
            _pizzaRepo.Remove(pizzas);
            await _pizzaRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PizzasExists(int id)
        {
            return _pizzaRepo.Exists(id);
        }
    }
}