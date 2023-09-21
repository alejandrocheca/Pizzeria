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
    public class PizzasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPizzaRepository _pizzaRepo;
        private readonly ICategoryRepository _categoryRepo;

        public PizzasController(AppDbContext context, IPizzaRepository pizzaRepo, ICategoryRepository categoryRepo)
        {
            _context = context;
            _pizzaRepo = pizzaRepo;
            _categoryRepo = categoryRepo;
        }
    }
}