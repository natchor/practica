using Dato.Entities;
using Dato.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Dato.Respositories
{
    public class AprobacionRepository : BaseRepository<Aprobacion, int>, IAprobacionRepository
    {
        private readonly ApplicationDbContext _context;
        public AprobacionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



        public async Task<List<int>> GetAprobadoresIds(AprobacionConfig ac, int userId)
        {
            List<int> ids = new List<int>();

            //using (var context = _context)
            //{
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = ac.Quien;

                if (ac.Quien.Contains("@UserId"))
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@UserId";
                    parameter.Value = userId;

                    command.Parameters.Add(parameter);
                }

                _context.Database.OpenConnection();
                DbDataReader reader = await command.ExecuteReaderAsync();

                while (reader.HasRows)
                {
                    //Console.WriteLine("\t{0}\t{1}", reader.GetName(0),
                    //    reader.GetName(1));

                    while (reader.Read())
                    {
                        //Console.WriteLine("\t{0}\t{1}", reader.GetInt32(0),
                        //    reader.GetString(1));
                        int id = reader.GetInt32("Id");
                        ids.Add(id);
                    }
                    reader.NextResult();
                }

                _context.Database.CloseConnection();


                //using (var result = command.ExecuteReader())
                //{
                //    int id = (int) result["Id"];
                //    ids.Add(id);
                //}
                //command.Connection.Close();
            };
            //};
            //_context.Dispose();



            return ids;
        }


        //
        public void UpdateAprobadoresIds(int num, int solId)
        {

            //using (var context = _context)
            //{
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {

                command.CommandText = "Update Aprobacion set FechaAprobacion = null, EstaAprobado = 0, Orden = Orden + 1 where solicitudId = " + solId + " and Orden >= " + num + "";


                //var parameter = command.CreateParameter();
                //parameter.ParameterName = "@solic";
                //parameter.Value = solId;
                //var parameter2 = command.CreateParameter();
                //parameter2.ParameterName = "@orden";
                //parameter2.Value = num;

                try
                {
                    _context.Database.OpenConnection();
                    command.ExecuteNonQuery();//  ExecuteReaderAsync();
                    _context.Database.CloseConnection();
                }
                catch (Exception e)
                {
                    string ex = e.ToString();
                }





            };

        }

        //
        public void UpdateAprobadoresActualiza(int num, int solId)
        {

            //using (var context = _context)
            //{
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {

                command.CommandText = "Update Aprobacion set FechaAprobacion = null, EstaAprobado = 0, Observacion= null where solicitudId = " + solId;
                //var parameter = command.CreateParameter();
                //parameter.ParameterName = "@solic";
                //parameter.Value = solId;
                //var parameter2 = command.CreateParameter();
                //parameter2.ParameterName = "@orden";
                //parameter2.Value = num;
                try
                {
                    _context.Database.OpenConnection();
                    command.ExecuteNonQuery();//  ExecuteReaderAsync();
                    _context.Database.CloseConnection();
                }
                catch (Exception e)
                {
                    string ex = e.ToString();
                }
            };
        }

        public async Task<int> GetObtenerNumCDP()
        {
            int numCdp = 0;
            //string obtenerNum = "SELECT NEXT VALUE FOR Solicitud_CDP";
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                //command.CommandText = "select * from solicitud where id = 13;";
                command.CommandText = "select TRY_CAST(next value for solicitud_cdp AS INT) as cdp,1 as q;";

                _context.Database.OpenConnection();
                DbDataReader reader = await command.ExecuteReaderAsync();
                //while (reader.HasRows)
                //{
                while (reader.Read())
                {
                    int q = reader.GetInt32("q");
                    int id = reader.GetInt32("cdp");
                    numCdp = (id);
                }
                reader.NextResult();
                //}
                _context.Database.CloseConnection();

            };
            //};
            //_context.Dispose();
            return numCdp;
        }


        public async Task<int> GetObtenerNumCS()
        {
            int numCs = 0;
            //string obtenerNum = "SELECT NEXT VALUE FOR Solicitud_CDP";
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                //command.CommandText = "select * from solicitud where id = 13;";
                command.CommandText = "select TRY_CAST(next value for convenio_cs AS INT) as cs,1 as q;";

                _context.Database.OpenConnection();
                DbDataReader reader = await command.ExecuteReaderAsync();
                //while (reader.HasRows)
                //{
                while (reader.Read())
                {
                    int q = reader.GetInt32("q");
                    int id = reader.GetInt32("cs");
                    numCs = (id);
                }
                reader.NextResult();
                //}
                _context.Database.CloseConnection();

            };
            //};
            //_context.Dispose();
            return numCs;
        }


        public void UpdateMatrizAprobacion(int aproConfId, int solId, int userID)
        {
            //int update = 0;


            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "delete from Aprobacion where solicitudId=" + solId + " and AprobacionConfigId=" + aproConfId + " and  UserAprobadorId <> " + userID + ";";
                try
                {
                    _context.Database.OpenConnection();
                    command.ExecuteNonQuery();//  ExecuteReaderAsync();
                    _context.Database.CloseConnection();
                }
                catch (Exception e)
                {
                    string ex = e.ToString();
                }
            };
            //};
            //_context.Dispose();
            //return numCdp;

        }


        public void InsertMatrizAprobacion(int solId)
        {
            //int update = 0;
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = " INSERT INTO Aprobacion(SolicitudId, AprobacionConfigId, UserAprobadorId, EstaAprobado, Orden) " +
                " SELECT " + solId + ", 1, Id,0, 2 from[User] where SectorId = 231 and CargoId = 2 " +
                " EXCEPT" +
                " SELECT SolicitudId, AprobacionConfigId, UserAprobadorId,0, Orden from Aprobacion where SolicitudId = " + solId;
                try
                {
                    _context.Database.OpenConnection();
                    command.ExecuteNonQuery();//  ExecuteReaderAsync();
                    _context.Database.CloseConnection();
                }
                catch (Exception e)
                {
                    string ex = e.ToString();
                }
            };
        }
    }
}
