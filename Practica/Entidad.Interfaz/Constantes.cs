namespace Entidad.Interfaz
{
    public struct Acciones
    {
        public const string Ingresar = "INGRESAR";
        public const string Ver = "VER";
        public const string Aprobar = "APROBAR";
        public const string Editar = "EDITAR";
        public const string GenerarOC = "GENERAROC";

    }

    public struct Estados
    {
        public const int Creada = 1;
        public const int ProcesoAprobacion = 2;
        public const int Aprobada = 3;
        public const int RechazadaEnAprobacion = 4;
        public const int Asignada = 8;
        public const int GenerandoOC = 9;
        public const int Finalizada = 10;

    }

    public struct TipoMoneda
    {
        public const int PesoChileno = 5; // peso chileno
    }

    public struct ModalidadCompra
    {
        public const int Anual = 1;
        public const int MultiAnual = 2; // multianual

    }

    public struct TipoCompra
    {
        public const int TratoDirecto = 4;

    }

    public struct AprobacionConfig
    {
        public const int AprobacionGabinete = 6;
        public const int AnalistaCompra = 1012;
    }


    public class Roles
    {
        public const int AnalistaPresupuesto = 2;
        public const int AsignadorSolicitud = 4;
        public const int AnalistaCompra = 5;

    }




}
