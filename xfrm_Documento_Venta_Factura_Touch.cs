using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Suite.ServicioSuite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using TfhkaNet.IF;
using TfhkaNet.IF.VE;

namespace Suite
{
    public partial class xfrm_Documento_Venta_Factura_Touch : XtraForm
    {
        #region Atributos y Propiedades

        private Tfhka Impresora = new Tfhka();
        private bool Respuesta;
        private bool Impconectada;

        private string unidades_autorizadas = parametros.Lista_Opciones_Usuario_M3.Where(xx => xx.codigo == "FACCAMUNI").FirstOrDefault().valor2;

        documento_venta documento;

        //Clase para compartir informacion con el formulario auxiliar
        public documento_venta documento_auxiliar { get; set; }
        public List<documento_venta_item> documento_auxiliar_items { get; set; }

        decimal peso_balanza = 0;

        int rowHandle;

        GridColumn column;

        BindingSource existenciaBindingSource = new BindingSource();

        ServicioSuiteClient svc = new ServicioSuiteClient("BasicHttpBinding_IServicioSuite", parametros.endpoint_address);

        articulo Articulo { get; set; }

        string moneda_para_mostrar { get; set; }

        //Propiedad para guardar las opciones de precios

        private string precio { get; set; }

        private string precio_autorizados { get; set; }

        //sys_rol_operacion cambia_precio = new sys_rol_operacion();

        //sys_rol_operacion precio_venta = new sys_rol_operacion();

        string unidad_nueva { get; set; } //Solo para uso en rilupUnidad Value Changing

        string unidad_anterior { get; set; }

        decimal cantidad_anterior { get; set; }

        //Propiedad para guardar las opciones de almacen
        private almacen Almacen { get; set; }
        
        private string almacenes_autorizados { get; set; }

        //sys_rol_operacion cambia_almacen = new sys_rol_operacion();

        bool suspendCellValueChanged = false;

        //Propiedad para guardar la configuración de monedas


        List<unidad> Lista_Unidades = new List<unidad>();

        #endregion

        #region Metodos Adicionales
        private void abrir_puerto_com()
        {
            try
            {
                Respuesta = Impresora.OpenFpCtrl(parametros.PuertoImpresoraFiscal);       //Abre el puerto seleccionado en el cmbPuerto <GB>
                if (Respuesta)
                {
                    Respuesta = Impresora.CheckFPrinter();              //Comprueba estado de la impresora <GB>
                    if (Respuesta)
                    {
                        Impconectada = Respuesta;
                        //MessageBox.Show("PUERTO ABIERTO", "The Factory HKA - Venezuela");          //<GB>
                    }
                    else
                    {

                        //MessageBox.Show("Error", "The Factory HKA - Venezuela"); 
                    }
                }
                else
                {
                    //MessageBox.Show("Error", "The Factory HKA - Venezuela");
                }
            }//try
            catch (InvalidOperationException)
            {
                //MessageBox.Show("Conecte impresora", "The Factory HKA - Venezuela");
            } //catch
            catch (ArgumentNullException)
            {
                //MessageBox.Show("Conecte impresora", "The Factory HKA - Venezuela");
            } //catch
            catch (Exception)
            {

                //MessageBox.Show("Conecte impresora", "The Factory HKA - Venezuela");
            } //catch
        }

        private void cerrar_puerto_com()
        {
            try
            {
                Impresora.CloseFpCtrl();
            }
            catch
            {

            }
            
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
                ms.Write(byteArrayIn, 0, byteArrayIn.Length);
                Image returnImage = Image.FromStream(ms, true);//Exception occurs here
                return returnImage;
            }
            catch { return null; }

        }

