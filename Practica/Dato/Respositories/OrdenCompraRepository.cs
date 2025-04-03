using System;
using System.Collections.Generic;
using System.Text;

using Dato.Entities;

namespace Dato.Respositories
{
    public class OrdenCompraRepository : BaseRepository<OrdenCompra, int>, IOrdenCompraRepository
    {
        public OrdenCompraRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}