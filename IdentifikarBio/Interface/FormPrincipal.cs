using IdentifikarBio.Database.Objetos;
using IdentifikarBio.Interface;
using IdentifikarBio.Suporte;
using System;
using System.Windows.Forms;


namespace IdentifikarBio
{
    public partial class FormPrincipal : Form
    {
        public static ObUsuario usuarioPad;
        public static string usuario;
        public bool preparandParaFechar = false;
        public static LogFile log;

        FormLogin formLogin;
        FormProxy formProxy;
        FormParametros frmParametro;
        FormCadastro frmCadastro;
        public FormPrincipal()
        {
            InitializeComponent();

            /* usuarioPad = new ObUsuario();
             usuarioPad.id_usuario = 2;
             usuarioPad.nome = "João da Silva";
             usuarioPad.username = "joaodasilva";
             usuarioPad.senha = "123";*/

            MenuItemCadastro.Visible = false;
            MenuItemRelatorios.Visible = false;
            MenuItemParametros.Visible = false;

            log = new LogFile(Parametros.EnderecoLog);
            DefineStatusBarra();
        }

        private void MenuItemCadastro_Click(object sender, EventArgs e)
        {
            if (frmCadastro == null || frmCadastro.IsDisposed)
            {
                frmCadastro = new FormCadastro();
                frmCadastro.Enabled = true;
                frmCadastro.Show(this);
            }
            else frmCadastro.BringToFront();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DefineStatusBarra();
        }

        public void DefineStatusBarra()
        {
            var dataAtual = DateTime.Now;
            tstlHora.Text = Convert.ToString(dataAtual.Hour).PadLeft(2, '0') + ":" + Convert.ToString(dataAtual.Minute).PadLeft(2, '0');

            var diaSemana = "";
            switch (dataAtual.DayOfWeek)
            {
                case DayOfWeek.Monday: diaSemana = "Segunda-Feira"; break;
                case DayOfWeek.Tuesday: diaSemana = "Terça-Feira"; break;
                case DayOfWeek.Wednesday: diaSemana = "Quarta-Feira"; break;
                case DayOfWeek.Thursday: diaSemana = "Quinta-Feira"; break;
                case DayOfWeek.Friday: diaSemana = "Sexta-Feira"; break;
                case DayOfWeek.Saturday: diaSemana = "Sábado"; break;
                case DayOfWeek.Sunday: diaSemana = "Domingo"; break;


            }
            tstlData.Text = diaSemana + ", " + Convert.ToString(dataAtual.Day).PadLeft(2, '0') + "/" + Convert.ToString(dataAtual.Month).PadLeft(2, '0') + "/" + Convert.ToString(dataAtual.Year).PadLeft(4, '0');
            this.tstlVersao.Text = "Versão: " + Application.ProductVersion;
        }

        public void HabilitaParametros(bool admin)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                if (admin == false)
                    MenuItemParametros.Enabled = false;
                else
                    return;
            }));
        }

        public void DefineUsuarioAtual(string _usuario)
        {
            try
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    usuario = _usuario;
                    tstlUsuario.Text = "Usuário: " + _usuario;
                    // ttsTipoUsuario.Text = usuario.IdTipoUsuario == 1 ? "Administrador Identifikar" : (usuario.IdTipoUsuario == 2 ? "Gerente de Operações" : "Operador de Pesagem");
                    log.write("Login realizado como: " + _usuario);

                    this.MenuInicial.Visible = true;
                    this.Enabled = true;
                    MenuItemCadastro.Visible = true;
                    MenuItemRelatorios.Visible = true;
                    MenuItemParametros.Visible = true;
                    this.Focus();
                }));
            }
            catch (Exception ex)
            {
                log.write("Falha na tela principal", ex);
                MessageBox.Show("Falha tela principal [" + ex.Message + "]\r\n\r\n" + ex.StackTrace, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.preparandParaFechar != true)
                {
                    var result = MessageBox.Show("Tem certeza que deseja fechar o Gestor Identifikar?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result != DialogResult.Yes)
                    {
                        log.write("Finalização do Aplicativo cancelado pelo usuário");
                        e.Cancel = true;
                    }
                    else
                    {
                        log.write("Finalizando Aplicativo");
                        this.preparandParaFechar = true;
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                log.write("Falha na tela principal", ex);
                MessageBox.Show("Falha tela principal [" + ex.Message + "]\r\n\r\n" + ex.StackTrace, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            if (usuario == null)
            {
                this.Enabled = false;
                this.MenuInicial.Visible = false;
                this.tstlUsuario.Text = "";
                log.write("Abrindo Tela de Login");
                this.Enabled = false;
                formLogin = new FormLogin(this);

                formLogin.Show(this);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void MenuItemParametros_Click(object sender, EventArgs e)
        {
            if (frmParametro == null || frmParametro.IsDisposed)
            {
                frmParametro = new FormParametros();
                frmParametro.Show(this);
            }
            else frmParametro.BringToFront();
        }

        public void AbrirTelaProxy()
        {

            this.Invoke(new MethodInvoker(() =>
            {
                formLogin.Hide();
                formProxy = new FormProxy(this);
                formProxy.Show(this);
            }));

            
        }

        public void FecharTelaProxy()
        {

            this.Invoke(new MethodInvoker(() =>
            {
                formProxy.Hide();
                formLogin.Show(this);
            }));
            
        }
    }
}

