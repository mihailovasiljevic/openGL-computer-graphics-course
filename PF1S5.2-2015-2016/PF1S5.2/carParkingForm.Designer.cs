namespace RacunarskaGrafika
{
    partial class carParkingForm
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
            this.m_world.Dispose();
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.openGlWorld = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.btnPonistiSveIzmene = new System.Windows.Forms.Button();
            this.tbPomeriDrugi = new System.Windows.Forms.TrackBar();
            this.lblPomeriDrugi = new System.Windows.Forms.Label();
            this.tbPomeriPrvi = new System.Windows.Forms.TrackBar();
            this.lblPomeriPrvi = new System.Windows.Forms.Label();
            this.lblPomeranjeAutomobila = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnIzborBoje = new System.Windows.Forms.Button();
            this.pnlIzborBOje = new System.Windows.Forms.Panel();
            this.lblIzborBoje = new System.Windows.Forms.Label();
            this.lblVertikalniStubiic = new System.Windows.Forms.Label();
            this.tbVisinaStubica = new System.Windows.Forms.TrackBar();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPomeriDrugi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPomeriPrvi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVisinaStubica)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.openGlWorld);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnPonistiSveIzmene);
            this.splitContainer1.Panel2.Controls.Add(this.tbPomeriDrugi);
            this.splitContainer1.Panel2.Controls.Add(this.lblPomeriDrugi);
            this.splitContainer1.Panel2.Controls.Add(this.tbPomeriPrvi);
            this.splitContainer1.Panel2.Controls.Add(this.lblPomeriPrvi);
            this.splitContainer1.Panel2.Controls.Add(this.lblPomeranjeAutomobila);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.btnIzborBoje);
            this.splitContainer1.Panel2.Controls.Add(this.pnlIzborBOje);
            this.splitContainer1.Panel2.Controls.Add(this.lblIzborBoje);
            this.splitContainer1.Panel2.Controls.Add(this.lblVertikalniStubiic);
            this.splitContainer1.Panel2.Controls.Add(this.tbVisinaStubica);
            this.splitContainer1.Size = new System.Drawing.Size(1284, 729);
            this.splitContainer1.SplitterDistance = 1024;
            this.splitContainer1.TabIndex = 0;
            // 
            // openGlWorld
            // 
            this.openGlWorld.AccumBits = ((byte)(0));
            this.openGlWorld.AutoCheckErrors = false;
            this.openGlWorld.AutoFinish = false;
            this.openGlWorld.AutoMakeCurrent = true;
            this.openGlWorld.AutoSwapBuffers = true;
            this.openGlWorld.BackColor = System.Drawing.Color.Black;
            this.openGlWorld.ColorBits = ((byte)(32));
            this.openGlWorld.DepthBits = ((byte)(16));
            this.openGlWorld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openGlWorld.Location = new System.Drawing.Point(0, 0);
            this.openGlWorld.Margin = new System.Windows.Forms.Padding(2);
            this.openGlWorld.Name = "openGlWorld";
            this.openGlWorld.Size = new System.Drawing.Size(1024, 729);
            this.openGlWorld.StencilBits = ((byte)(0));
            this.openGlWorld.TabIndex = 1;
            this.openGlWorld.Paint += new System.Windows.Forms.PaintEventHandler(this.OpenGlControlPaint);
            this.openGlWorld.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openGlWorld_KeyDown);
            this.openGlWorld.Resize += new System.EventHandler(this.OpenGlControlResize);
            // 
            // btnPonistiSveIzmene
            // 
            this.btnPonistiSveIzmene.BackColor = System.Drawing.Color.Red;
            this.btnPonistiSveIzmene.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPonistiSveIzmene.ForeColor = System.Drawing.Color.White;
            this.btnPonistiSveIzmene.Location = new System.Drawing.Point(25, 323);
            this.btnPonistiSveIzmene.Name = "btnPonistiSveIzmene";
            this.btnPonistiSveIzmene.Size = new System.Drawing.Size(189, 32);
            this.btnPonistiSveIzmene.TabIndex = 12;
            this.btnPonistiSveIzmene.Text = "Poništi sve izmene";
            this.btnPonistiSveIzmene.UseVisualStyleBackColor = false;
            // 
            // tbPomeriDrugi
            // 
            this.tbPomeriDrugi.Location = new System.Drawing.Point(127, 269);
            this.tbPomeriDrugi.Name = "tbPomeriDrugi";
            this.tbPomeriDrugi.Size = new System.Drawing.Size(87, 45);
            this.tbPomeriDrugi.TabIndex = 11;
            this.tbPomeriDrugi.Scroll += new System.EventHandler(this.tbPomeriDrugi_Scroll);
            // 
            // lblPomeriDrugi
            // 
            this.lblPomeriDrugi.AutoSize = true;
            this.lblPomeriDrugi.Location = new System.Drawing.Point(56, 276);
            this.lblPomeriDrugi.Name = "lblPomeriDrugi";
            this.lblPomeriDrugi.Size = new System.Drawing.Size(65, 13);
            this.lblPomeriDrugi.TabIndex = 10;
            this.lblPomeriDrugi.Text = "Pomeri drugi";
            // 
            // tbPomeriPrvi
            // 
            this.tbPomeriPrvi.Location = new System.Drawing.Point(127, 227);
            this.tbPomeriPrvi.Name = "tbPomeriPrvi";
            this.tbPomeriPrvi.Size = new System.Drawing.Size(87, 45);
            this.tbPomeriPrvi.TabIndex = 9;
            this.tbPomeriPrvi.Scroll += new System.EventHandler(this.tbPomeriPrvi_Scroll);
            // 
            // lblPomeriPrvi
            // 
            this.lblPomeriPrvi.AutoSize = true;
            this.lblPomeriPrvi.Location = new System.Drawing.Point(63, 237);
            this.lblPomeriPrvi.Name = "lblPomeriPrvi";
            this.lblPomeriPrvi.Size = new System.Drawing.Size(59, 13);
            this.lblPomeriPrvi.TabIndex = 8;
            this.lblPomeriPrvi.Text = "Pomeri prvi";
            // 
            // lblPomeranjeAutomobila
            // 
            this.lblPomeranjeAutomobila.AutoSize = true;
            this.lblPomeranjeAutomobila.Location = new System.Drawing.Point(21, 207);
            this.lblPomeranjeAutomobila.Name = "lblPomeranjeAutomobila";
            this.lblPomeranjeAutomobila.Size = new System.Drawing.Size(105, 13);
            this.lblPomeranjeAutomobila.TabIndex = 7;
            this.lblPomeranjeAutomobila.Text = "Pomeranje autombila";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DimGray;
            this.panel2.Location = new System.Drawing.Point(17, 193);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(208, 1);
            this.panel2.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Location = new System.Drawing.Point(15, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(208, 1);
            this.panel1.TabIndex = 5;
            // 
            // btnIzborBoje
            // 
            this.btnIzborBoje.Location = new System.Drawing.Point(70, 149);
            this.btnIzborBoje.Name = "btnIzborBoje";
            this.btnIzborBoje.Size = new System.Drawing.Size(144, 23);
            this.btnIzborBoje.TabIndex = 4;
            this.btnIzborBoje.Text = "Izaberi boju";
            this.btnIzborBoje.UseVisualStyleBackColor = true;
            this.btnIzborBoje.Click += new System.EventHandler(this.btnIzborBoje_Click);
            // 
            // pnlIzborBOje
            // 
            this.pnlIzborBOje.BackColor = System.Drawing.Color.Yellow;
            this.pnlIzborBOje.Location = new System.Drawing.Point(25, 149);
            this.pnlIzborBOje.Name = "pnlIzborBOje";
            this.pnlIzborBOje.Size = new System.Drawing.Size(26, 24);
            this.pnlIzborBOje.TabIndex = 3;
            // 
            // lblIzborBoje
            // 
            this.lblIzborBoje.AutoSize = true;
            this.lblIzborBoje.Location = new System.Drawing.Point(22, 129);
            this.lblIzborBoje.Name = "lblIzborBoje";
            this.lblIzborBoje.Size = new System.Drawing.Size(193, 13);
            this.lblIzborBoje.TabIndex = 2;
            this.lblIzborBoje.Text = "Izbor boje ambijentalnog izvora svetlosti";
            // 
            // lblVertikalniStubiic
            // 
            this.lblVertikalniStubiic.AutoSize = true;
            this.lblVertikalniStubiic.Location = new System.Drawing.Point(22, 45);
            this.lblVertikalniStubiic.Name = "lblVertikalniStubiic";
            this.lblVertikalniStubiic.Size = new System.Drawing.Size(123, 13);
            this.lblVertikalniStubiic.TabIndex = 1;
            this.lblVertikalniStubiic.Text = "Visina vertikalnih stubića";
            // 
            // tbVisinaStubica
            // 
            this.tbVisinaStubica.Location = new System.Drawing.Point(18, 66);
            this.tbVisinaStubica.Name = "tbVisinaStubica";
            this.tbVisinaStubica.Size = new System.Drawing.Size(196, 45);
            this.tbVisinaStubica.TabIndex = 0;
            this.tbVisinaStubica.Scroll += new System.EventHandler(this.tbVisinaStubica_Scroll);
            // 
            // carParkingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 729);
            this.Controls.Add(this.splitContainer1);
            this.Name = "carParkingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Form";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbPomeriDrugi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPomeriPrvi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVisinaStubica)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Tao.Platform.Windows.SimpleOpenGlControl openGlWorld;
        private System.Windows.Forms.Label lblVertikalniStubiic;
        private System.Windows.Forms.TrackBar tbVisinaStubica;
        private System.Windows.Forms.Label lblIzborBoje;
        private System.Windows.Forms.Panel pnlIzborBOje;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnIzborBoje;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TrackBar tbPomeriDrugi;
        private System.Windows.Forms.Label lblPomeriDrugi;
        private System.Windows.Forms.TrackBar tbPomeriPrvi;
        private System.Windows.Forms.Label lblPomeriPrvi;
        private System.Windows.Forms.Label lblPomeranjeAutomobila;
        private System.Windows.Forms.Button btnPonistiSveIzmene;
    }
}

