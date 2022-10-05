using IdentifikarBio.Database.Objetos;
using IdentifikarBio.Suporte;
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
    public partial class FormParametros : Form
    {
        
        public FormParametros()
        {
            InitializeComponent();

            cbListaCamera.SelectedIndex = unchecked((int)Parametros.ParametroSys.CameraIdDispositivo);
            cbResolucaoCamera.SelectedIndex = unchecked((int)Parametros.ParametroSys.CameraCapturaResolucao);
            cbListaScanner.SelectedIndex = unchecked((int)Parametros.ParametroSys.ScannerIdDispositivo);
            txtTimeout.Text = Parametros.ParametroSys.ScannerTimeOut.ToString();
            txtCapturaQualidade.Text = Parametros.ParametroSys.ScannerCapturaQualidade.ToString();
            trcQualidade.Value = unchecked((int)Parametros.ParametroSys.ScannerCapturaQualidade);
            txtParadaQualidade.Text = Parametros.ParametroSys.ScannerParadaQualidade.ToString();
            trcParadaQualidade.Value = unchecked((int)Parametros.ParametroSys.ScannerParadaQualidade);
            //txtTipoComposicao.Text = Parametros.ParametroSys.ScannerTipoComposicao.ToString();
            txtChecaRuido.Text = Parametros.ParametroSys.ScannerChecaRuido.ToString();
            cbListaScanner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbListaCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbResolucaoCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            Parametros.ParametroSys.ScannerIdDispositivo = unchecked((uint)cbListaScanner.SelectedIndex);
            Parametros.ParametroSys.ScannerCapturaQualidade = unchecked((uint)trcQualidade.Value);
            Parametros.ParametroSys.ScannerTimeOut = uint.Parse(txtTimeout.Text);
            Parametros.ParametroSys.ScannerChecaRuido = uint.Parse(txtChecaRuido.Text);
           // Parametros.ParametroSys.ScannerTipoComposicao = uint.Parse(txtTipoComposicao.Text);
            Parametros.ParametroSys.ScannerParadaQualidade = unchecked((uint)trcParadaQualidade.Value);

            Parametros.GravaParametros();
        }

        private void trcQualidade_Scroll(object sender, EventArgs e)
        {
            txtCapturaQualidade.Text = trcQualidade.Value.ToString(); 
        }

        private void txtCapturaQualidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            try {
                if (e.KeyChar == (char)13)
                {
                    if (txtCapturaQualidade.Text.Equals("")) { }
                    else
                        trcQualidade.Value = int.Parse(txtCapturaQualidade.Text);
                } }
            catch (Exception ex) {
                if (ex.Message.Contains("Maximum"))
                    MessageBox.Show("Erro: Valor máximo permitido: 100");
                else
                MessageBox.Show("Erro:" + ex.Message);
            }
            }

        private void trcParadaQualidade_Scroll(object sender, EventArgs e)
        {
            txtParadaQualidade.Text = trcParadaQualidade.Value.ToString();
        }

        private void txtParadaQualidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtCapturaQualidade.Text.Equals("")) { }
                    else
                        trcParadaQualidade.Value = int.Parse(txtParadaQualidade.Text);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Maximum"))
                    MessageBox.Show("Erro: Valor máximo permitido: 100");
                else
                    MessageBox.Show("Erro:" + ex.Message);
            }
        }
    }
    }

