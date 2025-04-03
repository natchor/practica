using Dato.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Dato.Respositories
{
    public class SolicitudRepository : BaseRepository<Solicitud, int>, ISolicitudRepository
    {
        private readonly ApplicationDbContext _context;

        public SolicitudRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetCorrelativoAnual()
        {
            int correlativoAnual = 0;
            //string obtenerNum = "SELECT NEXT VALUE FOR Solicitud_CDP";


            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                //command.CommandText = "select * from solicitud where id = 13;";
                command.CommandText = "	  select TRY_CAST(next value for reqCompra..Solicitud_CorrelativoAnual AS INT) as correlativoAnual;";

                _context.Database.OpenConnection();
                DbDataReader reader = await command.ExecuteReaderAsync();


                while (reader.Read())
                {
                    int id = reader.GetInt32("correlativoAnual");
                    correlativoAnual = (id);

                }
                reader.NextResult();
                //}
                _context.Database.CloseConnection();

            };
            //};
            //_context.Dispose();
            return correlativoAnual;
        }
    }
}
