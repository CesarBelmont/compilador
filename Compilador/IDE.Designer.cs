namespace Compilador
{
    partial class IDE
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDE));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarComoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deshacerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rehacerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cortarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copiarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pegarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.seleccionartodoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editorDeTexto = new System.Windows.Forms.RichTextBox();
            this.Linea = new System.Windows.Forms.Label();
            this.Columna = new System.Windows.Forms.Label();
            this.Col = new System.Windows.Forms.Label();
            this.Lin = new System.Windows.Forms.Label();
            this.panelLinCol = new System.Windows.Forms.Panel();
            this.panelET = new System.Windows.Forms.Panel();
            this.Salida = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.salidaLexico = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.arbolSintactico = new System.Windows.Forms.TreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.Errores = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.erroresLexicos = new System.Windows.Forms.RichTextBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.erroresSintacticos = new System.Windows.Forms.RichTextBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.panelBotones = new System.Windows.Forms.Panel();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnCompilar = new System.Windows.Forms.Button();
            this.btnRehacer = new System.Windows.Forms.Button();
            this.btnDeshacer = new System.Windows.Forms.Button();
            this.btnAbrir = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contadorLineas = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.panelLinCol.SuspendLayout();
            this.panelET.SuspendLayout();
            this.Salida.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.Errores.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.panelBotones.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.editarToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1292, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoToolStripMenuItem,
            this.abrirToolStripMenuItem,
            this.toolStripSeparator,
            this.guardarToolStripMenuItem,
            this.guardarComoToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripSeparator2,
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(71, 24);
            this.archivoToolStripMenuItem.Text = "&Archivo";
            // 
            // nuevoToolStripMenuItem
            // 
            this.nuevoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("nuevoToolStripMenuItem.Image")));
            this.nuevoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.nuevoToolStripMenuItem.Text = "&Nuevo";
            this.nuevoToolStripMenuItem.Click += new System.EventHandler(this.nuevoToolStripMenuItem_Click);
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("abrirToolStripMenuItem.Image")));
            this.abrirToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.abrirToolStripMenuItem.Text = "&Abrir";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(185, 6);
            // 
            // guardarToolStripMenuItem
            // 
            this.guardarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("guardarToolStripMenuItem.Image")));
            this.guardarToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            this.guardarToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.guardarToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.guardarToolStripMenuItem.Text = "&Guardar";
            this.guardarToolStripMenuItem.Click += new System.EventHandler(this.guardarToolStripMenuItem_Click);
            // 
            // guardarComoToolStripMenuItem
            // 
            this.guardarComoToolStripMenuItem.Name = "guardarComoToolStripMenuItem";
            this.guardarComoToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.guardarComoToolStripMenuItem.Text = "Guardar como...";
            this.guardarComoToolStripMenuItem.Click += new System.EventHandler(this.guardarComoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.salirToolStripMenuItem.Text = "&Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // editarToolStripMenuItem
            // 
            this.editarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deshacerToolStripMenuItem,
            this.rehacerToolStripMenuItem,
            this.toolStripSeparator3,
            this.cortarToolStripMenuItem,
            this.copiarToolStripMenuItem,
            this.pegarToolStripMenuItem,
            this.toolStripSeparator4,
            this.seleccionartodoToolStripMenuItem});
            this.editarToolStripMenuItem.Name = "editarToolStripMenuItem";
            this.editarToolStripMenuItem.Size = new System.Drawing.Size(60, 24);
            this.editarToolStripMenuItem.Text = "&Editar";
            // 
            // deshacerToolStripMenuItem
            // 
            this.deshacerToolStripMenuItem.Name = "deshacerToolStripMenuItem";
            this.deshacerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.deshacerToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.deshacerToolStripMenuItem.Text = "&Deshacer";
            this.deshacerToolStripMenuItem.Click += new System.EventHandler(this.deshacerToolStripMenuItem_Click);
            // 
            // rehacerToolStripMenuItem
            // 
            this.rehacerToolStripMenuItem.Name = "rehacerToolStripMenuItem";
            this.rehacerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.rehacerToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.rehacerToolStripMenuItem.Text = "&Rehacer";
            this.rehacerToolStripMenuItem.Click += new System.EventHandler(this.rehacerToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(193, 6);
            // 
            // cortarToolStripMenuItem
            // 
            this.cortarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cortarToolStripMenuItem.Image")));
            this.cortarToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cortarToolStripMenuItem.Name = "cortarToolStripMenuItem";
            this.cortarToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cortarToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.cortarToolStripMenuItem.Text = "Cor&tar";
            this.cortarToolStripMenuItem.Click += new System.EventHandler(this.cortarToolStripMenuItem_Click);
            // 
            // copiarToolStripMenuItem
            // 
            this.copiarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copiarToolStripMenuItem.Image")));
            this.copiarToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
            this.copiarToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copiarToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.copiarToolStripMenuItem.Text = "&Copiar";
            this.copiarToolStripMenuItem.Click += new System.EventHandler(this.copiarToolStripMenuItem_Click);
            // 
            // pegarToolStripMenuItem
            // 
            this.pegarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pegarToolStripMenuItem.Image")));
            this.pegarToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pegarToolStripMenuItem.Name = "pegarToolStripMenuItem";
            this.pegarToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pegarToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.pegarToolStripMenuItem.Text = "&Pegar";
            this.pegarToolStripMenuItem.Click += new System.EventHandler(this.pegarToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(193, 6);
            // 
            // seleccionartodoToolStripMenuItem
            // 
            this.seleccionartodoToolStripMenuItem.Name = "seleccionartodoToolStripMenuItem";
            this.seleccionartodoToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.seleccionartodoToolStripMenuItem.Text = "&Seleccionar todo";
            this.seleccionartodoToolStripMenuItem.Click += new System.EventHandler(this.seleccionartodoToolStripMenuItem_Click);
            // 
            // editorDeTexto
            // 
            this.editorDeTexto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorDeTexto.Location = new System.Drawing.Point(0, 0);
            this.editorDeTexto.Margin = new System.Windows.Forms.Padding(4);
            this.editorDeTexto.Name = "editorDeTexto";
            this.editorDeTexto.Size = new System.Drawing.Size(627, 468);
            this.editorDeTexto.TabIndex = 2;
            this.editorDeTexto.Text = "";
            this.editorDeTexto.WordWrap = false;
            this.editorDeTexto.VScroll += new System.EventHandler(this.editorDeTexto_VScroll);
            this.editorDeTexto.Click += new System.EventHandler(this.editorDeTexto_Click);
            this.editorDeTexto.TextChanged += new System.EventHandler(this.editorDeTexto_TextChanged);
            // 
            // Linea
            // 
            this.Linea.AutoSize = true;
            this.Linea.Location = new System.Drawing.Point(4, 0);
            this.Linea.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Linea.Name = "Linea";
            this.Linea.Size = new System.Drawing.Size(51, 17);
            this.Linea.TabIndex = 4;
            this.Linea.Text = "Linea: ";
            // 
            // Columna
            // 
            this.Columna.AutoSize = true;
            this.Columna.Location = new System.Drawing.Point(105, 0);
            this.Columna.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Columna.Name = "Columna";
            this.Columna.Size = new System.Drawing.Size(67, 17);
            this.Columna.TabIndex = 5;
            this.Columna.Text = "Columna:";
            // 
            // Col
            // 
            this.Col.AutoSize = true;
            this.Col.Location = new System.Drawing.Point(181, 0);
            this.Col.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Col.Name = "Col";
            this.Col.Size = new System.Drawing.Size(16, 17);
            this.Col.TabIndex = 6;
            this.Col.Text = "1";
            // 
            // Lin
            // 
            this.Lin.AutoSize = true;
            this.Lin.Location = new System.Drawing.Point(64, 0);
            this.Lin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lin.Name = "Lin";
            this.Lin.Size = new System.Drawing.Size(16, 17);
            this.Lin.TabIndex = 7;
            this.Lin.Text = "1";
            // 
            // panelLinCol
            // 
            this.panelLinCol.Controls.Add(this.Linea);
            this.panelLinCol.Controls.Add(this.Lin);
            this.panelLinCol.Controls.Add(this.Col);
            this.panelLinCol.Controls.Add(this.Columna);
            this.panelLinCol.Location = new System.Drawing.Point(413, 551);
            this.panelLinCol.Margin = new System.Windows.Forms.Padding(4);
            this.panelLinCol.Name = "panelLinCol";
            this.panelLinCol.Size = new System.Drawing.Size(243, 25);
            this.panelLinCol.TabIndex = 9;
            // 
            // panelET
            // 
            this.panelET.AutoScroll = true;
            this.panelET.Controls.Add(this.editorDeTexto);
            this.panelET.Location = new System.Drawing.Point(80, 71);
            this.panelET.Margin = new System.Windows.Forms.Padding(4);
            this.panelET.Name = "panelET";
            this.panelET.Size = new System.Drawing.Size(627, 468);
            this.panelET.TabIndex = 10;
            // 
            // Salida
            // 
            this.Salida.Controls.Add(this.tabPage1);
            this.Salida.Controls.Add(this.tabPage2);
            this.Salida.Controls.Add(this.tabPage3);
            this.Salida.Controls.Add(this.tabPage4);
            this.Salida.Controls.Add(this.tabPage5);
            this.Salida.Location = new System.Drawing.Point(714, 76);
            this.Salida.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Salida.Name = "Salida";
            this.Salida.SelectedIndex = 0;
            this.Salida.Size = new System.Drawing.Size(553, 468);
            this.Salida.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.salidaLexico);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(545, 439);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Lexico";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // salidaLexico
            // 
            this.salidaLexico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.salidaLexico.Location = new System.Drawing.Point(3, 2);
            this.salidaLexico.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.salidaLexico.Name = "salidaLexico";
            this.salidaLexico.Size = new System.Drawing.Size(539, 435);
            this.salidaLexico.TabIndex = 0;
            this.salidaLexico.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.arbolSintactico);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(545, 439);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Sintactico";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // arbolSintactico
            // 
            this.arbolSintactico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbolSintactico.Location = new System.Drawing.Point(3, 2);
            this.arbolSintactico.Name = "arbolSintactico";
            this.arbolSintactico.Size = new System.Drawing.Size(539, 435);
            this.arbolSintactico.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Size = new System.Drawing.Size(545, 439);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Semantico";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage4.Size = new System.Drawing.Size(545, 439);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Hash Table";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage5.Size = new System.Drawing.Size(545, 439);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Codigo Intermedio";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // Errores
            // 
            this.Errores.Controls.Add(this.tabPage6);
            this.Errores.Controls.Add(this.tabPage7);
            this.Errores.Controls.Add(this.tabPage8);
            this.Errores.Controls.Add(this.tabPage9);
            this.Errores.Location = new System.Drawing.Point(13, 583);
            this.Errores.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Errores.Name = "Errores";
            this.Errores.SelectedIndex = 0;
            this.Errores.Size = new System.Drawing.Size(1253, 183);
            this.Errores.TabIndex = 12;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.erroresLexicos);
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage6.Size = new System.Drawing.Size(1245, 154);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "Errores Lexicos";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // erroresLexicos
            // 
            this.erroresLexicos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.erroresLexicos.Location = new System.Drawing.Point(3, 2);
            this.erroresLexicos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.erroresLexicos.Name = "erroresLexicos";
            this.erroresLexicos.Size = new System.Drawing.Size(1239, 150);
            this.erroresLexicos.TabIndex = 0;
            this.erroresLexicos.Text = "";
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.erroresSintacticos);
            this.tabPage7.Location = new System.Drawing.Point(4, 25);
            this.tabPage7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage7.Size = new System.Drawing.Size(1245, 154);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "Errores Sintacticos";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // erroresSintacticos
            // 
            this.erroresSintacticos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.erroresSintacticos.Location = new System.Drawing.Point(3, 2);
            this.erroresSintacticos.Name = "erroresSintacticos";
            this.erroresSintacticos.Size = new System.Drawing.Size(1239, 150);
            this.erroresSintacticos.TabIndex = 0;
            this.erroresSintacticos.Text = "";
            // 
            // tabPage8
            // 
            this.tabPage8.Location = new System.Drawing.Point(4, 25);
            this.tabPage8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage8.Size = new System.Drawing.Size(1245, 154);
            this.tabPage8.TabIndex = 2;
            this.tabPage8.Text = "Errores Semanticos";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // tabPage9
            // 
            this.tabPage9.Location = new System.Drawing.Point(4, 25);
            this.tabPage9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage9.Size = new System.Drawing.Size(1245, 154);
            this.tabPage9.TabIndex = 3;
            this.tabPage9.Text = "Resultados";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardar.Image")));
            this.btnGuardar.Location = new System.Drawing.Point(3, 2);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(36, 28);
            this.btnGuardar.TabIndex = 13;
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // panelBotones
            // 
            this.panelBotones.Controls.Add(this.btnNuevo);
            this.panelBotones.Controls.Add(this.btnCompilar);
            this.panelBotones.Controls.Add(this.btnRehacer);
            this.panelBotones.Controls.Add(this.btnDeshacer);
            this.panelBotones.Controls.Add(this.btnAbrir);
            this.panelBotones.Controls.Add(this.btnGuardar);
            this.panelBotones.Location = new System.Drawing.Point(13, 31);
            this.panelBotones.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new System.Drawing.Size(253, 36);
            this.panelBotones.TabIndex = 14;
            // 
            // btnNuevo
            // 
            this.btnNuevo.FlatAppearance.BorderSize = 0;
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.Image = ((System.Drawing.Image)(resources.GetObject("btnNuevo.Image")));
            this.btnNuevo.Location = new System.Drawing.Point(87, 7);
            this.btnNuevo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(31, 22);
            this.btnNuevo.TabIndex = 16;
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnCompilar
            // 
            this.btnCompilar.FlatAppearance.BorderSize = 0;
            this.btnCompilar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompilar.Image = ((System.Drawing.Image)(resources.GetObject("btnCompilar.Image")));
            this.btnCompilar.Location = new System.Drawing.Point(195, 6);
            this.btnCompilar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCompilar.Name = "btnCompilar";
            this.btnCompilar.Size = new System.Drawing.Size(33, 25);
            this.btnCompilar.TabIndex = 15;
            this.btnCompilar.UseVisualStyleBackColor = true;
            this.btnCompilar.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRehacer
            // 
            this.btnRehacer.FlatAppearance.BorderSize = 0;
            this.btnRehacer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRehacer.Image = ((System.Drawing.Image)(resources.GetObject("btnRehacer.Image")));
            this.btnRehacer.Location = new System.Drawing.Point(160, 6);
            this.btnRehacer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRehacer.Name = "btnRehacer";
            this.btnRehacer.Size = new System.Drawing.Size(29, 25);
            this.btnRehacer.TabIndex = 15;
            this.btnRehacer.UseVisualStyleBackColor = true;
            this.btnRehacer.Click += new System.EventHandler(this.btnRehacer_Click);
            // 
            // btnDeshacer
            // 
            this.btnDeshacer.FlatAppearance.BorderSize = 0;
            this.btnDeshacer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeshacer.Image = ((System.Drawing.Image)(resources.GetObject("btnDeshacer.Image")));
            this.btnDeshacer.Location = new System.Drawing.Point(124, 6);
            this.btnDeshacer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDeshacer.Name = "btnDeshacer";
            this.btnDeshacer.Size = new System.Drawing.Size(29, 23);
            this.btnDeshacer.TabIndex = 15;
            this.btnDeshacer.UseVisualStyleBackColor = true;
            this.btnDeshacer.Click += new System.EventHandler(this.btnDeshacer_Click);
            // 
            // btnAbrir
            // 
            this.btnAbrir.FlatAppearance.BorderSize = 0;
            this.btnAbrir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrir.Image = ((System.Drawing.Image)(resources.GetObject("btnAbrir.Image")));
            this.btnAbrir.Location = new System.Drawing.Point(45, 6);
            this.btnAbrir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(37, 23);
            this.btnAbrir.TabIndex = 14;
            this.btnAbrir.UseVisualStyleBackColor = true;
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCerrar.Image")));
            this.btnCerrar.Location = new System.Drawing.Point(1251, 0);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(4);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(33, 28);
            this.btnCerrar.TabIndex = 15;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Image = ((System.Drawing.Image)(resources.GetObject("btnMinimizar.Image")));
            this.btnMinimizar.Location = new System.Drawing.Point(1212, 0);
            this.btnMinimizar.Margin = new System.Windows.Forms.Padding(4);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(31, 28);
            this.btnMinimizar.TabIndex = 16;
            this.btnMinimizar.UseVisualStyleBackColor = true;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMinimizar_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.contadorLineas);
            this.panel1.Location = new System.Drawing.Point(13, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(61, 468);
            this.panel1.TabIndex = 17;
            // 
            // contadorLineas
            // 
            this.contadorLineas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contadorLineas.Location = new System.Drawing.Point(0, 0);
            this.contadorLineas.Name = "contadorLineas";
            this.contadorLineas.Size = new System.Drawing.Size(61, 468);
            this.contadorLineas.TabIndex = 0;
            this.contadorLineas.Text = "";
            // 
            // IDE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1292, 782);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnMinimizar);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.panelBotones);
            this.Controls.Add(this.Errores);
            this.Controls.Add(this.Salida);
            this.Controls.Add(this.panelET);
            this.Controls.Add(this.panelLinCol);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "IDE";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Compilador";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelLinCol.ResumeLayout(false);
            this.panelLinCol.PerformLayout();
            this.panelET.ResumeLayout(false);
            this.Salida.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.Errores.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.panelBotones.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem guardarToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deshacerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rehacerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cortarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copiarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pegarToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem seleccionartodoToolStripMenuItem;
        private System.Windows.Forms.RichTextBox editorDeTexto;
        private System.Windows.Forms.Label Linea;
        private System.Windows.Forms.Label Columna;
        private System.Windows.Forms.Label Col;
        private System.Windows.Forms.Label Lin;
        private System.Windows.Forms.Panel panelLinCol;
        private System.Windows.Forms.Panel panelET;
        private System.Windows.Forms.TabControl Salida;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabControl Errores;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.ToolStripMenuItem guardarComoToolStripMenuItem;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Panel panelBotones;
        private System.Windows.Forms.Button btnAbrir;
        private System.Windows.Forms.Button btnRehacer;
        private System.Windows.Forms.Button btnDeshacer;
        private System.Windows.Forms.Button btnCompilar;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnMinimizar;
        public System.Windows.Forms.RichTextBox salidaLexico;
        private System.Windows.Forms.RichTextBox erroresLexicos;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox contadorLineas;
        private System.Windows.Forms.TreeView arbolSintactico;
        private System.Windows.Forms.RichTextBox erroresSintacticos;
    }
}

