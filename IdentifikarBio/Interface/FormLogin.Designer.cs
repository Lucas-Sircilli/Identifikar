namespace IdentifikarBio.Interface
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.lbUsuario = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.lbSenha = new System.Windows.Forms.Label();
            this.btnEntrar = new System.Windows.Forms.Button();
            this.lbVersao = new System.Windows.Forms.Label();
            this.txtUsuarios = new System.Windows.Forms.TextBox();
            this.bnIniciaLoader = new System.ComponentModel.BackgroundWorker();
            this.bgDesligaLoader = new System.ComponentModel.BackgroundWorker();
            this.bgExecutaConsulta = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.usuarioBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnConfigProxy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usuarioBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lbUsuario
            // 
            this.lbUsuario.AutoSize = true;
            this.lbUsuario.Location = new System.Drawing.Point(115, 15);
            this.lbUsuario.Name = "lbUsuario";
            this.lbUsuario.Size = new System.Drawing.Size(43, 13);
            this.lbUsuario.TabIndex = 1;
            this.lbUsuario.Text = "Usuário";
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(118, 74);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(207, 20);
            this.txtSenha.TabIndex = 4;
            this.txtSenha.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSenha_KeyPress);
            // 
            // lbSenha
            // 
            this.lbSenha.AutoSize = true;
            this.lbSenha.Location = new System.Drawing.Point(115, 58);
            this.lbSenha.Name = "lbSenha";
            this.lbSenha.Size = new System.Drawing.Size(38, 13);
            this.lbSenha.TabIndex = 3;
            this.lbSenha.Text = "Senha";
            // 
            // btnEntrar
            // 
            this.btnEntrar.Location = new System.Drawing.Point(333, 71);
            this.btnEntrar.Name = "btnEntrar";
            this.btnEntrar.Size = new System.Drawing.Size(121, 23);
            this.btnEntrar.TabIndex = 5;
            this.btnEntrar.Text = "Entrar";
            this.btnEntrar.UseVisualStyleBackColor = true;
            this.btnEntrar.Click += new System.EventHandler(this.btnEntrar_Click);
            // 
            // lbVersao
            // 
            this.lbVersao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbVersao.AutoSize = true;
            this.lbVersao.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbVersao.Location = new System.Drawing.Point(376, 115);
            this.lbVersao.Name = "lbVersao";
            this.lbVersao.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbVersao.Size = new System.Drawing.Size(78, 17);
            this.lbVersao.TabIndex = 7;
            this.lbVersao.Text = "Versão 1.0.0.0";
            this.lbVersao.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lbVersao.UseCompatibleTextRendering = true;
            this.lbVersao.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtUsuarios
            // 
            this.txtUsuarios.Location = new System.Drawing.Point(118, 31);
            this.txtUsuarios.Name = "txtUsuarios";
            this.txtUsuarios.Size = new System.Drawing.Size(336, 20);
            this.txtUsuarios.TabIndex = 2;
            // 
            // bnIniciaLoader
            // 
            this.bnIniciaLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bnIniciaLoader_DoWork);
            // 
            // bgDesligaLoader
            // 
            this.bgDesligaLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgDesligaLoader_DoWork);
            // 
            // bgExecutaConsulta
            // 
            this.bgExecutaConsulta.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgExecutaConsulta_DoWork);
            this.bgExecutaConsulta.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgExecutaConsulta_RunWorkerCompleted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 95);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnConfigProxy
            // 
            this.btnConfigProxy.AutoSize = true;
            this.btnConfigProxy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnConfigProxy.FlatAppearance.BorderSize = 0;
            this.btnConfigProxy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfigProxy.ForeColor = System.Drawing.Color.Black;
            this.btnConfigProxy.Image = global::IdentifikarBio.Properties.Resources.Diamond;
            this.btnConfigProxy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfigProxy.Location = new System.Drawing.Point(12, 108);
            this.btnConfigProxy.Name = "btnConfigProxy";
            this.btnConfigProxy.Size = new System.Drawing.Size(117, 26);
            this.btnConfigProxy.TabIndex = 6;
            this.btnConfigProxy.Text = "Proxy";
            this.btnConfigProxy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConfigProxy.UseVisualStyleBackColor = true;
            this.btnConfigProxy.Click += new System.EventHandler(this.btnConfigProxy_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(464, 141);
            this.Controls.Add(this.btnConfigProxy);
            this.Controls.Add(this.txtUsuarios);
            this.Controls.Add(this.lbVersao);
            this.Controls.Add(this.btnEntrar);
            this.Controls.Add(this.lbSenha);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.lbUsuario);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Activated += new System.EventHandler(this.FormLogin_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLogin_FormClosing);
            this.Load += new System.EventHandler(this.FormLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usuarioBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.BindingSource usuarioBindingSource;
        private System.Windows.Forms.Label lbUsuario;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Label lbSenha;
        private System.Windows.Forms.Button btnEntrar;
        private System.Windows.Forms.Label lbVersao;
        private System.Windows.Forms.TextBox txtUsuarios;
        private System.ComponentModel.BackgroundWorker bnIniciaLoader;
        private System.ComponentModel.BackgroundWorker bgDesligaLoader;
        private System.ComponentModel.BackgroundWorker bgExecutaConsulta;
        private System.Windows.Forms.Button btnConfigProxy;
    }
}