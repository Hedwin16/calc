using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Suite.ServicioSuite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TfhkaNet.IF;
using TfhkaNet.IF.VE;

namespace Suite
{
    public partial class xfrm_Archivo_Configuracion : XtraForm
    {
        #region Propiedades y Atributos

        ServicioSuiteClient svc = new ServicioSuiteClient("BasicHttpBinding_IServicioSuite", parametros.endpoint_address);

        private Tfhka Impresora = new Tfhka();
        private bool Respuesta;
        private bool Impconectada;
        private PrinterStatus StatusErrorIF;

        #endregion

        #region Métodos Adicionales

        private void BuscarConfiguracionSucursal()
        {
            try
            {
                configuracionBindingSource.DataSource = svc.Buscar_Configuracion(parametros.Sucursal.codigo);

                peFoto.Image = (Bitmap)((new ImageConverter()).ConvertFrom(((configuracion)configuracionBindingSource.Current).logo));

                //Obtiene el tipo de calculo de utilidad
                if (((configuracion)configuracionBindingSource.Current).costo_metodo_contribucion)
                {
                    rg_Calculo_Utilidad.SelectedIndex = 1;
                }
                else
                {
                    rg_Calculo_Utilidad.SelectedIndex = 0;
                }

                //Obtiene el almacen predeterminado de sistema
                //Verifica si la opcion de USAR ALMACEN PREDETERMINADO DE SISTEMA esta activa
                if (((configuracion)configuracionBindingSource.Current).almacen_venta_sistema)
                {
                    radioGroup1.SelectedIndex = almacenBindingSource.OfType<almacen>().ToList().FindIndex(x => x.codigo == ((configuracion)configuracionBindingSource.Current).almacen_venta);
                }

                //Verifica si la opcion de USAR PRECIO PREDETERMINADO DE SISTEMA esta activo
                if (((configuracion)configuracionBindingSource.Current).precio_venta_sistema)
                {
                    cbox_Precio_Activo.Text =  ((configuracion)configuracionBindingSource.Current).precio_venta;
                }

                //Verifica los precios activos de venta.

                string[] precios_activos = ((configuracion)configuracionBindingSource.Current).precio_venta_activo.Split(',');

                foreach (string precio in precios_activos)
                {
                    switch (precio)
                    {
                        case "1":
                            chk_Precio1.CheckState = CheckState.Checked;
                            break;
                        case "2":
                            chk_Precio2.CheckState = CheckState.Checked;
                            break;
                        case "3":
                            chk_Precio3.CheckState = CheckState.Checked;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
            }
        }
        private void BuscarMonedas()
        {
            try
            {
                monedaBindingSource.DataSource = svc.Lista_Monedas();
            }
            catch
            {
            }
        }
        private void BuscarAlmacenes()
        {
            try
            {
                almacenBindingSource.DataSource = svc.Lista_Almacenes(parametros.Sucursal.codigo, null, null, true, 0);

                foreach(almacen alm in almacenBindingSource)
                {
                    RadioGroupItem rgi = new RadioGroupItem(alm.codigo, alm.descripcion);

                    radioGroup1.Properties.Items.Add(rgi);
                }
            }
            catch
            {

            }
        }

        #endregion

        public xfrm_Archivo_Configuracion()
        {
            InitializeComponent();
        }

        private void xfrm_Archivo_Configuracion_Shown(object sender, EventArgs e)
        {
            BuscarMonedas();
            BuscarAlmacenes();

            BuscarConfiguracionSucursal();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] arr;
                ImageConverter converter = new ImageConverter();
                arr = (byte[])converter.ConvertTo(peFoto.Image, typeof(byte[]));

                ((configuracion)configuracionBindingSource.Current).logo = arr;

                //Verifica si la opcion de USAR ALMACEN PREDETERMINADO DE SISTEMA esta activa y asigna el codigo del almacen.
                if (((configuracion)configuracionBindingSource.Current).almacen_venta_sistema)
                {
                    ((configuracion)configuracionBindingSource.Current).almacen_venta = ((almacen)almacenBindingSource[radioGroup1.SelectedIndex]).codigo;
                }

                //Verifica si la opcion de USAR PRECIO PREDETERMINADO DE SISTEMA esta activo y asigna el codigo del precio.
                if (((configuracion)configuracionBindingSource.Current).precio_venta_sistema)
                {
                    ((configuracion)configuracionBindingSource.Current).precio_venta = cbox_Precio_Activo.Text;
                }

                //Verifica los precios activos de venta.

                string precios_activos = null;

                for (int i = 1; i < 4; i++)
                {
                    if(string.IsNullOrEmpty(precios_activos))
                    {
                        precios_activos = i.ToString();
                    }
                    else
                    {
                        precios_activos = precios_activos + "," + i.ToString();
                    }
                }

                ((configuracion)configuracionBindingSource.Current).precio_venta_activo = precios_activos;

                //Verifica modo de calculo de utilidad
                if (rg_Calculo_Utilidad.SelectedIndex == 0)
                {
                    ((configuracion)configuracionBindingSource.Current).costo_metodo_contribucion = false;
                }
                else
                {
                    ((configuracion)configuracionBindingSource.Current).costo_metodo_contribucion = true;

                }


                svc.Guardar_Configuracion((configuracion)configuracionBindingSource.Current, parametros.Sucursal.codigo, parametros.Usuario.codigo, utilidades.Estacion_GetName());
            }
            catch(Exception ex)
            {
                utilidades.MensajeError("Se produjo un error al guardar la configuración de la sucursal.", null, null, "Integra - Configuración de sucursal", "Error");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xfrm_Archivo_Configuracion_Load(object sender, EventArgs e)
        {
        }

        private void btnCambiarTasa_Click(object sender, EventArgs e)
        {
            xfrm_Cambio_Tasa frm = new xfrm_Cambio_Tasa();
            frm.ShowDialog();
        }

        private void peFoto_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog();

                dialog.Title = "Seleccionar logo de Sucursal";
                dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    peFoto.Image = Image.FromFile(dialog.FileName);
                }

                dialog.Dispose();
            }
            catch
            {
                MessageBox.Show("Se produjo un error obteniendo la imagen de la sucursal", "SUITE - Configuracion de Sucursal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
