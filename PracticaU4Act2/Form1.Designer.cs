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
            lblKey = new System.Windows.Forms.Label();
            txtKey = new System.Windows.Forms.TextBox();
            btnInsert = new System.Windows.Forms.Button();
            btnSearch = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            lblMetrics = new System.Windows.Forms.Label();
            lblSequence = new System.Windows.Forms.Label();
            txtSequence = new System.Windows.Forms.TextBox();
            btnLoad = new System.Windows.Forms.Button();
            btnRandom = new System.Windows.Forms.Button();
            btnSample = new System.Windows.Forms.Button();
            lblStatus = new System.Windows.Forms.Label();
            tabControl = new System.Windows.Forms.TabControl();
            tabTree = new System.Windows.Forms.TabPage();
            picTree = new System.Windows.Forms.PictureBox();
            txtTraversals = new System.Windows.Forms.TextBox();
            tabTests = new System.Windows.Forms.TabPage();
            txtTests = new System.Windows.Forms.TextBox();
            btnTests = new System.Windows.Forms.Button();
            tabControl.SuspendLayout();
            tabTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picTree).BeginInit();
            tabTests.SuspendLayout();
            SuspendLayout();
            //
            // lblKey
            //
            lblKey.AutoSize = true;
            lblKey.Location = new System.Drawing.Point(12, 18);
            lblKey.Name = "lblKey";
            lblKey.TabIndex = 0;
            lblKey.Text = "Clave:";
            //
            // txtKey
            //
            txtKey.Location = new System.Drawing.Point(62, 14);
            txtKey.Name = "txtKey";
            txtKey.Size = new System.Drawing.Size(80, 23);
            txtKey.TabIndex = 1;
            txtKey.KeyDown += txtKey_KeyDown;
            //
            // btnInsert
            //
            btnInsert.Location = new System.Drawing.Point(152, 11);
            btnInsert.Name = "btnInsert";
            btnInsert.Size = new System.Drawing.Size(90, 30);
            btnInsert.TabIndex = 2;
            btnInsert.Text = "Insertar";
            btnInsert.UseVisualStyleBackColor = true;
            btnInsert.Click += btnInsert_Click;
            //
            // btnSearch
            //
            btnSearch.Location = new System.Drawing.Point(248, 11);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(90, 30);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Buscar";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            //
            // btnDelete
            //
            btnDelete.Location = new System.Drawing.Point(344, 11);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(90, 30);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Eliminar";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            //
            // btnClear
            //
            btnClear.Location = new System.Drawing.Point(440, 11);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(100, 30);
            btnClear.TabIndex = 5;
            btnClear.Text = "Vaciar árbol";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            //
            // lblMetrics
            //
            lblMetrics.AutoSize = true;
            lblMetrics.Location = new System.Drawing.Point(560, 18);
            lblMetrics.Name = "lblMetrics";
            lblMetrics.TabIndex = 6;
            lblMetrics.Text = "Nodos: 0   |   Altura: —";
            lblMetrics.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            //
            // lblSequence
            //
            lblSequence.AutoSize = true;
            lblSequence.Location = new System.Drawing.Point(12, 54);
            lblSequence.Name = "lblSequence";
            lblSequence.TabIndex = 7;
            lblSequence.Text = "Secuencia:";
            //
            // txtSequence
            //
            txtSequence.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSequence.Location = new System.Drawing.Point(82, 50);
            txtSequence.Name = "txtSequence";
            txtSequence.Size = new System.Drawing.Size(560, 23);
            txtSequence.TabIndex = 8;
            //
            // btnLoad
            //
            btnLoad.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnLoad.Location = new System.Drawing.Point(652, 47);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(130, 30);
            btnLoad.TabIndex = 9;
            btnLoad.Text = "Cargar secuencia";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            //
            // btnRandom
            //
            btnRandom.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRandom.Location = new System.Drawing.Point(788, 47);
            btnRandom.Name = "btnRandom";
            btnRandom.Size = new System.Drawing.Size(110, 30);
            btnRandom.TabIndex = 10;
            btnRandom.Text = "15 aleatorias";
            btnRandom.UseVisualStyleBackColor = true;
            btnRandom.Click += btnRandom_Click;
            //
            // btnSample
            //
            btnSample.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSample.Location = new System.Drawing.Point(904, 47);
            btnSample.Name = "btnSample";
            btnSample.Size = new System.Drawing.Size(130, 30);
            btnSample.TabIndex = 11;
            btnSample.Text = "Árbol de ejemplo";
            btnSample.UseVisualStyleBackColor = true;
            btnSample.Click += btnSample_Click;
            //
            // lblStatus
            //
            lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblStatus.Location = new System.Drawing.Point(12, 86);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(1022, 40);
            lblStatus.TabIndex = 12;
            //
            // tabControl
            //
            tabControl.Controls.Add(tabTree);
            tabControl.Controls.Add(tabTests);
            tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl.Location = new System.Drawing.Point(12, 130);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(1022, 560);
            tabControl.TabIndex = 13;
            //
            // tabTree
            //
            tabTree.Controls.Add(picTree);
            tabTree.Controls.Add(txtTraversals);
            tabTree.Location = new System.Drawing.Point(4, 24);
            tabTree.Name = "tabTree";
            tabTree.Padding = new System.Windows.Forms.Padding(3);
            tabTree.Size = new System.Drawing.Size(1014, 532);
            tabTree.TabIndex = 0;
            tabTree.Text = "Árbol y recorridos";
            tabTree.UseVisualStyleBackColor = true;
            //
            // picTree
            //
            picTree.BackColor = System.Drawing.Color.White;
            picTree.Dock = System.Windows.Forms.DockStyle.Fill;
            picTree.Location = new System.Drawing.Point(3, 3);
            picTree.Name = "picTree";
            picTree.Size = new System.Drawing.Size(1008, 436);
            picTree.TabIndex = 0;
            picTree.TabStop = false;
            picTree.Paint += picTree_Paint;
            picTree.Resize += picTree_Resize;
            picTree.MouseClick += picTree_MouseClick;
            //
            // txtTraversals
            //
            txtTraversals.Dock = System.Windows.Forms.DockStyle.Bottom;
            txtTraversals.Font = new System.Drawing.Font("Consolas", 10F);
            txtTraversals.Location = new System.Drawing.Point(3, 439);
            txtTraversals.Multiline = true;
            txtTraversals.Name = "txtTraversals";
            txtTraversals.ReadOnly = true;
            txtTraversals.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtTraversals.Size = new System.Drawing.Size(1008, 90);
            txtTraversals.TabIndex = 1;
            txtTraversals.WordWrap = false;
            //
            // tabTests
            //
            tabTests.Controls.Add(txtTests);
            tabTests.Controls.Add(btnTests);
            tabTests.Location = new System.Drawing.Point(4, 24);
            tabTests.Name = "tabTests";
            tabTests.Padding = new System.Windows.Forms.Padding(3);
            tabTests.Size = new System.Drawing.Size(1014, 532);
            tabTests.TabIndex = 1;
            tabTests.Text = "Pruebas y mediciones";
            tabTests.UseVisualStyleBackColor = true;
            //
            // txtTests
            //
            txtTests.Dock = System.Windows.Forms.DockStyle.Fill;
            txtTests.Font = new System.Drawing.Font("Consolas", 10F);
            txtTests.Location = new System.Drawing.Point(3, 35);
            txtTests.Multiline = true;
            txtTests.Name = "txtTests";
            txtTests.ReadOnly = true;
            txtTests.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtTests.Size = new System.Drawing.Size(1008, 494);
            txtTests.TabIndex = 1;
            txtTests.WordWrap = false;
            //
            // btnTests
            //
            btnTests.Dock = System.Windows.Forms.DockStyle.Top;
            btnTests.Location = new System.Drawing.Point(3, 3);
            btnTests.Name = "btnTests";
            btnTests.Size = new System.Drawing.Size(1008, 32);
            btnTests.TabIndex = 0;
            btnTests.Text = "Ejecutar pruebas y mediciones";
            btnTests.UseVisualStyleBackColor = true;
            btnTests.Click += btnTests_Click;
            //
            // Form1
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1046, 702);
            Controls.Add(tabControl);
            Controls.Add(lblStatus);
            Controls.Add(btnSample);
            Controls.Add(btnRandom);
            Controls.Add(btnLoad);
            Controls.Add(txtSequence);
            Controls.Add(lblSequence);
            Controls.Add(lblMetrics);
            Controls.Add(btnClear);
            Controls.Add(btnDelete);
            Controls.Add(btnSearch);
            Controls.Add(btnInsert);
            Controls.Add(txtKey);
            Controls.Add(lblKey);
            MinimumSize = new System.Drawing.Size(900, 600);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Árbol binario de búsqueda";
            Load += Form1_Load;
            tabControl.ResumeLayout(false);
            tabTree.ResumeLayout(false);
            tabTree.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picTree).EndInit();
            tabTests.ResumeLayout(false);
            tabTests.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblMetrics;
        private System.Windows.Forms.Label lblSequence;
        private System.Windows.Forms.TextBox txtSequence;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabTree;
        private System.Windows.Forms.PictureBox picTree;
        private System.Windows.Forms.TextBox txtTraversals;
        private System.Windows.Forms.TabPage tabTests;
        private System.Windows.Forms.TextBox txtTests;
        private System.Windows.Forms.Button btnTests;
    }
}