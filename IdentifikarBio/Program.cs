using IdentifikarBio.Suporte;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IdentifikarBio
{
    static class Program
    {
        public static FormPrincipal fp;
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {

            if (ExisteProcesso())
            {
                MessageBox.Show("Esse programa já está em execução.", "Informação Identifikar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
                return;
            }

            Parametros pam = new Parametros();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            fp = new FormPrincipal();
            Application.Run(fp);
        }

        private static bool ExisteProcesso()
        {

            //var existe = Process.GetProcesses().
            //      Where(x => x.ProcessName == Application.ProductName).Count() > 1;
            return Process.GetProcessesByName("IdentifikarBio").Length > 1;
            //return existe;
        }
    }
}
