using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeria.Models;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;

namespace Pizzeria.Repositories
{
    public class CategoriasRepository : ICategoriasRepository
    {
        private readonly AppDbContext _context;

        public CategoriasRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categorias> Categorias => _context.Categorias.Include(x => x.Pizzas); //include here

        public void Add(Categorias categoria)
        {
            _context.Add(categoria);
        }

        public IEnumerable<Categorias> GetAll()
        {
            return _context.Categorias.ToList();
        }

        public async Task<IEnumerable<Categorias>> GetAllAsync()
        {
            return await _context.Categorias.ToListAsync();
        }

        public Categorias GetById(int? id)
        {
            return _context.Categorias.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Categorias> GetByIdAsync(int? id)
        {
            return await _context.Categorias.FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Pizzas.Any(p => p.Id == id);
        }

        public void Remove(Categorias categoria)
        {
            _context.Remove(categoria);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Categorias categoria)
        {
            _context.Update(categoria);
        }

    }
}