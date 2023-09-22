using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeria.Models;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;

namespace Pizzeria.Repositories
{
    public class IngredientesRepository : IIngredientesRepository
    {
        private readonly AppDbContext _context;

        public IngredientesRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Ingredientes> Ingredientes => _context.Ingredientes.Include(x => x.PizzaIngredients); //include here

        public void Add(Ingredientes ingrediente)
        {
            _context.Ingredientes.Add(ingrediente);
        }

        public IEnumerable<Ingredientes> GetAll()
        {
            return _context.Ingredientes.ToList();
        }

        public async Task<IEnumerable<Ingredientes>> GetAllAsync()
        {
            return await _context.Ingredientes.ToListAsync();
        }

        public Ingredientes GetById(int? id)
        {
            return _context.Ingredientes.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Ingredientes> GetByIdAsync(int? id)
        {
            return await _context.Ingredientes.FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Ingredientes.Any(p => p.Id == id);
        }

        public void Remove(Ingredientes ingrediente)
        {
            _context.Ingredientes.Remove(ingrediente);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Ingredientes ingrediente)
        {
            _context.Ingredientes.Update(ingrediente);
        }
    }
}
