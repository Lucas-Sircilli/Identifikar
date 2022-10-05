using IdentifikarBio.Database;
using IdentifikarBio.Database.Objetos;
using IdentifikarBio.Dispositivos;
using IdentifikarBio.Interface;
using IdentifikarBio.Suporte;
using MySql.Data.MySqlClient;
using NITGEN.SDK.NBioScan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media.Capture;
using Windows.Storage;
using static IdentifikarBio.Database.CadastroDAO;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Threading;
using System.Globalization;
using static IdentifikarBio.Suporte.Funcoes;

namespace IdentifikarBio
{
    public partial class FormCadastro : Form
    {
        GerenteConexoes requests = new GerenteConexoes();
        public static int contador;
        LogFile log;
        ObRespMunicipio listamunicipio = new ObRespMunicipio();
        ObRespCadastro consultaCadastro = new ObRespCadastro();
        ObRespImagemFoto consultaFoto = new ObRespImagemFoto();
        ObRespImagemAssinatura consultaAssinatura = new ObRespImagemAssinatura();
        ObRespImagemBiometria consultaBiometria = new ObRespImagemBiometria();
        List<ObRespImagemBiometria> listaBiometria = new List<ObRespImagemBiometria>();
        ObCadastro cadastro = new ObCadastro();
        FormImgBio frmImgBio;

        static ObImagem imagem;

        ResolucaoCamera dadosresolucao = new ResolucaoCamera();
        LeitorBiometrico leitorBiometrico;
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;

        public string caminhoImagemSalva = null;
        bool statuscamera = false, statusbiometria = false, statusassinatura = false;
        RJButton bbase;
        RJButton bPolegarD = new RJButton(530, 203, 1, "Polegar Direito");
        RJButton bIndicadorD = new RJButton(549, 146, 2, "Indicador Direito");
        RJButton bMedioD = new RJButton(569, 133, 3, "Médio Direito");
        RJButton bAnelarD = new RJButton(589, 138, 4, "Anelar Direito");
        RJButton bMinimoD = new RJButton(608, 169, 5, "Mínimo Direito");

        RJButton bPolegarE = new RJButton(154, 203, 6, "Polegar Esquerdo");
        RJButton bIndicadorE = new RJButton(135, 145, 7, "Indicador Esquerdo");
        RJButton bMedioE = new RJButton(115, 133, 8, "Médio Esquerdo");
        RJButton bAnelarE = new RJButton(95, 138, 9, "Anelar Esquerdo");
        RJButton bMinimoE = new RJButton(76, 169, 10, "Mínimo Esquerdo");

        GerenteAssinatura ucc;
        FormLoader loader;

       
        public FormCadastro()
        {
            try
            {
                InitializeComponent();
                
                log = new LogFile(Parametros.EnderecoLog);

                imagem = new ObImagem();
                imagem.BioCadastro = new ObImagemBiometria();
                imagem.Assinatura = new ObImagemAssinatura();
                imagem.Biometrias = new List<ObImagemBiometria>();
                imagem.Foto = new ObImagemFoto();

                leitorBiometrico = new LeitorBiometrico(log);

                
                cbSexo.SelectedIndex = 0;
                cbUF.SelectedIndex = 24;
                cbExpedido.SelectedIndex = 24;
                //cbExcessDigital.SelectedIndex = 0;

                tabBiometriaD.Controls.Add(bPolegarD);
                tabBiometriaD.Controls.Add(bIndicadorD);
                tabBiometriaD.Controls.Add(bMedioD);
                tabBiometriaD.Controls.Add(bAnelarD);
                tabBiometriaD.Controls.Add(bMinimoD);
                tabBiometriaD.Controls.Add(bPolegarE);
                tabBiometriaD.Controls.Add(bIndicadorE);
                tabBiometriaD.Controls.Add(bMedioE);
                tabBiometriaD.Controls.Add(bAnelarE);
                tabBiometriaD.Controls.Add(bMinimoE);

                bbase = bIndicadorD;

                bPolegarD.BringToFront();
                bIndicadorD.BringToFront();
                bMedioD.BringToFront();
                bAnelarD.BringToFront();
                bMinimoD.BringToFront();

                bPolegarE.BringToFront();
                bIndicadorE.BringToFront();
                bMedioE.BringToFront();
                bAnelarE.BringToFront();
                bMinimoE.BringToFront();

                bPolegarD.Click += trataCliqueDedos;
                bIndicadorD.Click += trataCliqueDedos;
                bMedioD.Click += trataCliqueDedos;
                bAnelarD.Click += trataCliqueDedos;
                bMinimoD.Click += trataCliqueDedos;

                bPolegarE.Click += trataCliqueDedos;
                bIndicadorE.Click += trataCliqueDedos;
                bMedioE.Click += trataCliqueDedos;
                bAnelarE.Click += trataCliqueDedos;
                bMinimoE.Click += trataCliqueDedos;

                loader = new FormLoader();
                ((Control)this.tabBiometriaAssinatura).Enabled = false;

                limpaDedos();
                bwCarregaDriver.RunWorkerAsync();
                
                cbListaDispositivos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                cbSexo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                cbCameraemUso.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                cbUF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                cbMunicipio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                cbExpedido.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                btnCPF.TabStop = false;
            }
            catch (Exception e) { }
        }

