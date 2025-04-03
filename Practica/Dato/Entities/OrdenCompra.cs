using System;
using System.Collections.Generic;
using System.Text;

namespace Dato.Entities
{
    public class OrdenCompra
    {
        public int Id { get; set; }
        public string ActividadProveedor { get; set; }
        public int Cantidad { get; set; }
        public int CantidadEvaluacion { get; set; }
        public string CargoContacto { get; set; }
        public string CargoContactoProveedor { get; set; }
        public string Cargos { get; set; }
        public string Categoria { get; set; }
        public string CodigoCategoria { get; set; }
        public int CodigoEstadoProveedor { get; set; }
        public string CodigoOC { get; set; }
        public string CodigoProducto { get; set; }
        public string CodigoSucursal { get; set; }
        public string CodigoTipo { get; set; }
        public string ComunaProveedor { get; set; }
        public string Descripcion { get; set; }
        public string Descuentos { get; set; }
        public string DireccionProveedor { get; set; }
        public string EspecificacionComprador { get; set; }
        public string EspecificacionProveedor { get; set; }
        public string EstadoProveedor { get; set; }
        public DateTime? FechaAceptacion { get; set; }
        public DateTime? FechaCancelacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public string Financiamiento { get; set; }
        public string FonoContacto { get; set; }
        public string FonoContactoProveedor { get; set; }
        public string FormaPago { get; set; }
        public string Impuestos { get; set; }
        public string ListadoCorrelativo { get; set; }
        public string MailContacto { get; set; }
        public string MailContactoProveedor { get; set; }
        public string Moneda { get; set; }
        public string Nombre { get; set; }
        public string NombreContacto { get; set; }
        public string NombreContactoProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string NombreSucursal { get; set; }
        public string PaisProveedor { get; set; }
        public decimal PorcentajeIva { get; set; }
        public decimal PrecioNeto { get; set; }
        public string Producto { get; set; }
        public string PromedioCalificacion { get; set; }
        public string ProveedorCodigo { get; set; }
        public string RegionProveedor { get; set; }
        public string RutSucursal { get; set; }
        public string TieneItems { get; set; }
        public string Tipo { get; set; }
        public string TipoMoneda { get; set; }
        public decimal Total { get; set; }
        public decimal Total2 { get; set; }
        public decimal TotalCargos { get; set; }
        public decimal TotalDescuentos { get; set; }
        public decimal TotalImpuestos { get; set; }
        public decimal TotalNeto { get; set; }
        public string Unidad { get; set; }


    }
}
