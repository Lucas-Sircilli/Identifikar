
using IdentifikarBio;
using IdentifikarBio.Database;
using IdentifikarBio.Database.Objetos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IdentifikarBio.Interface
{
    public partial class FormLogin : Form
    {
        private ObUsuario usuarioCriar = new ObUsuario();
        private ObToken token = new ObToken();
        ObRespToken respToken = new ObRespToken();
        GerenteConexoes teste = new GerenteConexoes();
        FormPrincipal formPrincipal;

        List<ObUsuario> usuarios;
        FormLoader loader;
        public FormLogin(FormPrincipal _formPrincipal)
        {
            formPrincipal = _formPrincipal;
            InitializeComponent();
            loader = new FormLoader();
        }

        public FormLogin()
        {
            InitializeComponent();
            loader = new FormLoader();

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            usuarioCriar.id_usuario = 1;
            usuarioCriar.nome = "Administração Indetifikar";
            usuarioCriar.username = "identifikar";

            this.lbVersao.Text = "Versão: " + Application.ProductVersion;
            //StaDBContext staDB = new StaDBContext();
            usuarios = new List<ObUsuario>();
            usuarios.Add(usuarioCriar);
            usuarios.Add(FormPrincipal.usuarioPad);
            this.usuarioBindingSource.DataSource = usuarios;
            txtUsuarios.Focus();
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FormPrincipal.usuario == null)
            {
                if (formPrincipal.preparandParaFechar != true)
                {
                    var result = MessageBox.Show("Você deseja realmente fechar o sistema Identifikar?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (result != DialogResult.Yes)
                        e.Cancel = true;
                    else
                    {
                        formPrincipal.preparandParaFechar = true;
                        Application.Exit();
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void consultaLogin()
        {
            try
            {

                token.user = txtUsuarios.Text;
                token.pass = txtSenha.Text;

                respToken = teste.AcessaWebservice(token);
                if (respToken != null && !string.IsNullOrEmpty(respToken.access_token))
                {

                    var user = txtUsuarios.Text;
                    formPrincipal.Invoke(new MethodInvoker(() =>
                    {
                        loader.Hide();
                        formPrincipal.DefineUsuarioAtual(user);
                        this.Close();
                        formPrincipal.HabilitaParametros(respToken.admin);
                    }));

                }
                else
                {
                    formPrincipal.Invoke(new MethodInvoker(() =>
                    {
                        this.Hide();
                        if (teste.responseFromServer.Contains("Credenciais inválidas"))
                            //MessageBox.Show(teste.responseFromServer);

                            FormPrincipal.log.write("Credenciais inválidas");
                        MessageBox.Show("Credenciais inválida\r\nVerifique o usuário e senha digitados", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        this.Show(formPrincipal);
                    }));
                    desbloqueiaTela();
                }


            }
            catch (Exception ex)
            {
                desbloqueiaTela();
                if (ex.Message.Contains("O nome remoto não pôde ser resolvido"))
                {
                    MessageBox.Show("Não foi possivel verificar o usuário no servidor\r\nVerifique a sua conexão e tente novamente", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Falha na tentativa de conexão ao servidor\r\n" + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                this.Show(formPrincipal);
                
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                var obt = new ObTipo(1);
                bgExecutaConsulta.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                desbloqueiaTela();
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

                btnEntrar_Click(sender, e);
            }
        }

        private void FormLogin_Activated(object sender, EventArgs e)
        {
            txtUsuarios.Focus();
        }

        private void bnIniciaLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            carregaLoader();
        }

        private void bgDesligaLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            descarregaLoader();
        }

        public void IniciaCarregaLoader()
        {
            if (bnIniciaLoader.IsBusy) return;
            bnIniciaLoader.RunWorkerAsync();
        }

        public void IniciaDescarregaLoader()
        {
            if (bgDesligaLoader.IsBusy) return;
            bgDesligaLoader.RunWorkerAsync();

        }

        public void carregaLoader()
        {
            try
            {
                btnEntrar.Invoke(new MethodInvoker(() =>
                {
                    if (loader == null || loader.IsDisposed)
                        loader = new FormLoader();
                    loader.Show(this);
                    loader.BringToFront();
                }));
            }
            catch (Exception e) { }
        }

        public void descarregaLoader()
        {
            try
            {
                btnEntrar.Invoke(new MethodInvoker(() =>
                {
                    loader.Hide();
                    loader.Close();

                }));
            }
            catch (Exception e) { }
        }

        private void bgExecutaConsulta_DoWork(object sender, DoWorkEventArgs e)
        {
            consultaLogin();
        }

        private void bgExecutaConsulta_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            descarregaLoader();
            //desbloqueiaTela();
        }

        private void desbloqueiaTela()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                this.Enabled = true;
                this.BringToFront();
            }));
        }

        private void btnConfigProxy_Click(object sender, EventArgs e)
        {
            formPrincipal.AbrirTelaProxy();
        }
    }
}
