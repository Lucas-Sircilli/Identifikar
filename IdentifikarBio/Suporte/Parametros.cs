using IdentifikarBio.Database.Objetos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Suporte
{
    public class Parametros
    {
        public static readonly String EnderecoParam = @"./parametros.conf";
        public static String EnderecoLog = @"./log/LogIK";
       
        public static String EnderecoBanco;
        public static String UsuarioBanco;
        public static String SenhaBanco;
        public static String InicialBanco;

        public static String ServerKey;
        public static String SenderId;

        public static String StringConexaoBanco;

        public static bool AtivaTail;
        public static bool Inicializado;

        public static int PortaServidor;
        public static int PortaServidorRelatorio;

        // public static string EnderecoServerSite = @"";

        public static ObParametros ParametroSys;
        

        public Parametros()
        {
            try
            {
                if (!File.Exists(EnderecoParam))
                {
                    ParametroSys = new ObParametros();
                    ParametroSys.ScannerIdDispositivo = 0;
                    ParametroSys.ScannerCapturaQualidade = 100;
                    ParametroSys.ScannerTimeOut = 10000;
                    ParametroSys.ScannerParadaQualidade = 30;
                    //ParametroSys.ScannerTipoComposicao = 0;
                    ParametroSys.ScannerChecaRuido = 20;
                    ParametroSys.CameraIdDispositivo = 0;
                    ParametroSys.CameraCapturaResolucao = 0;
                    ParametroSys.CameraAltura = 640;
                    ParametroSys.CameraLargura = 480;
                    GravaParametros();
                }
                else
                {
                    String param = File.ReadAllText(EnderecoParam);
                    if (!string.IsNullOrEmpty(param))
                    {
                        ParametroSys = JsonConvert.DeserializeObject<ObParametros>(param);
                    }
                    else
                    {
                        ParametroSys = new ObParametros();
                        ParametroSys.ScannerIdDispositivo = 0;
                        ParametroSys.ScannerCapturaQualidade = 100;
                        ParametroSys.ScannerTimeOut = 10000;
                        ParametroSys.ScannerParadaQualidade = 30;
                       // ParametroSys.ScannerTipoComposicao = 0;
                        ParametroSys.ScannerChecaRuido = 20;
                        GravaParametros();

                    }
                    Inicializado = true;
                }
            }
            catch (Exception e)
            {
             
            }
        }

        public static void GravaParametros()
        {
            try
            {
                var param = JsonConvert.SerializeObject(ParametroSys);
                File.WriteAllText(EnderecoParam, param);
            }
            catch(Exception e)
            {

            }
        }


        public static double ToDouble(object d, LogFile log)
        {
            try
            {
                String s = Convert.ToString(d);
                if ((s == " ") || (s == "")) s = "0";

                return Convert.ToDouble(s);
            }
            catch (Exception e)
            {
                log.write("FALHA PASSANDO VALOR PARA DOUBLE[" + d + "]", e);
            }
            return -1;
        }
    }
}
