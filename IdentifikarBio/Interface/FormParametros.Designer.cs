
namespace IdentifikarBio.Interface
{
    partial class FormParametros
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormParametros));
            this.tabParametros = new System.Windows.Forms.TabControl();
            this.tabParamScanner = new System.Windows.Forms.TabPage();
            this.trcParadaQualidade = new System.Windows.Forms.TrackBar();
            this.trcQualidade = new System.Windows.Forms.TrackBar();
            this.cbListaScanner = new System.Windows.Forms.ComboBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.txtChecaRuido = new System.Windows.Forms.TextBox();
            this.lbCapturaQualidade = new System.Windows.Forms.Label();
            this.lbChecaRuido = new System.Windows.Forms.Label();
            this.txtParadaQualidade = new System.Windows.Forms.TextBox();
            this.lbTimeout = new System.Windows.Forms.Label();
            this.lbMunicipio = new System.Windows.Forms.Label();
            this.txtCapturaQualidade = new System.Windows.Forms.TextBox();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.lbParadaQualidade = new System.Windows.Forms.Label();
            this.tabCamera = new System.Windows.Forms.TabPage();
            this.cbResolucaoCamera = new System.Windows.Forms.ComboBox();
            this.cbListaCamera = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.imgLista = new System.Windows.Forms.ImageList(this.components);
            this.btnGravar = new System.Windows.Forms.Button();
            this.tabParametros.SuspendLayout();
            this.tabParamScanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trcParadaQualidade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trcQualidade)).BeginInit();
            this.tabCamera.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabParametros
            // 
            this.tabParametros.Controls.Add(this.tabParamScanner);
            this.tabParametros.Controls.Add(this.tabCamera);
            this.tabParametros.ImageList = this.imgLista;
            this.tabParametros.Location = new System.Drawing.Point(14, 12);
            this.tabParametros.Name = "tabParametros";
            this.tabParametros.SelectedIndex = 0;
            this.tabParametros.Size = new System.Drawing.Size(706, 405);
            this.tabParametros.TabIndex = 26;
            // 
            // tabParamScanner
            // 
            this.tabParamScanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(205)))), ((int)(((byte)(219)))));
            this.tabParamScanner.Controls.Add(this.trcParadaQualidade);
            this.tabParamScanner.Controls.Add(this.trcQualidade);
            this.tabParamScanner.Controls.Add(this.cbListaScanner);
            this.tabParamScanner.Controls.Add(this.splitter1);
            this.tabParamScanner.Controls.Add(this.txtChecaRuido);
            this.tabParamScanner.Controls.Add(this.lbCapturaQualidade);
            this.tabParamScanner.Controls.Add(this.lbChecaRuido);
            this.tabParamScanner.Controls.Add(this.txtParadaQualidade);
            this.tabParamScanner.Controls.Add(this.lbTimeout);
            this.tabParamScanner.Controls.Add(this.lbMunicipio);
            this.tabParamScanner.Controls.Add(this.txtCapturaQualidade);
            this.tabParamScanner.Controls.Add(this.txtTimeout);
            this.tabParamScanner.Controls.Add(this.lbParadaQualidade);
            this.tabParamScanner.ImageIndex = 4;
            this.tabParamScanner.Location = new System.Drawing.Point(4, 23);
            this.tabParamScanner.Name = "tabParamScanner";
            this.tabParamScanner.Padding = new System.Windows.Forms.Padding(3);
            this.tabParamScanner.Size = new System.Drawing.Size(698, 378);
            this.tabParamScanner.TabIndex = 0;
            this.tabParamScanner.Text = "Scanner Biométrico";
            // 
            // trcParadaQualidade
            // 
            this.trcParadaQualidade.Location = new System.Drawing.Point(217, 214);
            this.trcParadaQualidade.Maximum = 100;
            this.trcParadaQualidade.Name = "trcParadaQualidade";
            this.trcParadaQualidade.Size = new System.Drawing.Size(145, 45);
            this.trcParadaQualidade.TabIndex = 49;
            this.trcParadaQualidade.Scroll += new System.EventHandler(this.trcParadaQualidade_Scroll);
            // 
            // trcQualidade
            // 
            this.trcQualidade.Location = new System.Drawing.Point(217, 101);
            this.trcQualidade.Maximum = 100;
            this.trcQualidade.Name = "trcQualidade";
            this.trcQualidade.Size = new System.Drawing.Size(145, 45);
            this.trcQualidade.TabIndex = 3;
            this.trcQualidade.Scroll += new System.EventHandler(this.trcQualidade_Scroll);
            // 
            // cbListaScanner
            // 
            this.cbListaScanner.FormattingEnabled = true;
            this.cbListaScanner.Items.AddRange(new object[] {
            "DFDU-500P"});
            this.cbListaScanner.Location = new System.Drawing.Point(217, 64);
            this.cbListaScanner.Name = "cbListaScanner";
            this.cbListaScanner.Size = new System.Drawing.Size(186, 21);
            this.cbListaScanner.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(3, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 372);
            this.splitter1.TabIndex = 48;
            this.splitter1.TabStop = false;
            // 
            // txtChecaRuido
            // 
            this.txtChecaRuido.Location = new System.Drawing.Point(217, 188);
            this.txtChecaRuido.Name = "txtChecaRuido";
            this.txtChecaRuido.Size = new System.Drawing.Size(186, 20);
            this.txtChecaRuido.TabIndex = 8;
            // 
            // lbCapturaQualidade
            // 
            this.lbCapturaQualidade.AutoSize = true;
            this.lbCapturaQualidade.Location = new System.Drawing.Point(101, 99);
            this.lbCapturaQualidade.Name = "lbCapturaQualidade";
            this.lbCapturaQualidade.Size = new System.Drawing.Size(110, 13);
            this.lbCapturaQualidade.TabIndex = 2;
            this.lbCapturaQualidade.Text = "Qualidade de Captura";
            // 
            // lbChecaRuido
            // 
            this.lbChecaRuido.AutoSize = true;
            this.lbChecaRuido.Location = new System.Drawing.Point(83, 195);
            this.lbChecaRuido.Name = "lbChecaRuido";
            this.lbChecaRuido.Size = new System.Drawing.Size(128, 13);
            this.lbChecaRuido.TabIndex = 7;
            this.lbChecaRuido.Text = "Nível de Ruído Aceitável";
            // 
            // txtParadaQualidade
            // 
            this.txtParadaQualidade.Location = new System.Drawing.Point(368, 222);
            this.txtParadaQualidade.Name = "txtParadaQualidade";
            this.txtParadaQualidade.Size = new System.Drawing.Size(35, 20);
            this.txtParadaQualidade.TabIndex = 12;
            this.txtParadaQualidade.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtParadaQualidade.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtParadaQualidade_KeyPress);
            // 
            // lbTimeout
            // 
            this.lbTimeout.AutoSize = true;
            this.lbTimeout.Location = new System.Drawing.Point(102, 155);
            this.lbTimeout.Name = "lbTimeout";
            this.lbTimeout.Size = new System.Drawing.Size(109, 13);
            this.lbTimeout.TabIndex = 5;
            this.lbTimeout.Text = "Time Out Padrão (ms)";
            // 
            // lbMunicipio
            // 
            this.lbMunicipio.AutoSize = true;
            this.lbMunicipio.Location = new System.Drawing.Point(111, 67);
            this.lbMunicipio.Name = "lbMunicipio";
            this.lbMunicipio.Size = new System.Drawing.Size(100, 13);
            this.lbMunicipio.TabIndex = 0;
            this.lbMunicipio.Text = "Selecionar Scanner";
            // 
            // txtCapturaQualidade
            // 
            this.txtCapturaQualidade.Location = new System.Drawing.Point(368, 101);
            this.txtCapturaQualidade.Name = "txtCapturaQualidade";
            this.txtCapturaQualidade.Size = new System.Drawing.Size(35, 20);
            this.txtCapturaQualidade.TabIndex = 4;
            this.txtCapturaQualidade.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCapturaQualidade.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCapturaQualidade_KeyPress);
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(217, 152);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(186, 20);
            this.txtTimeout.TabIndex = 6;
            // 
            // lbParadaQualidade
            // 
            this.lbParadaQualidade.AutoSize = true;
            this.lbParadaQualidade.Location = new System.Drawing.Point(25, 225);
            this.lbParadaQualidade.Name = "lbParadaQualidade";
            this.lbParadaQualidade.Size = new System.Drawing.Size(186, 13);
            this.lbParadaQualidade.TabIndex = 11;
            this.lbParadaQualidade.Text = "Qualidade Para Finalização de Leitura";
            // 
            // tabCamera
            // 
            this.tabCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(205)))), ((int)(((byte)(219)))));
            this.tabCamera.Controls.Add(this.cbResolucaoCamera);
            this.tabCamera.Controls.Add(this.cbListaCamera);
            this.tabCamera.Controls.Add(this.label1);
            this.tabCamera.Controls.Add(this.label2);
            this.tabCamera.ImageIndex = 2;
            this.tabCamera.Location = new System.Drawing.Point(4, 23);
            this.tabCamera.Name = "tabCamera";
            this.tabCamera.Padding = new System.Windows.Forms.Padding(3);
            this.tabCamera.Size = new System.Drawing.Size(698, 378);
            this.tabCamera.TabIndex = 1;
            this.tabCamera.Text = "Câmera Facial";
            // 
            // cbResolucaoCamera
            // 
            this.cbResolucaoCamera.FormattingEnabled = true;
            this.cbResolucaoCamera.Items.AddRange(new object[] {
            "800 x 640"});
            this.cbResolucaoCamera.Location = new System.Drawing.Point(217, 96);
            this.cbResolucaoCamera.Name = "cbResolucaoCamera";
            this.cbResolucaoCamera.Size = new System.Drawing.Size(244, 21);
            this.cbResolucaoCamera.TabIndex = 7;
            // 
            // cbListaCamera
            // 
            this.cbListaCamera.FormattingEnabled = true;
            this.cbListaCamera.Items.AddRange(new object[] {
            "HD Web Camera"});
            this.cbListaCamera.Location = new System.Drawing.Point(217, 64);
            this.cbListaCamera.Name = "cbListaCamera";
            this.cbListaCamera.Size = new System.Drawing.Size(244, 21);
            this.cbListaCamera.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Resolução";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(115, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Selecionar Câmera";
            // 
            // imgLista
            // 
            this.imgLista.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLista.ImageStream")));
            this.imgLista.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLista.Images.SetKeyName(0, "user.png");
            this.imgLista.Images.SetKeyName(1, "AttributeRelationshipEditor.png");
            this.imgLista.Images.SetKeyName(2, "CameraOutline.png");
            this.imgLista.Images.SetKeyName(3, "ContactCard.png");
            this.imgLista.Images.SetKeyName(4, "FieldProtected.png");
            // 
            // btnGravar
            // 
            this.btnGravar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGravar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGravar.Image = ((System.Drawing.Image)(resources.GetObject("btnGravar.Image")));
            this.btnGravar.Location = new System.Drawing.Point(643, 428);
            this.btnGravar.Name = "btnGravar";
            this.btnGravar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGravar.Size = new System.Drawing.Size(75, 25);
            this.btnGravar.TabIndex = 27;
            this.btnGravar.Text = " Gravar";
            this.btnGravar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGravar.UseVisualStyleBackColor = true;
            this.btnGravar.Click += new System.EventHandler(this.btnGravar_Click);
            // 
            // FormParametros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(205)))), ((int)(((byte)(219)))));
            this.ClientSize = new System.Drawing.Size(732, 463);
            this.Controls.Add(this.btnGravar);
            this.Controls.Add(this.tabParametros);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormParametros";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormParametros";
            this.tabParametros.ResumeLayout(false);
            this.tabParamScanner.ResumeLayout(false);
            this.tabParamScanner.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trcParadaQualidade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trcQualidade)).EndInit();
            this.tabCamera.ResumeLayout(false);
            this.tabCamera.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabParametros;
        private System.Windows.Forms.TabPage tabParamScanner;
        private System.Windows.Forms.ComboBox cbListaScanner;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox txtChecaRuido;
        private System.Windows.Forms.Label lbCapturaQualidade;
        private System.Windows.Forms.Label lbChecaRuido;
        private System.Windows.Forms.TextBox txtParadaQualidade;
        private System.Windows.Forms.Label lbTimeout;
        private System.Windows.Forms.Label lbMunicipio;
        private System.Windows.Forms.TextBox txtCapturaQualidade;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label lbParadaQualidade;
        private System.Windows.Forms.ImageList imgLista;
        private System.Windows.Forms.Button btnGravar;
        private System.Windows.Forms.TabPage tabCamera;
        private System.Windows.Forms.ComboBox cbListaCamera;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trcQualidade;
        private System.Windows.Forms.TrackBar trcParadaQualidade;
        private System.Windows.Forms.ComboBox cbResolucaoCamera;
    }
}