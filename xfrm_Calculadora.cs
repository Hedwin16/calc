using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Suite
{
    public partial class xfrm_Calculadora : DevExpress.XtraEditors.XtraForm
    {
        public decimal cantidad { get; set; }
        public bool modo_decimal { get; set; }
        public string tipoOperacion { get; set; }
        public decimal resultado { get; set; }
        public decimal num1 { get; set; }
        public decimal num2 { get; set; }
        public bool esResultado { get; set; }

        public double negativo;

        public bool? esOperador { get; set; }
        public xfrm_Calculadora()
        {
            InitializeComponent();
            resultado = 0m;
            num1 = 0m;
            num2 = 0m;
            esOperador = null;

            (new TabOrderManager(this)).SetTabOrder(TabOrderManager.TabScheme.DownFirst);
            txtCantidad.EditValue = 0m;
            negativo = 0d;
        }

        private void xfrm_Documento_Cantidad_Articulo_Touch_Load(object sender, EventArgs e)
        { 

        }

        private void custom_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("la tecla presionada es: " + e.KeyData.ToString());
            //Console.WriteLine("la tecla presionada es: "+e.KeyData.ToString());

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
                case "Return":
                    if (txtCantidad.Text != "")
                    {
                        if (resultado != 0)
                        {
                            num1 = resultado;
                        }

                        try
                        {
                            CalcularDesdeResultado();
                            txtCantidad.SelectAll();
                        }
                        catch (Exception ex)
                        {
                            utilidades.MensajeError("Formato de número no válido", null, null, "Integra - Calculadora", "Error");
                            reiniciarDatos();
                        }



                    }
                    break;

                case "OemMinus":
                    if(txtCantidad.Text.Contains("-"))
                        e.SuppressKeyPress = true;
                    break;
                case "Oemcomma":
                    if (txtCantidad.Text.Contains(","))
                        e.SuppressKeyPress = true;
                    break;

                case "Delete":
                    reiniciarDatos();
                    txtCantidad.SelectAll();
                    break;
                case "Home":
                    break;
                case "End":
                    break;
                case "PageUp":
                    break;
                case "Next":
                    break;
                case "Subtract":
                    e.SuppressKeyPress = true;
                    if (txtCantidad.Text == "" || txtCantidad.Text == "0")
                    {
                        setCambioOperador("-");
                        return;
                    }
                    asignarVariables("-");
                    txtCantidad.SelectAll();
                    Console.WriteLine("Substract Presionado");

                    break;
                case "Divide":
                    if (txtCantidad.Text == "" || txtCantidad.Text == "0")
                    {
                        setCambioOperador("/");
                        return;
                    }
                        
                    asignarVariables("/");

                    txtCantidad.SelectAll();
                    break;
                case "Add":
                    if (txtCantidad.Text == "" || txtCantidad.Text == "0")
                    {
                        setCambioOperador("+");
                        return;
                    }
                        
                    asignarVariables("+");
                    txtCantidad.SelectAll();
                    break;
                case "Multiply":
                    if (txtCantidad.Text == "" || txtCantidad.Text == "0")
                    {
                        setCambioOperador("*");
                        return;
                    }
                    asignarVariables("*");
                    
                    txtCantidad.SelectAll();
                    break;
                case "Decimal":
                    if (!txtCantidad.Text.Contains(","))
                        SendKeys.Send(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
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

        private void actualizar_cantidad(string numero)
        {
            try
            {
                if (txtCantidad.Text == "0") { txtCantidad.Text = ""; }

                //Verifica si el modo decimal ya está ingresado

                if (numero == "," && txtCantidad.Text.Contains(",")) { return;}
                
                if (numero == "," && !txtCantidad.Text.Contains(","))
                {
                    numero = ",";
                    modo_decimal = true;
                    if (txtCantidad.Text == "")
                        txtCantidad.Text = "0";
                }

                if (esResultado == true)
                {
                    txtCantidad.Text = "";
                    esResultado = false;
                }
                esOperador = false;

                txtCantidad.EditValue = txtCantidad.EditValue + numero;

            }
            catch (Exception ex)
            {
                string msj = "Exception: " + ex.Message;
                utilidades.MensajeError(msj, null, null, "Integra - Excepción ", "Advertencia");
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            actualizar_cantidad(btn.Text);
        }

        private void Operacion_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            

            if (txtCantidad.Text != "" && txtCantidad.Text != "0")
            {
                asignarVariables(btn.Text);
                return;
            }
                
            if (btn.Text != tipoOperacion)
            {
                txtOperaciones.Text = num1 + " " + btn.Text;
                tipoOperacion = btn.Text;
                esOperador = true;
                return;
            }

            //asignarTipoOperacion(btn.Text);

        }

        private void asignarVariables(string operador)
        {
            try
            {

                if (num1 == 0m)
                {
                    if(esOperador == true)
                    {
                        
                        tipoOperacion = operador;
                        txtOperaciones.Text = txtCantidad.Text + " " + tipoOperacion;
                        return;
                    }
                    num1 = Convert.ToDecimal(txtCantidad.Text);

                    modo_decimal = false;
                    txtCantidad.Text = "";
                    if (resultado != 0m)
                    {

                        num2 = num1;

                        num1 = resultado;
                        txtCantidad.Text = "0";
                        ObtenerResultado();
                        mostrarResultado();
                        num1 = 0;
                    }
                    tipoOperacion = operador;
                    if (num1 != 0m)
                    {
                        txtOperaciones.Text = num1 + " " + tipoOperacion;
                    }

                    else
                    {
                        txtOperaciones.Text = txtCantidad.Text + " " + tipoOperacion;
                    }
                        
                    return;
                }

                num2 = Convert.ToDecimal(txtCantidad.Text);
                ObtenerResultado();
                mostrarResultado();
                num1 = 0;
                tipoOperacion = operador;
                txtOperaciones.Text = resultado + " " + tipoOperacion;
            }
            catch (Exception ex)
            {
                utilidades.MensajeError("Se produjo un error en la cantidad ", ex.Message, null, "Integra - Calculadora", "Error");
                reiniciarDatos();

            }

        }

        private void mostrarResultado()
        {

            txtCantidad.EditValue = resultado;
            esResultado = true;


        }

        private void ObtenerResultado()
        {
            if (txtCantidad.Text != "")
            {
                try
                {
                    //cantidad = Convert.ToDecimal(txtCantidad.EditValue);

                    switch (tipoOperacion)
                    {
                        case "+":
                            txtCantidad.Text = decimal.Round((decimal)num1 + (decimal)num2, 3).ToString();
                            resultado = Convert.ToDecimal(txtCantidad.Text);
                            esOperador = true;

                            break;
                        case "-":
                            txtCantidad.Text = decimal.Round((decimal)num1 - (decimal)num2, 3).ToString();
                            resultado = Convert.ToDecimal(txtCantidad.Text);
                            esOperador = true;

                            break;
                        case "*":
                            txtCantidad.Text = decimal.Round((decimal)num1 * (decimal)num2, 3).ToString();
                            resultado = Convert.ToDecimal(txtCantidad.Text);
                            esOperador = true;

                            break;
                        case "/":

                            if (num2 != 0)
                            {
                                txtCantidad.Text = decimal.Round((decimal)num1 / (decimal)num2, 3).ToString();
                                resultado = Convert.ToDecimal(txtCantidad.Text);
                                esOperador = true;

                            }
                            else
                            {
                                utilidades.MensajeError("No se puede dividir número entre cero (0)", null, null, "Integra - Calculadora", "Advertencia");
                                return;
                            }

                            break;


                    }


                    //num1 = Convert.ToDecimal(txtCantidad.Text);
                    modo_decimal = false;
                    esOperador = true;


                }
                catch (Exception ex)
                {
                    utilidades.MensajeError("Se produjo un error en la cantidad ", ex.Message, null, "Suite - Calculadora", "Error");
                    modo_decimal = false;
                }

            }

        }

        private void Resultado_Click(object sender, EventArgs e)
        {
            //ObtenerResultado();
            if (txtCantidad.Text != "")
            {

                num1 = (resultado != 0) ? resultado:num1;
                CalcularDesdeResultado();

            }


        }

        private void CalcularDesdeResultado()
        {
            num2 = Convert.ToDecimal(txtCantidad.Text);
            ObtenerResultado();
            mostrarResultado();
            num1 = 0;
            tipoOperacion = "";
            txtOperaciones.Text = txtCantidad.Text + " " + tipoOperacion;
            reiniciarDatos(true);
            esResultado = true;
            esOperador = false;
        }

        private void simpleButtonMultiplicacion_Click(object sender, EventArgs e)
        {

        }

        private void xfrm_Calculadora_Load(object sender, EventArgs e)
        {

        }

        private void txtCantidad_Click(object sender, EventArgs e)
        {
            txtCantidad.SelectAll();
        }

        private void simpleButtonDel_Click(object sender, EventArgs e)
        {
            reiniciarDatos();
            //(new TabOrderManager(this)).SetTabOrder(TabOrderManager.TabScheme.DownFirst);

        }

        private void reiniciarDatos(bool enter = false)
        {
            txtCantidad.EditValue = !enter ? 0m : txtCantidad.EditValue;
            modo_decimal = false;
            num1 = 0m;
            num2 = 0m;
            resultado = 0m;
            tipoOperacion = "";
            txtOperaciones.Text = "";
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButtonBack_Click(object sender, EventArgs e)
        {

            try
            {

                if (txtCantidad.Text.Length > 1)
                {
                    txtCantidad.Text = txtCantidad.Text.Substring(0, txtCantidad.Text.Length - 1);
                    esResultado = false;
                }
                else
                {
                    txtCantidad.Text = "0";
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            }

        }

        private void txtCantidad_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSigno_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCantidad.Text))
            {
                try
                {
                    negativo = Convert.ToDouble(txtCantidad.Text);
                    negativo *= -1;
                    txtCantidad.Text = negativo.ToString();
                }
                catch (Exception ex)
                {
                    utilidades.MensajeError("Error en la cantidad ingresada ", null, null, "SUITE - Error al convertir decimal", "Error");
                    return;
                }
                
            }
            
        }
        private void ComprobarEntrada(object sender, EventArgs e)
        {
            if(txtCantidad.Text == "-")
            {
                txtCantidad.Text = "0";
            }
        }

        private void xfrm_Calculadora_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                //The char is not a number or a control key
                //Handle the event so the key press is accepted
                esOperador = false;
                //e.Handled = true;
                //Get out of there - make it safe to add stuff after the if statement
                return;
            }
        }

        private void setCambioOperador(string op)
        {
            if (txtCantidad.Text == "" || txtCantidad.Text == "0")
            {
                if (op != tipoOperacion)
                {
                    txtOperaciones.Text = num1 + " " + op;
                    tipoOperacion = op;
                    esOperador = true;
                    return;
                }
                return;
            }
        }
    }
}