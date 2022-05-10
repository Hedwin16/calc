namespace Suite
{
    partial class xfrm_Documento_Cantidad_Articulo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtCantidad = new DevExpress.XtraEditors.TextEdit();
            this.lupUnidades = new DevExpress.XtraEditors.LookUpEdit();
            this.unidadBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblUnidad = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lupLotes = new DevExpress.XtraEditors.LookUpEdit();
            this.loteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.existenciaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txtCantidadLote = new DevExpress.XtraEditors.TextEdit();
            this.lblCantidad = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtFechaVence = new DevExpress.XtraEditors.TextEdit();
            this.btnAgregarLote = new DevExpress.XtraEditors.SimpleButton();
            this.lblFechaVence = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.meDescripcionArticulo = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCantidad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lupUnidades.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unidadBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lupLotes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loteBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.existenciaBindingSource)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCantidadLote.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaVence.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.meDescripcionArticulo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCantidad
            // 
            this.txtCantidad.EditValue = "";
            this.txtCantidad.Location = new System.Drawing.Point(18, 247);
            this.txtCantidad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCantidad.Properties.Appearance.Options.UseFont = true;
            this.txtCantidad.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCantidad.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCantidad.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.txtCantidad.Properties.AppearanceDisabled.Options.UseFont = true;
            this.txtCantidad.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.txtCantidad.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCantidad.Properties.AppearanceFocused.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.txtCantidad.Properties.AppearanceFocused.Options.UseFont = true;
            this.txtCantidad.Properties.AppearanceFocused.Options.UseTextOptions = true;
            this.txtCantidad.Properties.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCantidad.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.txtCantidad.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.txtCantidad.Properties.AppearanceReadOnly.Options.UseTextOptions = true;
            this.txtCantidad.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCantidad.Properties.Mask.EditMask = "n3";
            this.txtCantidad.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCantidad.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCantidad.Properties.NullText = "0";
            this.txtCantidad.Size = new System.Drawing.Size(399, 66);
            this.txtCantidad.TabIndex = 6;
            this.txtCantidad.Click += new System.EventHandler(this.txtCantidad_Click);
            this.txtCantidad.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            this.txtCantidad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCantidad_KeyPress);
            // 
            // lupUnidades
            // 
            this.lupUnidades.EnterMoveNextControl = true;
            this.lupUnidades.Location = new System.Drawing.Point(18, 124);
            this.lupUnidades.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lupUnidades.Name = "lupUnidades";
            this.lupUnidades.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.lupUnidades.Properties.Appearance.Options.UseFont = true;
            this.lupUnidades.Properties.Appearance.Options.UseTextOptions = true;
            this.lupUnidades.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lupUnidades.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.lupUnidades.Properties.AppearanceDisabled.Options.UseFont = true;
            this.lupUnidades.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.lupUnidades.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lupUnidades.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.lupUnidades.Properties.AppearanceDropDown.Options.UseFont = true;
            this.lupUnidades.Properties.AppearanceDropDownHeader.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.lupUnidades.Properties.AppearanceDropDownHeader.Options.UseFont = true;
            this.lupUnidades.Properties.AppearanceFocused.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.lupUnidades.Properties.AppearanceFocused.Options.UseFont = true;
            this.lupUnidades.Properties.AppearanceFocused.Options.UseTextOptions = true;
            this.lupUnidades.Properties.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lupUnidades.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.lupUnidades.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.lupUnidades.Properties.AppearanceReadOnly.Options.UseTextOptions = true;
            this.lupUnidades.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lupUnidades.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lupUnidades.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descripcion", "Name2")});
            this.lupUnidades.Properties.DataSource = this.unidadBindingSource;
            this.lupUnidades.Properties.DisplayMember = "descripcion";
            this.lupUnidades.Properties.NullText = "";
            this.lupUnidades.Properties.ShowHeader = false;
            this.lupUnidades.Properties.ValueMember = "codigo";
            this.lupUnidades.Size = new System.Drawing.Size(399, 66);
            this.lupUnidades.TabIndex = 5;
            this.lupUnidades.EditValueChanged += new System.EventHandler(this.lupUnidades_EditValueChanged);
            this.lupUnidades.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // unidadBindingSource
            // 
            this.unidadBindingSource.DataSource = typeof(Suite.ServicioSuite.unidad);
            // 
            // lblUnidad
            // 
            this.lblUnidad.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnidad.Appearance.Options.UseFont = true;
            this.lblUnidad.Location = new System.Drawing.Point(18, 88);
            this.lblUnidad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblUnidad.Name = "lblUnidad";
            this.lblUnidad.Size = new System.Drawing.Size(155, 24);
            this.lblUnidad.TabIndex = 2;
            this.lblUnidad.Text = "UNIDAD DE VENTA";
            this.lblUnidad.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(18, 211);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(87, 24);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "CANTIDAD";
            this.labelControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(18, 27);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(173, 24);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "LOTES DEL ARTÍCULO";
            this.labelControl2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // lupLotes
            // 
            this.lupLotes.EnterMoveNextControl = true;
            this.lupLotes.Location = new System.Drawing.Point(18, 63);
            this.lupLotes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lupLotes.Name = "lupLotes";
            this.lupLotes.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold);
            this.lupLotes.Properties.Appearance.Options.UseFont = true;
            this.lupLotes.Properties.Appearance.Options.UseTextOptions = true;
            this.lupLotes.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lupLotes.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold);
            this.lupLotes.Properties.AppearanceDisabled.Options.UseFont = true;
            this.lupLotes.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.lupLotes.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lupLotes.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold);
            this.lupLotes.Properties.AppearanceDropDown.Options.UseFont = true;
            this.lupLotes.Properties.AppearanceDropDownHeader.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold);
            this.lupLotes.Properties.AppearanceDropDownHeader.Options.UseFont = true;
            this.lupLotes.Properties.AppearanceFocused.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold);
            this.lupLotes.Properties.AppearanceFocused.Options.UseFont = true;
            this.lupLotes.Properties.AppearanceFocused.Options.UseTextOptions = true;
            this.lupLotes.Properties.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lupLotes.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lupLotes.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.lupLotes.Properties.AppearanceReadOnly.Options.UseTextOptions = true;
            this.lupLotes.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lupLotes.Properties.AutoHeight = false;
            this.lupLotes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lupLotes.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Name2")});
            this.lupLotes.Properties.DataSource = this.loteBindingSource;
            this.lupLotes.Properties.DisplayMember = "codigo";
            this.lupLotes.Properties.NullText = "";
            this.lupLotes.Properties.ShowHeader = false;
            this.lupLotes.Properties.ValueMember = "codigo";
            this.lupLotes.Size = new System.Drawing.Size(399, 72);
            this.lupLotes.TabIndex = 2;
            this.lupLotes.EditValueChanged += new System.EventHandler(this.lupLotes_EditValueChanged);
            this.lupLotes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // loteBindingSource
            // 
            this.loteBindingSource.DataSource = typeof(Suite.ServicioSuite.lote);
            // 
            // existenciaBindingSource
            // 
            this.existenciaBindingSource.DataSource = typeof(Suite.ServicioSuite.existencia);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelControl3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.27397F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.72603F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(440, 743);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.txtCantidadLote);
            this.panelControl2.Controls.Add(this.lblCantidad);
            this.panelControl2.Controls.Add(this.lblUnidad);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.lupUnidades);
            this.panelControl2.Controls.Add(this.txtCantidad);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(4, 405);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(432, 333);
            this.panelControl2.TabIndex = 1;
            // 
            // txtCantidadLote
            // 
            this.txtCantidadLote.EditValue = "hgfdgfd";
            this.txtCantidadLote.Enabled = false;
            this.txtCantidadLote.Location = new System.Drawing.Point(230, 14);
            this.txtCantidadLote.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCantidadLote.Name = "txtCantidadLote";
            this.txtCantidadLote.Properties.AllowFocused = false;
            this.txtCantidadLote.Properties.Appearance.Font = new System.Drawing.Font("Calibri Light", 12F);
            this.txtCantidadLote.Properties.Appearance.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.ControlText;
            this.txtCantidadLote.Properties.Appearance.Options.UseFont = true;
            this.txtCantidadLote.Properties.Appearance.Options.UseForeColor = true;
            this.txtCantidadLote.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCantidadLote.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCantidadLote.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtCantidadLote.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Calibri Light", 12F);
            this.txtCantidadLote.Properties.AppearanceDisabled.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.ControlText;
            this.txtCantidadLote.Properties.AppearanceDisabled.Options.UseFont = true;
            this.txtCantidadLote.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtCantidadLote.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.txtCantidadLote.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCantidadLote.Properties.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtCantidadLote.Properties.AppearanceFocused.Font = new System.Drawing.Font("Calibri Light", 12F);
            this.txtCantidadLote.Properties.AppearanceFocused.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.ControlText;
            this.txtCantidadLote.Properties.AppearanceFocused.Options.UseFont = true;
            this.txtCantidadLote.Properties.AppearanceFocused.Options.UseForeColor = true;
            this.txtCantidadLote.Properties.AppearanceFocused.Options.UseTextOptions = true;
            this.txtCantidadLote.Properties.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCantidadLote.Properties.AppearanceFocused.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtCantidadLote.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCantidadLote.Properties.AppearanceReadOnly.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.ControlText;
            this.txtCantidadLote.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.txtCantidadLote.Properties.AppearanceReadOnly.Options.UseForeColor = true;
            this.txtCantidadLote.Properties.AppearanceReadOnly.Options.UseTextOptions = true;
            this.txtCantidadLote.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCantidadLote.Properties.AppearanceReadOnly.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtCantidadLote.Properties.AutoHeight = false;
            this.txtCantidadLote.Properties.Mask.EditMask = "n2";
            this.txtCantidadLote.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCantidadLote.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCantidadLote.Properties.ReadOnly = true;
            this.txtCantidadLote.Size = new System.Drawing.Size(188, 50);
            this.txtCantidadLote.TabIndex = 4;
            this.txtCantidadLote.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // lblCantidad
            // 
            this.lblCantidad.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidad.Appearance.Options.UseFont = true;
            this.lblCantidad.Location = new System.Drawing.Point(18, 24);
            this.lblCantidad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(188, 24);
            this.lblCantidad.TabIndex = 6;
            this.lblCantidad.Text = "CANTIDAD DISPONIBLE";
            this.lblCantidad.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.txtFechaVence);
            this.panelControl1.Controls.Add(this.btnAgregarLote);
            this.panelControl1.Controls.Add(this.lblFechaVence);
            this.panelControl1.Controls.Add(this.lupLotes);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(4, 175);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(432, 220);
            this.panelControl1.TabIndex = 0;
            // 
            // txtFechaVence
            // 
            this.txtFechaVence.EditValue = "hgfdgfd";
            this.txtFechaVence.Enabled = false;
            this.txtFechaVence.Location = new System.Drawing.Point(230, 152);
            this.txtFechaVence.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFechaVence.Name = "txtFechaVence";
            this.txtFechaVence.Properties.AllowFocused = false;
            this.txtFechaVence.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtFechaVence.Properties.Appearance.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            this.txtFechaVence.Properties.Appearance.Options.UseFont = true;
            this.txtFechaVence.Properties.Appearance.Options.UseForeColor = true;
            this.txtFechaVence.Properties.Appearance.Options.UseTextOptions = true;
            this.txtFechaVence.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtFechaVence.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtFechaVence.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtFechaVence.Properties.AppearanceDisabled.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            this.txtFechaVence.Properties.AppearanceDisabled.Options.UseFont = true;
            this.txtFechaVence.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtFechaVence.Properties.AppearanceDisabled.Options.UseTextOptions = true;
            this.txtFechaVence.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtFechaVence.Properties.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtFechaVence.Properties.AppearanceFocused.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtFechaVence.Properties.AppearanceFocused.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            this.txtFechaVence.Properties.AppearanceFocused.Options.UseFont = true;
            this.txtFechaVence.Properties.AppearanceFocused.Options.UseForeColor = true;
            this.txtFechaVence.Properties.AppearanceFocused.Options.UseTextOptions = true;
            this.txtFechaVence.Properties.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtFechaVence.Properties.AppearanceFocused.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtFechaVence.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFechaVence.Properties.AppearanceReadOnly.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            this.txtFechaVence.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.txtFechaVence.Properties.AppearanceReadOnly.Options.UseForeColor = true;
            this.txtFechaVence.Properties.AppearanceReadOnly.Options.UseTextOptions = true;
            this.txtFechaVence.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtFechaVence.Properties.AppearanceReadOnly.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtFechaVence.Properties.AutoHeight = false;
            this.txtFechaVence.Properties.Mask.EditMask = "d";
            this.txtFechaVence.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            this.txtFechaVence.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFechaVence.Properties.ReadOnly = true;
            this.txtFechaVence.Size = new System.Drawing.Size(188, 50);
            this.txtFechaVence.TabIndex = 3;
            this.txtFechaVence.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // btnAgregarLote
            // 
            this.btnAgregarLote.ImageOptions.Image = global::Suite.Properties.Resources.add_16x161;
            this.btnAgregarLote.Location = new System.Drawing.Point(292, 17);
            this.btnAgregarLote.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAgregarLote.Name = "btnAgregarLote";
            this.btnAgregarLote.Size = new System.Drawing.Size(124, 36);
            this.btnAgregarLote.TabIndex = 1;
            this.btnAgregarLote.Text = "AGREGAR";
            this.btnAgregarLote.Click += new System.EventHandler(this.btnAgregarLote_Click);
            this.btnAgregarLote.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // lblFechaVence
            // 
            this.lblFechaVence.Appearance.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaVence.Appearance.Options.UseFont = true;
            this.lblFechaVence.Location = new System.Drawing.Point(18, 168);
            this.lblFechaVence.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblFechaVence.Name = "lblFechaVence";
            this.lblFechaVence.Size = new System.Drawing.Size(177, 24);
            this.lblFechaVence.TabIndex = 7;
            this.lblFechaVence.Text = "FECHA VENCIMIENTO";
            this.lblFechaVence.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.meDescripcionArticulo);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(4, 5);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl3.Size = new System.Drawing.Size(432, 160);
            this.panelControl3.TabIndex = 2;
            // 
            // meDescripcionArticulo
            // 
            this.meDescripcionArticulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.meDescripcionArticulo.Location = new System.Drawing.Point(6, 7);
            this.meDescripcionArticulo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.meDescripcionArticulo.Name = "meDescripcionArticulo";
            this.meDescripcionArticulo.Properties.AllowFocused = false;
            this.meDescripcionArticulo.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.meDescripcionArticulo.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.meDescripcionArticulo.Properties.ReadOnly = true;
            this.meDescripcionArticulo.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.meDescripcionArticulo.Size = new System.Drawing.Size(420, 146);
            this.meDescripcionArticulo.TabIndex = 0;
            this.meDescripcionArticulo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            // 
            // xfrm_Documento_Cantidad_Articulo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 743);
            this.Controls.Add(this.tableLayoutPanel1);
            this.LookAndFeel.SkinName = "Suite_Azul";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "xfrm_Documento_Cantidad_Articulo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SUITE - Cantidad del artículo";
            this.Load += new System.EventHandler(this.xfrm_Documento_Cantidad_Articulo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custom_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtCantidad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lupUnidades.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unidadBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lupLotes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loteBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.existenciaBindingSource)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCantidadLote.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaVence.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.meDescripcionArticulo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtCantidad;
        private DevExpress.XtraEditors.LookUpEdit lupUnidades;
        private DevExpress.XtraEditors.LabelControl lblUnidad;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.BindingSource unidadBindingSource;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LookUpEdit lupLotes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl lblFechaVence;
        private DevExpress.XtraEditors.LabelControl lblCantidad;
        private System.Windows.Forms.BindingSource existenciaBindingSource;
        private DevExpress.XtraEditors.SimpleButton btnAgregarLote;
        private System.Windows.Forms.BindingSource loteBindingSource;
        private DevExpress.XtraEditors.TextEdit txtCantidadLote;
        private DevExpress.XtraEditors.TextEdit txtFechaVence;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.MemoEdit meDescripcionArticulo;
    }
}