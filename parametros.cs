using Suite.ServicioSuite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfhkaNet.IF;
//using TfhkaNet.IF.DO;
using TfhkaNet.IF.VE;

namespace Suite
{
    public static class parametros
    {
        public static empresa Empresa { get; set; }
        public static sucursal Sucursal { get; set; }
        public static usuario Usuario { get; set; }
        public static vendedor Vendedor { get; set; }

        //Almacena la direccion del servicio wcf de la sucursal seleccionada
        public static string endpoint_address { get; set; }
        public static CultureInfo culture1 { get; set; }
        public static CultureInfo culture2 { get; set; }

        #region Propiedades para Impresora Fiscal

        public static bool UsaImpresoraFiscal { get; set; }
        public static string ModeloImpresoraFiscal { get; set; }
        public static string SerialImpresoraFiscal { get; set; }
        public static string PuertoImpresoraFiscal { get; set; }
        public static bool UsaTasaAmpliada { get; set; }

        #endregion

        #region Timers
        public static string TimerOperaciones { get; set; }
        public static string TimerMenuInfo { get; set; }

        #endregion

        public static List<sys_submodulos_opciones> Lista_Opciones_Usuario_M0 = new List<sys_submodulos_opciones>();
        public static List<sys_submodulos_opciones> Lista_Opciones_Usuario_M00 = new List<sys_submodulos_opciones>();
        public static List<sys_submodulos_opciones> Lista_Opciones_Usuario_M1 = new List<sys_submodulos_opciones>();
        public static List<sys_submodulos_opciones> Lista_Opciones_Usuario_M2 = new List<sys_submodulos_opciones>();
        public static List<sys_submodulos_opciones> Lista_Opciones_Usuario_M3 = new List<sys_submodulos_opciones>();
        public static List<sys_submodulos_opciones> Lista_Opciones_Usuario_M4 = new List<sys_submodulos_opciones>();
        public static List<sys_submodulos_opciones> Lista_Opciones_Usuario_M5 = new List<sys_submodulos_opciones>();
        public static List<sys_submodulos_opciones> Lista_Opciones_Usuario_M6 = new List<sys_submodulos_opciones>();
        public static List<sys_submodulos_opciones> Lista_Opciones_Usuario_M7 = new List<sys_submodulos_opciones>();
        //public static List<sys_rol_operacion> Roles_Usuario { get; set; }
        public static List<sys_modulo> sys_Modulos { get; set; }
        //public static List<sys_operacion> sys_Operacions { get; set; }
        public static configuracion Configuracion { get; set; }

       //Propiedad para guardar el logo de la sucursal
        public static Image logo { get; set; }

        //Información de la moneda principal
        public static moneda Moneda {get; set; }

        //Configuracion predeterminada de Ventas
        public static string precio_predeterminado_de_venta { get; set; }

        //Indica si imprime solicitud de reposicion de inventario
        public static bool ImprimeSolicitudReposicion { get; set; }

        //Indica si imprime solicitud de reposicion de inventario
        public static bool UsaPantallaTactil { get; set; }

        //Indica si imprime solicitud de reposicion de inventario
        public static bool UsaPantallaAuxiliar { get; set; }

        //Indica si imprime solicitud de reposicion de inventario
        public static string Palette { get; set; }

        //Documento temporal para monitor auxiliar
        public static documento_venta documento_venta_auxiliar { get; set; }

        //Lista para guardar las operaciones por autorizar
        public static List<operacion_inventario> Lista_Autorizaciones { get; set; }

        #region Impresora Fiscal

        private static ReportData Reporte;
        private static ReportData[] Reportes;
        private static PrinterStatus StatusError;
        private static S1PrinterData EstatusS1;
        private static S2PrinterData EstatusS2;
        private static S3PrinterData EstatusS3;
        private static S4PrinterData EstatusS4;
        private static S5PrinterData EstatusS5;
        private static S6PrinterData EstatusS6;

        #endregion


        #region Archivos

        public static List<banco> Lista_Bancos { get; set; }
        public static List<tipo_cobro> Lista_Tipos_Cobro { get; set; }
        public static List<moneda> Lista_Monedas { get; set; }
        public static List<impuesto> Lista_Impuestos { get; set; }
        public static List<capacidad> Lista_Capacidades { get; set; }
        public static List<color> Lista_Colores { get; set; }
        public static List<garantia> Lista_Garantias { get; set; }
        public static List<categoria> Lista_Categorias { get; set; }
        public static List<unidad> Lista_Unidades { get; set; }
        public static List<norma> Lista_Normas { get; set; }
        public static List<almacen> Lista_Almacenes { get; set; }
        public static List<fabricante> Lista_Fabricantes { get; set; }
        public static List<pais> Lista_Paises { get; set; }
        public static List<vendedor> Lista_Vendedores { get; set; }
        public static List<marca> Lista_Marcas { get; set; }
        public static List<usuario> Lista_Usuarios { get; set; }
        public static List<sucursal> Lista_Sucursales { get; set; }

        #endregion

    }
}
