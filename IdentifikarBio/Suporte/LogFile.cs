using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Suporte
{
    public class LogFile
    {
        /**Variável de ajuste da data*/
        private DateTime dataHora;

        /**String de registro de log*/
        private String msgTempo;
        private String msgLog;
        private String file;
        private String ipClienteStatus = "";
        private String VERSAO = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private Stopwatch sw;


        /**Construtor principal da classe
         *
         */
        string sfile;
        public LogFile(String _file)
        {
            dataHora = DateTime.Now;
            sfile = _file;
            //String[] dd = dataHora.ToShortDateString().Split('/');
            //String data = dd[2] + "" + dd[1] + "" + dd[0];
            file = String.Format("{0}{1:00}{2:00}{3:0000}.log", _file, dataHora.Day, dataHora.Month, dataHora.Year);
            //file = _file + data + ".log";
            msgLog = "\r\n\r\n=============================================================\r\n";

            msgTempo = "TEMPOS DO SISTEMA:";
        }

        public void setIpClienteStatus(String ip)
        {
            ipClienteStatus = ip;
        }

        public String getIpClienteStatus()
        {
            return ipClienteStatus;
        }

        private void apagaLogs()
        {
            try
            {
                dataHora = DateTime.Now.AddDays(-7);
                var dfile = String.Format("{0}{1:00}{2:00}{3:0000}.log", sfile, dataHora.Day, dataHora.Month, dataHora.Year);

                if (File.Exists(dfile))
                    File.Delete(dfile);
                //file = _file + data + ".log";
            }
            catch (Exception e)
            {
                // write("Falha ao limpar logs", e);
            }
        }

        /**Metodo de escrita no log
         *
         * @param _mensagem Mensagem a ser registrada
         */

        public void writeLimpo(String msg)
        {
            if (msgLog != null)
                write(msg);
        }

        public void write(String _mensagem)
        {
            dataHora = DateTime.Now;
            String time = dataHora.ToLongTimeString();

            msgLog += time + " " + VERSAO + "> " + _mensagem + "\r\n";
            // Console.WriteLine(_mensagem);
            // if (Parametros.AtivaTail) fechar();
            fechar();
        }

        /**Metodo responsavel por logar debbugs
         * 
         * @param _mensagem
         */

        public void writeDebbug(String _mensagem)
        {
            write(_mensagem);
        }

        /**Metodo responsavel por logar warnings
         * 
         * @param _mensagem
         */

        public void writeWarn(String _mensagem)
        {

            write(_mensagem);
        }

        /**Metodo de escrita no log
         *
         * @param _mensagem Mensagem a ser registrada
         * @param ex Excecao a ser registrada
         */

        public void write(String _mensagem, Exception ex)
        {
            var msg = _mensagem + ": " + Funcoes.FormataExcecao(ex);
            write(_mensagem + ": " + Funcoes.FormataExcecao(ex));
        }


        /**Metodo de escrita no log
         *
         * @param ex Excecao a ser resgitrada
         */

        public void write(Exception ex)
        {
            write(ex.Message + ": " + ex.StackTrace);
        }

        public void iniciaTempo()
        {
            sw = new Stopwatch();
            sw.Start();
        }

        public void finalizaTempo(String msg)
        {
            try
            {
                if (sw != null) sw.Stop();
                msgTempo += "\r\n" + msg + ": [" + sw.ElapsedMilliseconds + "]";
                sw = null;
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText(Parametros.EnderecoLog, e.StackTrace);
            }
        }

        public void finalizaTempo(String msg, long tempo)
        {
            try
            {
                msgTempo += "\r\n" + msg + ": [" + tempo + "]";
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText(Parametros.EnderecoLog, e.StackTrace);
            }
        }

        public void fechar()
        {
            try
            {
                if (msgLog == null) return;
                else if (msgLog.Equals("\r\n\r\n=============================================================\r\n")) return;

                if (!Directory.Exists(".\\log")) Directory.CreateDirectory(".\\log");

                dataHora = DateTime.Now;

                file = String.Format("{0}{1:00}{2:00}{3:0000}.log", sfile, dataHora.Day, dataHora.Month, dataHora.Year);

                File.AppendAllText(file, msgLog);
                //File.AppendAllText(file, msgTempo);
                msgLog = "";
                msgTempo = "";
            }
            catch (Exception e)
            {
                // System.IO.File.WriteAllText(Parametros.EnderecoLogFalha, e.Message+"\n"+e.StackTrace);
            }
            apagaLogs();
        }


        public void limpar()
        {
            msgLog = null;
            msgTempo = null;
        }

        /**Metodo de leitura das mensagens de log
         *
         * @return
         */

        public String read()
        {
            return msgLog;
        }
    }
}
