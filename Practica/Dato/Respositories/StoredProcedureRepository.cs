
using Dato.Entities;
using Dato.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Dato.Respositories
{
    public class StoredProcedureRepository : BaseRepository<StoredProcedure, int>, IStoredProcedureRepository
    {
        private readonly ApplicationDbContext _context;
        public StoredProcedureRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public DataTable EjecutarProcedimientoAlmacenadoFull(string nombreReporte, string filtro)
        {
            DataTable dtable = new DataTable();
            var conectionString = _context.Database.GetConnectionString();
            using (SqlConnection sql = new SqlConnection(conectionString))
            {
                using (SqlCommand cmd = new SqlCommand(nombreReporte, sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@XML", SqlDbType.NText, 10000, filtro);

                    SqlDataAdapter ds = new SqlDataAdapter(cmd);
                    sql.Open();
                    ds.Fill(dtable);
                }
            }
            return dtable;
        }

        public DataTable EjecutarProcedimientoAlmacenado(string nombreReporte,string filtro)
        {
            DataTable dtable = new DataTable();
            var conectionString = _context.Database.GetConnectionString();
            using (SqlConnection sql = new SqlConnection(conectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Reporte_"+nombreReporte, sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@XML", SqlDbType.NText, 10000, filtro);

                    SqlDataAdapter ds = new SqlDataAdapter(cmd);
                    sql.Open();
                    ds.Fill(dtable);
                }
            }
            return dtable;
        }

    }
}

