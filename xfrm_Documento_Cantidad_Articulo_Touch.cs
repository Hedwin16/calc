using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Suite.ServicioSuite;
using System.ServiceModel;
using System.Globalization;

namespace Suite
{
    public partial class xfrm_Documento_Cantidad_Articulo_Touch : DevExpress.XtraEditors.XtraForm
    {
        #region Propiedades y Referencias
                ServicioSuiteClient svc = new ServicioSuiteClient("BasicHttpBinding_IServicioSuite", parametros.endpoint_address);//"BasicHttpBinding_IServicioSuite", "");
        public decimal cantidad { get; set; }
        public decimal cantidad_anterior { get; set; }
        public bool modo_decimal { get; set; }
        public decimal cantidad_actual_comprometida { get; set; }
        public string codigo_unidad_original { get; set; }
        public List<unidad> Lista_Unidades { get; set; }
        public string codigo_unidad { get; set; }
        public int unidad_seleccionada { get; set; }
        public string Codigo_Almacen { get; set; }
        public existencia Existencia { get; set; }
        public lote Lote { get; set; }
        public articulo Articulo { get; set; }
        public string tipo_operacion { get; set; }
        public bool Muestra_Lote { get; set; }
        

        #endregion

        #region Metodos adicionales

        private void Validar_Existencia()
        {
            try
            {
                existencia E_Temp = new existencia();

                decimal c = Convert.ToDecimal(txtCantidad.Text);

                if (codigo_unidad == Articulo.unidad_venta2)
                {
                    c = c * Articulo.cantidad_unidad2;

                    //Si es una factura de venta y ya hay alguna cantidad en la lista de articulos
                    //cantidad_actual_comprometida = cantidad_actual_comprometida * Articulo.cantidad_unidad2;
                }
                else if (codigo_unidad == Articulo.unidad_venta3)
                {
                    c = c * Articulo.cantidad_unidad3;

                    //Si es una factura de venta y ya hay alguna cantidad en la lista de articulos
                    //cantidad_actual_comprometida = cantidad_actual_comprometida * Articulo.cantidad_unidad3;
                }

                if(codigo_unidad_original != lupUnidades.EditValue.ToString())
                {
                    if (codigo_unidad_original == Articulo.unidad_venta2)
                    {
                        cantidad_anterior = cantidad_anterior * Articulo.cantidad_unidad2;

                        //Si es una factura de venta y ya hay alguna cantidad en la lista de articulos
                        //cantidad_actual_comprometida = cantidad_actual_comprometida * Articulo.cantidad_unidad2;
                    }
                    else if (codigo_unidad_original == Articulo.unidad_venta3)
                    {
                        cantidad_anterior = cantidad_anterior * Articulo.cantidad_unidad3;

                        //Si es una factura de venta y ya hay alguna cantidad en la lista de articulos
                        //cantidad_actual_comprometida = cantidad_actual_comprometida * Articulo.cantidad_unidad3;
                    }
                }

                if (!Articulo.lote)
                {
                    existenciaBindingSource.DataSource = svc.Buscar_Existencias(parametros.Sucursal.codigo, Articulo.id, Articulo.unidad_venta1, Codigo_Almacen, null, null, null, true).FirstOrDefault();

                    if (existenciaBindingSource == null)
                    {
                        utilidades.MensajeError("El artículo no posee stock en el almacén seleccionado.", null, null, "SUITE - Operaciones de Inventario", "Error");
                        cantidad = 0m;

                        return;
                    }

                    //Obtenemos la cantidad actual y le sumamos el comprometido actual que tenga el usuario
                    decimal actual = ((existencia)existenciaBindingSource.Current).actual;// + cantidad_actual_comprometida;
                    decimal comprometido = ((existencia)existenciaBindingSource.Current).comprometido;// - cantidad_anterior;

                    if (tipo_operacion != "CAR")// || tipo_operacion == "DES")
                    {
                        if ((actual - comprometido) < c)
                        {
                            utilidades.MensajeError("La cantidad solicitada excede la cantidad disponible en el almacén seleccionado.", null, null, "SUITE - Operaciones de Inventario", "Error");

                            cantidad = 0m;

                            return;
                        }
                    }

                    Existencia = new existencia
                    {
                        actual = actual,
                        comprometido = comprometido,
                        lote = null
                    };

                    this.Close();
                }
                else
                {
                    //Si el articulo trae un codigo de lote se verifica el lote
                    if (Lote != null || !string.IsNullOrEmpty(Articulo.numero_lote))
                    {
                        string nrolote = (Lote != null) ? Lote.codigo : Articulo.numero_lote;

                        existenciaBindingSource.DataSource = svc.Buscar_Existencias_Lotes(parametros.Sucursal.codigo, Articulo.id, Articulo.unidad_venta1, Codigo_Almacen, null, null, nrolote, 0, DateTime.MinValue, null).FirstOrDefault();

                        if (existenciaBindingSource == null)
                        {
                            utilidades.MensajeError("El artículo no posee stock en el almacén seleccionado.", null, null, "SUITE - Operaciones de Inventario", "Error");
                            cantidad = 0m;

                            return;
                        }

                        if (((existencia)existenciaBindingSource.Current).actual - ((existencia)existenciaBindingSource.Current).comprometido < c)
                        {
                            utilidades.MensajeError("La cantidad solicitada excede la cantidad disponible del lote en el almacén seleccionado.", null, null, "SUITE - Operaciones de Inventario", "Error");

                            cantidad = 0m;

                            return;
                        }

                        Existencia = new existencia
                        {
                            actual = ((existencia)existenciaBindingSource.Current).actual,
                            comprometido = ((existencia)existenciaBindingSource.Current).comprometido,
                            lote = ((existencia)existenciaBindingSource.Current).lote
                        };

                        this.Close();
                    }
                    else
                    {
                        decimal cant = 0m;
                        decimal actual = 0m;
                        decimal comprometido = 0m;

                        foreach (existencia item in svc.Buscar_Existencias_Lotes(parametros.Sucursal.codigo, Articulo.id, Articulo.unidad_venta1, Codigo_Almacen, null, null, null, 0, DateTime.MinValue, null).ToList())
                        {
                            cant = cant + item.actual - item.comprometido;
                            actual = actual + item.actual;
                            comprometido = comprometido + item.comprometido;
                        }

                        if ((actual - comprometido) < c)
                        {
                            utilidades.MensajeError("La cantidad solicitada excede la cantidad disponible en el almacén seleccionado.", null, null, "SUITE - Operaciones de Inventario", "Error");
                            cantidad = 0m;
                            return;
                        }

                        Existencia = new existencia
                        {
                            actual = actual,
                            comprometido = comprometido,
                            lote = null
                        };

                        this.Close();
                    }
                }
            }
            catch (FaultException ex)
            {
                utilidades.MensajeError("Se produjo una excepción validando la existencia del artículo.", null, null, "SUITE - Operaciones de Inventario", "Error");
            }
            catch (Exception ex)
            {
                utilidades.MensajeError("Se produjo una excepción validando la existencia del artículo.", null, null, "SUITE - Operaciones de Inventario", "Error");
            }
        }

