using Biblioteca.Librerias;
using DemoIntro.Models;
using Entidad.Interfaz.Models.BitacoraModels;
using Entidad.Interfaz.Models.RoleModels;
using Entidad.Interfaz.Models.TipoMonedaModels;
using Entidad.Interfaz.Models.UserModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Web.Controllers
{
    public class BaseController : Controller
    {

        //private readonly ISectorService _servSector;


        //public BaseController(ISectorService sectorService)
        //{
        //    _servSector = sectorService;
        //}

        //public BaseController(
        //    // IEmailService emailService
        //     IBitacoraService bitacoraService)
        //{

        //    //_servEmail = emailService;
        //    _servBitacora = bitacoraService;
        //}

        public BitacoraModel guardarBitacora(string observacion, int userId, int solicitudId,int tipoBita = default)
        {
            DateTime fecha = DateTime.Now;
            BitacoraModel bitacora = new BitacoraModel();
            bitacora.Fecha = fecha;
            bitacora.Observacion = observacion;
            bitacora.UserId = userId;
            bitacora.SolicitudId = solicitudId;
            bitacora.TipoBitacora = tipoBita;

            return bitacora;

        }

        

        /// <summary>
        /// TODO: en caso de error de CMF, apuntar a https://mindicador.cl/
        /// </summary>
        /// <param name="monedalist"></param>
        public void ObtenerValorMonedas(List<TipoMonedaModel> monedalist)
        {
            string apiKey = SiteKeys.CMFKey;
            DateTime fecha = DateTime.Now.Date;

            foreach (TipoMonedaModel item in monedalist)
            {
                string valor = string.Empty;
                if (item.FechaSolicitud != null && item.FechaSolicitud.ToString("yyyyMMdd") != fecha.ToString("yyyyMMdd"))
                {
                    try
                    {
                        if (item.Codigo != "CLP")
                            valor = GetItem(item.UrlCMF, apiKey, item).ToString();
                    }
                    catch (Exception)
                    {

                    }

                }
                else
                {
                    if (item.Codigo != "CLP")
                        valor = item.Valor.ToString();
                }

                switch (item.Codigo)
                {
                    case "UF":
                        ViewBag.uf = valor;
                        break;
                    case "USD":
                        ViewBag.usd = valor;
                        break;
                    case "EUR":
                        ViewBag.eur = valor;
                        break;
                    case "UTM":
                        ViewBag.utm = valor;
                        break;
                    default:
                        break;
                }


            }

        }

        //public void GuardaBitacora() { 

        //_servBitacora.
        //}
        public string ObtenerValorMonedadiaEspecifico(string UrlCMF,TipoMonedaModel item)
        {
            string apiKey = SiteKeys.CMFKey;
            DateTime fecha = DateTime.Now.Date;
            string valor = string.Empty;
            try
            {
                valor = GetItem(UrlCMF, apiKey, item).ToString();



            }
            catch (Exception)
            {

            }
            return valor;
        }


        private string GetItem(string url, string apiKey, TipoMonedaModel item)
        {

            var client = new WebClient();
            //10 mil peticiones mensuales se guardara este dato en bd tabla valor
            var result = client.DownloadString(string.Format(url, apiKey));
            string obj = result;
            //JObject.Parse(result);
            //var status = (bool)obj.SelectToken("success");

            // var doc = XDocument.Parse(obj);

            XmlDocument docu = new XmlDocument();
            docu.LoadXml(obj);

            TipoMonedaModel valorMoneda = GuardarValoresMonedas(docu, item);

            return valorMoneda.Valor.ToString();

        }

        public static string GetItemOC(string id)
        {
            //id = "584105-221-AG21";
            string apiKey = SiteKeys.MPKey;
            var url = $"https://api.mercadopublico.cl/servicios/v1/publico/ordenesdecompra.json?codigo={id}&ticket={apiKey}";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return "";
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                           // Console.WriteLine(responseBody);
                            return responseBody;
                        }
                    }
                }
                return "";
            }
            catch (WebException ex)
            {
                return ""; // Handle error
            }
        }


        public TipoMonedaModel GuardarValoresMonedas(XmlDocument docu, TipoMonedaModel item)
        {
            //DateTime fecha = DateTime.Now;

            foreach (XmlNode node in docu.DocumentElement.ChildNodes)
            {

                if (node.HasChildNodes)
                {
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        //Console.WriteLine(node.ChildNodes[i].FirstChild.Name + " : " + node.ChildNodes[i].FirstChild.InnerText); fecha
                        //Console.WriteLine(node.ChildNodes[i].LastChild.Name + " : " + node.ChildNodes[i].LastChild.InnerText); valor
                        item.Valor = Convert.ToDecimal(node.ChildNodes[i].LastChild.InnerText);
                        //item.FechaSolicitud = fecha;
                        item.FechaReferencia = DateTime.Parse(node.ChildNodes[i].FirstChild.InnerText);
                        //int ret = _servTipoMoneda.Guardar(item);
                        ////int ret = _servSolicitud.Guardar(solicitud);
                        //return node.ChildNodes[i].LastChild.InnerText;
                        return item;

                    }
                }
            }

            return item;

        }

        public async Task CreateAuthenticationTicket(UserModel user)
        {
            var key = Encoding.ASCII.GetBytes(SiteKeys.Token);
            var JWToken = new JwtSecurityToken(
                            issuer: SiteKeys.WebSiteDomain,
                            audience: SiteKeys.WebSiteDomain,
                            claims: GetUserClaims(user),
                            notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                            expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        );

            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            HttpContext.Session.SetString("JWToken", token);
        }


        private IEnumerable<Claim> GetUserClaims(UserModel user)
        {
            List<Claim> claims = new List<Claim>();

            string rolesStr = string.Join(",", user.UserRoles.Select(ur => ur.Role.Nombre)).ToUpper();
            List<RoleModel> roleModel = user.UserRoles.Select(ur => ur.Role).ToList();


            claims.AddRange(new[] {
                new Claim(CustomClaims.UserId, user.Id._toString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, rolesStr),
                new Claim(PermissionsClaims.AdmMantenedores, roleModel.Where(r => r.AdmMantenedores).Select(r => r.AdmMantenedores).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.ApruebaCDP, roleModel.Where(r => r.ApruebaCDP).Select(r => r.ApruebaCDP).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.FinalizaSolicitud, roleModel.Where(r => r.FinalizaSolicitud).Select(r => r.FinalizaSolicitud).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.IngresaOC, roleModel.Where(r => r.IngresaOC).Select(r => r.IngresaOC).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.IngresaSolicitud, roleModel.Where(r => r.IngresaSolicitud).Select(r => r.IngresaSolicitud).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.PuedeAsignar, roleModel.Where(r => r.PuedeAsignar).Select(r => r.PuedeAsignar).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.VeGestionSolicitudes, roleModel.Where(r => r.VeGestionSolicitudes).Select(r => r.VeGestionSolicitudes).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.ModificaMatrizAprobacion, roleModel.Where(r => r.ModificaMatrizAprobacion).Select(r => r.ModificaMatrizAprobacion).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.VePorFinalizar, roleModel.Where(r => r.VerPorFinalizar).Select(r => r.VerPorFinalizar).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.AjustarCDP, roleModel.Where(r => r.AjustarCDP).Select(r => r.AjustarCDP).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.Reportes, roleModel.Where(r => r.Reportes).Select(r => r.Reportes).FirstOrDefault().ToString()),
                new Claim(PermissionsClaims.VeMisGestiones, roleModel.Where(r => r.VerMisGestiones).Select(r => r.VerMisGestiones).FirstOrDefault().ToString()),



                new Claim(CustomClaims.FullName, user.FullName),
                new Claim(CustomClaims.RoleStr,  rolesStr),
                new Claim(CustomClaims.Cargo, user.Cargo.Nombre),
                new Claim(CustomClaims.Sector, user.Sector.Nombre),
                new Claim(CustomClaims.CargoId, user.CargoId.ToString()),
                new Claim(CustomClaims.RoleId, string.Join(",", user.UserRoles.Select(ur => ur.Role.Id))),
                new Claim(CustomClaims.SectorId, user.SectorId._toString()),
                new Claim(CustomClaims.SectorDemandante, user.Sector.TienePresupuesto ? user.SectorId._toString() : user.Sector.SectorPadreId._toString()),
                new Claim(CustomClaims.SectorPadreId, user.Sector.SectorPadreId.ToString())


            });

            return claims.AsEnumerable<Claim>();
        }

    }

    public struct Role
    {
        public const string Admin = "Admin";
        public const string Presupuesto = "Presupuesto";
        public const string Solicitante = "Solicitante";
        public const string Aprobador = "Aprovador";
    }
}
