
namespace IdentifikarBio
{
    partial class FormPrincipal
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.MenuInicial = new System.Windows.Forms.MenuStrip();
            this.MenuItemCadastro = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemRelatorios = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemParametros = new System.Windows.Forms.ToolStripMenuItem();
            this.lbData = new System.Windows.Forms.Label();
            this.ststBarraInferior = new System.Windows.Forms.StatusStrip();
            this.tstlHora = new System.Windows.Forms.ToolStripStatusLabel();
            this.tstlData = new System.Windows.Forms.ToolStripStatusLabel();
            this.tstlUsuario = new System.Windows.Forms.ToolStripStatusLabel();
            this.tstlVersao = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.MenuInicial.SuspendLayout();
            this.ststBarraInferior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuInicial
            // 
            this.MenuInicial.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuInicial.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemCadastro,
            this.MenuItemRelatorios,
            this.MenuItemParametros});
            this.MenuInicial.Location = new System.Drawing.Point(0, 0);
            this.MenuInicial.Name = "MenuInicial";
            this.MenuInicial.Size = new System.Drawing.Size(800, 24);
            this.MenuInicial.TabIndex = 0;
            this.MenuInicial.Text = "menuStrip1";
            // 
            // MenuItemCadastro
            // 
            this.MenuItemCadastro.Name = "MenuItemCadastro";
            this.MenuItemCadastro.Size = new System.Drawing.Size(66, 20);
            this.MenuItemCadastro.Text = "Cadastro";
            this.MenuItemCadastro.Click += new System.EventHandler(this.MenuItemCadastro_Click);
            // 
            // MenuItemRelatorios
            // 
            this.MenuItemRelatorios.Enabled = false;
            this.MenuItemRelatorios.Name = "MenuItemRelatorios";
            this.MenuItemRelatorios.Size = new System.Drawing.Size(71, 20);
            this.MenuItemRelatorios.Text = "Relatórios";
            // 
            // MenuItemParametros
            // 
            this.MenuItemParametros.Name = "MenuItemParametros";
            this.MenuItemParametros.Size = new System.Drawing.Size(79, 20);
            this.MenuItemParametros.Text = "Parâmetros";
            this.MenuItemParametros.Click += new System.EventHandler(this.MenuItemParametros_Click);
            // 
            // lbData
            // 
            this.lbData.AutoSize = true;
            this.lbData.Location = new System.Drawing.Point(304, 418);
            this.lbData.Name = "lbData";
            this.lbData.Size = new System.Drawing.Size(0, 13);
            this.lbData.TabIndex = 2;
            // 
            // ststBarraInferior
            // 
            this.ststBarraInferior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(205)))), ((int)(((byte)(219)))));
            this.ststBarraInferior.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ststBarraInferior.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstlHora,
            this.tstlData,
            this.tstlUsuario,
            this.tstlVersao});
            this.ststBarraInferior.Location = new System.Drawing.Point(0, 428);
            this.ststBarraInferior.Name = "ststBarraInferior";
            this.ststBarraInferior.Size = new System.Drawing.Size(800, 22);
            this.ststBarraInferior.TabIndex = 3;
            // 
            // tstlHora
            // 
            this.tstlHora.BackColor = System.Drawing.Color.Transparent;
            this.tstlHora.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.tstlHora.Name = "tstlHora";
            this.tstlHora.Padding = new System.Windows.Forms.Padding(0, 0, 60, 0);
            this.tstlHora.Size = new System.Drawing.Size(93, 17);
            this.tstlHora.Text = "Hora";
            this.tstlHora.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tstlData
            // 
            this.tstlData.BackColor = System.Drawing.Color.Transparent;
            this.tstlData.Name = "tstlData";
            this.tstlData.Size = new System.Drawing.Size(31, 17);
            this.tstlData.Text = "Data";
            // 
            // tstlUsuario
            // 
            this.tstlUsuario.Name = "tstlUsuario";
            this.tstlUsuario.Padding = new System.Windows.Forms.Padding(200, 0, 0, 0);
            this.tstlUsuario.Size = new System.Drawing.Size(247, 17);
            this.tstlUsuario.Text = "Usuário";
            // 
            // tstlVersao
            // 
            this.tstlVersao.Name = "tstlVersao";
            this.tstlVersao.Padding = new System.Windows.Forms.Padding(0, 0, 210, 0);
            this.tstlVersao.Size = new System.Drawing.Size(251, 17);
            this.tstlVersao.Text = "Versao";
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLogo.Image = global::IdentifikarBio.Properties.Resources.logoidentifikar;
            this.pbLogo.Location = new System.Drawing.Point(245, 188);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(333, 76);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 4;
            this.pbLogo.TabStop = false;
            this.pbLogo.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(205)))), ((int)(((byte)(219)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.ststBarraInferior);
            this.Controls.Add(this.lbData);
            this.Controls.Add(this.MenuInicial);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuInicial;
            this.Name = "FormPrincipal";
            this.Text = "Sistema Identifikar";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            this.MenuInicial.ResumeLayout(false);
            this.MenuInicial.PerformLayout();
            this.ststBarraInferior.ResumeLayout(false);
            this.ststBarraInferior.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuInicial;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCadastro;
        private System.Windows.Forms.ToolStripMenuItem MenuItemRelatorios;
        private System.Windows.Forms.ToolStripMenuItem MenuItemParametros;
        private System.Windows.Forms.Label lbData;
        private System.Windows.Forms.StatusStrip ststBarraInferior;
        private System.Windows.Forms.ToolStripStatusLabel tstlHora;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel tstlData;
        private System.Windows.Forms.ToolStripStatusLabel tstlUsuario;
        private System.Windows.Forms.ToolStripStatusLabel tstlVersao;
        private System.Windows.Forms.PictureBox pbLogo;
    }
}

