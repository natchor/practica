using System;


namespace Entidad.Interfaz.Models.EstadoModels
{
    public class EstadoModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public bool Estado { get; set; }

        public bool PermiteGenerarOC { get; set; }

        public string CodigoStr { get; set; }


    }


}

