using System;
using System.Text;
using Entidad.Interfaz.Models.TipoCompraModels;
using Entidad.Interfaz.Models.EstadoModels;

namespace Entidad.Interfaz.Models.EstadoCompraModels
{
    public class EstadoCompraModel
    {

        public int Id { get; set; }
        public int EstadoId { get; set; }
        public int TipoCompraId { get; set; }

        public TipoCompraModel TipoCompra { get; set; }
        public EstadoModel Estado { get; set; }
    }
}
