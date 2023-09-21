using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Pizzeria.Data;

namespace Pizzeria.Controllers
{
    [Authorize]
    public class VistaController : Controller {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VistaController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            var reviews = await _context.Reviews.Include(r => r.Pizza).Include(r => r.User).ToListAsync();
            return View(reviews);
        }

        // GET: Reviews
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                var allReviews = _context.Reviews.Include(r => r.Pizza).Include(r => r.User).ToList();
                return View(allReviews);
            }
            else
            {
                var reviews = _context.Reviews.Include(r => r.Pizza).Include(r => r.User)
                    .Where(r => r.User == user).ToList();
                return View(reviews);
            }
        }

        // GET: Reviews
        [AllowAnonymous]
        public async Task<IActionResult> ListAll()
        {
            var reviews = await _context.Reviews.Include(r => r.Pizza).Include(r => r.User).ToListAsync();
            return View(reviews);
        }

        private async Task<List<Reviews>> SortReviews(string sortBy, bool isDescending)
        {
            var reviewsList = _context.Reviews.Include(r => r.Pizza).Include(r => r.User);
            IQueryable<Reviews> result;

            if (sortBy == null || sortBy == "")
            {
                result = reviewsList;
            }

            if (isDescending == false)
            {
                result = sortBy.ToLower() switch
                {
                    "fecha" => reviewsList.OrderBy(x => x.Fecha),
                    "calificacion" => reviewsList.OrderBy(x => x.Calificacion),
                    "titulo" => reviewsList.OrderBy(x => x.Titulo),
                    "pizza name" => reviewsList.OrderBy(x => x.Pizza.Nombre),
                    _ => reviewsList.OrderBy(x => x.Pizza.Id),
                };
            }
            else
            {
                result = sortBy.ToLower() switch
                {
                    "fecha" => reviewsList.OrderByDescending(x => x.Fecha),
                    "calificacion" => reviewsList.OrderByDescending(x => x.Calificacion),
                    "titulo" => reviewsList.OrderByDescending(x => x.Titulo),
                    "pizza name" => reviewsList.OrderByDescending(x => x.Pizza.Nombre),
                    _ => reviewsList.OrderByDescending(x => x.Pizza.Id),
                };
            }

            //Partial view?
            return await result.ToListAsync();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AjaxListReviews(string sortBy, bool isDescending)
        {
            var listOfReviews = await SortReviews(sortBy, isDescending);

            return PartialView(listOfReviews);
        }

        // GET: Reviews
        [AllowAnonymous]
        public async Task<IActionResult> PizzaReviews(int? pizzaId)
        {
            if (pizzaId == null)
            {
                return NotFound();
            }
            var pizza = _context.Pizzas.FirstOrDefault(x => x.Id == pizzaId);
            if (pizza == null)
            {
                return NotFound();
            }
            var reviews = await _context.Reviews.Include(r => r.Pizza).Include(r => r.User).Where(x => x.Pizza.Id == pizza.Id).ToListAsync();
            if (reviews == null)
            {
                return NotFound();
            }
            ViewBag.NombrePizza = pizza.Nombre;
            ViewBag.PizzaId = pizza.Id;

            return View(reviews);
        }

        // GET: Reviews/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.Pizza)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (reviews == null)
            {
                return NotFound();
            }

            return View(reviews);
        }

        // GET: Reviews/Create
        public IActionResult CreateWithPizza(int? pizzaId)
        {
            var review = new Reviews();

            if (pizzaId == null)
            {
                return NotFound();
            }

            var pizza = _context.Pizzas.FirstOrDefault(p => p.Id == pizzaId);

            if (pizza == null)
            {
                return NotFound();
            }

            review.Pizza = pizza;
            review.PizzaId = pizza.Id;
            ViewData["PizzaId"] = new SelectList(_context.Pizzas.Where(p => p.Id == pizzaId), "Id", "Nombre");
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Calificacion"] = new SelectList(listOfGrades, "Id", "Value");

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWithPizza(int pizzaId, Reviews reviews)
        {
            if (pizzaId != reviews.PizzaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                reviews.UserId = userId;
                reviews.Fecha = DateTime.Now;

                _context.Add(reviews);
                await _context.SaveChangesAsync();
                return Redirect($"PizzaReviews?pizzaId={pizzaId}");
            }
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Calificacion"] = new SelectList(listOfGrades, "Id", "Value", reviews.Calificacion);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas.Where(p => p.Id == pizzaId), "Id", "Nombre", reviews.PizzaId);
            return View(reviews);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Calificacion"] = new SelectList(listOfGrades, "Id", "Value");
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Nombre");
            return View();
        }

        // POST: Reviews/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Description,Calificacion,PizzaId")] Reviews reviews)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                reviews.UserId = userId;

                reviews.Fecha = DateTime.Now;
                _context.Add(reviews);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Calificacion"] = new SelectList(listOfGrades, "Id", "Value", reviews.Calificacion);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Nombre", reviews.PizzaId);

            return View(reviews);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isAdmin = userRoles.Any(r => r == "Admin");

            if (reviews == null)
            {
                return NotFound();
            }

            if (isAdmin == false)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                if (reviews.UserId != userId)
                {
                    return BadRequest("No tienes permiso para editar esta vista.");
                }
            }
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Calificacion"] = new SelectList(listOfGrades, "Id", "Value", reviews.Calificacion);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Nombre", reviews.PizzaId);
            return View(reviews);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Description,Calificacion,Fecha,PizzaId")] Reviews reviews)
        {
            if (id != reviews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                try
                {
                    if (reviews.Fecha == DateTime.MinValue)
                    {
                        reviews.Fecha = DateTime.Now;
                    }
                    reviews.UserId = userId;

                    _context.Update(reviews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewsExists(reviews.Id))
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
            var listOfNumbers = new List<int>() { 1, 2, 3, 4, 5 };
            var listOfGrades = listOfNumbers.Select(x => new { Id = x, Value = x.ToString() });
            ViewData["Calificacion"] = new SelectList(listOfGrades, "Id", "Value", reviews.Calificacion);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Nombre", reviews.PizzaId);
            return View(reviews);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.Pizza)
                .SingleOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isAdmin = userRoles.Any(r => r == "Admin");

            if (reviews == null)
            {
                return NotFound();
            }

            if (isAdmin == false)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                if (reviews.UserId != userId)
                {
                    return BadRequest("No tienes permiso para editar esta vista.");
                }
            }

            return View(reviews);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reviews = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            _context.Reviews.Remove(reviews);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ReviewsExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}