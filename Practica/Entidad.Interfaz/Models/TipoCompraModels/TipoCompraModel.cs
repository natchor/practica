using System;


namespace Entidad.Interfaz.Models.TipoCompraModels
{
    public class TipoCompraModel
    {


        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Contrato { get; set; } //indica si el tipo de compra tiene o no contrato
        public bool Estado { get; set; }


    }
}

