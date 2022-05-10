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

namespace Suite
{
    public partial class xfrmCalendario : DevExpress.XtraEditors.XtraForm
    {
        #region Atributos, Propiedades y Referencias
        public DateTime? fecha { get; set; }

        #endregion


        public xfrmCalendario()
        {
            InitializeComponent();
        }

        private void calendarControl1_Click(object sender, EventArgs e)
        {

        }

        private void xfrmCalendario_Load(object sender, EventArgs e)
        {

        }

        private void xfrmCalendario_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void calendarControl1_DoubleClick(object sender, EventArgs e)
        {
            fecha = calendarControl1.DateTime;

            this.Close();
        }
    }
}