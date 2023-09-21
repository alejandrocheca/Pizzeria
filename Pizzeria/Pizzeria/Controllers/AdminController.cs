using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pizzeria.Repositories;
using Pizzeria.Models;
using Pizzeria.Data;

namespace Pizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAdminRepository _adminRepo;

        public AdminController(AppDbContext context, IAdminRepository adminRepo)
        {
            _context = context;
            _adminRepo = adminRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ClearDatabase()
        {
            _adminRepo.ClearDatabase();
            return RedirectToAction("Index", "Pizzas", null);
        }

        public IActionResult SeedDatabase()
        {
            _adminRepo.ClearDatabase();
            _adminRepo.SeedDatabase();
            return RedirectToAction("Index", "Pizzas", null);
        }

    }
}