using Biblioteca.Seguridad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using Web.ReqCompra.Models;
using SpreadsheetLight;
using Dato.Respositories;
using System.Data;

using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;

using System.Web;




using AppS = System.Configuration.ConfigurationManager;

using System.Threading;
using System.Net;
using DemoIntro.Models;
using Biblioteca.Librerias;
using Nancy.Json;
using System.Linq;

namespace Web.ReqCompra.Controllers
{
    public class ReporteController : Controller
    {
        private readonly IStoredProcedureRepository _repProcedure;

        public ReporteController(
            IStoredProcedureRepository procedureRepo
           )
        {
            _repProcedure = procedureRepo;
        }

        public IActionResult Reporte()
        {
            return View();
        }


        //public ActionResult DescargarReporteTiempoRespuesta() { 

        //    var rpt= new report
        //}

        private string CrearCarpeta(string folderPath)
        {
            string path = SiteKeys.ReportPath;
            string ruta = path + folderPath;
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
                Console.WriteLine(ruta);
            }
            return ruta;
        }

        [HttpGet("DescargarArchivo/{nombre}")]
        public IActionResult DescargarArchivo(string nombre)
        {
            byte[] fileBytes = null;
            string filePath = string.Empty;
            string contentType = string.Empty;
            string fileName = string.Empty;

            using (WindowsLogin wl = new WindowsLogin())
            {
                System.Security.Principal.WindowsIdentity.RunImpersonated(wl.Identity.AccessToken, () =>
                {

                    string nombreReport = nombre;
                    filePath = CrearReporte(nombreReport);
                    fileName = nombreReport + ".xlsx";
                    fileBytes = null;
                    contentType = string.Empty;

                    fileBytes = System.IO.File.ReadAllBytes(filePath);

                });
            };


            if (!new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType))
                contentType = "application/force-download";

