using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Repositories
{
    public interface IPedidoRepository
    {
        Task CreateOrderAsync(Pedido pedido);

    }
}