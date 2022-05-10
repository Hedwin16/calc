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
namespace Suite
{
    public partial class xfrm_Documento_Cantidad_Articulo : DevExpress.XtraEditors.XtraForm
    {
        #region Propiedades y Referencias
        ServicioSuiteClient svc = new ServicioSuiteClient("BasicHttpBinding_IServicioSuite", parametros.endpoint_address);
        public decimal cantidad { get; set; }
        public decimal cantidad_anterior { get; set; }
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

        private void Buscar_Lotes()
        {
            try
            {
                foreach (lote item in svc.Lista_Lotes(parametros.Sucursal.codigo, null, Articulo.id, null, null, null))
                {
                    loteBindingSource.Add(item);
                }
            }
            catch(FaultException ex)
            {

            }
            catch (Exception ex)
            {
                utilidades.MensajeError("Se produjo un error al intentar obtener los lotes del artículo.", ex.Message, null, "SUITE - Lotes de artículo", "Error");
            }
        }

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
                        cantidad = 0;

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

                            cantidad = 0;

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
                            cantidad = 0;

                            return;
                        }

                        if (((existencia)existenciaBindingSource.Current).actual - ((existencia)existenciaBindingSource.Current).comprometido < c)
                        {
                            utilidades.MensajeError("La cantidad solicitada excede la cantidad disponible del lote en el almacén seleccionado.", null, null, "SUITE - Operaciones de Inventario", "Error");

                            cantidad = 0;

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
                        decimal cant = 0;
                        decimal actual = 0;
                        decimal comprometido = 0;

                        foreach (existencia item in svc.Buscar_Existencias_Lotes(parametros.Sucursal.codigo, Articulo.id, Articulo.unidad_venta1, Codigo_Almacen, null, null, null, 0, DateTime.MinValue, null).ToList())
                        {
                            cant = cant + item.actual - item.comprometido;
                            actual = actual + item.actual;
                            comprometido = comprometido + item.comprometido;
                        }

                        if ((actual - comprometido) < c)
                        {
                            utilidades.MensajeError("La cantidad solicitada excede la cantidad disponible en el almacén seleccionado.", null, null, "SUITE - Operaciones de Inventario", "Error");
                            cantidad = 0;
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

        private void Informacion_Existencia()
        {
            try
            {
                if (!Articulo.lote)
                {
                    try
                    {
                        existenciaBindingSource.DataSource = svc.Buscar_Existencias(parametros.Sucursal.codigo, Articulo.id, Articulo.unidad_venta1, Codigo_Almacen, null, null, null, true).FirstOrDefault();

                        //decimal actual = ((existencia)existenciaBindingSource.Current).actual;
                        //decimal comprometido = ((existencia)existenciaBindingSource.Current).comprometido;

                        Existencia = new existencia
                        {
                            actual = ((existencia)existenciaBindingSource.Current).actual,
                            comprometido = ((existencia)existenciaBindingSource.Current).comprometido,
                            lote = null
                        };

                        txtCantidadLote.EditValue = Existencia.actual - Existencia.comprometido;
                    }
                    catch(FaultException ex)
                    {
                        txtCantidadLote.EditValue = 0;
                        Existencia = null;
                    }
                    catch(Exception)
                    {
                        utilidades.MensajeError("Se produjo una excepción validando la existencia del artículo.", null, null, "SUITE - Operaciones de Inventario", "Error");
                    }
                }
                else
                {
                    //Si el articulo trae un codigo de lote se verifica el lote
                    if (Lote != null)
                    {

                        try
                        {
                            existenciaBindingSource.DataSource = svc.Buscar_Existencias_Lotes(parametros.Sucursal.codigo, Articulo.id, Articulo.unidad_venta1, Codigo_Almacen, null, null, Lote.codigo, 0, DateTime.MinValue, null).FirstOrDefault();

                            //decimal actual = ((existencia)existenciaBindingSource.Current).actual;
                            //decimal comprometido = ((existencia)existenciaBindingSource.Current).comprometido;

                            Existencia = new existencia
                            {
                                actual = ((existencia)existenciaBindingSource.Current).actual,
                                comprometido = ((existencia)existenciaBindingSource.Current).comprometido,
                                lote = Lote.codigo
                            };

                            txtCantidadLote.EditValue = Existencia.actual - Existencia.comprometido;
                            txtFechaVence.EditValue = ((existencia)existenciaBindingSource.Current).fecha_vence;
                        }
                        catch (FaultException ex)
                        {
                            txtCantidadLote.EditValue = 0;
                            Existencia = null;

                            if (tipo_operacion == "TRA")
                            {
                                utilidades.MensajeError(ex.Message, null, null, "SUITE - Operaciones de Inventario", "Error");
                            }
                        }
                        catch (Exception)
                        {
                            utilidades.MensajeError("Se produjo una excepción validando la existencia del artículo.", null, null, "SUITE - Operaciones de Inventario", "Error");
                        }
                    }
                    else
                    {

                        try
                        {
                            decimal cant = 0;
                            decimal actual = 0;
                            decimal comprometido = 0;

                            foreach (existencia item in svc.Buscar_Existencias(parametros.Sucursal.codigo, Articulo.id, Articulo.unidad_venta1, Codigo_Almacen, null, null, null, true).ToList())
                            {
                                cant = cant + item.actual - item.comprometido;
                                actual = actual + item.actual;
                                comprometido = comprometido + item.comprometido;
                            }

                            Existencia = new existencia
                            {
                                actual = actual,
                                comprometido = comprometido,
                                lote = null
                            };

                            txtCantidadLote.EditValue = cant;
                        }
                        catch (FaultException ex)
                        {
                            txtCantidadLote.EditValue = 0;
                            Existencia = null;
                        }
                        catch (Exception)
                        {
                            utilidades.MensajeError("Se produjo una excepción validando la existencia del artículo.", null, null, "SUITE - Operaciones de Inventario", "Error");
                        }
                    }
                }
            }
            catch(FaultException ex)
            {

            }
            catch (Exception ex)
            {
                utilidades.MensajeError("Se produjo una excepción validando la existencia del artículo.", null, null, "SUITE - Operaciones de Inventario", "Error");
            }
        }

        private void Validar_Maximo_Minimo()
        {

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
        public xfrm_Documento_Cantidad_Articulo()
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

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    cantidad = Convert.ToDecimal(txtCantidad.EditValue);
                    codigo_unidad = lupUnidades.EditValue.ToString();

                    switch (tipo_operacion)
                    {
                        case "TRA":

                            if (Articulo.lote)
                            {
                                if (lupLotes.ItemIndex < 0)
                                {
                                    utilidades.MensajeError("Debe especificar un lote para el artículo seleccionado.", null, null, "SUITE - Cantidad de Artículo", "Error");

                                    return;
                                }
                            }

                            Validar_Existencia();
                            break;

                        case "DES":

                            if (Articulo.lote)
                            {
                                if (lupLotes.ItemIndex < 0)
                                {
                                    utilidades.MensajeError("Debe especificar un lote para el artículo seleccionado.", null, null, "SUITE - Cantidad de Artículo", "Error");

                                    return;
                                }
                            }

                            Validar_Existencia();
                            break;
                        case "CAR":

                            if (Articulo.lote)
                            {
                                if (lupLotes.ItemIndex < 0)
                                {
                                    utilidades.MensajeError("Debe especificar un lote para el artículo seleccionado.", null, null, "SUITE - Cantidad de Artículo", "Error");

                                    return;
                                }
                            }

                            Informacion_Existencia();
                            this.Close();
                            break;
                        case "FACVTA":
                            Validar_Existencia();
                            break;
                        case "PEDVTA":
                            Validar_Existencia();
                            break;
                        case "DEVVTA":
                            Lote.cantidad = Convert.ToDecimal(txtCantidad.EditValue);
                            this.Close();
                            break;
                        case "AJU":

                            if (Articulo.lote)
                            {
                                if (lupLotes.ItemIndex < 0)
                                {
                                    utilidades.MensajeError("Debe especificar un lote para el artículo seleccionado.", null, null, "SUITE - Cantidad de Artículo", "Error");

                                    return;
                                }
                            }

                            cantidad = Convert.ToDecimal(txtCantidad.EditValue);
                            this.Close();
                            break;
                        default:

                            if (Articulo.lote)
                            {
                                if (lupLotes.ItemIndex < 0)
                                {
                                    utilidades.MensajeError("Debe especificar un lote para el artículo seleccionado.", null, null, "SUITE - Cantidad de Artículo", "Error");

                                    return;
                                }
                            }

                            cantidad = Convert.ToDecimal(txtCantidad.EditValue);
                            this.Close();
                            break;
                    }
                }
            }
            catch(FaultException ex)
            {
               //utilidades.MensajeError("Se produjo una excepción validando la existencia del artículo.", ex.Message, null, "Suite - Existencias", "Advertencia");
            }
            catch (Exception ex)
            {
                utilidades.MensajeError("Se produjo un error validando la existencia del artículo.", ex.Message, null, "Suite - Existencias", "Error");
            }
        }