        private void actualizar_cantidad(string numero)
        {
            try
            {
                //Verifica si el modo decimal ya está ingresado
                if (numero == "," && modo_decimal)
                {
                    numero = "";
                }

                if (numero == "," && !modo_decimal)
                {
                    numero = ",";
                    modo_decimal = true;
                }

                //numero = (modo_decimal) ? "" : ",";  

                txtCantidad.EditValue = txtCantidad.EditValue + numero;
                
            }
            catch(Exception ex) {
                string msj = "Exception: " + ex.Message;
                utilidades.MensajeError(msj, null, null, "Integra - Excepción ", "Advertencia");
            }
        }

        private bool Verificar_Lote_Existe(string codigo_lote)
        {
            try
            {
                foreach (lote item in loteBindingSource)
                {
                    if(item.codigo == codigo_lote)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Metodos de Controles 
        public xfrm_Documento_Cantidad_Articulo_Touch()
        {
            InitializeComponent();

            (new TabOrderManager(this)).SetTabOrder(TabOrderManager.TabScheme.DownFirst);
        }

        private void custom_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData.ToString())
            {
                case "F1":
                    break;
                case "F2":
                    break;
                case "F3":
                    break;
                case "F4":
                    break;
                case "F5":
                    break;
                case "F6":
                    break;
                case "F7":
                    break;
                case "F8":
                    break;
                case "F9":
                    break;
                case "F10":
                    break;
                case "F11":
                    break;
                case "F12":
                    break;

                case "Escape":

                    this.Close();

                    break;

                case "Delete":
                    break;
                case "Home":
                    break;
                case "End":
                    break;
                case "PageUp":
                    break;
                case "Next":
                    break;
                case "Up":

                    SendKeys.Send("+{TAB}");

                    break;
                case "Down":

                    SendKeys.Send("{TAB}");

                    break;

                case "Left":

                    SendKeys.Send("+{TAB}");

                    break;
                case "Right":

                    SendKeys.Send("{TAB}");

                    break;
                default:
                    break;
            }
        }


        private void lupUnidades_EditValueChanged(object sender, EventArgs e)
        {
            if(Existencia != null)
            {
            }
        }

        private void xfrm_Documento_Cantidad_Articulo_Touch_Load(object sender, EventArgs e)
        {
            try
            {
                cantidad = 0m;
                modo_decimal = false;

                Muestra_Lote = false;

                txtCantidad.EditValue = cantidad;

                codigo_unidad_original = codigo_unidad;

                unidadBindingSource.DataSource = Lista_Unidades;
                lupUnidades.ItemIndex = unidad_seleccionada;
            }
            catch (FaultException Ex)
            {
                cantidad = 0m;
                this.Close();
            }
            catch (Exception Ex)
            {
                utilidades.MensajeError(Ex.Message, null, null, "SUITE - Operaciones de Inventario", "Error");
                cantidad = 0m;
                this.Close();
            }
        }

        private void txtCantidad_Click(object sender, EventArgs e)
        {
            txtCantidad.SelectAll();
        }

        #endregion

        private void simpleButtonDel_Click(object sender, EventArgs e)
        {
            txtCantidad.EditValue = 0m;
            modo_decimal = false;
        }

        private void simpleButtonSeparator_Click(object sender, EventArgs e)
        {

            SimpleButton btn = sender as SimpleButton;
            actualizar_cantidad(btn.Text);

        }

        private void simpleButton_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            actualizar_cantidad(btn.Text);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                cantidad = Convert.ToDecimal(txtCantidad.EditValue);
                codigo_unidad = lupUnidades.EditValue.ToString();

                Validar_Existencia();
            }
            catch (FaultException ex)
            {
                //utilidades.MensajeError("Se produjo una excepción validando la existencia del artículo.", ex.Message, null, "Suite - Existencias", "Advertencia");
            }
            catch (Exception ex)
            {
                utilidades.MensajeError("Se produjo un error validando la existencia del artículo.", ex.Message, null, "Suite - Existencias", "Error");
            }
        }
    }
}