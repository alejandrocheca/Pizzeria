using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Repositories
{
    public interface IIngredientesRepository
    {
        IEnumerable<Ingredientes> Ingredientes { get; }

        Ingredientes GetById(int? id);
        Task<Ingredientes> GetByIdAsync(int? id);

        bool Exists(int id);

        IEnumerable<Ingredientes> GetAll();
        Task<IEnumerable<Ingredientes>> GetAllAsync();

        void Add(Ingredientes ingrediente);
        void Update(Ingredientes ingrediente);
        void Remove(Ingredientes ingrediente);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}