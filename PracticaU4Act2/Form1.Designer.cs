namespace PracticaU4Act2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblClave = new System.Windows.Forms.Label();
            txtClave = new System.Windows.Forms.TextBox();
            btnInsertar = new System.Windows.Forms.Button();
            btnBuscar = new System.Windows.Forms.Button();
            btnEliminar = new System.Windows.Forms.Button();
            btnLimpiar = new System.Windows.Forms.Button();
            lblMetricas = new System.Windows.Forms.Label();
            lblSecuencia = new System.Windows.Forms.Label();
            txtSecuencia = new System.Windows.Forms.TextBox();
            btnCargar = new System.Windows.Forms.Button();
            btnAleatorio = new System.Windows.Forms.Button();
            btnEjemplo = new System.Windows.Forms.Button();
            lblEstado = new System.Windows.Forms.Label();
            tabControl = new System.Windows.Forms.TabControl();
            tabArbol = new System.Windows.Forms.TabPage();
            picArbol = new System.Windows.Forms.PictureBox();
            txtRecorridos = new System.Windows.Forms.TextBox();
            tabPruebas = new System.Windows.Forms.TabPage();
            txtPruebas = new System.Windows.Forms.TextBox();
            btnPruebas = new System.Windows.Forms.Button();
            tabControl.SuspendLayout();
            tabArbol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picArbol).BeginInit();
            tabPruebas.SuspendLayout();
            SuspendLayout();
            //
            // lblClave
            //
            lblClave.AutoSize = true;
            lblClave.Location = new System.Drawing.Point(12, 18);
            lblClave.Name = "lblClave";
            lblClave.TabIndex = 0;
            lblClave.Text = "Clave:";
            //
            // txtClave
            //
            txtClave.Location = new System.Drawing.Point(62, 14);
            txtClave.Name = "txtClave";
            txtClave.Size = new System.Drawing.Size(80, 23);
            txtClave.TabIndex = 1;
            txtClave.KeyDown += txtClave_KeyDown;
            //
            // btnInsertar
            //
            btnInsertar.Location = new System.Drawing.Point(152, 11);
            btnInsertar.Name = "btnInsertar";
            btnInsertar.Size = new System.Drawing.Size(90, 30);
            btnInsertar.TabIndex = 2;
            btnInsertar.Text = "Insertar";
            btnInsertar.UseVisualStyleBackColor = true;
            btnInsertar.Click += btnInsertar_Click;
            //
            // btnBuscar
            //
            btnBuscar.Location = new System.Drawing.Point(248, 11);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new System.Drawing.Size(90, 30);
            btnBuscar.TabIndex = 3;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // btnEliminar
            //
            btnEliminar.Location = new System.Drawing.Point(344, 11);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new System.Drawing.Size(90, 30);
            btnEliminar.TabIndex = 4;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            //
            // btnLimpiar
            //
            btnLimpiar.Location = new System.Drawing.Point(440, 11);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new System.Drawing.Size(100, 30);
            btnLimpiar.TabIndex = 5;
            btnLimpiar.Text = "Vaciar árbol";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            //
            // lblMetricas
            //
            lblMetricas.AutoSize = true;
            lblMetricas.Location = new System.Drawing.Point(560, 18);
            lblMetricas.Name = "lblMetricas";
            lblMetricas.TabIndex = 6;
            lblMetricas.Text = "Nodos: 0   |   Altura: —";
            lblMetricas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            //
            // lblSecuencia
            //
            lblSecuencia.AutoSize = true;
            lblSecuencia.Location = new System.Drawing.Point(12, 54);
            lblSecuencia.Name = "lblSecuencia";
            lblSecuencia.TabIndex = 7;
            lblSecuencia.Text = "Secuencia:";
            //
            // txtSecuencia
            //
            txtSecuencia.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSecuencia.Location = new System.Drawing.Point(82, 50);
            txtSecuencia.Name = "txtSecuencia";
            txtSecuencia.Size = new System.Drawing.Size(560, 23);
            txtSecuencia.TabIndex = 8;
            //
            // btnCargar
            //
            btnCargar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCargar.Location = new System.Drawing.Point(652, 47);
            btnCargar.Name = "btnCargar";
            btnCargar.Size = new System.Drawing.Size(130, 30);
            btnCargar.TabIndex = 9;
            btnCargar.Text = "Cargar secuencia";
            btnCargar.UseVisualStyleBackColor = true;
            btnCargar.Click += btnCargar_Click;
            //
            // btnAleatorio
            //
            btnAleatorio.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAleatorio.Location = new System.Drawing.Point(788, 47);
            btnAleatorio.Name = "btnAleatorio";
            btnAleatorio.Size = new System.Drawing.Size(110, 30);
            btnAleatorio.TabIndex = 10;
            btnAleatorio.Text = "15 aleatorias";
            btnAleatorio.UseVisualStyleBackColor = true;
            btnAleatorio.Click += btnAleatorio_Click;
            //
            // btnEjemplo
            //
            btnEjemplo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnEjemplo.Location = new System.Drawing.Point(904, 47);
            btnEjemplo.Name = "btnEjemplo";
            btnEjemplo.Size = new System.Drawing.Size(130, 30);
            btnEjemplo.TabIndex = 11;
            btnEjemplo.Text = "Árbol de ejemplo";
            btnEjemplo.UseVisualStyleBackColor = true;
            btnEjemplo.Click += btnEjemplo_Click;
            //
            // lblEstado
            //
            lblEstado.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblEstado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblEstado.Location = new System.Drawing.Point(12, 86);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new System.Drawing.Size(1022, 40);
            lblEstado.TabIndex = 12;
            //
            // tabControl
            //
            tabControl.Controls.Add(tabArbol);
            tabControl.Controls.Add(tabPruebas);
            tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl.Location = new System.Drawing.Point(12, 130);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(1022, 560);
            tabControl.TabIndex = 13;
            //
            // tabArbol
            //
            tabArbol.Controls.Add(picArbol);
            tabArbol.Controls.Add(txtRecorridos);
            tabArbol.Location = new System.Drawing.Point(4, 24);
            tabArbol.Name = "tabArbol";
            tabArbol.Padding = new System.Windows.Forms.Padding(3);
            tabArbol.Size = new System.Drawing.Size(1014, 532);
            tabArbol.TabIndex = 0;
            tabArbol.Text = "Árbol y recorridos";
            tabArbol.UseVisualStyleBackColor = true;
            //
            // picArbol
            //
            picArbol.BackColor = System.Drawing.Color.White;
            picArbol.Dock = System.Windows.Forms.DockStyle.Fill;
            picArbol.Location = new System.Drawing.Point(3, 3);
            picArbol.Name = "picArbol";
            picArbol.Size = new System.Drawing.Size(1008, 436);
            picArbol.TabIndex = 0;
            picArbol.TabStop = false;
            picArbol.Paint += picArbol_Paint;
            picArbol.Resize += picArbol_Resize;
            picArbol.MouseClick += picArbol_MouseClick;
            //
            // txtRecorridos
            //
            txtRecorridos.Dock = System.Windows.Forms.DockStyle.Bottom;
            txtRecorridos.Font = new System.Drawing.Font("Consolas", 10F);
            txtRecorridos.Location = new System.Drawing.Point(3, 439);
            txtRecorridos.Multiline = true;
            txtRecorridos.Name = "txtRecorridos";
            txtRecorridos.ReadOnly = true;
            txtRecorridos.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtRecorridos.Size = new System.Drawing.Size(1008, 90);
            txtRecorridos.TabIndex = 1;
            txtRecorridos.WordWrap = false;
            //
            // tabPruebas
            //
            tabPruebas.Controls.Add(txtPruebas);
            tabPruebas.Controls.Add(btnPruebas);
            tabPruebas.Location = new System.Drawing.Point(4, 24);
            tabPruebas.Name = "tabPruebas";
            tabPruebas.Padding = new System.Windows.Forms.Padding(3);
            tabPruebas.Size = new System.Drawing.Size(1014, 532);
            tabPruebas.TabIndex = 1;
            tabPruebas.Text = "Pruebas y mediciones";
            tabPruebas.UseVisualStyleBackColor = true;
            //
            // txtPruebas
            //
            txtPruebas.Dock = System.Windows.Forms.DockStyle.Fill;
            txtPruebas.Font = new System.Drawing.Font("Consolas", 10F);
            txtPruebas.Location = new System.Drawing.Point(3, 35);
            txtPruebas.Multiline = true;
            txtPruebas.Name = "txtPruebas";
            txtPruebas.ReadOnly = true;
            txtPruebas.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtPruebas.Size = new System.Drawing.Size(1008, 494);
            txtPruebas.TabIndex = 1;
            txtPruebas.WordWrap = false;
            //
            // btnPruebas
            //
            btnPruebas.Dock = System.Windows.Forms.DockStyle.Top;
            btnPruebas.Location = new System.Drawing.Point(3, 3);
            btnPruebas.Name = "btnPruebas";
            btnPruebas.Size = new System.Drawing.Size(1008, 32);
            btnPruebas.TabIndex = 0;
            btnPruebas.Text = "Ejecutar pruebas y mediciones";
            btnPruebas.UseVisualStyleBackColor = true;
            btnPruebas.Click += btnPruebas_Click;
            //
            // Form1
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1046, 702);
            Controls.Add(tabControl);
            Controls.Add(lblEstado);
            Controls.Add(btnEjemplo);
            Controls.Add(btnAleatorio);
            Controls.Add(btnCargar);
            Controls.Add(txtSecuencia);
            Controls.Add(lblSecuencia);
            Controls.Add(lblMetricas);
            Controls.Add(btnLimpiar);
            Controls.Add(btnEliminar);
            Controls.Add(btnBuscar);
            Controls.Add(btnInsertar);
            Controls.Add(txtClave);
            Controls.Add(lblClave);
            MinimumSize = new System.Drawing.Size(900, 600);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Árbol binario de búsqueda";
            Load += Form1_Load;
            tabControl.ResumeLayout(false);
            tabArbol.ResumeLayout(false);
            tabArbol.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picArbol).EndInit();
            tabPruebas.ResumeLayout(false);
            tabPruebas.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblClave;
        private System.Windows.Forms.TextBox txtClave;
        private System.Windows.Forms.Button btnInsertar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Label lblMetricas;
        private System.Windows.Forms.Label lblSecuencia;
        private System.Windows.Forms.TextBox txtSecuencia;
        private System.Windows.Forms.Button btnCargar;
        private System.Windows.Forms.Button btnAleatorio;
        private System.Windows.Forms.Button btnEjemplo;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabArbol;
        private System.Windows.Forms.PictureBox picArbol;
        private System.Windows.Forms.TextBox txtRecorridos;
        private System.Windows.Forms.TabPage tabPruebas;
        private System.Windows.Forms.TextBox txtPruebas;
        private System.Windows.Forms.Button btnPruebas;
    }
}