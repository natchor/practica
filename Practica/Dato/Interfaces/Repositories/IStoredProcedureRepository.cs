using Dato.Entities;
using Dato.Interfaces.Repositories;
using System.Data;

namespace Dato.Respositories
{
    public interface IStoredProcedureRepository : IRepository<StoredProcedure, int>
    {
        DataTable EjecutarProcedimientoAlmacenado(string nombreReport,string filtros);
        DataTable EjecutarProcedimientoAlmacenadoFull(string nombreReport, string filtros);
    }
}