        public void EnviaCadastroCompleto()
        {
            try
            {
                
                btnGravar.Invoke(new MethodInvoker(() =>
                {
                    if (string.IsNullOrEmpty(txtCPF.Text))
                    {
                        MessageBox.Show("O campo do CPF é obrigatório");
                        return;
                    }

                    if (!Funcoes.ValidateCPF(txtCPF.Text))
                    {
                        MessageBox.Show("Este não é um CPF Valido");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtRENACH.Text))
                    {
                        MessageBox.Show("O campo do Renach é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtNome.Text))
                    {
                        MessageBox.Show("O campo de Nome é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtEndereco.Text))
                    {
                        MessageBox.Show("O campo de Endereço é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtNumero.Text))
                    {
                        MessageBox.Show("O campo de Número é obrigatório");
                        return;
                    }

                    
                    if (string.IsNullOrEmpty(txtBairro.Text))
                    {
                        MessageBox.Show("O campo de Bairro é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtCEP.Text))
                    {
                        MessageBox.Show("O campo de CEP é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(cbUF.Text))
                    {
                        MessageBox.Show("O campo de UF é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtTelefone.Text))
                    {
                        MessageBox.Show("O campo de Telefone é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtEmail.Text))
                    {
                        MessageBox.Show("O campo de E-mail é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtRG.Text))
                    {
                        MessageBox.Show("O campo de RG (Registro Geral) é obrigatório");
                        return;
                    }


                    if (string.IsNullOrEmpty(txtExpedido.Text))
                    {
                        MessageBox.Show("O campo de Órgão de Expedição é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtComplemento.Text))
                    {
                        MessageBox.Show("O campo do Complemento é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(cbExpedido.Text))
                    {
                        MessageBox.Show("O campo de Estado do Órgão de Expedição é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtDataNascimento.Text))
                    {
                        MessageBox.Show("O campo de Data de Nascimento é obrigatório");
                        return;
                    }

                    DateTime dt = DateTime.Now;
                    if (!DateTime.TryParse(txtDataNascimento.Text, out dt))
                    {
                        MessageBox.Show("A data de nascimento é inválida");
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtExpedidoData.Text) && txtExpedidoData.Text!= "  /  /")
                    {
                        dt = DateTime.Now;
                        if (!DateTime.TryParse(txtExpedidoData.Text, out dt))
                        {
                            MessageBox.Show("A data de expedição é inválida");
                            return;
                        }
                    }

                    if (string.IsNullOrEmpty(txtFiliacaoMae.Text))
                    {
                        MessageBox.Show("O campo de Filiação Mãe é obrigatório");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtFiliacaoPai.Text))
                    {
                        MessageBox.Show("O campo de Filiação Pai é obrigatório");
                        return;
                    }

                    

                    cadastro.cpf = txtCPF.Text.Replace(",", "").Replace("-", "");

                    cadastro.complemento = txtComplemento.Text;
                    cadastro.uf = cbUF.Text;
                    cadastro.bairro = txtBairro.Text;
                    cadastro.cep = txtCEP.Text.Replace("-", "");
                    cadastro.telefone = "(" + txtDDDTelefone.Text + ") " + txtTelefone.Text;
                    cadastro.telefone2 = "(" + txtDDDCelular.Text + ") " + txtCelular.Text;
                    cadastro.DDDCelular = txtDDDCelular.Text;
                    cadastro.DDDTelefone = txtDDDTelefone.Text;
                    cadastro.email = txtEmail.Text;
                    cadastro.endereco = txtEndereco.Text;
                    cadastro.registro_emissor = txtExpedido.Text;
                    cadastro.registro_uf = cbExpedido.Text;
                    cadastro.nome_mae = txtFiliacaoMae.Text;
                    cadastro.nome_pai = txtFiliacaoPai.Text;
                    cadastro.Municipio = cbMunicipio.Text;

                    if (!string.IsNullOrEmpty(txtIdCandidato.Text))
                        cadastro.id_candidatos = int.Parse(txtIdCandidato.Text);

                    if (!string.IsNullOrEmpty(txtExpedidoData.Text) && txtExpedidoData.Text != "  /  /" && txtExpedidoData.Text.Length == 10)
                    {
                        
                        cadastro.registro_expedicao = Convert.ToDateTime(txtExpedidoData.Text);
                    }
                    else
                        cadastro.registro_expedicao = null;
                    

                    if (!string.IsNullOrEmpty(txtDataNascimento.Text))
                    {
                        cadastro.nascimento = Convert.ToDateTime(txtDataNascimento.Text);
                    }

                    
                    cadastro.nome = txtNome.Text;
                    cadastro.numero = txtNumero.Text;
                    cadastro.renach = txtRENACH.Text;
                    cadastro.registro = txtRG.Text.Replace(",", "");
                    cadastro.registro = cadastro.registro.Replace("-", "");
                    

                    if (cbSexo.SelectedIndex == 0)
                        cadastro.sexo = "M";
                    else cadastro.sexo = "F";
                    cadastro.registro_uf = cbUF.Text;
                    cadastro.complemento = txtComplemento.Text;

                    requests.IncluiAlteraDado(cadastro);
                    PesquisaCadastroIncluido();

                    DialogResult dialogResult = MessageBox.Show("Registro gravado com sucesso!\r\nDeseja continuar editando este cadastro?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {

                    }
                    else
                    {
                        
                        this.Close();
                        loader.Close();
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("[btnGravar_Click] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                var obt = new ObTipo(6);
                bgExecutaConsultas.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void populaCamposAssinatura(ObRespImagemAssinatura consultaAssinatura)
        {
            try
            {

                consultaAssinatura.cpf = cadastro.cpf.Replace("-", "").Replace(",", "");
                consultaAssinatura.renach = cadastro.renach;

                try
                {
                    if (consultaAssinatura.foto != null & consultaAssinatura.foto.Length > 0)
                    {
                        MemoryStream img = new MemoryStream(consultaAssinatura.foto);
                        pbImgAceita.Image = Image.FromStream(img);
                    }
                }
                catch (Exception e)
                {
                    File.WriteAllBytes(@".\TesteAssinatura.png", consultaAssinatura.foto);
                    MessageBox.Show("Servidor retornou uma imagem inválida para a assinatura", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("[populaCamposAssinatura] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void populaCamposBiometria(ObRespImagemBiometria consultaBiometria)
        {

        }
        private void populaCamposFoto(ObRespImagemFoto consultaFoto)
        {

            try
            {
                if (consultaFoto.foto != null & consultaFoto.foto.Length > 0)
                {
                    MemoryStream img = new MemoryStream(consultaFoto.foto);
                    pbFotoFrente.Image = Image.FromStream(img);
                    pbFotoFrente.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception e)
            {
                File.WriteAllBytes(@".\TesteFoto.png", consultaAssinatura.foto);
                MessageBox.Show("Servidor retornou uma imagem inválida para a Foto", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void populaCamposPesquisa(ObRespCadastro consulta)
        {
            try
            {
                btnGravar.Invoke(new MethodInvoker(() =>
                {
                    string[] telefone = consulta.telefone.Replace("(", "").Split(')');
                    string[] telefone2 = consulta.telefone2.Replace("(", "").Split(')');
                    txtCPF.Text = consulta.cpf;
                    txtCPFAssinatura.Text = consulta.cpf;
                    txtCPFBio.Text = consulta.cpf;
                    txtCPFFacial.Text = consulta.cpf;

                    txtNome.Text = consulta.nome;
                    txtNomeAssinatura.Text = consulta.nome;
                    txtNomeBio.Text = consulta.nome;
                    txtNomeFacial.Text = consulta.nome;

                    txtRENACH.Text = consulta.renach;
                    txtRENACHAssinatura.Text = consulta.renach;
                    txtRENACHBio.Text = consulta.renach;
                    txtRENACHFacial.Text = consulta.renach;

                    if (consulta.sexo == "M")
                        cbSexo.SelectedIndex = 0;
                    else
                        cbSexo.SelectedIndex = 1;

                    txtIdCandidato.Text = (consulta.id_candidatos).ToString();
                    txtIdCandidato.Enabled = false;
                    txtBairro.Text = consulta.bairro;
                    txtTelefone.Text = telefone.Length > 1 ? telefone[1] : telefone[0];
                    txtCelular.Text = telefone2.Length > 1 ? telefone2[1] : telefone2[0];
                    txtCEP.Text = consulta.cep;
                    txtDDDCelular.Text = telefone2.Length > 1 ? telefone2[0] : "";
                    txtDDDTelefone.Text = telefone.Length > 1 ? telefone[0] : "";
                    txtEmail.Text = consulta.email;
                    txtNumero.Text = consulta.numero;
                    txtEndereco.Text = consulta.endereco;
                    txtExpedido.Text = consulta.registro_emissor;
                    cbExpedido.Text = consulta.registro_uf;
                    txtFiliacaoMae.Text = consulta.nome_mae;
                    txtFiliacaoPai.Text = consulta.nome_pai;

                    populaCampoCep();
                    txtDataNascimento.Text = Convert.ToString(consulta.nascimento);
                    txtExpedidoData.Text = Convert.ToString(consulta.registro_expedicao);


                    txtRG.Text = consulta.registro;
                    txtComplemento.Text = consulta.complemento;

                    cadastro.id_candidatos = consulta.id_candidatos;
                    cadastro.renach = consulta.renach;
                    cadastro.cpf = consulta.cpf.Replace(",", "").Replace("-", "");
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("[populaCamposPesquisa] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public bool ExecultaConsultaRenach()
        {
            try
            {

                int a = 3;
                cadastro.renach = txtRENACH.Text;
                cadastro.cpf = txtCPF.Text.Replace(",", "");
                cadastro.cpf = cadastro.cpf.Replace("-", "");

                if (txtIdCandidato.Text != "")
                    cadastro.id_candidatos = int.Parse(txtIdCandidato.Text);
                consultaCadastro = requests.ConsultaDado(cadastro, a);


                if (consultaCadastro == null)
                {
                    MessageBox.Show("Não encontrou dados associados a este CPF", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    populaCamposPesquisa(consultaCadastro);
                }

                try
                {
                    imagem.Assinatura.renach = cadastro.renach;
                    consultaAssinatura = requests.ConsultaAssinatura(imagem, a);
                    if (consultaAssinatura != null)
                    {
                        populaCamposAssinatura(consultaAssinatura);
                    }
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("6003"))
                    {
                        MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                try
                {
                    imagem.Foto.renach = cadastro.renach;
                    consultaFoto = requests.ConsultaFoto(imagem, a);
                    if (consultaFoto != null)
                    {
                        populaCamposFoto(consultaFoto);
                    }
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("6003"))
                    {
                        MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                try
                {
                    imagem.BioCadastro.renach = cadastro.renach;
                    var digbio = requests.ConsultaBiometriaTodas(imagem, a)[0];
                    if (digbio != null)
                    {
                        log.write("CONSULTA DE BIOMETRIAS RETORNOU: " + digbio.digitais.Count);
                        int contador = 0;
                        listaBiometria = new List<ObRespImagemBiometria>();
                        foreach (var bio in digbio.digitais)
                        {
                            if (!string.IsNullOrEmpty(bio.coletado) && bio.coletado == "1")
                            {
                                log.write("DEDO COLETADO: " + bio.indice + " status = " + bio.coletado);
                                var bioTemp = requests.ConsultaBiometriaIndice(imagem, a, bio.indice, log);
                                log.write("DEDO COLETADO 2: " + bioTemp.foto.Length + " coletado: " + bioTemp.coletado);
                               // File.WriteAllBytes(".\\WSQC-" + cadastro.renach + "-" + bioTemp.indice + ".wsq", bioTemp.foto);

                                listaBiometria.Add(bioTemp);

                                var bbb = bPolegarD;
                                switch (bio.indice)
                                {
                                    case 0: bbb = bPolegarD; break;
                                    case 1: bbb = bIndicadorD; break;
                                    case 2: bbb = bMedioD; break;
                                    case 3: bbb = bAnelarD; break;
                                    case 4: bbb = bMinimoD; break;
                                    case 5: bbb = bPolegarE; break;
                                    case 6: bbb = bIndicadorE; break;
                                    case 7: bbb = bMedioE; break;
                                    case 8: bbb = bAnelarE; break;
                                    case 9: bbb = bMinimoE; break;
                                }

                                bbb.Invoke(new MethodInvoker(() =>
                                {
                                    bbb.BackColor = Color.DarkBlue;
                                    bbb.status = 2;
                                    txtAvisoBio.Text = "Escolha o próximo dedo";
                                }));
                                if (bbb.status == 2 || bbb.status == 1)
                                {
                                    contador++;
                                }
                            }
                            else if (!string.IsNullOrEmpty(bio.coletado) && bio.coletado == "0")
                            {
                                bbase.Invoke(new MethodInvoker(() =>
                                {
                                    bbase.BackColor = Color.DarkMagenta;
                                    bbase.status = 2;
                                    txtAvisoBio.Text = "Escolha o próximo dedo";
                                }));

                                log.write("DEDO COLETADO COMO EXCECAO: " + bio.indice + " status = " + bio.coletado);
                                var bioT = new ObRespImagemBiometria();
                                bioT.coletado = bio.coletado;
                                bioT.cpf = bio.cpf;
                                bioT.data_hora_cadastro = bio.data_hora_cadastro;
                                bioT.indice = bio.indice.ToString();
                                bioT.minucias = (bio.minucias ?? 0);
                                bioT.renach = bio.renach;
                                bioT.foto = null;
                                listaBiometria.Add(bioT);
                            }
                            else
                            {
                                log.write("DEDO NAO COLETADO: " + bio.indice + " status = " + bio.coletado);
                                var bioT = new ObRespImagemBiometria();
                                bioT.coletado = bio.coletado;
                                bioT.cpf = bio.cpf;
                                bioT.data_hora_cadastro = bio.data_hora_cadastro;
                                bioT.indice = bio.indice.ToString();
                                bioT.minucias = (bio.minucias ?? 0);
                                bioT.renach = bio.renach;
                                bioT.foto = null;
                                listaBiometria.Add(bioT);
                            }
                        }
                        txtAvisoBio.Invoke(new MethodInvoker(() =>
                        {
                            if (contador == 10)
                                txtAvisoBio.Text = "Biometrias Capturadas com Sucesso!!";
                        }));

                        if (statusbiometria == false)
                            ((Control)this.tabBiometriaD).Enabled = false;
                        if (statuscamera == false)
                            ((Control)this.tabBiometriaF).Enabled = false;
                        if (statusassinatura == false)
                            ((Control)this.tabBiometriaAssinatura).Enabled = false;
                        //populaCamposFoto(consultaFoto);
                    }
                    else
                        log.write("CONSULTA DE BIOMETRIAS VEIO VAZIA");
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("6003"))
                    {
                        MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                //if(consultaBiometria != null)

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return true;
        }

        private void btnRENACH_Click(object sender, EventArgs e)
        {
            try
            {
                var obt = new ObTipo(1);
                bgExecutaConsultas.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void PesquisaCadastroIncluido()
        {
            try
            {
                int a = 1;
                cadastro.renach = txtRENACH.Text;
                cadastro.cpf = txtCPF.Text.Replace(",", "").Replace("-", "");

                if (txtIdCandidato.Text != "")
                    cadastro.id_candidatos = int.Parse(txtIdCandidato.Text);
                consultaCadastro = requests.ConsultaDado(cadastro, a);
                

                if (consultaCadastro == null)
                {
                    MessageBox.Show("Não encontrou dados associados a este CPF", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    populaCamposPesquisa(consultaCadastro);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("[PesquisaCadastroIncluido] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCPF_Click(object sender, EventArgs e)
        {
            try
            {
                var obt = new ObTipo(2);
                bgExecutaConsultas.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void ConsultaPorCpf()
        {
            try
            {
                int a = 1;
                cadastro.renach = txtRENACH.Text;
                cadastro.cpf = txtCPF.Text.Replace(",", "").Replace("-", "");

                if (txtIdCandidato.Text != "")
                    cadastro.id_candidatos = int.Parse(txtIdCandidato.Text);
                consultaCadastro = requests.ConsultaDado(cadastro, a);

                if (consultaCadastro == null)
                {
                    MessageBox.Show("Não encontrou dados associados a este CPF", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    populaCamposPesquisa(consultaCadastro);
                }

                try
                {
                    imagem.Assinatura.cpf = cadastro.cpf;
                    consultaAssinatura = requests.ConsultaAssinatura(imagem, a);
                    if (consultaAssinatura != null)
                    {
                        populaCamposAssinatura(consultaAssinatura);
                    }
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("6003"))
                    {
                        MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                try
                {
                    imagem.Foto.cpf = cadastro.cpf;
                    consultaFoto = requests.ConsultaFoto(imagem, a);
                    if (consultaFoto != null)
                    {
                        populaCamposFoto(consultaFoto);
                    }
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("6003"))
                    {
                        MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                try
                {
                    imagem.BioCadastro.cpf = cadastro.cpf;                   
                    var digbio = requests.ConsultaBiometriaTodas(imagem, a)[0];
                    if (digbio != null)
                    {
                        log.write("CONSULTA DE BIOMETRIAS RETORNOU: " + digbio.digitais.Count);
                        int contador = 0;
                        listaBiometria = new List<ObRespImagemBiometria>();
                        foreach (var bio in digbio.digitais)
                        {
                            if (!string.IsNullOrEmpty(bio.coletado) && bio.coletado == "1")
                            {
                                log.write("DEDO COLETADO: " + bio.indice + " status = " + bio.coletado);
                                var bioTemp = requests.ConsultaBiometriaIndice(imagem, a, bio.indice, log);
                                //File.WriteAllBytes(".\\WSQC-" + cadastro.cpf + "-" + bioTemp.indice + ".wsq", bioTemp.foto);

                                log.write("DEDO COLETADO 2: " + bioTemp.foto.Length + " coletado: " + bioTemp.coletado);
                                listaBiometria.Add(bioTemp);

                                var bbb = bPolegarD;
                                switch (bio.indice)
                                {
                                    case 0: bbb = bPolegarD; break;
                                    case 1: bbb = bIndicadorD; break;
                                    case 2: bbb = bMedioD; break;
                                    case 3: bbb = bAnelarD; break;
                                    case 4: bbb = bMinimoD; break;
                                    case 5: bbb = bPolegarE; break;
                                    case 6: bbb = bIndicadorE; break;
                                    case 7: bbb = bMedioE; break;
                                    case 8: bbb = bAnelarE; break;
                                    case 9: bbb = bMinimoE; break;
                                }

                                bbb.Invoke(new MethodInvoker(() =>
                                {
                                    bbb.BackColor = Color.DarkBlue;
                                    bbb.status = 2;
                                    txtAvisoBio.Text = "Escolha o próximo dedo";
                                }));
                                if (bbb.status == 2 || bbb.status == 1)
                                {
                                    contador++;
                                }
                            }
                            else
                            {
                                log.write("DEDO NAO COLETADO: " + bio.indice + " status = " + bio.coletado);
                                var bioT = new ObRespImagemBiometria();
                                bioT.coletado = bio.coletado;
                                bioT.cpf = bio.cpf;
                                bioT.data_hora_cadastro = bio.data_hora_cadastro;
                                bioT.indice = bio.indice.ToString();
                                bioT.minucias = (bio.minucias ?? 0);
                                bioT.renach = bio.renach;
                                bioT.foto = null;
                                listaBiometria.Add(bioT);
                            }
                        }
                        txtAvisoBio.Invoke(new MethodInvoker(() =>
                        {
                            if (contador == 10)
                                txtAvisoBio.Text = "Biometrias Capturadas com Sucesso!!";
                        }));

                        if (statusbiometria == false)
                            ((Control)this.tabBiometriaD).Enabled = false;
                        if (statuscamera == false)
                            ((Control)this.tabBiometriaF).Enabled = false;
                        if (statusassinatura == false)
                            ((Control)this.tabBiometriaAssinatura).Enabled = false;
                        //populaCamposFoto(consultaFoto);
                    }
                    else
                        log.write("CONSULTA DE BIOMETRIAS VEIO VAZIA");
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("6003"))
                    {
                        MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void populaCampoCep()
        {
            IniciaCarregaLoader();
            try
            {
                var resp = ConsultaCEP.consultaPorCep(txtCEP.Text, log);
                if (resp != null)
                {
                    txtEndereco.Invoke(new MethodInvoker(() =>
                    {
                        txtEndereco.Text = resp.logradouro;
                        txtBairro.Text = resp.bairro;
                        cbMunicipio.Text = resp.localidade;
                        cbUF.Text = resp.uf;
                    }));
                    cbUF.Invoke(new MethodInvoker(() =>
                    {
                        listamunicipio = requests.listaMunicipio(cbUF.Text);
                    }));

                    var mun = resp.localidade.ToUpper();
                    var bindingSource = new BindingSource();
                    bindingSource.DataSource = listamunicipio.municipios.Select(x => x.municipio).ToList();
                    cbMunicipio.DataSource = bindingSource.DataSource;

                    var item = listamunicipio.municipios.Where(x => x.municipio.Contains(RemoveAccents(mun))).FirstOrDefault();
                    txtEndereco.Invoke(new MethodInvoker(() =>
                    {
                        if (item != null)
                        {
                            cbMunicipio.Text = item.municipio;
                            cadastro.id_municipios = item.id_municipios;

                        }
                        else
                        {
                            cbMunicipio.Text = "";
                        }
                    }));

                }
                else
                    MessageBox.Show("Não foi possível consultar este CEP, verifique se o mesmo está atualizado ou escrito corretamente", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    log.write("Não foi possível consultar o CEP informado");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnConsultaCep_Click(object sender, EventArgs e)
        {
            try
            {
                var obt = new ObTipo(3);
                bgExecutaConsultas.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void imgMaoEsquerda_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

        }

        private void FormCadastro_Load(object sender, EventArgs e)
        {
            IniciaCarregaLoader();
        }

        public void IniciaCarregaLoader()
        {
            if (bgCarregaLoader.IsBusy) return;
            bgCarregaLoader.RunWorkerAsync();


            //carregaLoader();
        }

        public void IniciaDescarregaLoader()
        {
            if (bgDescarregaLoader.IsBusy) return;
            bgDescarregaLoader.RunWorkerAsync();
        }

        public async Task<bool> carregaLoader()
        {
            try
            {
                btnGravar.Invoke(new MethodInvoker(() =>
                {
                    if (loader == null || loader.IsDisposed)
                        loader = new FormLoader();
                    loader.Visible = false;
                    loader.Show(this);
                    loader.TopMost = true;
                    loader.BringToFront();
                }));

                bloqueiaTela();
            }
            catch (Exception e)
            {
            }
            return true;
        }

        public async Task<bool> descarregaLoader()
        {
            try
            {
                btnGravar.Invoke(new MethodInvoker(() =>
                {
                    loader.Hide();
                }));

                //desbloqueiaTela();
            }
            catch (Exception e) { }
            return true;
        }

        private void lbLeitorEmUso_Click(object sender, EventArgs e)
        {

        }

        private void imgMaoDireita_Click(object sender, EventArgs e)
        {
            /*var ob = (PictureBox)sender;

            var ee = (MouseEventArgs)e;
            var p = tabBiometriaD.Location;
            bbase.Location = new Point(ob.Location.X + ee.X, ob.Location.Y+ee.Y); ;
            label1.Text = string.Format("X: {0} Y: {1}", ob.Location.X+ee.X, ob.Location.Y+ee.Y);*/
        }

        private void tabBiometriaD_Click(object sender, EventArgs e)
        {

            /*var ee = (MouseEventArgs)e;
             var p = tabBiometriaD.Location;
            bbase.Location = new Point(ee.X, ee.Y); ;
            label1.Text = string.Format("X: {0} Y: {1}", ee.X, ee.Y);*/
        }

        bool IsPrimeiroDedo = false;
        private bool PrimeiroDedo()
        {
            try
            {
                bbase = bPolegarD;
                if (bbase.status != 2)
                {
                    bbase.Invoke(new MethodInvoker(() =>
                    {
                        bbase.BackColor = Color.DarkGreen;
                        bbase.status = 1;
                        txtAvisoBio.Text = "Pressione o dedo " + bbase.nomeDedo + " no leitor";

                    }));
                }
                else;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[PrimeiroDedo] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return true;
        }

        bool executandoBio = false;
        private void trataCliqueDedos(object sender, EventArgs e)
        {
            try
            {
                if (executandoBio) return;

                var bt = (RJButton)sender;
                bbase = bt;

                bbase.Invoke(new MethodInvoker(() =>
                {
                    bbase.BackColor = Color.DarkGreen;
                    bbase.status = 1;
                    txtAvisoBio.Text = "Pressione o dedo " + bbase.nomeDedo + " no leitor";
                    txtAvisoBio.BackColor = Color.FromArgb(246, 241, 154);
                }));

                while (backgroundWorker1.IsBusy)
                {
                    Thread.Sleep(100);
                }
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("[trataCliqueDedos] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            /*var ee = (MouseEventArgs)e;
             var p = tabBiometriaD.Location;
            bbase.Location = new Point(ee.X, ee.Y); ;
            label1.Text = string.Format("X: {0} Y: {1}", ee.X, ee.Y);*/
        }

        public void limpaDedos()
        {
            bPolegarD.BackColor = Color.Transparent;
            bIndicadorD.BackColor = Color.Transparent;
            bMedioD.BackColor = Color.Transparent;
            bAnelarD.BackColor = Color.Transparent;
            bMinimoD.BackColor = Color.Transparent;

            bPolegarE.BackColor = Color.Transparent;
            bIndicadorE.BackColor = Color.Transparent;
            bMedioE.BackColor = Color.Transparent;
            bAnelarE.BackColor = Color.Transparent;
            bMinimoE.BackColor = Color.Transparent;
        }



        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtCPF_TextChanged(object sender, EventArgs e)
        {

            txtCPFFacial.Text = txtCPF.Text;
            txtCPFBio.Text = txtCPF.Text;
            txtCPFAssinatura.Text = txtCPF.Text;
        }

        private void txtRENACH_TextChanged(object sender, EventArgs e)
        {

            txtRENACHFacial.Text = txtRENACH.Text;
            txtRENACHBio.Text = txtRENACH.Text;
            txtRENACHAssinatura.Text = txtRENACH.Text;

        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

            txtNomeFacial.Text = txtNome.Text;
            txtNomeBio.Text = txtNome.Text;
            txtNomeAssinatura.Text = txtNome.Text;
        }

        private void btnIniciarScanner_Click(object sender, EventArgs e)
        {
            PrimeiroDedo();

        }

        public void EnviaSucessoDedo()
        {
            try
            {

                bbase.Invoke(new MethodInvoker(() =>
                {
                    bbase.BackColor = Color.DarkGray;
                    bbase.status = 2;
                    txtAvisoBio.Text = "Enviando registro para o servidor...";
                }));

                imagem.BioCadastro.id_candidatos = cadastro.id_candidatos.Value;
                imagem.BioCadastro.cpf = cadastro.cpf;
                imagem.BioCadastro.renach = cadastro.renach;
                imagem.BioCadastro.indice = bbase.indice;
                imagem.BioCadastro.coletado = 1;
                imagem.BioCadastro.minucias = 0;
                //imagem.BioCadastro.foto = File.ReadAllBytes(@"..\ImagemNova.wsq");
                requests.IncluiAlteraBiometria(imagem, DefineRespostaEnvioBio, log);


            }
            catch (Exception ex)
            {
                MessageBox.Show("[DefineSucessoDedo] " + ex.Message + " - " + ex.StackTrace, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public int DefineSucessoDedo(bool t)
        {

            try
            {
                var obt = new ObTipo(2);
                bgExecutaBioConsultas.RunWorkerAsync(obt);
                carregaLoader();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return 0;
        }

        public int DefineRespostaEnvioBio(ObRespImagemBiometria ob)
        {
            try
            {
                if (string.IsNullOrEmpty(ob.erro) && ob.coletado == "1")
                {
                    bbase.Invoke(new MethodInvoker(() =>
                    {
                        bbase.BackColor = Color.DarkBlue;
                        bbase.status = 2;
                        txtAvisoBio.Text = "Escolha o próximo dedo";
                    }));

                    
                }

                else if (string.IsNullOrEmpty(ob.erro) && ob.coletado == "0")
                {
                    bbase.Invoke(new MethodInvoker(() =>
                    {
                        bbase.BackColor = Color.DarkMagenta;
                        bbase.status = 2;
                        txtAvisoBio.Text = "Escolha o próximo dedo";
                    }));
                }
                else if (!string.IsNullOrEmpty(ob.erro))
                {
                    MessageBox.Show(ob.erro, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                int a = 1;
                if (bPolegarD.status == 2)
                    a++;
                if (bIndicadorD.status == 2)
                    a++;
                if (bMedioD.status == 2)
                    a++;
                if (bAnelarD.status == 2)
                    a++;
                if (bMinimoD.status == 2)
                    a++;
                if (bPolegarE.status == 2)
                    a++;
                if (bIndicadorE.status == 2)
                    a++;
                if (bMedioE.status == 2)
                    a++;
                if (bAnelarE.status == 2)
                    a++;
                if (bMinimoE.status == 2)
                    a++;
                if (a == 10)
                    txtAvisoBio.Text = "Biometrias Cadastradas com Sucesso!!";

            }
            catch (Exception ex)
            {
                MessageBox.Show("[DefineRespostaEnvioBio] " + ex.Message + " - " + ex.StackTrace, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            executandoBio = false;
            descarregaLoader();
            desbloqueiaTela();
            return 0;
        }

        public int DefineFalhaDedo(string t)
        {
            try
            {
                if (EnviadoExcecao != true)
                {
                    txtAvisoBio.Invoke(new MethodInvoker(() =>
                    {
                        bbase.BackColor = Color.Red;
                        txtAvisoBio.BackColor = Color.Red;
                        bbase.status = 3;
                        LeitorBiometrico b = new LeitorBiometrico(log);
                        txtAvisoBio.Text = "Error: " + b.getErro();
                    }));
                }
                else
                {
                    txtAvisoBio.Invoke(new MethodInvoker(() =>
                    {
                        bbase.BackColor = Color.DarkMagenta;
                        bbase.status = 2;
                        txtAvisoBio.Text = "Escolha o próximo dedo";
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[DefineFalhaDedo] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            executandoBio = false;
            descarregaLoader();
            desbloqueiaTela();
            return 0;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            txtAvisoBio.Invoke(new MethodInvoker(() =>
            {
                executandoBio = true;
            }));

            leitorBiometrico.CapturaDedo(pbImgbio, DefineSucessoDedo, DefineFalhaDedo);

        }

        private void bwCarregaDriver_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (leitorBiometrico.VerificaConexao())
                {

                    cbListaDispositivos.Invoke(new MethodInvoker(() =>
                    {
                        if (leitorBiometrico.listaDispositivos != null && leitorBiometrico.listaDispositivos.Count > 0)
                        {
                            cbListaDispositivos.Items.AddRange(leitorBiometrico.listaDispositivos.ToArray());
                            cbListaDispositivos.SelectedIndex = leitorBiometrico.IndexSelecaoScanner;
                            cbListaDispositivos.Enabled = true;

                            //PrimeiroDedo();
                            ((Control)this.tabBiometriaD).Enabled = true;

                            btnFuncScanner.Image = global::IdentifikarBio.Properties.Resources.FlagDarkGreen;
                            statusbiometria = true;
                            btnIniciarScanner.Enabled = false;
                        }
                    }));
                    //MessageBox.Show("Dispositivo Encontrado com sucesso.");

                }
                else
                {
                    
                    btnFuncScanner.Invoke(new MethodInvoker(() =>
                    {
                        ((Control)this.tabBiometriaD).Enabled = false;
                        btnFuncScanner.Image = global::IdentifikarBio.Properties.Resources.FlagRed;
                    }));
                }
            }
            catch (Exception ex)
            {
                btnFuncScanner.Invoke(new MethodInvoker(() =>
                {
                    ((Control)this.tabBiometriaD).Enabled = false;
                    btnFuncScanner.Image = global::IdentifikarBio.Properties.Resources.FlagRed;
                }));
            }

            try
            {
                int penDataType;
                List<wgssSTU.IPenDataTimeCountSequence> penTimeData = null;
                List<wgssSTU.IPenData> penData = null;

                wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();
                if (usbDevices.Count != 0)
                {
                    wgssSTU.IUsbDevice usbDevice = usbDevices[0];
                    tlpAssinatura.Invoke(new MethodInvoker(() =>
                    {
                        ucc = new GerenteAssinatura(usbDevice, false, this);
                        tlpAssinatura.Controls.Add(ucc);
                        ucc.Dock = DockStyle.Fill;
                        ucc.BringToFront();
                        ucc.Show();

                        ((Control)this.tabBiometriaAssinatura).Enabled = true;
                        btnFuncAssinatura.Image = global::IdentifikarBio.Properties.Resources.FlagDarkGreen;
                        statusassinatura = true;
                    }));
                }
                else
                {
                    btnFuncAssinatura.Invoke(new MethodInvoker(() =>
                    {
                        ((Control)this.tabBiometriaAssinatura).Enabled = false;
                        btnFuncAssinatura.Image = global::IdentifikarBio.Properties.Resources.FlagRed;
                    }));
                }
            }
            catch (Exception ex)
            {
                btnFuncAssinatura.Invoke(new MethodInvoker(() =>
                {
                    ((Control)this.tabBiometriaAssinatura).Enabled = false;
                    btnFuncAssinatura.Image = global::IdentifikarBio.Properties.Resources.FlagRed;
                }));
            }

            try
            {
                cbCameraemUso.Invoke(new MethodInvoker(() =>
                {
                    filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                    if (filterInfoCollection != null && filterInfoCollection.Count > 0)
                    {

                        foreach (FilterInfo filterInfo in filterInfoCollection)
                            cbCameraemUso.Items.Add(filterInfo.Name);

                        videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
                        videoCaptureDevice.VideoResolution = videoCaptureDevice.VideoCapabilities[0];
                        videoCaptureDevice.NewFrame += new NewFrameEventHandler(VideoCaptureDevice_NewFrame);
                       
                        videoCaptureDevice.DesiredFrameRate = 10;

                        //videoCaptureDevice.Start();

                        cbCameraemUso.SelectedIndex = 0;
                        ((Control)this.tabBiometriaF).Enabled = true;
                        btnFuncCamera.Image = global::IdentifikarBio.Properties.Resources.FlagDarkGreen;
                        statuscamera = true;

                    }
                    else
                    {
                        btnFuncCamera.Invoke(new MethodInvoker(() =>
                        {
                            ((Control)this.tabBiometriaF).Enabled = false;
                            btnFuncCamera.Image = global::IdentifikarBio.Properties.Resources.FlagRed;
                        }));
                    }
                }));


            }
            catch (Exception ex)
            {
                btnFuncCamera.Invoke(new MethodInvoker(() =>
                {
                    ((Control)this.tabBiometriaF).Enabled = false;
                    btnFuncCamera.Image = global::IdentifikarBio.Properties.Resources.FlagRed;
                }));
            }

            try
            {
                btnFuncAssinatura.Invoke(new MethodInvoker(() =>
                {
                    this.Enabled = true;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("[bwCarregaDriver_DoWork] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            descarregaLoader();
        }

        private void btnLimparAssinatura_Click(object sender, EventArgs e)
        {
            ucc.clearScreen();
        }

        public async Task<bool> EnviaAssinatura()
        {
            try
            {

                log.write("Iniciando Envio de assinatura");
                //populaCamposAssinatura(consultaAssinatura);
                imagem.Assinatura.id_candidatos = cadastro.id_candidatos;
                imagem.Assinatura.cpf = cadastro.cpf;
                imagem.Assinatura.renach = cadastro.renach;
                log.write("Iniciando Envio de assinatura2");
                imagem.Assinatura.foto = ucc.SaveImage();
                log.write("Iniciando Envio de assinatura 3");
                MemoryStream img = new MemoryStream(imagem.Assinatura.foto);
                pbImgAceita.Image = Image.FromStream(img);
                log.write("Iniciando Envio de assinatura 4");
                consultaAssinatura = await requests.IncluiAlteraAssinatura(imagem, log);
                ucc.clearScreen();
                //MessageBox.Show("Inclusão e/ou alteração de assinatura feita com sucesso!!");


            }
            catch (Exception ex)
            {
                log.write("FALHA ENVIO DE ASSINATURA", ex);
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return true;
        }
        private void btnAceitarAssinatura_Click(object sender, EventArgs e)
        {
            try
            {
                var obt = new ObTipo(5);
                bgExecutaConsultas.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void tabCadastro_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                if (cadastro.id_candidatos == null || cadastro.id_candidatos == 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Não é possível realizar a captura de imagens sem que o cadastro seja preenchido e enviado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    e.Cancel = !((Control)e.TabPage).Enabled;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            try
            {
                int sourceWidth = imgToResize.Width;
                int sourceHeight = imgToResize.Height;
                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)size.Width / (float)sourceWidth);
                nPercentH = ((float)size.Height / (float)sourceHeight);
                if (nPercentH < nPercentW)
                    nPercent = nPercentH;
                else
                    nPercent = nPercentW;

                int destWidth = size.Width;
                int destHeight = size.Height;

                Bitmap b = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage((System.Drawing.Image)b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                b.SetResolution(300, 300);
                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                g.Dispose();
                return (System.Drawing.Image)b;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return null;
        }

        private void btnTirarFoto_ClickAsync(object sender, EventArgs e)
        {

            try
            {
                var obt = new ObTipo(4);
                bgExecutaConsultas.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void TiraFotoEnvia()
        {
            try
            {
                if (File.Exists(@".\ImagemWebCam.jpg")) File.Delete(@".\ImagemWebCam.jpg");
                var bitmap2 = new Bitmap(dadosresolucao.Widtha, dadosresolucao.Heighta);
                
                using (var g = Graphics.FromImage(bitmap2))
                {
                    var section = new Rectangle(dadosresolucao.Corte, 0, dadosresolucao.Widtha, dadosresolucao.Heighta);
                    g.DrawImage(imageBase, 0, 0, section, GraphicsUnit.Pixel);
                    
                    imageBase = bitmap2;
                }

                System.Drawing.Image i = resizeImage(bitmap2, new Size(480, 640));
                string caminhoImagemSalva = @".\ImagemWebCam.jpg";
                var conseg = false;

                int tentativa = 0;
                while (!conseg)
                {
                    tentativa++;
                    if (tentativa >= 4)
                    {
                        MessageBox.Show("Falha nas permissões para gerar imagem", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    try
                    {
                        i.Save(caminhoImagemSalva, ImageFormat.Jpeg);
                        i.Dispose();
                        conseg = true;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                pbFotoFrente.Image = bitmap2;
                pbFotoFrente.SizeMode = PictureBoxSizeMode.Zoom;
                imagem.Foto.renach = cadastro.renach;
                imagem.Foto.cpf = cadastro.cpf;
                consultaFoto = requests.IncluiAlteraFoto(imagem).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        Image imageBase;


        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Image img = (Bitmap)eventArgs.Frame.Clone();

                var bitmap = new Bitmap(dadosresolucao.Widtha, dadosresolucao.Heighta);
                using (var g = Graphics.FromImage(bitmap))
                {
                    var section = new Rectangle(dadosresolucao.Corte, 0, dadosresolucao.Widtha, dadosresolucao.Heighta);
                    g.DrawImage(img, 0, 0, section, GraphicsUnit.Pixel);
                    imageBase = img;

                    Pen p = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0)), 7);
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawEllipse(p, new Rectangle((int)dadosresolucao.X, (int)dadosresolucao.Y, (int)dadosresolucao.Circwidth, dadosresolucao.Vermelho));

                    Pen p2 = new Pen(new SolidBrush(Color.FromArgb(255, 204, 0)), 7);
                    p2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawEllipse(p2, new Rectangle((int)dadosresolucao.X, (int)dadosresolucao.Y, (int)dadosresolucao.Circwidth, dadosresolucao.Amarelo));

                    Pen p1 = new Pen(new SolidBrush(Color.FromArgb(0, 255, 0)), 7);
                    p1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawEllipse(p1, new Rectangle((int)dadosresolucao.X, (int)dadosresolucao.Y, (int)dadosresolucao.Circwidth, dadosresolucao.Verde));


                }

                pbFotoPrincipal.Image = bitmap;
                pbFotoPrincipal.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("Memória insuficiente"))
                {
                    videoCaptureDevice.Stop();
                    videoCaptureDevice.Start();
                }
                else if (ex.Message.Contains("Parâmetro inválido"))
                {
                    videoCaptureDevice.Stop();
                    //videoCaptureDevice.Start();
                }
                else
                    MessageBox.Show("[VideoCapture_NewFrame] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void capturaResolucao()
        {
            try
            {
                var achouRes = false;
                for (int i = 0; i < videoCaptureDevice.VideoCapabilities.Length; i++)
                {
                    int[] resolution_size = new int[2];

                    
                    resolution_size[0] = videoCaptureDevice.VideoCapabilities[i].FrameSize.Width;
                    resolution_size[1] = videoCaptureDevice.VideoCapabilities[i].FrameSize.Height;
                    //if(resolution_size[0] >= Parametros.ParametroSys.CameraAltura && resolution_size[1] >= Parametros.ParametroSys.CameraLargura)
                    //{ 
                    if (resolution_size[0] >= 800 && resolution_size[1] >= 640)
                    {
                    achouRes = true;
                    // Regra de 3
                    int r3 = (480 * resolution_size[1] / 640);

                    // Largura total subtrai largura r3 e dividi por 2
                    int valorcorte = (resolution_size[0] - r3) / 2;
                    dadosresolucao.Widtha = r3;
                    dadosresolucao.Corte = valorcorte;
                    dadosresolucao.Heighta = resolution_size[1];
                    // Calculo para obter o valor do eixo x do retangulo
                    double x = (dadosresolucao.Widtha * 12.5) / 100;

                    // Calculo para obter o valor do height do circulo retangulo
                    int vermelho = (dadosresolucao.Heighta * 90) / 100;
                    int amarelo = (dadosresolucao.Heighta * 60) / 100;
                    int verde = (dadosresolucao.Heighta * 75) / 100;

                    // Calculo para width
                    double circwidth = (dadosresolucao.Widtha * 75) / 100;

                    // Calculo para eixo y
                    double y = x / 2;

                    dadosresolucao.X = x;
                    dadosresolucao.Vermelho = vermelho;
                    dadosresolucao.Amarelo = amarelo;
                    dadosresolucao.Verde = verde;
                    dadosresolucao.Circwidth = circwidth;
                    dadosresolucao.Y = y;


                    videoCaptureDevice.VideoResolution = videoCaptureDevice.VideoCapabilities[i];

                    i += videoCaptureDevice.VideoCapabilities.Length;
                    // }
                }
                }

                if (!achouRes)
                {

                    MessageBox.Show("Resolução não suportada pelo sistema, é necessário resolução mínima de 800x640", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[capturaResolucao] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void cbCameraemUso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                videoCaptureDevice.Stop();
                videoCaptureDevice.Source = filterInfoCollection[cbCameraemUso.SelectedIndex].MonikerString;
                // size = new FormCadastro();

                capturaResolucao();
                btnIniciaVideo.Enabled = true;
                btnParaVideo.Enabled = false;
                pbFotoPrincipal.Image = null;
                /*    videoCaptureDevice.Source = filterInfoCollection[cbCameraemUso.SelectedIndex].MonikerString;

                    size = new FormCadastro();

                    int a = videoCaptureDevice.VideoCapabilities.Length;
                    for (int i = 0; i < videoCaptureDevice.VideoCapabilities.Length; i++)
                    {
                        string resolution = "Resolution Number " + Convert.ToString(i);
                        string[] resolution_size = videoCaptureDevice.VideoCapabilities[i].FrameSize.ToString().Split(',');
                        resolution_size[0] = resolution_size[0].Substring(7);

                        resolution_size[1] = resolution_size[1].Substring(8);
                        resolution_size[1] = resolution_size[1].Remove(4);

                        if (int.Parse(resolution_size[0]) >= 800 && int.Parse(resolution_size[1]) >= 640)
                        {

                            // Regra de 3
                            int r3 = (480 * int.Parse(resolution_size[1]) / 640);
                            // Largura total subtrai largura r3 e dividi por 2
                            int valorcorte = (int.Parse(resolution_size[0]) - r3 ) / 2;
                            size.Widtha = r3;
                            size.Corte = valorcorte;
                            size.Heighta = int.Parse(resolution_size[1]);
                            videoCaptureDevice.VideoResolution = videoCaptureDevice.VideoCapabilities[i];

                            i += videoCaptureDevice.VideoCapabilities.Length;
                        }

                    }*/

                //videoCaptureDevice.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("[cbCameraemUso_SelectedIndexChanged] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tabBiometriaF_Enter(object sender, EventArgs e)
        {


        }

        private void tabBiometriaF_Leave(object sender, EventArgs e)
        {
            /*if (videoCaptureDevice.IsRunning == true)
                videoCaptureDevice.Stop();*/
        }

        private void FormCadastro_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (videoCaptureDevice != null && videoCaptureDevice.IsRunning == true)
                {
                    videoCaptureDevice.SignalToStop();
                    videoCaptureDevice.Stop();
                    videoCaptureDevice.WaitForStop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[Fechando formulário] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }

            try
            {

                if (ucc != null)
                    ucc.Close();

                // if(leitorBiometrico.m_NBioScan.CloseDevice(leitorBiometrico.IndexSelecaoScanner))
            }
            catch (Exception ex)
            {
                MessageBox.Show("[Fechando formulário] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;


            }
        }

        private void tabBiometriaD_Enter(object sender, EventArgs e)
        {
            if (!IsPrimeiroDedo)
            {
                IsPrimeiroDedo = true;
                PrimeiroDedo();
            }
        }

        private void txtDataNascimento1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tlpAssinatura_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pbImgbio_Click(object sender, EventArgs e)
        {

        }

        private void cbUF_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                listamunicipio = requests.listaMunicipio(cbUF.Text);
                var bindingSource = new BindingSource();
                bindingSource.DataSource = listamunicipio.municipios.Select(x => x.municipio).ToList();
                cbMunicipio.DataSource = bindingSource.DataSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[cbUF_SelectedIndexChanged] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var item = listamunicipio.municipios.Where(x => x.municipio.Contains(cbMunicipio.Text)).FirstOrDefault();
                if (item != null)
                {
                    cbMunicipio.Text = item.municipio;
                    cadastro.id_municipios = item.id_municipios;

                }
                else
                {
                    cbMunicipio.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[cbMunicipio_SelectedIndexChanged] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnIdCandidato_Click(object sender, EventArgs e)
        {
            try
            {
                int a = 2;
                cadastro.renach = txtRENACH.Text;
                cadastro.cpf = txtCPF.Text.Replace(",", "");
                cadastro.cpf = cadastro.cpf.Replace("-", "");
                //cadastro.cpf = txtCPF.Text;
                if (txtIdCandidato.Text != "")
                    cadastro.id_candidatos = int.Parse(txtIdCandidato.Text);
                consultaCadastro = requests.ConsultaDado(cadastro, a);
                //var cad = cadastro.cpf;

                if (consultaCadastro == null)
                {
                    MessageBox.Show("Não encontrou dados associados a este ID", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    populaCamposPesquisa(consultaCadastro);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[btnIdCandidato_Click] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMunicipio_Click(object sender, EventArgs e)
        {

        }

        private void btnIniciaVideo_Click(object sender, EventArgs e)
        {

            try
            {
                var obt = new ObTipo(7);
                bgExecutaConsultas.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }            
        }

        public async Task<bool> IniciarCamera()
        {
            try
            {
                
                videoCaptureDevice.Start();
                btnCaptura.Invoke(new MethodInvoker(() =>
                {
                    btnCaptura.Enabled = true;
                    btnParaVideo.Enabled = true;
                }));
                    btnIniciaVideo.Enabled = false;
                
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[Iniciando vídeo] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        public async Task<bool> ConsultaBiometrias()
        {
            try
            {
                int a = 1;
                imagem.BioCadastro.cpf = cadastro.cpf;
                var digbio = requests.ConsultaBiometriaTodas(imagem, a)[0];
                if (digbio != null)
                {
                    log.write("CONSULTA DE BIOMETRIAS RETORNOU: " + digbio.digitais.Count);
                    int contador = 0;
                    listaBiometria = new List<ObRespImagemBiometria>();
                    foreach (var bio in digbio.digitais)
                    {
                        if (!string.IsNullOrEmpty(bio.coletado) && bio.coletado == "1")
                        {
                            log.write("DEDO COLETADO: " + bio.indice + " status = " + bio.coletado);
                            var bioTemp = requests.ConsultaBiometriaIndice(imagem, a, bio.indice, log);
                            //File.WriteAllBytes(".\\WSQC-" + cadastro.cpf + "-" + bioTemp.indice + ".wsq", bioTemp.foto);

                            log.write("DEDO COLETADO 2: " + bioTemp.foto.Length + " coletado: " + bioTemp.coletado);
                            listaBiometria.Add(bioTemp);

                            var bbb = bPolegarD;
                            switch (bio.indice)
                            {
                                case 0: bbb = bPolegarD; break;
                                case 1: bbb = bIndicadorD; break;
                                case 2: bbb = bMedioD; break;
                                case 3: bbb = bAnelarD; break;
                                case 4: bbb = bMinimoD; break;
                                case 5: bbb = bPolegarE; break;
                                case 6: bbb = bIndicadorE; break;
                                case 7: bbb = bMedioE; break;
                                case 8: bbb = bAnelarE; break;
                                case 9: bbb = bMinimoE; break;
                            }

                            bbb.Invoke(new MethodInvoker(() =>
                            {
                                bbb.BackColor = Color.DarkBlue;
                                bbb.status = 2;
                                txtAvisoBio.Text = "Escolha o próximo dedo";
                            }));
                            if (bbb.status == 2 || bbb.status == 1)
                            {
                                contador++;
                            }
                        }
                        else
                        {
                            log.write("DEDO NAO COLETADO: " + bio.indice + " status = " + bio.coletado);
                            var bioT = new ObRespImagemBiometria();
                            bioT.coletado = bio.coletado;
                            bioT.cpf = bio.cpf;
                            bioT.data_hora_cadastro = bio.data_hora_cadastro;
                            bioT.indice = bio.indice.ToString();
                            bioT.minucias = (bio.minucias ?? 0);
                            bioT.renach = bio.renach;
                            bioT.foto = null;
                            listaBiometria.Add(bioT);
                        }
                    }
                    txtAvisoBio.Invoke(new MethodInvoker(() =>
                    {
                        if (contador == 10)
                            txtAvisoBio.Text = "Biometrias Capturadas com Sucesso!!";
                    }));

                    log.write("BIOMETRIAS ENCONTRADAS: " + listaBiometria.Count());
                    foreach (var b in listaBiometria)
                    {
                        if (b.formato == null)
                        {
                            if (b.coletado == "1" && b.foto != null)
                            {
                                log.write("Dedo Coletado e com foto: " + b.indice);
                                b.foto = leitorBiometrico.ImagemWsqToRaw(b.foto, log);
                                b.formato = 1;
                                log.write("Dedo Coletado e com foto: " + b.indice + " TRANSFORMADO");
                            }
                            else
                            {
                                log.write("Dedo NÃO Coletado ou SEM foto: " + b.indice);
                            }
                        }
                        else
                        {
                            log.write("imagem ja estava formatada: " + b.indice);
                        }
                    }

                    descarregaLoader();
                    desbloqueiaTela();
                    txtAvisoBio.Invoke(new MethodInvoker(() =>
                    {
                        if (frmImgBio == null || frmImgBio.IsDisposed)
                        {
                            frmImgBio = new FormImgBio(listaBiometria);
                            frmImgBio.Show(this);
                        }
                        else frmImgBio.BringToFront();
                    }));
                }
                else
                {
                    descarregaLoader();
                    desbloqueiaTela();
                    log.write("CONSULTA DE BIOMETRIAS VEIO VAZIA");
                    MessageBox.Show("CONSULTA DE BIOMETRIAS VEIO VAZIA", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                    
            }
            catch (Exception ex)
            {
                descarregaLoader();
                desbloqueiaTela();
                if (!ex.Message.Contains("6003"))
                {
                    MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                EnviadoExcecao = true;
                var obt = new ObTipo(3);
                bgExecutaBioConsultas.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void EnviaBioExcecao()
        {
            try
            {
                bbase.Invoke(new MethodInvoker(() =>
                {
                    bbase.BackColor = Color.DarkMagenta;
                    bbase.status = 3;
                    txtAvisoBio.Text = "Enviando registro para o servidor";
                }));

                imagem.BioCadastro.id_candidatos = cadastro.id_candidatos.Value;
                imagem.BioCadastro.cpf = cadastro.cpf;
                imagem.BioCadastro.renach = cadastro.renach;
                imagem.BioCadastro.indice = bbase.indice;
                imagem.BioCadastro.coletado = 0;
                imagem.BioCadastro.minucias = 0;
                //imagem.BioCadastro.foto = File.ReadAllBytes(@"..\ImagemNova.wsq");

                requests.IncluiAlteraBiometria(imagem, DefineRespostaEnvioBio, log);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public bool EnviadoExcecao = false;
        private void btnMarcarExcecao_Click(object sender, EventArgs e)
        {
            try
            {
                EnviadoExcecao = true;
                var obt = new ObTipo(1);
                bgExecutaBioConsultas.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void bgCarregaLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            carregaLoader();
        }

        private void bgDescarregaLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            descarregaLoader();
        }

        private void bloqueiaTela()
        {
            try
            {
                bbase.Invoke(new MethodInvoker(() =>
                {
                    this.Enabled = false;
                }));
            }
            catch (Exception e) { }
        }

        private void desbloqueiaTela()
        {
            try
            {
                bbase.Invoke(new MethodInvoker(() =>
                {
                    this.Enabled = true;
                    this.BringToFront();
                }));
            }
            catch (Exception e) { }
        }


        private void bgConsultaRenach_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var obt = (ObTipo)e.Argument;
                switch (obt.Operacao)
                {
                    case 1: ExecultaConsultaRenach(); break;
                    case 2: ConsultaPorCpf(); break;
                    case 3: populaCampoCep(); break;
                    case 4: TiraFotoEnvia(); break;
                    case 5: EnviaAssinatura(); break;
                    case 6: EnviaCadastroCompleto(); break;
                    case 7: IniciarCamera(); break;
                    case 8: PararCamera(); break;
                }

                e.Cancel = true;
            }
            catch (Exception ex) { }
        }

        private void bgConsultaRenach_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                descarregaLoader();
                desbloqueiaTela();
            }
            catch (Exception ex) { }
        }


        private void bgExecutaBioConsultas_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var obt = (ObTipo)e.Argument;
                switch (obt.Operacao)
                {
                    case 1: EnviaBioExcecao(); break;
                    case 2: EnviaSucessoDedo(); break;
                    case 3: ConsultaBiometrias(); break;
                }
            }
            catch (Exception ex) { }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (videoCaptureDevice != null && videoCaptureDevice.IsRunning == true)
                {
                    videoCaptureDevice.SignalToStop();
                    videoCaptureDevice.Stop();
                    videoCaptureDevice.WaitForStop();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("[Fechando formulário] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                

            }

            try
            {

                if (ucc != null)
                    ucc.Close();

                // if(leitorBiometrico.m_NBioScan.CloseDevice(leitorBiometrico.IndexSelecaoScanner))
            }
            catch (Exception ex)
            {
                MessageBox.Show("[Fechando formulário] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                


            }
            this.Close();
        }

        private void txtCPF_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                btnCPF_Click(sender, e);
            }
        }

        private void txtRENACH_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                btnRENACH_Click(sender, e);
            }
        }

        private void txtCEP_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                btnConsultaCep_Click(sender, e);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            leitorBiometrico.TestaImagem(log);
        }

        private void txtFiliacaoPai_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnParaVideo_Click(object sender, EventArgs e)
        {
            try
            {
                var obt = new ObTipo(8);
                bgExecutaConsultas.RunWorkerAsync(obt);
                carregaLoader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
        public async Task<bool> PararCamera()
        {
            try
            {
                
                    if (videoCaptureDevice != null && videoCaptureDevice.IsRunning == true)
                {
                    videoCaptureDevice.SignalToStop();
                    videoCaptureDevice.Stop();
                    videoCaptureDevice.WaitForStop();
                }
                btnParaVideo.Enabled = false;
                btnParaVideo.Invoke(new MethodInvoker(() =>
                {
                    
                btnIniciaVideo.Enabled = true;
                pbFotoPrincipal.Image = null;
                btnCaptura.Enabled = false;
                }));
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[Parando vídeo] " + ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
   
        }
    }
}

