using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Repositories
{
    public interface ICategoriasRepository
    {
        IEnumerable<Categorias> Categorias { get; }

        Categorias GetById(int? id);
        Task<Categorias> GetByIdAsync(int? id);

        bool Exists(int id);

        IEnumerable<Categorias> GetAll();
        Task<IEnumerable<Categorias>> GetAllAsync();

        void Add(Categorias categoria);
        void Update(Categorias categoria);
        void Remove(Categorias categoria);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}