            return File(fileBytes, contentType, fileName);
        }


        [HttpGet("VerReporte")]
        public string VerReporte(string id)
        {
            DataTable dtable = new DataTable();
            //LLeno el datatable a futuro enviar nombre de procedimeinto por parametro
            dtable = _repProcedure.EjecutarProcedimientoAlmacenado("TiempoRespuesta", id);
            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            if (dtable.Rows.Count > 0)
            {
                var lista1 = dtable.AsEnumerable().Select(x => new
                {
                    Fecha = x["Fecha"].ToString(),
                    FechaAnterior = x["FechaAnterior"].ToString(),
                    Observacion = x["Observacion"].ToString(),
                    dias = x["dias"].ToString(),
                    OrigenUserName = x["OrigenUserName"].ToString(),
                    OrigenNombreCargo = x["OrigenNombreCargo"].ToString(),
                    OrigenNombreSector = x["OrigenNombreSector"].ToString(),
                    OrigenUserRole = x["OrigenUserRole"].ToString(),
                    DestinoUserName = x["DestinoUserName"].ToString(),
                    DestinoNombreCargo = x["DestinoNombreCargo"].ToString(),
                    DestinoNombreSector = x["DestinoNombreSector"].ToString(),
                    DestinoUserRole = x["DestinoUserRole"].ToString(),
                    NroSolicitud = x["NroSolicitud"].ToString(),
                    IniciativaVigenteId = x["IniciativaVigenteId"].ToString(),
                    IniciativaVigente = x["IniciativaVigente"].ToString(),
                    NombreCompra = x["NombreCompra"].ToString(),
                    MontoUTM = x["MontoUTM"].ToString(),
                    CDPNum = x["CDPNum"].ToString(),
                    ContraparteTecnicaId = x["ContraparteTecnicaId"].ToString(),
                    FechaCreacion = x["FechaCreacion"].ToString(),
                    OrdenCompra = x["OrdenCompra"].ToString(),
                    FaseCDP = x["FaseCDP"].ToString(),
                    MontoCLP = x["MontoCLP"].ToString(),
                }).ToList();


                var TheJson = TheSerializer.Serialize(new { lista1 });
                return TheJson;


            }
            else
            {
                var TheJson = TheSerializer.Serialize(null);
                return TheJson;
            }
        }

        public void Muestra_Archivo(string ruta, string tipoArchivo, string nombreArchivo)
        {
            String archivo = ruta + "\\" + nombreArchivo;
            try
            {


                string remoteUri = "http://www.contoso.com/library/homepage/images/";
                string fileName = "ms-banner.pdf", myStringWebResource = null;
                // Create a new WebClient instance.
                WebClient myWebClient = new WebClient();
                // Concatenate the domain with the Web resource filename.
                string carpetadescarga = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//" + fileName;
                myStringWebResource = remoteUri + fileName;

                // Download the Web resource and save it into the current filesystem folder.
                myWebClient.DownloadFile("http://bibing.us.es/proyectos/abreproy/11833/fichero/2.Capitulo2.pdf", carpetadescarga);
                Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, myStringWebResource);

            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public string CrearReporte(string nombreReport)
        {
            DataTable dtable = new DataTable();
            //LLeno el datatable a futuro enviar nombre de procedimeinto por parametro
            dtable = _repProcedure.EjecutarProcedimientoAlmacenado(nombreReport, "");
            string ruta = CrearCarpeta(User.FindFirst(CustomClaims.UserId).Value._toString());
            //string hora = DateTime.Now.ToString("hh:mm:ss");


            DateTime parsedDate = DateTime.Now;
            string fecha = parsedDate.ToString("yyyyMMddhhmmss"); ;
            string rutaFinal = ruta + "\\" + fecha + nombreReport + ".xlsx";
            //string rutaFinal = @"C:\firmas\Plantilla.xlsx";
            #region formatos
            //// The default format code is "General".
            //sl.SetCellValue(2, 1, 12345.678909);
            //SLStyle style;
            //style = sl.CreateStyle();

            //style.FormatCode = "#,##0.00";
            //sl.SetCellValue(2, 2, 12345.678909);
            //sl.SetCellStyle(2, 2, style);

            //style.FormatCode = "0.00";
            //sl.SetCellValue(2, 3, 5.6789);
            //sl.SetCellStyle(2, 3, style);

            //style.FormatCode = "$#,##0.00_);[Red]($#,##0.00)";
            //sl.SetCellValue(2, 4, -123456789.5678);
            //sl.SetCellStyle(2, 4, style);

            //style.FormatCode = "_($* #,##0.00_);_($* (#,##0.00);_($* \" - \"??_);_(@_)";
            //sl.SetCellValue(2, 5, -123456789.5678);
            //sl.SetCellStyle(2, 5, style);
            //sl.SetCellValue(3, 5, 123456789.5678);
            //sl.SetCellStyle(3, 5, style);

            //style.FormatCode = "0.00%";
            //sl.SetCellValue(2, 6, 5.6789);
            //sl.SetCellStyle(2, 6, style);

            //style.FormatCode = "# ?/?";
            //sl.SetCellValue(2, 7, 5.6789);
            //sl.SetCellStyle(2, 7, style);

            //style.FormatCode = "0.000E+00";
            //sl.SetCellValue(2, 8, 12345.678909);
            //sl.SetCellStyle(2, 8, style);

            //sl.SetCellValue(2, 9, true);
            //sl.SetCellValue(2, 10, false);

            //sl.AutoFitColumn(1, 10);

            //sl.AddWorksheet("Dates");

            //style.FormatCode = "dd/mm/yyyy";
            //sl.SetCellValue(2, 1, new DateTime(2718, 2, 8));
            //sl.SetCellStyle(2, 1, style);

            //style.FormatCode = "mmmm dd, yyyy";
            //sl.SetCellValue(2, 2, new DateTime(2718, 2, 8));
            //sl.SetCellStyle(2, 2, style);

            //style.FormatCode = "d mmmmm";
            //sl.SetCellValue(2, 3, new DateTime(2718, 2, 8));
            //sl.SetCellStyle(2, 3, style);

            //style.FormatCode = "mmm-yyyy";
            //sl.SetCellValue(2, 4, new DateTime(2718, 2, 8));
            //sl.SetCellStyle(2, 4, style);

            //style.FormatCode = "dd/mm/yyyy h:mm:ss";
            //sl.SetCellValue(2, 5, new DateTime(2718, 2, 8, 15, 34, 59));
            //sl.SetCellStyle(2, 5, style);

            //style.FormatCode = "dd/mm/yyyy h:mm:ss AM/PM";
            //sl.SetCellValue(2, 6, new DateTime(2718, 2, 8, 15, 34, 59));
            //sl.SetCellStyle(2, 6, style);

            //sl.AutoFitColumn(1, 6);

            //dt.Columns.Add("Nombre", typeof(string));
            //dt.Columns.Add("Edad", typeof(int));
            //dt.Columns.Add("Sexo", typeof(string));

            //var lista2 = dtable.AsEnumerable().Select(x => new
            //{

            //    Fecha = x["Fecha"].ToString(),
            //    FechaAnterior = x["FechaAnterior"].ToString(),
            //    Observacion = x["Observacion"].ToString(),
            //    dias = x["dias"].ToString(),
            //    OrigenUserName = x["OrigenUserName"].ToString(),
            //    OrigenNombreCargo = x["OrigenNombreCargo"].ToString(),
            //    OrigenNombreSector = x["OrigenNombreSector"].ToString(),
            //    OrigenUserRole = x["OrigenUserRole"].ToString(),
            //    DestinoUserName = x["DestinoUserName"].ToString(),
            //    DestinoNombreCargo = x["DestinoNombreCargo"].ToString(),
            //    DestinoNombreSector = x["DestinoNombreSector"].ToString(),
            //    DestinoUserRole = x["DestinoUserRole"].ToString(),
            //    NroSolicitud = x["NroSolicitud"].ToString(),
            //    IniciativaVigenteId = x["IniciativaVigenteId"].ToString(),
            //    IniciativaVigente = x["IniciativaVigente"].ToString(),
            //    NombreCompra = x["NombreCompra"].ToString(),
            //    MontoUTM = x["MontoUTM"].ToString(),
            //    CDPNum = x["CDPNum"].ToString(),
            //    ContraparteTecnicaId = x["ContraparteTecnicaId"].ToString(),
            //    FechaCreacion = x["FechaCreacion"].ToString(),
            //    OrdenCompra = x["OrdenCompra"].ToString(),
            //    FaseCDP = x["FaseCDP"].ToString(),
            //    MontoCLP = x["MontoCLP"].ToString(),


            //}).ToList();

            //sl.SaveAs(ruta2);

            //SLDocument sl = new SLDocument(ruta2);
            #endregion

            SLDocument sl = new SLDocument();
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, nombreReport);
            sl.SetCellValue("A1", "Reporte");
            sl.SetCellValue("B1", nombreReport);
            sl.SetCellValue("A2", "Fecha reporte:");
            sl.SetCellValue("B2", parsedDate.ToString());

            SLStyle style = sl.CreateStyle();

            style.Font.FontSize = 18;
            //style.Font.FontColor = System.Drawing.Color.Blue;
            style.Font.Bold = true;
            //style.Font.Italic = true;
            //style.Font.Strike = true;
            //style.Font.Underline = UnderlineValues.Double;
            sl.SetCellStyle("B1", style);



            int iStartRowIndex = 3;
            int iStartColumnIndex = 1;
            sl.AutoFitColumn(1, 6);
            sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dtable, true);

            // This part sets the style, but you might be using a template file,
            // so the styles are probably already set.

            SLStyle style2 = sl.CreateStyle();
            style2.FormatCode = "yyyy/mm/dd hh:mm:ss";
            sl.SetColumnStyle(1, style2);
            sl.SetColumnStyle(2, style2);



            // estas lineas de codigo muestran el tamaño del dataset como si fuera un diseño de tabla de excel 
            // encabezadode un color
            // - 1 pares sin color.
            // + 1 impares gris

            int iEndRowIndex = iStartRowIndex + dtable.Rows.Count + 1 - 1;

            int iEndColumnIndex = iStartColumnIndex + dtable.Columns.Count - 1;
            SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
            table.SetTableStyle(SLTableStyleTypeValues.Medium17);
            table.HasTotalRow = true;

            //table.SetTotalRowFunction(1, SLTotalsRowFunctionValues.Sum);
            sl.InsertTable(table);

            sl.SaveAs(rutaFinal);
            return rutaFinal;
            //Muestra_Archivo("C://firmas", "Excel", "Plantilla.xlsx");
        }

        public void VerReporteTiempoRespuesta()
        {

        }
    }
}