        private void lupUnidades_EditValueChanged(object sender, EventArgs e)
        {
            if(Existencia != null)
            {
                if (lupUnidades.EditValue.ToString() == Articulo.unidad_venta1)
                {
                    txtCantidadLote.EditValue = Existencia.actual - Existencia.comprometido;
                }
                else if (lupUnidades.EditValue.ToString() == Articulo.unidad_venta2)
                {
                    txtCantidadLote.EditValue = (Existencia.actual - Existencia.comprometido) / Articulo.cantidad_unidad2;
                }
                else if (lupUnidades.EditValue.ToString() == Articulo.unidad_venta3)
                {
                    txtCantidadLote.EditValue = (Existencia.actual - Existencia.comprometido) / Articulo.cantidad_unidad3;
                }
            }
        }

        private void xfrm_Documento_Cantidad_Articulo_Load(object sender, EventArgs e)
        {
            try
            {
                if(!Muestra_Lote)
                {
                    //tableLayoutPanel1.RowStyles[1].Height = 0;

                    tableLayoutPanel1.RowStyles[1].Height = 0;
                    this.Size = new Size(this.Width, this.Height / 2 + 170);

                    txtCantidad.Select();
                    txtCantidad.Focus();

                    txtCantidad.EditValue = cantidad;
                    cantidad_anterior = cantidad;

                    codigo_unidad_original = codigo_unidad;

                    unidadBindingSource.DataSource = Lista_Unidades;
                    lupUnidades.ItemIndex = unidad_seleccionada;

                    meDescripcionArticulo.Text = Articulo.descripcion;

                    Informacion_Existencia();

                    return;
                }

                txtCantidad.EditValue = cantidad;
                cantidad_anterior = cantidad;

                codigo_unidad_original = codigo_unidad;

                unidadBindingSource.DataSource = Lista_Unidades;
                lupUnidades.ItemIndex = unidad_seleccionada;

                Buscar_Lotes();
                lupLotes.Select();
                lupLotes.Focus();
                meDescripcionArticulo.Text = Articulo.descripcion;
            }
            catch (FaultException Ex)
            {
                //utilidades.MensajeError(Ex.Message, null, null, "SUITE - Operaciones de Inventario", "Error");
                cantidad = 0;
                this.Close();
            }
            catch (Exception Ex)
            {
                utilidades.MensajeError(Ex.Message, null, null, "SUITE - Operaciones de Inventario", "Error");
                cantidad = 0;
                this.Close();
            }
        }