        private void actualizar_cantidad()
        {
            if (documento_venta_itemBindingSource.Count == 0)
            {
                utilidades.MensajeError("No hay artículos en la lista.", null, null, "SUITE - Factura de venta", "Advertencia");
                return;
            }
            try
            {
                unidad_anterior = gvArticulos.GetFocusedRowCellValue("unidad").ToString();

                //Busca la información del Articulo
                buscar_articulo_por_id(((documento_venta_item)documento_venta_itemBindingSource.Current).id_articulo, 0, null, null);

                var xxx = gvArticulos.GetFocusedRowCellValue("unidad").ToString();
                var zzz = gvArticulos.GetFocusedRowCellValue("id_articulo").ToString();

                #region Creamos una lista de las unidades del articulo.

                List<unidad> lista_unidades_temp = new List<unidad>();

                foreach (unidad item in Lista_Unidades.Where(x => x.id_articulo == Convert.ToInt32(gvArticulos.GetFocusedRowCellValue("id_articulo").ToString())).ToList())
                {
                    unidad U = new unidad
                    {
                        codigo = item.codigo,
                        descripcion = item.descripcion,
                        activo = item.activo,
                        id_articulo = item.id_articulo
                    };

                    lista_unidades_temp.Add(U);
                }

                #endregion

                xfrm_Documento_Cantidad_Articulo_Touch frmCantidad = new xfrm_Documento_Cantidad_Articulo_Touch();
                frmCantidad.cantidad = Convert.ToDecimal(gvArticulos.GetFocusedRowCellValue("cantidad").ToString());
                frmCantidad.Lista_Unidades = Lista_Unidades.Where(x => x.id_articulo == Convert.ToInt32(gvArticulos.GetFocusedRowCellValue("id_articulo").ToString())).ToList();
                frmCantidad.unidad_seleccionada = lista_unidades_temp.FindIndex(x => x.codigo == gvArticulos.GetFocusedRowCellValue("unidad").ToString()); // rilupUnidad.GetDataSourceRowIndex("codigo", gvArticulos.GetFocusedRowCellValue("unidad"));
                frmCantidad.Codigo_Almacen = ((documento_venta)documento_ventaBindingSource.Current).codigo_almacen; frmCantidad.codigo_unidad = gvArticulos.GetFocusedRowCellValue("unidad").ToString();
                frmCantidad.Articulo = Articulo;
                frmCantidad.tipo_operacion = "FACVTA";
                frmCantidad.cantidad_actual_comprometida = ((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad;

                frmCantidad.Muestra_Lote = false;

                frmCantidad.ShowDialog();

                if (frmCantidad.cantidad <= 0)
                {
                    return;
                }

                ((documento_venta_item)documento_venta_itemBindingSource.Current).unidad = frmCantidad.codigo_unidad;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad = frmCantidad.cantidad;

                calcular_total_item();
            }
            catch (FaultException FEx)
            {
                utilidades.MensajeError("Se produjo un error al actualizar la cantidad del artículo.", FEx.Message, null, "Suite - Factura de Venta", "Error");
            }
            catch (Exception Ex)
            {
                utilidades.MensajeError("Se produjo un error al actualizar la cantidad del artículo.", Ex.Message, null, "Suite - Factura de Venta", "Error");
            }
        }

        private void agregar_articulo()
        {
            int index = -1;

            try
            {
                #region Modificar artículo existente

                if (!Articulo.balanza)
                {
                    if (parametros.Configuracion.agrupar_articulo_documentos)
                    {
                        if (Articulo.lote)
                        {
                            index = verificar_articulo_en_grid(Articulo.id, Articulo.unidad, ((documento_venta)documento_ventaBindingSource.Current).codigo_almacen);
                        }
                        else
                        {
                            index = verificar_articulo_en_grid(Articulo.id, Articulo.unidad, ((documento_venta)documento_ventaBindingSource.Current).codigo_almacen);
                        }

                        if (index >= 0)
                        {
                            ((documento_venta_item)documento_venta_itemBindingSource[index]).cantidad = (peso_balanza > 0) ? ((documento_venta_item)documento_venta_itemBindingSource[index]).cantidad + peso_balanza : ((documento_venta_item)documento_venta_itemBindingSource[index]).cantidad + 1;

                            documento_venta_itemBindingSource.Position = index;
                            calcular_total_item();

                            //Limpiar
                            unidadBindingSource.Clear();
                            txtCodigoArticulo.Focus();

                            gvArticulos.FocusedRowHandle = index;

                            return;
                        }
                    }
                }

                #endregion

                guardar_unidades_articulo();

                #region Agregar un nuevo artículo a la lista

                try
                {
                    Image newImage = byteArrayToImage(Articulo.imagen);
                }
                catch
                { }

                documento_venta_item docitem = new documento_venta_item
                {
                    tipo_documento = "FAC",
                    almacen = Almacen.codigo,
                    id_articulo = Articulo.id,
                    articulo = Articulo.codigo,
                    imagen = Articulo.imagen,
                    cantidad = (peso_balanza > 0) ? peso_balanza : 1,
                    unidad = Articulo.unidad,
                    descripcion_unidad = Lista_Unidades.Where(x => x.id_articulo == Articulo.id && x.codigo == Articulo.unidad).FirstOrDefault().descripcion,
                    precio_venta = precio,
                    descuento = 0,
                    detalle_articulo = string.Empty,
                    descripcion_articulo = Articulo.descripcion,
                    fecha_vence = Convert.ToDateTime("1900-01-01")
                };

                documento_venta_itemBindingSource.Add(docitem);

                if (!Articulo.balanza)
                {
                    //Obtenemos la posicion del articulo que se agrega
                    index = verificar_articulo_en_grid(Articulo.id, Articulo.unidad, ((documento_venta_item)documento_venta_itemBindingSource.Current).almacen);
                    documento_venta_itemBindingSource.Position = index;
                }
                else
                {
                    documento_venta_itemBindingSource.MoveLast();
                }

                calcular_total_item();

                #endregion

                //Limpiar
                unidadBindingSource.Clear();
                txtCodigoArticulo.Focus();

                gvArticulos.FocusedRowHandle = index;

                peso_balanza = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error agregando el artículo al documento.", "SUITE - Factura", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void agregar_cliente()
        {
            try
            {
                xfrm_Documento_Cliente formularioCliente = new xfrm_Documento_Cliente();
                formularioCliente.ShowDialog();

                if (formularioCliente.Cliente != null)
                {
                    clienteBindingSource.DataSource = formularioCliente.Cliente;

                    ((documento_venta)documento_ventaBindingSource.Current).codigo_cliente = ((cliente)clienteBindingSource.Current).codigo;
                    ((documento_venta)documento_ventaBindingSource.Current).numero_identificacion = ((cliente)clienteBindingSource.Current).numero_identificacion;
                }
            }
            catch (FaultException FEx)
            {
                utilidades.MensajeError("Se produjo un error al importar la información del cliente", FEx.Message, FEx.InnerException.Message, "Suite - Facturación", "Error");
            }
            catch (Exception Ex)
            {
                utilidades.MensajeError("Se produjo un error al importar la información del cliente", Ex.Message, Ex.InnerException.Message, "Suite - Facturación", "Error");
            }
        }

        private bool buscar_articulo_por_id(int id, decimal cantidad_venta, string almacen_venta, string unidad_venta)
        {
            try
            {
                //Buscamos buscamos la información del artículo
                Articulo = null;

                Articulo = svc.Buscar_ID_Articulo(parametros.Sucursal.codigo, id, "venta");

                if (Articulo != null)
                {
                    //Si se especifica una unidad se modifica el valor
                    if (!string.IsNullOrEmpty(unidad_venta))
                    {
                        Articulo.unidad = unidad_venta;
                    }

                    //Valida que el usuario tenga permiso para utilizar la unidad del articulo seleccionada
                    if (!validar_acceso_unidad(Articulo.unidad))
                    {
                        return false;
                    }

                    if (!validar_existencia(Articulo, cantidad_venta, almacen_venta, unidad_venta))
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    MessageBox.Show("El código del artículo no existe o no está disponible.", "Suite - Factura de Venta", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    txtCodigoArticulo.Text = string.Empty;

                    return false;
                }
            }
            catch (FaultException Ex)
            {
                MessageBox.Show("Se produjo un error al buscar el codigo indicado." + Environment.NewLine + Environment.NewLine + Ex.Message, "Suite - Factura de Venta", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtCodigoArticulo.Text = string.Empty;

                return false;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Se produjo un error al buscar el codigo indicado.", "Suite - Factura de Venta", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtCodigoArticulo.Text = string.Empty;

                return false;
            }
        }

        private bool buscar_articulo_por_codigo(string filtro, string tipo_filtro)
        {
            try
            {
                //Buscamos buscamos la información del artículo

                Articulo = null;

                try
                {
                    List<articulo> Lista_Articulos = new List<articulo>();

                    try
                    {
                        Lista_Articulos = svc.Lista_Articulos(parametros.Sucursal.codigo, "CÓDIGO", filtro, null, null, null, null, null, null, false, true, 0, false).ToList();
                    }
                    catch (Exception ex)
                    { }

                    if (Lista_Articulos.Count > 0)
                    {
                        if (Lista_Articulos.Count == 1)
                        {
                            Articulo = svc.Buscar_Articulo(parametros.Sucursal.codigo, filtro, true, parametros.Usuario.codigo, null, "venta", "factura");
                        }
                        else
                        {
                            xfrm_Lista_Articulos_Similares frm = new xfrm_Lista_Articulos_Similares();
                            frm.Lista_Articulos = Lista_Articulos;
                            frm.ShowDialog();
                            Articulo = svc.Buscar_Articulo(parametros.Sucursal.codigo, frm.codigo_articulo, true, parametros.Usuario.codigo, null, "venta", "factura");
                        }
                    }
                    else
                    {
                        //Segmenta el código para validar si se trata de un artículo
                        //que usa balanza.

                        string codigo_balanza = filtro.Substring(parametros.Configuracion.balanza_digito_inicial, parametros.Configuracion.balanza_digitos_codigo);

                        //Busca el artículo según el código extraído anteriormente

                        Articulo = svc.Buscar_Articulo(parametros.Sucursal.codigo, codigo_balanza, true, parametros.Usuario.codigo, null, "venta", "factura");

                        //Si el producto usa balanza extrae el peso de la cadena

                        if (Articulo.balanza)
                        {
                            if (Articulo.decimales)
                            {
                                peso_balanza = Convert.ToDecimal((filtro.Substring(parametros.Configuracion.balanza_digito_inicial + parametros.Configuracion.balanza_digitos_codigo, parametros.Configuracion.balanza_digitos_medida) + "," + filtro.Substring(parametros.Configuracion.balanza_digito_inicial + parametros.Configuracion.balanza_digitos_codigo + parametros.Configuracion.balanza_digitos_medida, parametros.Configuracion.balanza_digitos_decimal)));
                            }
                            else
                            {
                                peso_balanza = Convert.ToDecimal((filtro.Substring(parametros.Configuracion.balanza_digito_inicial + parametros.Configuracion.balanza_digitos_codigo, parametros.Configuracion.balanza_digitos_medida) + filtro.Substring(parametros.Configuracion.balanza_digito_inicial + parametros.Configuracion.balanza_digitos_codigo + parametros.Configuracion.balanza_digitos_medida, parametros.Configuracion.balanza_digitos_decimal)));
                            }

                        }
                        else
                        {
                            peso_balanza = 0;
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                if (Articulo != null)
                {
                    //Valida que el usuario tenga permiso para utilizar la unidad del articulo seleccionada

                    if (!validar_acceso_unidad(Articulo.unidad))
                    {
                        return false;
                    }

                    if (!validar_existencia(Articulo, 0, null, null))
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    MessageBox.Show("El código del artículo no existe o no está disponible.", "Suite - Factura de Venta", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    txtCodigoArticulo.Text = string.Empty;

                    Articulo = null;

                    return false;
                }
            }
            catch (FaultException Ex)
            {
                MessageBox.Show("Se produjo un error al buscar el codigo indicado." + Environment.NewLine + Environment.NewLine + Ex.Message, "Suite - Factura de Venta", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtCodigoArticulo.Text = string.Empty;

                Articulo = null;

                return false;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Se produjo un error al buscar el codigo indicado.", "Suite - Factura de Venta", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtCodigoArticulo.Text = string.Empty;

                return false;
            }
        }

        private void buscar_impresora_fiscal()
        {
            try
            {
                string puerto = null;

                foreach (string COM in SerialPort.GetPortNames())
                {
                    utilidades.BarraProgreso(this, true, "Buscando impresora fiscal en el puerto: " + COM);

                    puerto = utilidades.buscar_impresora(COM, parametros.SerialImpresoraFiscal);

                    if (!string.IsNullOrEmpty(puerto))
                    {
                        utilidades.BarraProgreso(this, false, "Buscando impresora fiscal en el puerto: " + COM);
                        utilidades.MensajeError("La impresora fiscal con el número de registro indicado se encontró en el puerto: " + COM, null, null, "Integra - Configuración de impresora fiscal", "Información");

                        parametros.PuertoImpresoraFiscal = puerto;

                        guardar_configuracion_fiscal();

                        break;
                    }

                    utilidades.BarraProgreso(this, false, "Buscando impresora fiscal en el puerto: " + COM);
                }

                if (string.IsNullOrEmpty(puerto))
                {
                    utilidades.BarraProgreso(this, false, "Buscando impresora fiscal en el puerto");

                    utilidades.MensajeError("No se encontró ninguna impresora fiscal con el serial indicado.", null, null, "Integra - Configuración de impresora fiscal", "Advertencia");

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                utilidades.BarraProgreso(this, false, "Buscando impresora fiscal en el puerto");
                utilidades.MensajeError("Se produjo un error al buscar la impresora fiscal.", null, null, "Integra - Configuración de impresora fiscal", "Advertencia");

                this.Close();
            }
        }

        private bool buscar_articulo_por_descripcion(string filtro, string tipo_filtro)
        {
            try
            {
                //Buscamos buscamos la información del artículo
                Articulo = null;

                if (!string.IsNullOrEmpty(filtro))
                {
                    xfrm_Lista_Articulos_Venta formulario = new xfrm_Lista_Articulos_Venta();
                    formulario.tipo_filtro = tipo_filtro;
                    formulario.ShowDialog();

                    if (formulario.Articulo == null)
                    {
                        return false;
                    }

                    try
                    {
                        if (buscar_articulo_por_id(formulario.Articulo.id, 0, null, null))
                        {
                            agregar_articulo();
                        }

                        txtCodigoArticulo.Text = string.Empty;

                        return true;
                    }
                    catch (FaultException Ex)
                    {
                        MessageBox.Show(Ex.Message, "Suite - Archivo de Artículos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        txtCodigoArticulo.Text = string.Empty;

                        txtCodigoArticulo.Focus();

                        return false;
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("Se produjo un error al validar el código del artículo." + Environment.NewLine + Environment.NewLine + Ex.Message, "Suite - Archivo de Artículos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        txtCodigoArticulo.Text = string.Empty;

                        txtCodigoArticulo.Focus();

                        return false;
                    }
                }
                else
                {
                    lista_articulos();

                    return true;
                }
            }
            catch (FaultException Ex)
            {
                MessageBox.Show("Se produjo un error al buscar el codigo indicado." + Environment.NewLine + Environment.NewLine + Ex.Message, "Suite - Factura de Venta", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtCodigoArticulo.Text = string.Empty;

                return false;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Se produjo un error al buscar el codigo indicado.", "Suite - Factura de Venta", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtCodigoArticulo.Text = string.Empty;

                return false;
            }
        }

        private void calcular_total_item()
        {
            try
            {
                suspendCellValueChanged = true;

                documento_venta_item dci = svc.Calcular_Total_Item_Venta(parametros.Sucursal.codigo, ((documento_venta_item)documento_venta_itemBindingSource.Current), false);

                ((documento_venta_item)documento_venta_itemBindingSource.Current).precio_unitario_moneda2 = dci.precio_unitario_moneda2;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).base_imponible_moneda2 = dci.base_imponible_moneda2;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).exento_moneda2 = dci.exento_moneda2;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).impuesto_moneda2 = dci.impuesto_moneda2;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).descuento_moneda2 = dci.descuento_moneda2;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).total_moneda2 = dci.total_moneda2;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).total_neto_moneda2 = dci.total_neto_moneda2;

                ((documento_venta_item)documento_venta_itemBindingSource.Current).base_imponible = dci.base_imponible;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).exento = dci.exento;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).impuesto = dci.impuesto;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).total = dci.total;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).total_neto = dci.total_neto;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).descuento = dci.descuento;
                ((documento_venta_item)documento_venta_itemBindingSource.Current).precio_unitario = dci.precio_unitario;

                calcular_totales();

                suspendCellValueChanged = false;
            }
            catch (Exception Ex)
            {
            }
        }

        private void calcular_total_item_todos()
        {
            try
            {
                foreach (documento_venta_item item in documento_venta_itemBindingSource)
                {
                    suspendCellValueChanged = true;

                    documento_venta_item dci = svc.Calcular_Total_Item_Venta(parametros.Sucursal.codigo, item, false);

                    item.precio_unitario_moneda2 = dci.precio_unitario_moneda2;
                    item.base_imponible_moneda2 = dci.base_imponible_moneda2;
                    item.exento_moneda2 = dci.exento_moneda2;
                    item.impuesto_moneda2 = dci.impuesto_moneda2;
                    item.descuento_moneda2 = dci.descuento_moneda2;
                    item.total_moneda2 = dci.total_moneda2;
                    item.total_neto_moneda2 = dci.total_neto_moneda2;

                    item.base_imponible = dci.base_imponible;
                    item.exento = dci.exento;
                    item.impuesto = dci.impuesto;
                    item.total = dci.total;
                    item.total_neto = dci.total_neto;
                    item.descuento = dci.descuento;
                    item.precio_unitario = dci.precio_unitario;

                }
                calcular_totales();

                suspendCellValueChanged = false;
            }
            catch (Exception Ex)
            {
            }
        }

        private void calcular_totales()
        {
            try
            {

                decimal cantidadarticulos = 0;

                if (documento_venta_itemBindingSource.Count > 0)
                {
                    List<documento_venta_item> listaItems = new List<documento_venta_item>();

                    foreach (documento_venta_item item in documento_venta_itemBindingSource)
                    {
                        cantidadarticulos = cantidadarticulos + item.cantidad;

                        listaItems.Add(item);
                    }

                    var totales = svc.Calcular_Total_Documento_Venta(parametros.Sucursal.codigo, listaItems.ToArray());

                    ((documento_venta)documento_ventaBindingSource.Current).total_neto = totales.total_neto;
                    ((documento_venta)documento_ventaBindingSource.Current).total = totales.total;
                    ((documento_venta)documento_ventaBindingSource.Current).total_descuento = totales.total_descuento;
                    ((documento_venta)documento_ventaBindingSource.Current).base_imponible = totales.base_imponible;
                    ((documento_venta)documento_ventaBindingSource.Current).total_impuesto = totales.total_impuesto;
                    ((documento_venta)documento_ventaBindingSource.Current).total_exento = totales.total_exento;


                    ((documento_venta)documento_ventaBindingSource.Current).total_neto_moneda2 = totales.total_neto_moneda2;
                    ((documento_venta)documento_ventaBindingSource.Current).total_moneda2 = totales.total_moneda2;
                    ((documento_venta)documento_ventaBindingSource.Current).total_descuento_moneda2 = totales.total_descuento_moneda2;
                    ((documento_venta)documento_ventaBindingSource.Current).base_imponible_moneda2 = totales.base_imponible_moneda2;
                    ((documento_venta)documento_ventaBindingSource.Current).total_impuesto_moneda2 = totales.total_impuesto_moneda2;
                    ((documento_venta)documento_ventaBindingSource.Current).total_exento_moneda2 = totales.total_exento_moneda2;

                }
                else
                {
                    ((documento_venta)documento_ventaBindingSource.Current).total_neto = 0;
                    ((documento_venta)documento_ventaBindingSource.Current).total = 0;
                    ((documento_venta)documento_ventaBindingSource.Current).total_descuento = 0;
                    ((documento_venta)documento_ventaBindingSource.Current).base_imponible = 0;
                    ((documento_venta)documento_ventaBindingSource.Current).total_impuesto = 0;
                    ((documento_venta)documento_ventaBindingSource.Current).total_exento = 0;


                    ((documento_venta)documento_ventaBindingSource.Current).total_neto_moneda2 = 0;
                    ((documento_venta)documento_ventaBindingSource.Current).total_moneda2 = 0;
                    ((documento_venta)documento_ventaBindingSource.Current).total_descuento_moneda2 = 0;
                    ((documento_venta)documento_ventaBindingSource.Current).base_imponible_moneda2 = 0;
                    ((documento_venta)documento_ventaBindingSource.Current).total_impuesto_moneda2 = 0;
                    ((documento_venta)documento_ventaBindingSource.Current).total_exento_moneda2 = 0;
                }

                txtCantidadArticulos.EditValue = cantidadarticulos;
            }
            catch
            {
            }
        }

        private void cambiar_cantidad(bool mas)
        {
            try
            {
                if (mas)
                {
                    if (!validar_cantidad())
                    {
                        return;
                    }

                    ((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad = ((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad + 1;

                    calcular_total_item();
                }
                else
                {
                    if (Convert.ToDecimal(((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad) - 1 <= 0)
                    {
                        utilidades.MensajeError("La cantidad del artículo no puede ser 0.", null, null, "SUITE - Factura de venta", "Error");

                        return;
                    }
                    ((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad = ((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad - 1;

                    calcular_total_item();
                }
            }
            catch(Exception ex)
            { }
        }

        private void cambiar_configuracion_regional(string moneda_primaria, string moneda_secundaria)
        {
            CultureInfo culture;

            culture = CultureInfo.CreateSpecificCulture(moneda_primaria);
            culture.NumberFormat.CurrencyPositivePattern = 2;

            txtTotal.Properties.Mask.Culture = culture;

            culture = CultureInfo.CreateSpecificCulture(moneda_secundaria);
            culture.NumberFormat.CurrencyPositivePattern = 2;

            txtTotalMoneda2.Properties.Mask.Culture = culture;
        }

        private void cambio_almacen()
        {
            try
            {
                sys_submodulos_opciones cambia_almacen = parametros.Lista_Opciones_Usuario_M3.Where(x => x.codigo == "FACAUTCAMALM").FirstOrDefault();

                if (documento_venta_itemBindingSource.Count == 0)
                {
                    utilidades.MensajeError("No hay artículos en la lista.", null, null, "SUITE - Factura de venta", "Advertencia");
                    return;
                }

                xfrm_Documento_Almacenes formulario = new xfrm_Documento_Almacenes();

                switch (cambia_almacen.valor1)
                {
                    case "1":

                        formulario.almacenes = almacenes_autorizados;
                        formulario.ShowDialog();

                        if (formulario.Almacen != null)
                        {
                            //Busca la información del Articulo
                            if (!buscar_articulo_por_id(((documento_venta_item)documento_venta_itemBindingSource.Current).id_articulo, 0, null, null))
                            {
                                return;
                            }

                            ((documento_venta_item)documento_venta_itemBindingSource.Current).almacen = formulario.Almacen.codigo;
                        }

                        break;

                    case "2":

                        MessageBox.Show("La configuración del perfil del usuario no permite cambiar el almacén en éste módulo.");

                        break;

                    case "3":

                        formulario.almacenes = null;
                        if (!string.IsNullOrEmpty(txtCodigoArticulo.Text)) { formulario.Articulo = Articulo; }
                        formulario.ShowDialog();

                        if (formulario.Almacen != null)
                        {
                            //Busca la información del Articulo
                            if (!buscar_articulo_por_id(((documento_venta_item)documento_venta_itemBindingSource.Current).id_articulo, 0, formulario.Almacen.codigo, null))
                            {
                                return;
                            }

                            ((documento_venta_item)documento_venta_itemBindingSource.Current).almacen = formulario.Almacen.codigo;
                        }

                        break;

                    default:
                        break;
                }
            }
            catch
            {
            }
        }

        private void cambiar_unidad()
        {
            try
            {
                if (documento_venta_itemBindingSource.Count == 0)
                {
                    utilidades.MensajeError("No hay artículos en la lista.", null, null, "SUITE - Factura de venta", "Advertencia");
                    return;
                }

                //Guardamos la unidad actual
                //string Unidad_Actual = ((documento_venta_item)documento_venta_itemBindingSource.Current).unidad;

                List<unidad> Lista_Unidades_Temp = new List<unidad>();

                //Filtramos las unidades del articulo
                Lista_Unidades_Temp = Lista_Unidades.Where(x => x.id_articulo == ((documento_venta_item)documento_venta_itemBindingSource.Current).id_articulo).ToList();

                //Obtenemos el indice donde esta la unidad actual
                //para pasar a la siguiente
                int index = Lista_Unidades_Temp.FindIndex(x => x.codigo == ((documento_venta_item)documento_venta_itemBindingSource.Current).unidad);

                //Guardarmos la unidad actual como anterior antes de cambiar
                unidad_anterior = ((documento_venta_item)documento_venta_itemBindingSource.Current).unidad;

                //Guardarmos la cantidad actual como anterior antes de cambiar
                cantidad_anterior = ((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad;

                if (index < Lista_Unidades_Temp.Count - 1)
                {
                    ////Busca la información del Articulo
                    //if(!buscar_articulo_por_id(((documento_venta_item)documento_venta_itemBindingSource.Current).id_articulo, false, 0, null, Lista_Unidades_Temp[index + 1].codigo))
                    //{
                    //    return;
                    //}

                    if (!validar_existencia(Articulo, 0, null, Lista_Unidades_Temp[index + 1].codigo))
                    {
                        return;
                    }

                    //((documento_venta_item)documento_venta_itemBindingSource.Current).unidad = Lista_Unidades_Temp[index + 1].codigo;


                    ////Valida que el usuario tenga permiso para utilizar la unidad del articulo seleccionada

                    //if (!validar_acceso_unidad(Lista_Unidades_Temp[index + 1].codigo))
                    //{
                    //    return;
                    //}

                    ((documento_venta_item)documento_venta_itemBindingSource.Current).unidad = Lista_Unidades_Temp[index + 1].codigo;
                }
                else
                {
                    if (!validar_existencia(Articulo, 0, null, Lista_Unidades_Temp[0].codigo))
                    {
                        return;
                    }

                    //Valida que el usuario tenga permiso para utilizar la unidad del articulo seleccionada

                    //if (!validar_acceso_unidad(Lista_Unidades_Temp[0].codigo))
                    //{
                    //    return;
                    //}

                    ((documento_venta_item)documento_venta_itemBindingSource.Current).unidad = Lista_Unidades_Temp[0].codigo;
                }

                calcular_total_item();
            }
            catch
            {

            }
        }

        private void cargar_configuracion_de_ventas()
        {
            try
            {
                txtCodigoArticulo.Focus();

                #region Configuración de Moneda

                if (parametros.Configuracion.moneda_principal != parametros.Configuracion.moneda_secundaria)
                {
                    

                    ((documento_venta)documento_ventaBindingSource.Current).tipo_moneda = parametros.Configuracion.moneda_principal;
                    ((documento_venta)documento_ventaBindingSource.Current).factor_cambio = parametros.Configuracion.tasa_cambio;

                    bsiTasaDia.Caption = String.Format("{0:C}", parametros.Configuracion.tasa_cambio);
                }

                #endregion

                #region Precio de Venta predeterminado

                if (parametros.Configuracion.precio_venta_sistema)
                {
                    precio = parametros.Configuracion.precio_venta;
                }
                else
                {
                    //Operacion = parametros.sys_Operacions.Where(x => x.idModulo == Modulo && x.descripcion == "Precio de venta por defecto").FirstOrDefault().id;

                    precio = parametros.Lista_Opciones_Usuario_M3.Where(x => x.codigo == "FACPREPRE").FirstOrDefault().valor1;

                }

                #endregion

                #region Vendedor predeterminado

                parametros.Vendedor = parametros.Lista_Vendedores.Where(x => x.codigo == parametros.Lista_Opciones_Usuario_M3.Where(xx => xx.codigo == "FACVENPRE").FirstOrDefault().valor1).FirstOrDefault();

                ((documento_venta)documento_ventaBindingSource.Current).codigo_vendedor = parametros.Vendedor.codigo;

                #endregion

                #region Precios disponibles

                sys_submodulos_opciones cambia_precio = parametros.Lista_Opciones_Usuario_M3.Where(x => x.codigo == "FACCAMPRE").FirstOrDefault();

                if (cambia_precio.valor1 == "1")
                {
                    precio_autorizados = cambia_precio.valor2;
                }

                #endregion

                #region Almacenes disponibles

                try
                {
                    sys_submodulos_opciones cambia_almacen = parametros.Lista_Opciones_Usuario_M3.Where(x => x.codigo == "FACAUTCAMALM").FirstOrDefault();

                    if (cambia_almacen.valor1 == "1")
                    {
                        almacenes_autorizados = cambia_almacen.valor2;
                    }

                    sys_submodulos_opciones smo3 = parametros.Lista_Opciones_Usuario_M3.Where(x => x.codigo == "FACALMPRE").FirstOrDefault();
                    string almacen_predeterminado = smo3.valor1;

                    if (cambia_almacen.valor1 == "2" && string.IsNullOrEmpty(almacen_predeterminado))
                    {
                        utilidades.MensajeError("El usuario no tiene configurado el acceso a los almacenes.", null, null, "Suite - Factura de Venta", "Error");

                        this.Close();
                    }

                    if (!string.IsNullOrEmpty(almacen_predeterminado))
                    {
                        Almacen = svc.Buscar_Almacen(parametros.Sucursal.codigo, almacen_predeterminado, null);
                        ((documento_venta)documento_ventaBindingSource.Current).codigo_almacen = almacen_predeterminado;
                    }
                    else
                    {
                        string[] lista_Almacenes = almacenes_autorizados.Split(',');

                        Almacen = svc.Buscar_Almacen(parametros.Sucursal.codigo, lista_Almacenes[0].ToString(), null);

                        ((documento_venta)documento_ventaBindingSource.Current).codigo_almacen = lista_Almacenes[0].ToString();
                    }

                    bsiAlmacenPorDefecto.Caption = Almacen.codigo + " - " + Almacen.descripcion;
                }
                catch (FaultException ex)
                {
                    utilidades.MensajeError("Se produjo un error obteniendo la lista de almacenes disponibles.", ex.Message, null, "Suite - Registro de Compra", "Error");

                    this.Close();

                    return;
                }
                catch (Exception ex)
                {
                    utilidades.MensajeError("Se produjo un error obteniendo la lista de almacenes disponibles.", ex.Message, null, "Suite - Registro de Compra", "Error");

                    this.Close();

                    return;
                }


                #endregion

                #region Barra de Estado

                bsiFecha.Caption = DateTime.Today.ToShortDateString();
                bsiUsuario.Caption = parametros.Usuario.descripcion;
                bsiSucursal.Caption = parametros.Sucursal.descripcion;


                #endregion
            }
            catch (Exception ex)
            {

            }


        }

        private void cargar_documento()
        {
            xfrm_Documento_Cargar frm = new xfrm_Documento_Cargar();
            frm.solicita = "factura_venta";
            frm.ShowDialog();

            if (frm.Documento_Venta != null)
            {
                utilidades.BarraProgreso(this, true, "Importando documento de venta.");

                if (documento_venta_itemBindingSource.Count > 0)
                {
                    DialogResult mensaje = MessageBox.Show("¿Desea agregar los artículos del documento seleccionado a la factura actual?", "Suite - Cargar documento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (mensaje == DialogResult.Yes)
                    {
                        foreach (var item in frm.Documento_Venta.Documento_Venta_Items)
                        {
                            if (buscar_articulo_por_id(item.id_articulo, item.cantidad, item.almacen, item.unidad))
                            {
                                guardar_unidades_articulo();

                                item.descripcion_unidad = Lista_Unidades.Where(x => x.id_articulo == Articulo.id && x.codigo == Articulo.unidad_venta1).FirstOrDefault().descripcion;

                                documento_venta_itemBindingSource.Add(item);
                            }
                        }
                    }
                    else
                    {
                        documento_ventaBindingSource.DataSource = frm.Documento_Venta;
                        documento_venta_itemBindingSource.Clear();

                        foreach (var item in frm.Documento_Venta.Documento_Venta_Items)
                        {
                            if (buscar_articulo_por_id(item.id_articulo, item.cantidad, item.almacen, item.unidad))
                            {
                                guardar_unidades_articulo();

                                item.descripcion_unidad = Lista_Unidades.Where(x => x.id_articulo == Articulo.id && x.codigo == Articulo.unidad_venta1).FirstOrDefault().descripcion;

                                documento_venta_itemBindingSource.Add(item);
                            }
                        }
                    }
                }
                else
                {
                    documento_ventaBindingSource.DataSource = frm.Documento_Venta;
                    documento_venta_itemBindingSource.Clear();

                    foreach (var item in frm.Documento_Venta.Documento_Venta_Items)
                    {
                        if (buscar_articulo_por_id(item.id_articulo, item.cantidad, item.almacen, item.unidad))
                        {
                            guardar_unidades_articulo();

                            item.descripcion_unidad = Lista_Unidades.Where(x => x.id_articulo == Articulo.id && x.codigo == Articulo.unidad_venta1).FirstOrDefault().descripcion;

                            documento_venta_itemBindingSource.Add(item);
                        }
                    }
                }

                clienteBindingSource.DataSource = svc.Buscar_Cliente(parametros.Sucursal.codigo, ((documento_venta)documento_ventaBindingSource.Current).codigo_cliente, true);

                calcular_total_item_todos();

                txtCodigoArticulo.Focus();

                // if(((documento_venta)documento_ventaBindingSource.Current).estado == "registrado") { ((documento_venta)documento_ventaBindingSource.Current).numero_documento = null; }

                ((documento_venta)documento_ventaBindingSource.Current).estado = null;

                utilidades.BarraProgreso(this, false, "Importando documento de venta.");
            }
        }

        private void guardar_configuracion_fiscal()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

                config.AppSettings.Settings.Remove("PuertoImpresoraFiscal");
                config.AppSettings.Settings.Add("PuertoImpresoraFiscal", parametros.PuertoImpresoraFiscal);
                config.Save(ConfigurationSaveMode.Modified);

                Properties.Settings.Default.Save();
                config.Save(ConfigurationSaveMode.Full);

                ConfigurationManager.RefreshSection("appSettings");
                ConfigurationManager.RefreshSection("system.serviceModel/client");
            }
            catch (CommunicationException comEx)
            {
                MessageBox.Show(comEx.InnerException.Message);
            }
        }

        private void guardar_documento()
        {
            try
            {
                if (string.IsNullOrEmpty(((documento_venta)documento_ventaBindingSource.Current).codigo_cliente))
                {
                    agregar_cliente();

                    if (string.IsNullOrEmpty(((documento_venta)documento_ventaBindingSource.Current).codigo_cliente))
                    {
                        return;
                    }
                }

                utilidades.BarraProgreso(this, true, "Guardando documento");

                ((documento_venta)documento_ventaBindingSource.Current).Documento_Venta_Items = documento_venta_itemBindingSource.List.OfType<documento_venta_item>().ToArray();

                ((documento_venta)documento_ventaBindingSource.Current).fecha_emision = DateTime.Now;
                ((documento_venta)documento_ventaBindingSource.Current).fecha_vence = DateTime.Now;
                ((documento_venta)documento_ventaBindingSource.Current).estacion = utilidades.Estacion_GetName();
                ((documento_venta)documento_ventaBindingSource.Current).usuario = parametros.Usuario.codigo;
                ((documento_venta)documento_ventaBindingSource.Current).codigo_vendedor = parametros.Vendedor.codigo;
                ((documento_venta)documento_ventaBindingSource.Current).tipo_documento = "FAC";
                ((documento_venta)documento_ventaBindingSource.Current).direccion_entrega = "N/A";

                svc.Guardar_Documento_Venta(parametros.Sucursal.codigo, (documento_venta)documento_ventaBindingSource.Current);

                utilidades.BarraProgreso(this, false, "Guardando documento");

                txtCantidadArticulos.EditValue = 0;

                documento_ventaBindingSource.Clear();
                documento_venta_itemBindingSource.Clear();
                documento_ventaBindingSource.AddNew();

                parametros.Configuracion = svc.Buscar_Configuracion(parametros.Sucursal.codigo);

                cargar_configuracion_de_ventas();

                lblIdentificacion.Text = string.Empty;
                lblRazonSocial.Text = string.Empty;
            }
            catch (FaultException FEx)
            {
                utilidades.BarraProgreso(this, false, "Guardando documento");

                MessageBox.Show(FEx.Message);
            }
            catch (Exception Ex)
            {
                utilidades.BarraProgreso(this, false, "Guardando documento");

                MessageBox.Show(Ex.Message);
            }
        }

        private void guardar_unidades_articulo()
        {
            try
            {
                Lista_Unidades.RemoveAll(x => x.id_articulo == Articulo.id);

                unidad u = svc.Buscar_Unidad(parametros.Sucursal.codigo, Articulo.unidad_venta1, true);
                if (u != null)
                {
                    u.id_articulo = Articulo.id;

                    Lista_Unidades.Add(u);
                }
            }
            catch (Exception ex)
            {
                utilidades.MensajeError("Se produjo una excepción al obtener las unidades del artículo.", null, null, "Suite - Archivo de artículo", "Error");
            }

            try
            {
                unidad u = svc.Buscar_Unidad(parametros.Sucursal.codigo, Articulo.unidad_venta2, true);
                if (u != null)
                {
                    u.id_articulo = Articulo.id;

                    Lista_Unidades.Add(u);
                }
            }
            catch (Exception ex)
            {
                utilidades.MensajeError("Se produjo una excepción al obtener las unidades del artículo.", null, null, "Suite - Archivo de artículo", "Error");
            }

            try
            {
                unidad u = svc.Buscar_Unidad(parametros.Sucursal.codigo, Articulo.unidad_venta3, true);
                
                if(u != null)
                {
                    u.id_articulo = Articulo.id;

                    Lista_Unidades.Add(u);
                }

            }
            catch (Exception ex)
            {
                utilidades.MensajeError("Se produjo una excepción al obtener las unidades del artículo.", null, null, "Suite - Archivo de artículo", "Error");
            }
        }

        private void lista_articulos()
        {
            try
            {
                xfrm_Lista_Articulos_Venta formulario = new xfrm_Lista_Articulos_Venta();
                formulario.origenArchivo = false;
                formulario.almacen = Almacen.codigo;
                formulario.validar_existencia = true;

                formulario.ShowDialog();

                if (formulario.Articulo != null)
                {
                    try
                    {
                        if (buscar_articulo_por_id(formulario.Articulo.id, 0, null, null))
                        {
                            //Guardar_Unidades_Articulo();
                            agregar_articulo();
                        }

                        txtCodigoArticulo.Text = string.Empty;
                    }
                    catch (FaultException Ex)
                    {
                        MessageBox.Show(Ex.Message, "Suite - Archivo de Artículos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        txtCodigoArticulo.Text = string.Empty;

                        txtCodigoArticulo.Focus();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("Se produjo un error al validar el código del artículo." + Environment.NewLine + Environment.NewLine + Ex.Message, "Suite - Archivo de Artículos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        txtCodigoArticulo.Text = string.Empty;

                        txtCodigoArticulo.Focus();
                    }
                }
            }
            catch (FaultException Ex)
            {
                MessageBox.Show(Ex.Message, "Suite - Archivo de Artículos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Se produjo un error al validar el código del artículo." + Environment.NewLine + Environment.NewLine + Ex.Message, "Suite - Archivo de Artículos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cambiar_moneda(string moneda)
        {
           

            if (moneda == "secundaria")
            {
                foreach (GridColumn item in gvArticulos.Columns)
                {
                    item.VisibleIndex = -1;
                }                

                //gvArticulos.Columns["imagen"].VisibleIndex = 0;
                gvArticulos.Columns["articulo"].VisibleIndex = 1;
                gvArticulos.Columns["descripcion_articulo"].VisibleIndex = 2;
                gvArticulos.Columns["unidad"].VisibleIndex = 3;
                gvArticulos.Columns["precio_unitario_moneda2"].VisibleIndex = 4;
                gvArticulos.Columns["cantidad"].VisibleIndex = 5;
                gvArticulos.Columns["total_neto_moneda2"].VisibleIndex = 6;

                txtBaseImponible.DataBindings.Clear();
                txtExento.DataBindings.Clear();
                txtDescuento.DataBindings.Clear();
                txtImpuesto.DataBindings.Clear();

                txtBaseImponible.DataBindings.Add("EditValue", documento_ventaBindingSource, "base_imponible_moneda2", true, DataSourceUpdateMode.Never);
                txtExento.DataBindings.Add("EditValue", documento_ventaBindingSource, "total_exento_moneda2", true, DataSourceUpdateMode.Never);
                txtDescuento.DataBindings.Add("EditValue", documento_ventaBindingSource, "total_descuento_moneda2", true, DataSourceUpdateMode.Never);
                txtImpuesto.DataBindings.Add("EditValue", documento_ventaBindingSource, "total_impuesto_moneda2", true, DataSourceUpdateMode.Never);

                CultureInfo culture = CultureInfo.CreateSpecificCulture(parametros.Configuracion.moneda_secundaria);
                culture.NumberFormat.CurrencyPositivePattern = 2;

                RepositoryItemTextEdit textEdit = new RepositoryItemTextEdit();
                textEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                textEdit.Mask.EditMask = "c2";
                textEdit.Mask.UseMaskAsDisplayFormat = true;
                textEdit.Mask.Culture = culture;
                textEdit.ReadOnly = true;
                gcArticulos.RepositoryItems.Add(textEdit);
                colprecio_unitario_moneda2.ColumnEdit = textEdit;

                RepositoryItemTextEdit textEdit2 = new RepositoryItemTextEdit();
                textEdit2.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                textEdit2.Mask.EditMask = "c2";
                textEdit2.Mask.UseMaskAsDisplayFormat = true;
                textEdit2.ReadOnly = true;
                textEdit2.Mask.Culture = culture;
                gcArticulos.RepositoryItems.Add(textEdit2);
                coltotal_moneda2.ColumnEdit = textEdit2;

                moneda_para_mostrar = parametros.Configuracion.moneda_secundaria;
            }
            else
            {
                foreach (GridColumn item in gvArticulos.Columns)
                {
                    item.VisibleIndex = -1;
                }

                //gvArticulos.Columns["imagen"].VisibleIndex = 0;
                gvArticulos.Columns["articulo"].VisibleIndex = 1;
                gvArticulos.Columns["descripcion_articulo"].VisibleIndex = 2;
                gvArticulos.Columns["unidad"].VisibleIndex = 3;
                gvArticulos.Columns["precio_unitario"].VisibleIndex = 4;
                gvArticulos.Columns["cantidad"].VisibleIndex = 5;
                gvArticulos.Columns["total_neto"].VisibleIndex = 6;

                txtBaseImponible.DataBindings.Clear();
                txtExento.DataBindings.Clear();
                txtDescuento.DataBindings.Clear();
                txtImpuesto.DataBindings.Clear();

                txtBaseImponible.DataBindings.Add("EditValue", documento_ventaBindingSource, "base_imponible", true, DataSourceUpdateMode.Never);
                txtExento.DataBindings.Add("EditValue", documento_ventaBindingSource, "total_exento", true, DataSourceUpdateMode.Never);
                txtDescuento.DataBindings.Add("EditValue", documento_ventaBindingSource, "total_descuento", true, DataSourceUpdateMode.Never);
                txtImpuesto.DataBindings.Add("EditValue", documento_ventaBindingSource, "total_impuesto", true, DataSourceUpdateMode.Never);

                moneda_para_mostrar = parametros.Configuracion.moneda_principal;
            }
        }

        private void mostrar_moneda_secundaria()
        {
        }

        private void modificar_lista_de_articulo()
        {
            try
            {
                foreach (documento_venta_item item in documento_venta_itemBindingSource)
                {
                    item.total_moneda2 = (parametros.Configuracion.moneda_mayor == parametros.Configuracion.moneda_principal) ? item.total_moneda2 * parametros.Configuracion.tasa_cambio : item.total_moneda2 / parametros.Configuracion.tasa_cambio;
                    item.base_imponible_moneda2 = (parametros.Configuracion.moneda_mayor == parametros.Configuracion.moneda_principal) ? item.base_imponible * parametros.Configuracion.tasa_cambio : item.base_imponible / parametros.Configuracion.tasa_cambio;
                    item.impuesto_moneda2 = (parametros.Configuracion.moneda_mayor == parametros.Configuracion.moneda_principal) ? item.impuesto * parametros.Configuracion.tasa_cambio : item.impuesto / parametros.Configuracion.tasa_cambio;
                }

                calcular_totales();

                //Limpiar
                unidadBindingSource.Clear();
                txtCodigoArticulo.Focus();
            }
            catch
            {
            }
        }

        private void mostrar_lista_clientes()
        {
            try
            {
                xfrm_Listas lista = new xfrm_Listas();
                lista.archivo = "clientes";
                lista.ShowDialog();

                if (lista.bs != null)
                {
                    clienteBindingSource.DataSource = lista.bs;

                    ((documento_venta)documento_ventaBindingSource.Current).codigo_cliente = ((cliente)clienteBindingSource.Current).codigo;
                    ((documento_venta)documento_ventaBindingSource.Current).numero_identificacion = ((cliente)clienteBindingSource.Current).numero_identificacion;
                }
            }
            catch
            {
            }
        }

        private void quitar_articulo()
        {
            if (documento_venta_itemBindingSource.Count > 0)
            {
                documento_venta_itemBindingSource.RemoveCurrent();

                calcular_totales();

                txtCodigoArticulo.Select();
                txtCodigoArticulo.Focus();
            }
        }

        private void siguiente()
        {
            try
            {
                if (documento_venta_itemBindingSource.Count == 0)
                {
                    MessageBox.Show("No ha seleccionado ningún artículo para el documento.", "Suite - Facturación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (string.IsNullOrEmpty(((documento_venta)documento_ventaBindingSource.Current).codigo_cliente))
                    {
                        agregar_cliente();

                        if (string.IsNullOrEmpty(((documento_venta)documento_ventaBindingSource.Current).codigo_cliente))
                        {
                            return;
                        }
                    }

                    xfrm_Documento_Venta_Cobro frm = new xfrm_Documento_Venta_Cobro();
                    ((documento_venta)documento_ventaBindingSource.Current).tipo_documento = "FAC";
                    frm.documento_Venta = (documento_venta)documento_ventaBindingSource.Current;
                    frm.documento_Venta.Documento_Venta_Items = documento_venta_itemBindingSource.List.OfType<documento_venta_item>().ToArray();
                    frm.documento_Venta.Cliente = (cliente)clienteBindingSource.Current;
                    frm.Cliente = (cliente)clienteBindingSource.Current;                    

                    frm.ShowDialog();

                    if (frm.success)
                    {
                        if(!parametros.UsaImpresoraFiscal)
                        {
                            xfrm_Documento_Impresion frmImprimir = new xfrm_Documento_Impresion();
                            frmImprimir.tipo_operacion = "ventas";
                            frmImprimir.documento_Venta = frm.documento_Venta;
                            frmImprimir.ShowDialog();
                        }

                        txtCantidadArticulos.EditValue = 0;

                        documento_ventaBindingSource.Clear();
                        documento_venta_itemBindingSource.Clear();
                        documento_ventaBindingSource.AddNew();

                        parametros.Configuracion = svc.Buscar_Configuracion(parametros.Sucursal.codigo);

                        //if (parametros.Configuracion.proximo_factura.ToString().Length < 10)
                        //{
                        //    lblProximoDocumento.Text = parametros.Configuracion.proximo_factura.ToString().PadLeft(8, '0') + "  ";
                        //}

                        parametros.documento_venta_auxiliar = null;

                        cargar_configuracion_de_ventas();

                        lblIdentificacion.Text = string.Empty;
                        lblRazonSocial.Text = string.Empty;
                    }
                }
            }
            catch (FaultException FEx)
            {
                MessageBox.Show("Se produjo un error al eliminar la información temporal del vendedor.", "SUITE - Factura de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Se produjo un error al eliminar la información temporal del vendedor.", "SUITE - Facturac de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void subir_articulo()
        {
            try
            {
                buscar_articulo_por_codigo(((documento_venta_item)documento_venta_itemBindingSource.Current).articulo, "codigo");

               Articulo.cantidad = ((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad;
               Articulo.unidad = ((documento_venta_item)documento_venta_itemBindingSource.Current).unidad;
               Articulo.precio_unitario = ((documento_venta_item)documento_venta_itemBindingSource.Current).precio_unitario;
               Articulo.base_imponible = ((documento_venta_item)documento_venta_itemBindingSource.Current).base_imponible;
            }
            catch
            {
            }
        }

        private bool validar_acceso_unidad(string unidad)
        {
            //Valida que el usuario tenga permiso para utilizar la unidad del articulo seleccionada
            string[] unidades = unidades_autorizadas.Split(',');

            if (!unidades.Contains(unidad))
            {
                utilidades.MensajeError("No tiene permitido utilizar la unidad de venta seleccionada", unidad, null, "Integra - Configuración de ventas", "Advertencia");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool validar_cantidad()
        {
            try
            {
                Articulo = svc.Buscar_ID_Articulo(parametros.Sucursal.codigo, ((documento_venta_item)documento_venta_itemBindingSource.Current).id_articulo, null);

                if (!Articulo.lote)
                {
                    existenciaBindingSource.DataSource = svc.Buscar_Existencias(parametros.Sucursal.codigo, ((documento_venta_item)documento_venta_itemBindingSource.Current).id_articulo, ((documento_venta_item)documento_venta_itemBindingSource.Current).unidad, ((documento_venta_item)documento_venta_itemBindingSource.Current).almacen, null, null, null, true).FirstOrDefault();

                    if (existenciaBindingSource == null)
                    {
                        utilidades.MensajeError("El artículo no posee stock en el almacén seleccionado.", null, null, "SUITE - Factura de venta", "Error");

                        return false;
                    }

                    //Obtenemos la cantidad actual y le sumamos el comprometido actual que tenga el usuario
                    decimal actual = ((existencia)existenciaBindingSource.Current).actual;
                    decimal comprometido = ((existencia)existenciaBindingSource.Current).comprometido;

                    if ((actual - comprometido) < (((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad + 1))
                    {
                        utilidades.MensajeError("La cantidad solicitada excede la cantidad disponible en el almacén seleccionado.", null, null, "SUITE - Factura de venta", "Error");

                        return false;
                    }

                    return true;
                }
                else
                {
                    decimal cant = 0;
                    decimal actual = 0;
                    decimal comprometido = 0;

                    foreach (existencia item in svc.Buscar_Existencias_Lotes(parametros.Sucursal.codigo, Articulo.id, Articulo.unidad_venta1, ((documento_venta_item)documento_venta_itemBindingSource.Current).almacen, null, null, null, 0, DateTime.MinValue, null).ToList())
                    {
                        cant = cant + item.actual - item.comprometido;
                        actual = actual + item.actual;
                        comprometido = comprometido + item.comprometido;
                    }

                    if ((actual - comprometido) < (((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad + 1))
                    {
                        utilidades.MensajeError("La cantidad solicitada excede la cantidad disponible en el almacén seleccionado.", null, null, "SUITE - Factura de venta", "Error");

                        return false;
                    }

                    return true;
                }
            }
            catch(Exception ex)
            {
                utilidades.MensajeError("El artículo no posee stock en el almacén seleccionado.", null, null, "SUITE - Factura de venta", "Error");

                return false;
            }
        }

        private bool validar_existencia(articulo Articulo, decimal cantidad_venta, string almacen_venta, string unidad_venta)
        {
            try
            {
                decimal cantidad = 1;
                string Almacen = ((documento_venta)documento_ventaBindingSource.Current).codigo_almacen;
                string Unidad = Articulo.unidad;

                if (cantidad_venta > 0) { cantidad = cantidad_venta; }
                if (!string.IsNullOrEmpty(almacen_venta)) { Almacen = almacen_venta; }
                if (!string.IsNullOrEmpty(unidad_venta)) { Unidad = unidad_venta; }

                int index = -1;

                index = verificar_articulo_en_grid(Articulo.id, Unidad, Almacen);

                if (index > -1)
                {
                    //Si el articulo existe se toma el almacen que tiene asignado.
                    Almacen = ((documento_venta_item)documento_venta_itemBindingSource[index]).almacen;

                    //Toma el valor de cantidad y se suma a la del articulo ya existente
                    cantidad = cantidad + ((documento_venta_item)documento_venta_itemBindingSource[index]).cantidad;

                    //Guardamos la cantidad anterior para despues validar existencia
                    cantidad_anterior = Convert.ToDecimal(gvArticulos.GetFocusedRowCellValue("cantidad").ToString());
                    unidad_anterior = gvArticulos.GetFocusedRowCellValue("unidad").ToString();
                }

                if (Unidad == Articulo.unidad_venta2)
                {
                    cantidad = cantidad * Articulo.cantidad_unidad2;
                }
                else if (Unidad == Articulo.unidad_venta3)
                {
                    cantidad = cantidad * Articulo.cantidad_unidad3;
                }

                if (!Articulo.lote)
                {
                    existenciaBindingSource.DataSource = svc.Buscar_Existencias(parametros.Sucursal.codigo, Articulo.id, Unidad, Almacen, null, null, null, true).FirstOrDefault();

                    if (existenciaBindingSource == null)
                    {
                        utilidades.MensajeError("La cantidad solicitada excede la existencia disponible del artículo.", Articulo.descripcion, null, "SUITE - Stock de Artículos", "Error");

                        //Si el artículo ya existe

                        if (index > -1)
                        {
                            ((documento_venta_item)documento_venta_itemBindingSource[gvArticulos.FocusedRowHandle]).unidad = unidad_anterior;
                            ((documento_venta_item)documento_venta_itemBindingSource[gvArticulos.FocusedRowHandle]).cantidad = cantidad_anterior;

                            gvArticulos.SetFocusedRowCellValue("unidad", unidad_anterior);
                        }

                        return false;
                    }

                    decimal actual = ((existencia)existenciaBindingSource.Current).actual;
                    decimal comprometido = ((existencia)existenciaBindingSource.Current).comprometido;

                    if ((actual - comprometido) < cantidad)
                    {
                        utilidades.MensajeError("La cantidad solicitada excede la existencia disponible del artículo.", Articulo.descripcion, null, "SUITE - Stock de Artículos", "Error");

                        if (index > -1)
                        {
                            ((documento_venta_item)documento_venta_itemBindingSource[gvArticulos.FocusedRowHandle]).unidad = unidad_anterior;
                            ((documento_venta_item)documento_venta_itemBindingSource[gvArticulos.FocusedRowHandle]).cantidad = cantidad_anterior;

                            gvArticulos.SetFocusedRowCellValue("unidad", unidad_anterior);
                        }

                        cantidad = 0;

                        return false;
                    }

                    return true;
                }
                else
                {
                    decimal cant = 0;
                    decimal actual = 0;
                    decimal comprometido = 0;

                    //Verificamos la disponibilidad del lote actual
                    foreach (existencia item in svc.Buscar_Existencias_Lotes(parametros.Sucursal.codigo, Articulo.id, Articulo.unidad_venta1, Almacen, null, null, null, 0, DateTime.MinValue, null).ToList())
                    {
                        cant = cant + item.actual - item.comprometido;
                        actual = actual + item.actual;
                        comprometido = comprometido + item.comprometido;
                    }

                    if ((actual - comprometido) < cantidad)
                    {
                        utilidades.MensajeError("La cantidad solicitada excede la cantidad disponible en el almacén seleccionado.", Articulo.descripcion, null, "SUITE - Operaciones de Inventario", "Error");

                        if (index > -1)
                        {
                            ((documento_venta_item)documento_venta_itemBindingSource[gvArticulos.FocusedRowHandle]).unidad = unidad_anterior;
                            ((documento_venta_item)documento_venta_itemBindingSource[gvArticulos.FocusedRowHandle]).cantidad = cantidad_anterior;

                            gvArticulos.SetFocusedRowCellValue("unidad", unidad_anterior);
                        }

                        return false;
                    }

                    return true;
                }
            }
            catch (FaultException ex)
            {
                utilidades.MensajeError("La cantidad solicitada excede la existencia disponible del artículo.", Articulo.descripcion, null, "SUITE - Stock de Artículos", "Error");

                return false;
            }
            catch (Exception ex)
            {
                utilidades.MensajeError("Se produjo una excepción validando la existencia del artículo.", Articulo.descripcion, null, "SUITE - Operaciones de Inventario", "Error");

                return false;
            }
        }

        private void ventana_descuento()
        {
            //xfrm_Documento_Descuento formulario = new xfrm_Documento_Descuento();
            //formulario.tipo_operacion = "FACTURAVENTA";
            //formulario.ShowDialog();
        }

        private int verificar_articulo_en_grid(int articulo, string unidad, string almacen)
        {
            try
            {
                var obj = documento_venta_itemBindingSource.List.OfType<documento_venta_item>().ToList().Find(x => x.id_articulo == articulo && x.unidad == unidad && x.almacen == almacen);
                var pos = documento_venta_itemBindingSource.IndexOf(obj);

                return pos;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ocurrio un error verificando el artículo en la grilla.", "Suite - Factura de Venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void ver_stock()
        {
            if (documento_venta_itemBindingSource.Count == 0)
            {
                utilidades.MensajeError("No hay artículos en la lista.", null, null, "SUITE - Factura de venta", "Advertencia");
                return;
            }

            if (documento_venta_itemBindingSource.Count > 0)
            {
                xfrm_Documento_Existencia_Articulo frmExistencias = new xfrm_Documento_Existencia_Articulo();
                frmExistencias.Articulo = ((documento_venta_item)documento_venta_itemBindingSource.Current).id_articulo;
                frmExistencias.Almacen = parametros.Lista_Almacenes.Where(x => x.codigo == ((documento_venta_item)documento_venta_itemBindingSource.Current).almacen).FirstOrDefault();
                frmExistencias.ShowDialog();
            }
        }
               
        #region MouseMove

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        #endregion

        #region Métodos de Controles
        private void custom_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData.ToString())
            {
                case "F1":
                    break;
                case "F2":

                    agregar_cliente();

                    break;

                case "F3":

                    lista_articulos();

                    break;
                case "F4":

                    cambio_almacen();
                    break;

                case "F5":

                    xfrm_Documento_Venta_Consulta formulario = new xfrm_Documento_Venta_Consulta();
                    formulario.almacen = ((documento_venta)documento_ventaBindingSource.Current).codigo_almacen;
                    formulario.ShowDialog();

                    break;

                case "F6":

                    cambiar_unidad();

                    break;

                case "F7":

                    

                    break;

                case "F8":

                    ver_stock();

                    break;

                case "F9":

                    actualizar_cantidad();

                    break;

                case "F10":

                    ventana_descuento();

                    break;

                case "F11":

                    break;

                case "F12":

                    btnSiguiente.PerformClick();

                    break;

                case "Escape":

                    if (!string.IsNullOrEmpty(((documento_venta)documento_ventaBindingSource.Current).codigo_cliente) || documento_venta_itemBindingSource.Count > 0)
                    {
                        DialogResult mensaje = utilidades.MensajeError("¿Dese cancelar el documento actual?.", null, null, "SUITE - Factura de venta", "Pregunta");

                        if (mensaje == DialogResult.Yes)
                        {
                            //xfrm_Clave_Autorizacion auto = new xfrm_Clave_Autorizacion();
                            //auto.tipo_operacion = "cancelar_factura";
                            //auto.ShowDialog();

                            //if(auto.Autorizado)
                            //{
                            //    MessageBox.Show("autorizado");
                            //}

                            documento_ventaBindingSource.Clear();
                            documento_venta_itemBindingSource.Clear();
                            documento_ventaBindingSource.AddNew();

                            parametros.Configuracion = svc.Buscar_Configuracion(parametros.Sucursal.codigo);

                            if (parametros.Configuracion.proximo_factura.ToString().Length < 10)
                            {
                                //lblProximoDocumento.Text = parametros.Configuracion.proximo_factura.ToString().PadLeft(8, '0') + "  ";
                            }

                            cargar_configuracion_de_ventas();

                            lblIdentificacion.Text = string.Empty;
                            lblRazonSocial.Text = string.Empty;
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                    break;
                case "Delete":

                    quitar_articulo();

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

                    //SendKeys.Send("+{TAB}");

                    break;
                case "Down":

                    //SendKeys.Send("{TAB}");

                    break;

                case "Left":

                    SendKeys.Send("+{TAB}");

                    break;
                case "Right":

                    SendKeys.Send("{TAB}");

                    break;
                case "+":

                    if(gcArticulos.Focused)
                    {
                        cambiar_cantidad(true);
                    }

                    break;
                case "-":

                    if (gcArticulos.Focused)
                    {
                        cambiar_cantidad(false);
                    }

                    break;
                case "Oemplus":

                    if (gcArticulos.Focused)
                    {
                        cambiar_cantidad(true);
                    }

                    break;
                case "OemMinus":

                    if (gcArticulos.Focused)
                    {
                        cambiar_cantidad(false);
                    }

                    break;
                default:
                    break;
            }

            if (e.Control && e.KeyCode == Keys.S)
            {
                btnSiguiente.PerformClick();
            }

            if (e.Control && e.KeyCode == Keys.G)
            {
                btnGuardar.PerformClick();
            }

            if (e.Control && e.KeyCode == Keys.L)
            {
                btnCargar.PerformClick();
            }

            if (e.Control && e.KeyCode == Keys.P)
            {
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                cambio_almacen();
            }

            var obj = sender.GetType().Name;

            switch (obj)
            {
                case "TextEdit":

                    TextEdit te = (TextEdit)sender;

                    if (te.Name == "txtCodigoArticulo")
                    {
                        if (e.KeyData.ToString() == "Down")
                        {
                            if (documento_venta_itemBindingSource.Count > 0)
                            {
                                gvArticulos.Focus();
                            }
                        }
                    }

                    break;

                case "GridControl":

                    if (e.KeyData.ToString() == "Up")
                    {
                        if (gvArticulos.GetFocusedDataSourceRowIndex() == 0)
                        {

                            txtCodigoArticulo.Focus();
                        }
                    }

                    break;
                default:
                    break;
            }
        }
        private void custom_TextBox_GotFocus(object sender, EventArgs e)
        {
            TextEdit textBox = (TextEdit)sender;

            textBox.SelectAll();
        }

        #region Configuración del Formulario
        public xfrm_Documento_Venta_Factura_Touch()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture(parametros.Configuracion.moneda_principal);
            culture.NumberFormat.CurrencyPositivePattern = 2;

            // this may fail sometimes: (see Drachenkatze's comment below)
            // var culture = new CultureInfo("en-US");

            //Culture for any thread
            CultureInfo.DefaultThreadCurrentCulture = culture;

            //Culture for UI in any thread
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            InitializeComponent();
            (new TabOrderManager(this)).SetTabOrder(TabOrderManager.TabScheme.AcrossFirst);

            try
            {
                gvArticulos.Appearance.EvenRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#bdc3c7");

                documento = new documento_venta();

                documento_ventaBindingSource.DataSource = documento;

                cambiar_configuracion_regional(parametros.Configuracion.moneda_principal, parametros.Configuracion.moneda_secundaria);
            }
            catch
            {
            }
        }
        private void xfrm_Documento_Factura_Venta_Resize(object sender, EventArgs e)
        {
        }
        private void xfrm_Documento_Factura_Venta_Shown(object sender, EventArgs e)
        {
            lblRazonSocial.Text = string.Empty;
            lblIdentificacion.Text = string.Empty;

            documento_ventaBindingSource.AddNew();

            ((documento_venta)documento_ventaBindingSource.Current).tipo_documento = "FAC";

            parametros.Configuracion = svc.Buscar_Configuracion(parametros.Sucursal.codigo);

            if (parametros.Configuracion.proximo_factura.ToString().Length < 10)
            {
                //lblProximoDocumento.Text = parametros.Configuracion.proximo_factura.ToString().PadLeft(8, '0') + "  ";
            }

            cargar_configuracion_de_ventas();

            cambiar_moneda("secundaria");

            #region Validar impresora fiscal

            if (parametros.UsaImpresoraFiscal)
            {
                utilidades.BarraProgreso(this, true, "Verificando conexión de la impresora fiscal.");

                abrir_puerto_com();

                if (!Impresora.CheckFPrinter())
                {
                    utilidades.BarraProgreso(this, false, "Verificando conexión de la impresora fiscal.");

                    cerrar_puerto_com();

                    DialogResult mensaje = utilidades.MensajeError("La impresora fiscal no está conectada. ¿Desea que el sistema busque automaticamente la impresora configurada?", null, null, "Integra - Conexión de impresora fiscal", "Pregunta");

                    if (mensaje == DialogResult.Yes)
                    {
                        buscar_impresora_fiscal();
                    }
                    else
                    {
                        this.Close();
                    }
                }

                cerrar_puerto_com();

                utilidades.BarraProgreso(this, false, "Verificando conexión de la impresora fiscal.");
            }

            #endregion

            if (parametros.UsaPantallaAuxiliar)
            {
                btnVentanaAuxiliar.PerformClick();
            }

            if(parametros.Palette == "Oscuro")
            {
               pc_Total_Titulo.BackColor = ColorTranslator.FromHtml("#10ac84");
               pc_Cantidad_Articulos.BackColor = ColorTranslator.FromHtml("#10ac84");
               pc_Datos_Clientes.BackColor = ColorTranslator.FromHtml("#10ac84");
            }
        }
        private void xfrm_Documento_Venta_Factura_Touch_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                svc.Eliminar_Existencia_Comprometida(parametros.Sucursal.codigo, parametros.Usuario.codigo, parametros.Vendedor.codigo, "factura", null, null);
            }
            catch (FaultException FEx)
            {
                MessageBox.Show("Se produjo un error al eliminar la información temporal del vendedor.", "SUITE - Facturac de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Se produjo un error al eliminar la información temporal del vendedor.", "SUITE - Facturac de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region  Configuración de botones

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {            
            siguiente();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            cargar_documento();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar_documento();
        }

        private void btnCambiarMoneda_Click(object sender, EventArgs e)
        {
            if (moneda_para_mostrar == parametros.Configuracion.moneda_principal)
            {
                cambiar_moneda("secundaria");
            }
            else
            {
                cambiar_moneda("principal");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process p = System.Diagnostics.Process.Start("calc.exe");
            xfrm_Calculadora xfrmCalculadora = new xfrm_Calculadora();
            xfrmCalculadora.Show();
        }

        #endregion

        #region  Configuración de TextBox

        private void txtCodigoArticulo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    if (!string.IsNullOrEmpty(txtCodigoArticulo.Text))
                    {
                        if (buscar_articulo_por_codigo(txtCodigoArticulo.Text, "codigo"))
                        {
                            agregar_articulo();
                        }
                    }
                    else
                    {
                        //txtDescripcionArticulo.Focus();
                    }

                    txtCodigoArticulo.Text = string.Empty;
                    //txtDescripcionArticulo.Text = string.Empty;
                }
                catch (FaultException Ex)
                {
                    MessageBox.Show(Ex.Message, "Suite - Archivo de Artículos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    txtCodigoArticulo.Text = string.Empty;
                    //txtDescripcionArticulo.Text = string.Empty;

                    txtCodigoArticulo.Focus();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Se produjo un error al validar el código del artículo." + Environment.NewLine + Environment.NewLine + Ex.Message, "Suite - Archivo de Artículos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    txtCodigoArticulo.Text = string.Empty;
                    //txtDescripcionArticulo.Text = string.Empty;

                    txtCodigoArticulo.Focus();
                }
            }
        }
        private void txtDescripcionArticulo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13)
            //{
            //    try
            //    {
            //        if (!string.IsNullOrEmpty(txtDescripcionArticulo.Text))
            //        {
            //            if (buscar_articulo_por_descripcion(txtDescripcionArticulo.Text, "descripcion"))
            //            {
            //            }
            //        }
            //        else
            //        {

            //        }

            //        txtCodigoArticulo.Text = string.Empty;
            //        txtDescripcionArticulo.Text = string.Empty;
            //    }
            //    catch (FaultException Ex)
            //    {
            //        MessageBox.Show(Ex.Message, "Suite - Archivo de Artículos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //        txtCodigoArticulo.Text = string.Empty;
            //        txtDescripcionArticulo.Text = string.Empty;

            //        txtCodigoArticulo.Focus();
            //    }
            //    catch (Exception Ex)
            //    {
            //        MessageBox.Show("Se produjo un error al validar el código del artículo." + Environment.NewLine + Environment.NewLine + Ex.Message, "Suite - Archivo de Artículos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //        txtCodigoArticulo.Text = string.Empty;
            //        txtDescripcionArticulo.Text = string.Empty;

            //        txtCodigoArticulo.Focus();
            //    }
            //}
        }

        #endregion

        #region Configuracion de Hipervinculos

        private void hlSiguiente_Click(object sender, EventArgs e)
        {
            siguiente();
        }

        private void hlSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hlClientes_Click(object sender, EventArgs e)
        {
            mostrar_lista_clientes();
        }

        private void hlCambioMonedas_Click(object sender, EventArgs e)
        {
            mostrar_moneda_secundaria();
        }

        private void hlDescuentoArtículo_Click(object sender, EventArgs e)
        {
            ventana_descuento();
        }

        private void hlDeposito_Click(object sender, EventArgs e)
        {
            cambio_almacen();
        }

        private void hlConsulta_Click(object sender, EventArgs e)
        {
            xfrm_Documento_Venta_Consulta formulario = new xfrm_Documento_Venta_Consulta();
            formulario.ShowDialog();
        }

        private void hlCambiarMoneda_Click(object sender, EventArgs e)
        {
            if (moneda_para_mostrar == "principal")
            {
                gvArticulos.Columns["precio_unitario"].Visible = false;
                gvArticulos.Columns["total"].Visible = false;

                gvArticulos.Columns["precio_unitario_moneda2"].Visible = true;
                gvArticulos.Columns["total_moneda2"].Visible = true;

                gvArticulos.Columns["articulo"].VisibleIndex = 0;
                gvArticulos.Columns["descripcion_articulo"].VisibleIndex = 1;
                gvArticulos.Columns["descripcion_unidad"].VisibleIndex = 2;
                gvArticulos.Columns["precio_unitario"].VisibleIndex = -1;
                gvArticulos.Columns["precio_unitario_moneda2"].VisibleIndex = 3;
                gvArticulos.Columns["cantidad"].VisibleIndex = 4;
                gvArticulos.Columns["descuento"].VisibleIndex = -1;
                gvArticulos.Columns["descuento_moneda2"].VisibleIndex = -1;
                gvArticulos.Columns["base_imponible"].VisibleIndex = -1;
                gvArticulos.Columns["base_imponible_moneda2"].VisibleIndex = -1;
                gvArticulos.Columns["impuesto"].VisibleIndex = -1;
                gvArticulos.Columns["impuesto_moneda2"].VisibleIndex = -1;
                gvArticulos.Columns["total"].VisibleIndex = -1;
                gvArticulos.Columns["total_moneda2"].VisibleIndex = 5;

                moneda_para_mostrar = "secundaria";

                var culture = CultureInfo.CreateSpecificCulture(parametros.Configuracion.moneda_secundaria);
                culture.NumberFormat.CurrencyPositivePattern = 2;

                // this may fail sometimes: (see Drachenkatze's comment below)
                // var culture = new CultureInfo("en-US");

                //Culture for any thread
                CultureInfo.DefaultThreadCurrentCulture = culture;

                //Culture for UI in any thread
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
            else
            {
                gvArticulos.Columns["precio_unitario"].Visible = true;
                gvArticulos.Columns["total"].Visible = true;

                gvArticulos.Columns["precio_unitario_moneda2"].Visible = false;
                gvArticulos.Columns["total_moneda2"].Visible = false;

                gvArticulos.Columns["articulo"].VisibleIndex = 0;
                gvArticulos.Columns["descripcion_articulo"].VisibleIndex = 1;
                gvArticulos.Columns["descripcion_unidad"].VisibleIndex = 2;
                gvArticulos.Columns["precio_unitario"].VisibleIndex = 3;
                gvArticulos.Columns["precio_unitario_moneda2"].VisibleIndex = -1;
                gvArticulos.Columns["cantidad"].VisibleIndex = 4;
                gvArticulos.Columns["descuento"].VisibleIndex = -1;
                gvArticulos.Columns["descuento_moneda2"].VisibleIndex = -1;
                gvArticulos.Columns["base_imponible"].VisibleIndex = -1;
                gvArticulos.Columns["base_imponible_moneda2"].VisibleIndex = -1;
                gvArticulos.Columns["impuesto"].VisibleIndex = -1;
                gvArticulos.Columns["impuesto_moneda2"].VisibleIndex = -1;
                gvArticulos.Columns["total"].VisibleIndex = 5;
                gvArticulos.Columns["total_moneda2"].VisibleIndex = -1;

                var culture = CultureInfo.CreateSpecificCulture(parametros.Configuracion.moneda_principal);
                culture.NumberFormat.CurrencyPositivePattern = 2;

                // this may fail sometimes: (see Drachenkatze's comment below)
                // var culture = new CultureInfo("en-US");

                //Culture for any thread
                CultureInfo.DefaultThreadCurrentCulture = culture;

                //Culture for UI in any thread
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                moneda_para_mostrar = "principal";
            }
        }

        private void hlArticulos_Click(object sender, EventArgs e)
        {
            lista_articulos();
        }

        private void hlCantidad_Click(object sender, EventArgs e)
        {
            actualizar_cantidad();
        }

        private void hlPrecios_Click(object sender, EventArgs e)
        {
            xfrm_Documento_Precios frmPrecios = new xfrm_Documento_Precios();
            frmPrecios.codigoArticulo = ((documento_venta_item)documento_venta_itemBindingSource.Current).articulo;
            frmPrecios.unidad = ((documento_venta_item)documento_venta_itemBindingSource.Current).unidad;
            frmPrecios.ShowDialog();
        }

        private void hlStock_Click(object sender, EventArgs e)
        {
            ver_stock();
        }

        #endregion

        #region Configuración de GridView

        private void gvArticulos_DoubleClick(object sender, EventArgs e)
        {
            subir_articulo();
        }
        private void gvArticulos_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
            {
                int rowHandle = e.HitInfo.RowHandle;
                // Delete existing menu items, if any.
                e.Menu.Items.Clear();
                // Add the Rows submenu with the 'Delete Row' command
                e.Menu.Items.Add(CreateMenuCantidad(view, rowHandle));
                e.Menu.Items.Add(CreateMenuAlmacen(view, rowHandle));
                e.Menu.Items.Add(CreateMenuQuitar(view, rowHandle));
                //e.Menu.Items.Add(CreateMenuSeparador(view, rowHandle));

                // Add the 'Cell Merging' check menu item.
                //DXMenuItem item = CreateMenuItemCellMerging(view, rowHandle);
                //item.BeginGroup = true;
                //e.Menu.Items.Add(item);
            }

            //DXMenuCheckItem CreateCheckItem(string caption, GridColumn column, Image image)
            //{
            //    DXMenuCheckItem item = new DXMenuCheckItem(caption,
            //      !column.OptionsColumn.AllowMove, image, new EventHandler(OnCanMoveItemClick));
            //    item.Tag = new MenuColumnInfo(column);
            //    return item;
            //}
        }
        private void gvArticulos_RowStyle(object sender, RowStyleEventArgs e)
        {
            e.HighPriority = true;
            GridView View = sender as GridView;

            if (e.RowHandle >= 0)
            {
                if (View.GetRowCellValue(e.RowHandle, colalmacen).ToString() != Almacen.codigo)
                {
                   e.Appearance.ForeColor = Color.Blue;
                }
            }

            if (e.RowHandle % 2 == 0)
            {
                //e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#7bed9f");
            }
            else
            {
                e.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml("#f8f9fa");
                //e.Appearance.BackColor2 = System.Drawing.ColorTranslator.FromHtml("#20bf6b");
            }

            if ((e.State & DevExpress.XtraGrid.Views.Base.GridRowCellState.Selected) != 0)
            {
                // check some conditions
                //e.HighPriority = true;
                e.Appearance.BackColor = ColorTranslator.FromHtml("#a9d6e5");
                //e.Appearance.ForeColor = Color.White;
            }
        }
        private void gvArticulos_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
        }
        private void gvArticulos_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //if (documento_venta_itemBindingSource.Count > 0)
            //{
            //    GridView view = sender as GridView;

            //    if (e.Column.FieldName == "unidad")
            //    {
            //        rilupUnidad.DisplayMember = "descripcion";
            //        rilupUnidad.ValueMember = "codigo";

            //        LookUpColumnInfo c1 = new LookUpColumnInfo();
            //        c1.FieldName = "descripcion";

            //        rilupUnidad.Columns.Clear();

            //        rilupUnidad.Columns.Add(c1);

            //        rilupUnidad.DataSource = Lista_Unidades.Where(x => x.id_articulo == Convert.ToInt32(gvArticulos.GetFocusedRowCellValue("id_articulo").ToString()));
            //    }
            //}
        }
        private void gvArticulos_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            cantidad_anterior = ((documento_venta_item)documento_venta_itemBindingSource[gvArticulos.FocusedRowHandle]).cantidad;
            unidad_anterior = ((documento_venta_item)documento_venta_itemBindingSource[gvArticulos.FocusedRowHandle]).unidad;
        }
        private void rilupUnidad_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                LookUpEdit lookUpEdit = (LookUpEdit)sender;

                if (!validar_existencia(Articulo, 0, null, lookUpEdit.EditValue.ToString()))
                {
                    return;
                }

                ((documento_venta_item)documento_venta_itemBindingSource[gvArticulos.FocusedRowHandle]).unidad = lookUpEdit.EditValue.ToString();

                calcular_total_item();
            }
            catch (FaultException FEx)
            {
                MessageBox.Show("Se produjo un error al eliminar la información temporal del vendedor.", "SUITE - Facturac de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Se produjo un error al eliminar la información temporal del vendedor.", "SUITE - Facturac de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void rilupUnidad_EditValueChanging(object sender, ChangingEventArgs e)
        {
            cantidad_anterior = ((documento_venta_item)documento_venta_itemBindingSource.Current).cantidad;
            unidad_anterior = e.OldValue.ToString();
            unidad_nueva = e.NewValue.ToString();

            svc.Eliminar_Existencia_Comprometida(parametros.Sucursal.codigo, parametros.Usuario.codigo, parametros.Vendedor.codigo, "factura", ((documento_venta_item)documento_venta_itemBindingSource.Current).id_articulo, ((documento_venta_item)documento_venta_itemBindingSource.Current).unidad);

        }

        #endregion

        #region Popup Menu

        private void bbiItemsDetalle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (documento_venta_itemBindingSource.Count > 0)
            {
                xfrm_Documento_Detalle_Articulo frm = new xfrm_Documento_Detalle_Articulo();
                frm.Articulo = ((documento_venta_item)documento_venta_itemBindingSource.Current).descripcion_articulo;
                frm.Detalle = ((documento_venta_item)documento_venta_itemBindingSource.Current).detalle_articulo;
                frm.ShowDialog();

                if (frm.Detalle != null)
                {
                    ((documento_venta_item)documento_venta_itemBindingSource.Current).detalle_articulo = frm.Detalle;
                }
            }
        }

        private void bbiItemsQuitar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            quitar_articulo();
        }

        private void barManagerItems_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridView view = gcArticulos.FocusedView as GridView;
            if (e.Item.Caption == "Show field and row handle")
                MessageBox.Show("Field = " + column.FieldName + ", RowHandle = " + rowHandle);
            if (e.Item.Caption == "RemoveCurrentRow")
                view.DeleteRow(gvArticulos.FocusedRowHandle);
        }

        DXMenuItem CreateMenuCantidad(GridView view, int rowHandle)
        {
            DXMenuItem menuItemCantidad = new DXMenuItem("CANTIDAD", new EventHandler(OnCantidadRowClick), imageCollection1.Images[1]);
            menuItemCantidad.Tag = new RowInfo(view, rowHandle);
            menuItemCantidad.Enabled = view.IsDataRow(rowHandle) || view.IsGroupRow(rowHandle);
            return menuItemCantidad;
        }

        DXMenuItem CreateSubMenuImpuesto(GridView view, int rowHandle)
        {
            //DXSubMenuItem dxsm = new DXSubMenuItem("TODOS");

            //DXMenuItem menuItemExentos = new DXMenuItem("EXENTO", new EventHandler(OnTodoExentoRowClick)); //, imageCollection1.Images[0]);
            //menuItemExentos.Tag = new RowInfo(view, rowHandle);
            //menuItemExentos.Enabled = view.IsDataRow(rowHandle) || view.IsGroupRow(rowHandle);
            //dxsm.Items.Add(menuItemExentos);

            //DXMenuItem menuItemGravables = new DXMenuItem("GRAVABLE", new EventHandler(OnTodoGravableRowClick)); //, imageCollection1.Images[0]);
            //menuItemGravables.Tag = new RowInfo(view, rowHandle);
            //menuItemGravables.Enabled = view.IsDataRow(rowHandle) || view.IsGroupRow(rowHandle);
            //dxsm.Items.Add(menuItemGravables);

            //return dxsm;

            return null;
        }

        DXMenuItem CreateMenuAlmacen(GridView view, int rowHandle)
        {
            DXMenuItem menuItemCantidad = new DXMenuItem("ALMACÉN", new EventHandler(OnAlmacenRowClick), imageCollection1.Images[0]);
            menuItemCantidad.Tag = new RowInfo(view, rowHandle);
            menuItemCantidad.Enabled = view.IsDataRow(rowHandle) || view.IsGroupRow(rowHandle);
            return menuItemCantidad;
        }

        DXMenuItem CreateMenuQuitar(GridView view, int rowHandle)
        {
            DXMenuItem menuItemCantidad = new DXMenuItem("QUITAR", new EventHandler(OnQuitarRowClick), imageCollection1.Images[2]); menuItemCantidad.Tag = new RowInfo(view, rowHandle);
            menuItemCantidad.Enabled = view.IsDataRow(rowHandle) || view.IsGroupRow(rowHandle);
            return menuItemCantidad;
        }

        DXMenuItem CreateMenuSeparador(GridView view, int rowHandle)
        {
            DXMenuHeaderItem menuItem = new DXMenuHeaderItem(); // ("IMPUESTOS DE ARTICULOS");
            menuItem.Caption = "IMPUESTOS DE ARTICULOS";
            menuItem.Enabled = view.IsDataRow(rowHandle) || view.IsGroupRow(rowHandle);
            return menuItem;
        }

        void OnQuitarRowClick(object sender, EventArgs e)
        {
            if (documento_venta_itemBindingSource.Count > 0)
            {
                quitar_articulo();
            }
        }

        void OnCantidadRowClick(object sender, EventArgs e)
        {
            DXMenuItem menuItem = sender as DXMenuItem;
            RowInfo ri = menuItem.Tag as RowInfo;
            if (ri != null)
            {
                actualizar_cantidad();
            }
        }

        void OnAlmacenRowClick(object sender, EventArgs e)
        {
            DXMenuItem menuItem = sender as DXMenuItem;
            RowInfo ri = menuItem.Tag as RowInfo;
            if (ri != null)
            {
                cambio_almacen();
            }
        }

        #endregion

        private void gcArticulos_Click(object sender, EventArgs e)
        {

        }

        private void btnVentanaAuxiliar_Click(object sender, EventArgs e)
        {
            try
            {
                Screen[] screens = Screen.AllScreens;

                if (screens.Length > 1)
                {
                    xfrm_Documento_Venta_Auxiliar formulario = new xfrm_Documento_Venta_Auxiliar();
                    formulario.Location = screens[1].WorkingArea.Location;
                    formulario.Show();
                }
            }
            catch(Exception ex)
            { }
        }

        private void documento_ventaBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            parametros.documento_venta_auxiliar = (documento_venta)documento_ventaBindingSource.Current;
        }

        private void documento_venta_itemBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            parametros.documento_venta_auxiliar.Documento_Venta_Items = documento_venta_itemBindingSource.OfType<documento_venta_item>().ToArray();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            actualizar_cantidad();
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            cambiar_unidad();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ver_stock();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            xfrm_Documento_Venta_Consulta formulario = new xfrm_Documento_Venta_Consulta();
            formulario.almacen = ((documento_venta)documento_ventaBindingSource.Current).codigo_almacen;
            formulario.ShowDialog();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(((documento_venta)documento_ventaBindingSource.Current).codigo_cliente) || documento_venta_itemBindingSource.Count > 0)
            {
                DialogResult mensaje = utilidades.MensajeError("¿Dese cancelar el documento actual?.", null, null, "SUITE - Factura de venta", "Pregunta");

                if (mensaje == DialogResult.Yes)
                {
                    //xfrm_Clave_Autorizacion auto = new xfrm_Clave_Autorizacion();
                    //auto.tipo_operacion = "cancelar_factura";
                    //auto.ShowDialog();

                    //if(auto.Autorizado)
                    //{
                    //    MessageBox.Show("autorizado");
                    //}

                    documento_ventaBindingSource.Clear();
                    documento_venta_itemBindingSource.Clear();
                    documento_ventaBindingSource.AddNew();

                    parametros.Configuracion = svc.Buscar_Configuracion(parametros.Sucursal.codigo);

                    if (parametros.Configuracion.proximo_factura.ToString().Length < 10)
                    {
                        //lblProximoDocumento.Text = parametros.Configuracion.proximo_factura.ToString().PadLeft(8, '0') + "  ";
                    }

                    cargar_configuracion_de_ventas();

                    lblIdentificacion.Text = string.Empty;
                    lblRazonSocial.Text = string.Empty;
                }
            }
            else
            {
                this.Close();
            }
        }

        private void btnArticulos_Click(object sender, EventArgs e)
        {
            lista_articulos();
        }

        private void btnCambios_Click(object sender, EventArgs e)
        {
            xfrm_Gestion_Cambios frmCambios = new xfrm_Gestion_Cambios();
            frmCambios.ShowDialog();
        }

        #endregion


    }
}