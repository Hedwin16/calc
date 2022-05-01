using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormCalculator
{
    public partial class Calculator : Form
    {

        double Numero1 = 0, Numero2 = 0;
        decimal num1 = 0, num2 = 0;
        char Operador;
        public string tipoOperacion { get; set; }
        public bool modo_decimal { get; set; }
        public decimal cantidad { get; set; }
        public bool esResultado { get; set; }
        public decimal resultado { get; set; }
        public bool btnDelActivo { get; set; }


        public Calculator()
        {
            InitializeComponent();
            resultado = 0m;
            num1 = 0m;
            num2 = 0m;
            btnDelActivo = false;

            txtCantidad.Text = "0";
        }

        private void agregarNumero(object sender, EventArgs e)
        {

            var boton = ((Button)sender);

            // if (txtResultado.Text == "0")
            //     txtResultado.Text = "";

            // txtResultado.Text += boton.Text;
            actualizar_cantidad(boton.Text);


        }

        private void actualizar_cantidad(string numero)
        {
            try
            {
                if (txtCantidad.Text == "0")
                    txtCantidad.Text = "";
                //Verifica si el modo decimal ya está ingresado
                if (numero == "," && modo_decimal)
                {
                    numero = "";
                }

                if (numero == "," && !modo_decimal)
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


                //numero = (modo_decimal) ? "" : ",";  

                txtCantidad.Text = txtCantidad.Text + numero;

            }
            catch (Exception ex)
            {
                string msj = "Exception: " + ex.Message;
                //utilidades.MensajeError(msj, null, null, "Integra - Excepción ", "Advertencia");
                System.Console.WriteLine("Integra - Exepcion: " + msj);
            }
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

                            break;
                        case "-":
                            txtCantidad.Text = decimal.Round((decimal)num1 - (decimal)num2, 3).ToString();
                            resultado = Convert.ToDecimal(txtCantidad.Text);

                            break;
                        case "*":
                            txtCantidad.Text = decimal.Round((decimal)num1 * (decimal)num2, 3).ToString();
                            resultado = Convert.ToDecimal(txtCantidad.Text);

                            break;
                        case "/":

                            if (num2 != 0)
                            {
                                txtCantidad.Text = decimal.Round((decimal)num1 / (decimal)num2, 3).ToString();
                                resultado = Convert.ToDecimal(txtCantidad.Text);

                            }
                            else
                            {
                                MessageBox.Show("Se produjo un error en la cantidad ingresada: ", "Integra - Calculadora - Errorrr");
                                return;
                            }

                            break;


                    }


                    //num1 = Convert.ToDecimal(txtCantidad.Text);
                    modo_decimal = false;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se produjo un error en la cantidad ingresada: " + ex, "Integra - Calculadora - Errorrr");
                    modo_decimal = false;
                }

            }

        }
        private void btnResultado_Click(object sender, EventArgs e)
        {
            /*
            Numero2 = Convert.ToDouble(txtCantidad.Text);

            if (Operador == '+')
            {
            txtCantidad.Text = (Numero1 + Numero2).ToString();
            Numero1 = Convert.ToDouble(txtCantidad.Text);
            }
            else if (Operador == '−')
            {
            txtCantidad.Text = (Numero1 - Numero2).ToString();
            Numero1 = Convert.ToDouble(txtCantidad.Text);
            }
            else if (Operador == 'X')
            {
            txtCantidad.Text = (Numero1 * Numero2).ToString();
            Numero1 = Convert.ToDouble(txtCantidad.Text);
            }
            else if (Operador == '∕')
            {
            if (txtCantidad.Text != "0")
            {
            txtCantidad.Text = (Numero1 / Numero2).ToString();
            Numero1 = Convert.ToDouble(txtCantidad.Text);
            }
            else
            {
            MessageBox.Show("No se puede dividir por cero!!");
            }
            }
            */
            if (txtCantidad.Text != "")
            {
                if (resultado != 0)
                {
                    num1 = resultado;
                }

                num2 = Convert.ToDecimal(txtCantidad.Text);
                ObtenerResultado();
                mostrarResultado();
                num1 = 0;
                tipoOperacion = "";
                txtOperaciones.Text = txtCantidad.Text + " " + tipoOperacion;
                reiniciarDatos(true);
                /*
                ObtenerResultado();
                mostrarResultado();
                num1 = 0m;*/
            }


        }

        private void btnQuitar_Click(object sender, EventArgs e)
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

        private void btnBorrarTodo_Click(object sender, EventArgs e)
        {
            reiniciarDatos(false);
            //Numero1 = 0;
            //Numero2 = 0;
            //Operador = '\0';
            //txtCantidad.Text = "0";
        }

        private void reiniciarDatos(bool enter = false)
        {
            
            txtCantidad.Text = (!enter) ? "0": txtCantidad.Text;
            modo_decimal = false;
            num1 = 0m;
            num2 = 0m;
            resultado = 0m;
            tipoOperacion = "";
            txtOperaciones.Text = "";
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            txtCantidad.Text = "0";
        }

        private void btnPunto_Click(object sender, EventArgs e)
        {
            if (!txtCantidad.Text.Contains("."))
            {
                txtCantidad.Text += ".";
            }
        }

        private void btnSigno_Click(object sender, EventArgs e)
        {
            Numero1 = Convert.ToDouble(txtCantidad.Text);
            Numero1 *= -1;
            txtCantidad.Text = Numero1.ToString();
        }

        private void clickOperador(object sender, EventArgs e)
        {
            /*var boton = ((Button)sender);
            Numero1 = Convert.ToDouble(txtCantidad.Text);
            Operador = Convert.ToChar(boton.Tag);

            if (Operador == '²')
            {
            Numero1 = Math.Pow(Numero1, 2);
            txtCantidad.Text = Numero1.ToString();
            }
            else if (Operador == '√')
            {
            Numero1 = Math.Sqrt(Numero1);
            txtCantidad.Text = Numero1.ToString();
            }
            else
            {
            txtCantidad.Text = "0";
            }*/

            var btn = ((Button)sender);
            asignarVariables(btn.Text);
            esResultado = true;


        }

        private void asignarVariables(string operador)
        {
            try
            {
                if (num1 == 0m)
                {
                    num1 = Convert.ToDecimal(txtCantidad.Text);

                    modo_decimal = false;
                    //txtCantidad.Text = "";
                    if (resultado != 0m)
                    {

                        num1 = resultado;
                        num2 = Convert.ToDecimal(txtCantidad.Text);

                        txtCantidad.Text = "0";
                        ObtenerResultado();
                        mostrarResultado();
                        num1 = 0;
                    }
                    tipoOperacion = operador;
                    txtOperaciones.Text = txtCantidad.Text + " " + tipoOperacion;
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
                MessageBox.Show("Error Calculadora: "+ex);
                reiniciarDatos(false);

            }


        }

        private void mostrarResultado()
        {
            if (btnDelActivo)
            {
                try
                {
                    resultado = Convert.ToDecimal(txtCantidad.Text);
                    txtCantidad.Text = resultado.ToString();
                    esResultado = true;
                    btnDelActivo = false;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se produjo un error en la cantidad ingresada: " + ex, "Integra - Calculadora - Errorrr");
                }



            }
            else
            {
                txtCantidad.Text = resultado.ToString();
                esResultado = true;


            }

        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