        private void lupLotes_EditValueChanged(object sender, EventArgs e)
        {
            if (lupLotes.ItemIndex > -1)
            {
                Lote = (lote)loteBindingSource[lupLotes.ItemIndex];

                Informacion_Existencia();

                lupUnidades.Focus();
                lupUnidades.Select();
            }
            else
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void btnAgregarLote_Click(object sender, EventArgs e)
        {
            try
            {
                xfrm_Documento_Compra_Lote_Info frm = new xfrm_Documento_Compra_Lote_Info();
                frm.articulo = Articulo.id;
                frm.Articulo = Articulo;
                frm.ShowDialog();

                if (string.IsNullOrEmpty(frm.Lote.codigo))
                {
                    utilidades.MensajeError("La información del lote esta incompleta.", null, null, "SUITE - Lotes de artículos", "Información");

                    return;
                }

                if(!Verificar_Lote_Existe(frm.Lote.codigo))
                {
                    utilidades.MensajeError("El lote ya se encuentra registrado.", null, null, "SUITE - Lotes de artículos", "Información");

                    return;
                }

                lote Lote = new lote
                {
                    codigo = frm.Lote.codigo,
                    articulo = frm.Lote.articulo,
                    fecha_lote = frm.Lote.fecha_lote,
                    fecha_vence = frm.Lote.fecha_vence
                };

                loteBindingSource.Add(Lote);

                existencia Nueva_Existencia = new existencia
                {
                    id_articulo = Articulo.id,
                    almacen = Codigo_Almacen,
                    unidad = Articulo.unidad_venta1,
                    articulo1 = Articulo.codigo,
                    lote = frm.Lote.codigo,
                    actual = frm.Lote.cantidad,
                    fecha_lote = frm.Lote.fecha_lote,
                    fecha_vence = frm.Lote.fecha_vence
                };

                existenciaBindingSource.Add(Nueva_Existencia);

                lupLotes.ItemIndex = loteBindingSource.Count - 1;

                txtCantidad.EditValue = Nueva_Existencia.actual;

                txtCantidad.Focus();

            }
            catch (FaultException FEx)
            {
                utilidades.MensajeError("Se produjo un error al guardar la información del lote.", FEx.Message, null, "Suite - Información del Lote", "Información");
            }
            catch (Exception Ex)
            {
                utilidades.MensajeError("Se produjo un error al guardar la información del lote.", Ex.Message, null, "Suite - Información del Lote", "Información");
            }
        }

        private void txtCantidad_Click(object sender, EventArgs e)
        {
            txtCantidad.SelectAll();
        }

        #endregion
    }
}