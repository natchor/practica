using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace Biblioteca.Librerias
{
    //
    // Summary:
    //     Defines logging severity levels.
    public enum LogLevel
    {
        //
        // Summary:
        //     Logs that track the general flow of the application. These logs should have long-term
        //     value.
        Information = 2,
        //
        // Summary:
        //     Logs that highlight an abnormal or unexpected event in the application flow,
        //     but do not otherwise cause the application execution to stop.
        Warning = 3,
        //
        // Summary:
        //     Logs that highlight when the current flow of execution is stopped due to a failure.
        //     These should indicate a failure in the current activity, not an application-wide
        //     failure.
        Error = 4,
        //
        // Summary:
        //     Logs that describe an unrecoverable application or system crash, or a catastrophic
        //     failure that requires immediate attention.
        Critical = 5
    }

    public class LogEvent
    {
        //private string _path = ConfigurationManager.AppSettings["logPath"];

        private static IConfigurationSection _configuration;


        public static void Configure(IConfigurationSection configuration)
        {
            _configuration = configuration;
        }

        public string _path => _configuration["logPath"];

        public void Registrar(string aplicacion, string mensaje, LogLevel tipo)
        {
            try
            {
                this.Save(aplicacion, mensaje, tipo);
            }
            catch
            {
                throw;
            }
        }

        public void Registrar(string aplicacion, Exception excepcion, LogLevel tipo)
        {
            try
            {

                string error = excepcion.InnerException != null ? excepcion.InnerException.ToString() : excepcion.Message.ToString();
                string mensaje = $"{error}{Environment.NewLine}{excepcion.StackTrace}";

                this.Save(aplicacion, mensaje, tipo);
            }
            catch
            {
                throw;
            }
        }

        private void Save(string aplicacion, string mensaje, LogLevel tipo)
        {
            try
            {
                if (string.IsNullOrEmpty(this._path))
                    throw new ArgumentException();

                string nombre = string.Format("Log{0}_{1}.txt", aplicacion, DateTime.Today.ToString("yyyyMMdd"));
                string fullPath = null;

                FileInfo info = new FileInfo(this._path);

                if ((info.Attributes.Equals(FileAttributes.Directory)))
                    fullPath = info.FullName;
                else
                    fullPath = info.ToString();

                fullPath = string.Format("{0}\\{1}", fullPath, nombre);

                StringBuilder linea = new StringBuilder();

                linea.AppendLine(string.Format("[{0:00}:{1:00}:{2:00}:{3:0000}] {4} | {5}",
                                                       DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond,
                                                       tipo.ToString(), mensaje));
                bool error = true;
                int cont = 0;

                while (error)
                {
                    try
                    {
                        File.AppendAllText(fullPath, linea.ToString());

                        error = false;
                    }
                    catch
                    {
                        cont += 1;
                        if (cont > 3)
                            error = false;
                        else
                            System.Threading.Tasks.Task.Delay(2000); //2seg
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private bool _disposed = false;

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    //nothing
                }

                this._disposed = true;
            }
        }

        ~LogEvent()
        {
            this.Dispose(false);
        }
    }


}
