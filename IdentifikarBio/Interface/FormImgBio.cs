using IdentifikarBio.Database.Objetos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IdentifikarBio.Interface
{
    public partial class FormImgBio : Form
    {
        List<ObRespImagemBiometria> lista = new List<ObRespImagemBiometria>();

        public FormImgBio(List<ObRespImagemBiometria> _lista)
        {
            InitializeComponent();
            lista = _lista;

            populaImagens();
        }

        public void populaImagens()
        {
            foreach(var b in lista)
            {
                try
                {
                    if (b.coletado == "1")
                    {
                        if (b.foto != null && b.foto.Length > 0)
                        {
                            using (MemoryStream img = new MemoryStream(b.foto))
                            {
                                switch (b.indice)
                                {
                                    case "0": pbImagem0.Image = Image.FromStream(img); break;
                                    case "1": pbImagem1.Image = Image.FromStream(img); break;
                                    case "2": pbImagem2.Image = Image.FromStream(img); break;
                                    case "3": pbImagem3.Image = Image.FromStream(img); break;
                                    case "4": pbImagem4.Image = Image.FromStream(img); break;
                                    case "5": pbImagem5.Image = Image.FromStream(img); break;
                                    case "6": pbImagem6.Image = Image.FromStream(img); break;
                                    case "7": pbImagem7.Image = Image.FromStream(img); break;
                                    case "8": pbImagem8.Image = Image.FromStream(img); break;
                                    case "9": pbImagem9.Image = Image.FromStream(img); break;
                                }

                                img.Close();
                            }
                        }
                        else
                        {

                        }                        
                    }                
                }
                catch(Exception e)
                {

                }
               
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void FormImgBio_Load(object sender, EventArgs e)
        {

        }
    }
}
