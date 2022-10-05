using IdentifikarBio.Interface;
using IdentifikarBio.Suporte;
using IdentifikarBio.Database.Objetos;
using NITGEN.SDK.NBioScan;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using NITGEN.SDK.NBioBSP;
using System.Runtime.InteropServices;

namespace IdentifikarBio.Dispositivos
{

    public class LeitorBiometrico
    {
        [DllImport("nimgconv.dll")]
        private static extern int NBioAPI_ImgConvRawToWSQBuf(System.Byte[] lpRawBuffer,
                                                      uint nWidth,
                                                      uint nHeight,
                                                      byte[] lpWSQBuffer,
                                                      out int nReturn_size,
                                                      float p);

       /* NBioAPI_RETURN NBioAPI NBioAPI_ImgConvRawToWSQBuf(
 IN LPBYTE lpRawBuffer,
 IN int nWidth,
 IN int nHeight,
 OUT LPBYTE lpWSQBuffer,
 OUT int* nReturn_size,
 IN float q);*/


        [DllImport("nimgconv.dll")]
        private static extern int NBioAPI_ImgConvWSQToRawBuf(
                                                           System.Byte[] lpWSQBuffer,
                                                           System.Byte[] lpRawBuffer,
                                                           out int nReturn_size,
                                                           out uint nWidth,
                                                           out uint nHeight);

        /* NBioAPI_RETURN NBioAPI NBioAPI_ImgConvWSQToRawBuf(
  IN LPBYTE lpWSQBuffer,
  OUT LPBYTE lpRawBuffer,
  OUT int* nReturn_size,
  OUT int* nWidth,
  OUT int* nHeight);*/


        [DllImport("nimgconv.dll")]
        private static extern int NBioAPI_ImgConvRawToBmpBuf(
                                                           System.Byte[] lpImageBuffer,
                                                           uint nWidth,
                                                           uint nHeight,
                                                           System.Byte[] lpBMPBuffer,
                                                           out int nReturn_size
                                                          );

        [DllImport("nimgconv.dll")]
        private static extern int NBioAPI_ImgConvRawToJpgBuf(
                                                           System.Byte[] lpRawBuffer,
                                                           uint nWidth,
                                                           uint nHeight,
                                                           int nQuality,
                                                           System.Byte[] lpJpgBuffer,
                                                           out uint nJpgBufLen
                                                          );
        /*
         NBioAPI_RETURN NBioAPI NBioAPI_ImgConvRawToJpgBuf(
 IN LPBYTE lpRawBuffer,
 IN UINT nWidth,
 IN UINT nHeight,
 IN int nQuality,
 OUT LPBYTE lpJpgBuffer,
 OUT int * nJpgBufLen); 
         */

        public LogFile log;
        
        public LeitorBiometrico(LogFile _log)
        {
            log = _log;
            
        }

        public byte[] ImagemWsqToRaw(byte[] foto, LogFile log)
        {
            try
            {
                log.write("Iniciando ImagemWsqToRaw: " + foto[0]);
                //File.WriteAllBytes(@".\TESTES-" + DateTime.Now.Millisecond + ".wsq", foto);
                if (foto[0] == 0) return null;

                
                string szFileName = @".\DigitalTransf"+DateTime.Now.Millisecond+".wsq";
                using (FileStream fs = new FileStream(szFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    BinaryWriter bw = new BinaryWriter(fs);

                    bw.Write(foto);
                    bw.Close();
                    fs.Close();
                    fs.Dispose();
                    bw.Dispose();

                    log.write(szFileName + " saved");

                    float qualidade = 0.75f;
                    uint width = 800;
                    uint height = 750;
                    int lenRaw = 800 * 750;
                    uint lenBmp = 0;
                    uint nDPI = 500;
                    System.Byte[] imgRaw = new System.Byte[width * height];
                    System.Byte[] imgBmp = new System.Byte[width * height];

                    log.write("leitura do arquivo wsq");
                    var imgWsq = File.ReadAllBytes(szFileName);

                    log.write("iniciando NBioAPI_ImgConvWSQToRawBuf: ");
                    var respwsq = NBioAPI_ImgConvWSQToRawBuf(imgWsq,
                                               imgRaw,
                                               out lenRaw,
                                               out height, out width);

                    log.write("iniciando NBioAPI_ImgConvRawToJpgBuf");
                    respwsq = NBioAPI_ImgConvRawToJpgBuf(imgRaw, height, width, 50, imgBmp, out lenBmp);

                    log.write("Apagando arquivo wsq");
                   File.Delete(szFileName);

                    log.write("Retornando imagem");
                    return imgBmp;
                }
            }
            catch (Exception e)
            {
                log.write("FALHA NA ImagemWsqToRaw", e);
                //funcFalha("ERRO SaveFlatCapturedImage: " + e.Message + " - " + e.StackTrace);
                MessageBox.Show(e.ToString());
            }
            return null;
        }



        // Em teste
        //private NBioScan m_NBioScan;
        private NBioScan.Type.INIT_INFO_0 initinfo0;
        
        private NBioScan.Type.CAPTURE_OPTION m_CaptureOption;
        private NBioScan.Type.CAPTURED_IMAGE m_CapturedImage;
        private NBioScan.Type.TEMPLATES_DATA m_TemplateDatas;



        private Thread m_CaptureThread;
        private bool m_bRunRollThread;
        private bool m_bRunFlatThread;
        private bool m_bComposeStart;
        private bool m_bFlatStop;

        public NBioScan m_NBioScan;
        private short m_OpenedDeviceID;
        private NBioScan.Type.DEVICE_INFO_EX[] m_DeviceInfoEx;
        public List<String> listaDispositivos;
        public int IndexSelecaoScanner = 0;
        // Em teste
        public bool VerificaConexao()
        {

            listaDispositivos = new List<string>();
            m_NBioScan = new NBioScan();
            m_OpenedDeviceID = NBioScan.Type.DEVICE_ID.NONE;
            m_CaptureOption = new NBioScan.Type.CAPTURE_OPTION();
            try
            {
                uint nNumDevice, i;
                short[] nDeviceID;
                StringBuilder valores = new StringBuilder();

                //log.write("== NBioScan_EnumerateDevice ==");

                m_NBioScan.EnumerateDevice(out nNumDevice, out nDeviceID, out m_DeviceInfoEx);

                if (m_NBioScan.m_dwResult != NBioScan.Error.NONE)
                {
                    //log.write("NBioScan SDK Error[" + m_NBioScan.m_dwResult.ToString() + "]");
                    log.write("NBioScan SDK Error[" + m_NBioScan.m_dwResult.ToString() + "]");
                }
                else
                {
                    log.write("Current Device Count: " + nNumDevice.ToString());

                    /*cbListaDispositivos.Enabled = true;
                    cbListaDispositivos.Items.Clear();
                    cbListaDispositivos.Items.Add("Auto_Detect");*/

                    //listaDispositivos.Add("Detecção Automática");

                    for (i = 0; i < nNumDevice; i++)
                    {
                        listaDispositivos.Add(m_DeviceInfoEx[i].Name);
                    }

                    IndexSelecaoScanner = 0;
                    //cbListaDispositivos.SelectedIndex = 0;

                }

                m_NBioScan.CloseDevice(m_OpenedDeviceID);

                m_OpenedDeviceID = NBioScan.Type.DEVICE_ID.AUTO;

                if (IndexSelecaoScanner > 0)
                    m_OpenedDeviceID = (short)(m_DeviceInfoEx[IndexSelecaoScanner - 1].NameID + (m_DeviceInfoEx[IndexSelecaoScanner - 1].Instance << 8));


                m_NBioScan.OpenDevice(m_OpenedDeviceID);

                if (m_NBioScan.m_dwResult == NBioScan.Error.NONE)
                {
                    NBioScan.Type.DEVICE_INFO_0 deviceinfo0;

                    m_NBioScan.GetDeviceInfo(m_OpenedDeviceID, out deviceinfo0);

                    if (m_NBioScan.m_dwResult == NBioScan.Error.NONE)
                    {
                        valores.Clear();
                        valores.Append(" ").AppendLine(deviceinfo0.ImageFlatHeight.ToString());
                        valores.Append(" ").AppendLine("Image Flat Width: " + deviceinfo0.ImageFlatHeight.ToString());
                        valores.Append(" ").AppendLine("Image Flat Height: " + deviceinfo0.ImageFlatHeight.ToString());
                        valores.Append(" ").AppendLine("Image Roll Width: " + deviceinfo0.ImageRollWidth.ToString());
                        valores.Append(" ").AppendLine("Image Roll Height: " + deviceinfo0.ImageRollHeight.ToString());
                        valores.Append(" ").AppendLine("Brightness: " + deviceinfo0.Brightness.ToString() +
                                                ", Contrast: " + deviceinfo0.Contrast.ToString() +
                                                ", Gain: " + deviceinfo0.Gain.ToString());

                        log.write(valores.ToString());
                        return true;
                    }
                    else
                    {
                        log.write("NBioScan SDK Error[" + NBioScan.Error.GetErrorDescription(m_NBioScan.m_dwResult) + "]");
                        return false;
                    }
                }
                else
                {
                    log.write("NBioScan SDK Error[" + NBioScan.Error.GetErrorDescription(m_NBioScan.m_dwResult) + "]");
                    return false;
                }


                
            }
            catch (Exception ex)
            {

            }
            return false;

        }

        public string DefineErro(uint idErro)
        {
            switch (idErro)
            {
                case NBioScan.Error.NONE: return "Sucesso"; break;
                case NBioScan.Error.NOT_SUPPORT_DEVICEIOCONTROL: return "Falha na inicialização do scanner"; break;
                case NBioScan.Error.NOT_SUPPORT_AUTOON: return "Falha na inicialização do scanner"; break;
                case NBioScan.Error.SET_CALLBACK: return "Retorno inexperado para o scanner"; break;
                case NBioScan.Error.CAPTURE_TIMEOUT: return "Estourou o tempo para captura biométrica"; break;
                case NBioScan.Error.LOST_DEVICE: return "Scanner desconectado"; break;
                case NBioScan.Error.CAPTURE_FAIL: return "Falha na captura do scanner"; break;
                case NBioScan.Error.CAPTURE_REMOVE_FINGER: return "É necessário retirar o dedo do scanner"; break;
                case NBioScan.Error.DEVICE_CONTRAST: return "Contraste inválido para o scanner"; break;
                case NBioScan.Error.CAPTURE_DIRTY_SENSOR: return "O Sensor esta muito sujo para uso"; break;
                case NBioScan.Error.NOT_SUPPORT_ADJUST: return "Ajuste não suportado para o scanner"; break;
                case NBioScan.Error.EXTRACTION_FAIL: return "Falha na extração biométrica"; break;
                case NBioScan.Error.VERIFICATION_FAIL: return "Falha na verificação"; break;
                case NBioScan.Error.SEGMENT_NO_FINGER: return "Impressão digital não detectada"; break;
                case NBioScan.Error.SEGMENT_MANY_FINGER: return "Existem muitas impressões digitais"; break;
                case NBioScan.Error.SEGMENT_ONE_FINGER: return "Detectado apenas um dia"; break;
                case NBioScan.Error.SEGMENT_TWO_FINGER: return "Dois dedos detectados"; break;
                case NBioScan.Error.CAPTURE_FINGER_SLIDING: return "Falha na captura da biometria"; break;
                case NBioScan.Error.DEVICE_BRIGHTNESS: return "Definição de brilho inválido"; break;
                case NBioScan.Error.GET_DEVICE_INFO: return "Falha recuperando informações do scanner"; break;
                case NBioScan.Error.FUNCTION_FAIL: return "Falha de detecção do scanner"; break;
                case NBioScan.Error.OUT_OF_MEMORY: return "Falta de memória para leitura biométrica"; break;
                case NBioScan.Error.INVALID_LICENSE: return "Licença inválida"; break;
                case NBioScan.Error.EXPIRED_LICENSE: return "Licença expirada"; break;
                case NBioScan.Error.INVALID_STRUCT_TYPE: return "Estrutura inválida do scanner"; break;
                case NBioScan.Error.INVALID_DEFAULT_TIMEOUT: return "Timeout inválido"; break;
                case NBioScan.Error.INVALID_CAPTURE_QUALITY: return "Qualidade de captura inválida"; break;
                case NBioScan.Error.DEVICE_NOT_OPENED: return "Dispositivo não iniciado"; break;
                case NBioScan.Error.INVALID_PARAM: return "Parametro inválido"; break;
                case NBioScan.Error.INVALID_TEMPLATE_TYPE: return "Template inválido"; break;
                case NBioScan.Error.INVALID_COMPOSE_TYPE: return "Tipo de composição inválido"; break;
                case NBioScan.Error.INVALID_SENSOR_NOISE_LEVEL: return "Nivel de ruido inválido no sensor"; break;
                case NBioScan.Error.DEVICE_OPEN_FAIL: return "Falha na abertura do scanner"; break;
                case NBioScan.Error.DEVICE_ALREADY_OPENED: return "Scanner já inicializado"; break;
                case NBioScan.Error.INVALID_DEVICE_ID: return "Id de dispositivo inválido"; break;
                case NBioScan.Error.WRONG_DEVICE_ID: return "Id de dispositivo errado"; break;
                case NBioScan.Error.DEVICE_INIT_FAIL: return "Falha na inicialização do dispositivo"; break;
                case NBioScan.Error.INVALID_CPATURE_OPTION: return "Opção de captura inválida"; break;
                case NBioScan.Error.INVALID_HANDLE: return "Descritor inválido para o dispositivo"; break;
                default: return "Erro [idErro] não definido para leitor biométrico"; break;
            }
            return "";
        }

        Func<bool, int> funcSucesso;
        Func<string, int> funcFalha;
        public static string a;
        public String getErro()
        {
            return a;
        }

        // Método de Captura baseado pelo evento do BT_CAPTURE_CLICK
        public async Task<bool> CapturaDedo(PictureBox pbImg, Func<bool, int> functionToPass, Func<string, int> functionToPasserror)
        {
            try
            {
                funcSucesso = functionToPass;
                funcFalha = functionToPasserror;
                m_CapturedImage = null;
                m_CaptureOption.CFType = NBioScan.Type.CF_TYPE.ROLL_SINGLE;
                m_CaptureOption.ImageQuality = Parametros.ParametroSys.ScannerCapturaQualidade;
                m_CaptureOption.RollPreviewStopQuality = Parametros.ParametroSys.ScannerParadaQualidade;
                m_CaptureOption.TimeOut = Parametros.ParametroSys.ScannerTimeOut;

                m_NBioScan.GetInitInfo(out initinfo0);

                if (m_NBioScan.m_dwResult != NBioScan.Error.NONE)
                {
                    log.write("NBioScan SDK Error[" + NBioScan.Error.GetErrorDescription(m_NBioScan.m_dwResult) + "]");
                    functionToPasserror(DefineErro(m_NBioScan.m_dwResult));
                    return false;
                }
                else
                {
                    initinfo0.DefaultTimeout = Parametros.ParametroSys.ScannerTimeOut;
                     initinfo0.RollPriviewStopQuality = Parametros.ParametroSys.ScannerParadaQualidade;
                    initinfo0.CaptureImageQuality = Parametros.ParametroSys.ScannerCapturaQualidade;
                    // initinfo0.RollComposeType = Parametros.ParametroSys.ScannerTipoComposicao;
                    initinfo0.CheckSensorNoiseLevel = Parametros.ParametroSys.ScannerChecaRuido;
                    
                    
                }
                
                m_NBioScan.SetInitInfo(initinfo0);

                if (m_NBioScan.m_dwResult != NBioScan.Error.NONE)
                {
                    log.write("NBioScan SDK Error 2[" + NBioScan.Error.GetErrorDescription(m_NBioScan.m_dwResult) + "]");
                    functionToPasserror(DefineErro(m_NBioScan.m_dwResult));
                    return false;
                }                

                pbImg.Invoke(new MethodInvoker(() =>
                {
                    m_CaptureOption.FingerDrawWnd = pbImg.Handle;
                }));

                m_CaptureOption.CallBackFunction = new NBioScan.Type.CAPTURE_CALLBACK_FUNC(MyCaptureCallBack);

                if (m_bRunRollThread)
                {
                    m_bComposeStart = true;
                    return true;
                }
                else
                {
                    log.write("Iniciando Captura");
                    //BT_CAPTURE.Enabled = false;
                    //Application.DoEvents();

                    m_NBioScan.Capture(out m_CapturedImage, m_CaptureOption);

                    if (m_NBioScan.m_dwResult == NBioScan.Error.NONE)
                    {
                        log.write("Fim da Captura");
                        SaveFlatCapturedImage(log);

                        return true;
                    }
                    else if (m_NBioScan.m_dwResult != NBioScan.Error.NONE)
                    {
                        log.write("Fim da Captura[Error: " + DefineErro(m_NBioScan.m_dwResult) + "]");
                        a = DefineErro(m_NBioScan.m_dwResult);


                        funcFalha(a);
                        return false;
                    }                   
                    else
                    {                        
                        m_NBioScan.Capture(out m_CapturedImage, m_CaptureOption);
                        if (m_NBioScan.m_dwResult == NBioScan.Error.NONE)
                        {
                            log.write("Roll Capture end");
                            SaveRollCapturedImage(log);
                            funcSucesso(true);
                        }
                        else
                        {
                            log.write("Roll Capture end[Error: " + DefineErro(m_NBioScan.m_dwResult) + "]");
                            funcFalha("Roll Capture end[Error: " + DefineErro(m_NBioScan.m_dwResult) + "]");
                        }
                        m_bRunRollThread = false;
                    }

                }
            }
            catch (Exception e)
            {
                funcFalha("Falha SDK BIO: " + e.Message + " - " + e.StackTrace);
            }
            return true;
        }

        public uint MyCaptureCallBack(ref NBioScan.Type.CAPTURE_CALLBACK_PARAM pParam, IntPtr pUserParam)
        {
            try
            {
                string szComposing = (pParam.DeviceError == NBioScan.Type.CAPTURE_CALLBACK_MSG.COMPOSE_ING) ? "Compose start" : null;
                string szDeviceError = pParam.DeviceError.ToString();
                string szQuality = pParam.Quality.ToString();
                m_CaptureOption.CFType = NBioScan.Type.CF_TYPE.ROLL_SINGLE;

                if (m_CaptureOption.CFType == NBioScan.Type.CF_TYPE.ROLL_SINGLE)
                {
                    if (szComposing != null)
                    {

                        log.write(szComposing);
                        // LB_CAPTURERET.SelectedIndex = LB_CAPTURERET.Items.Count - 1;
                    }

                    else
                    {
                        var resp = DefineErro(m_NBioScan.m_dwResult);
                        if(!resp.Contains("Sucesso"))
                        log.write("Device Error: " + resp);
                        // LB_CAPTURERET.SelectedIndex = LB_CAPTURERET.Items.Count - 1;
                    }
                }


                if (m_bComposeStart)
                    return NBioScan.Type.CAPTURE_CALLBACK_MSG.COMPOSE_START;

                if (m_bFlatStop)
                    return 1;

            }
            catch (Exception e)
            {

            }
            return 0;
        }

        private void SaveRollCapturedImage(LogFile log)
        {
            string szFileName = ".\\RollImage_.png";
            FileStream fs = new FileStream(szFileName, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);


            //funcSucesso(true);

            bw.Write(m_CapturedImage.Image);
            bw.Close();
            fs.Close();
            log.write(szFileName + " saved");
        }

        public void TestaImagem(LogFile log)
        {
            NBioAPI nBioAPI = new NBioAPI();
            try
            {              

                float qualidade = 15.0f;
                int lenWSQ = 800 * 750;
               byte[] imgWSQ = new byte[800 * 750];
                var imgRaw = File.ReadAllBytes(@".\Digital800_750-733.raw");
                var respwsq = NBioAPI_ImgConvRawToWSQBuf(imgRaw,    //raw Image
                                                   800,
                                                   750,
                                                   imgWSQ,              //WSQ Image Buffer
                                                   out lenWSQ,      //WSQ image file Length.		
                                                   qualidade            //quality value 0.1 - 7.0 ex)float  m_WSQquality = 0.75; // 15:1
                                                   );
                byte[] nimgWsq = new byte[lenWSQ];
                for(int i=0; i< lenWSQ; i++)
                {
                    nimgWsq[i] = imgWSQ[i];
                }
                File.WriteAllBytes(@".\ImagemNova.wsq", nimgWsq);
                File.WriteAllBytes(@".\ImagemNova" + DateTime.Now.Millisecond + ".wsq", nimgWsq);
                //File.Delete(szFileName);
                //funcSucesso(true);

            }
            catch (Exception e)
            {              
                MessageBox.Show(e.ToString());
            }
        }
    


    private void SaveFlatCapturedImage(LogFile log)
        {
            NBioAPI nBioAPI = new NBioAPI();


            string szFileName = @".\Digital" + m_CapturedImage.Width.ToString() + "_" + m_CapturedImage.Height.ToString() + "-" + DateTime.Now.Millisecond + ".raw";
            FileStream fs = new FileStream(szFileName, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                
                bw.Write(m_CapturedImage.Image);
                bw.Close();
                fs.Close();
                log.write(szFileName + " saved");

                float qualidade = 15.0f;
                int lenWSQ = 800 * 750;
                System.Byte[] imgWSQ = new System.Byte[800 * 750];
                var imgRaw = File.ReadAllBytes(szFileName);
                var respwsq = NBioAPI_ImgConvRawToWSQBuf(imgRaw,    //raw Image
                                                   m_CapturedImage.Width,
                                                   m_CapturedImage.Height,
                                                   imgWSQ,              //WSQ Image Buffer
                                                   out lenWSQ,      //WSQ image file Length.		
                                                   qualidade            //quality value 0.1 - 7.0 ex)float  m_WSQquality = 0.75; // 15:1
                                                   );

                byte[] nimgWsq = new byte[lenWSQ];
                for (int i = 0; i < lenWSQ; i++)
                {
                    nimgWsq[i] = imgWSQ[i];
                }

                File.WriteAllBytes(@".\ImagemNova.wsq", nimgWsq);
               // File.WriteAllBytes(@".\ImagemNova"+DateTime.Now.Millisecond+".wsq", nimgWsq);
                File.Delete(szFileName);
                funcSucesso(true);

            }
            catch (Exception e)
            {
                funcFalha("ERRO SaveFlatCapturedImage: " + e.Message + " - " + e.StackTrace);
                MessageBox.Show(e.ToString());
            }

            /*string szFileName = @"..\Digital" + m_CapturedImage.Width.ToString() + "_" + m_CapturedImage.Height.ToString()+".raw";
            FileStream fs = new FileStream(szFileName, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {

                bw.Write(m_CapturedImage.Image);
                bw.Close();
                fs.Close();
                log.write(szFileName + " saved");

                float qualidade = 0.75f;
                int lenWSQ = 600 * 650;
                System.Byte[] imgWSQ = new System.Byte[600 * 650];
                var imgRaw = File.ReadAllBytes(szFileName);
                var respwsq = NBioAPI_ImgConvRawToWSQBuf(imgRaw,    //raw Image
                                                   m_CapturedImage.Width,
                                                   m_CapturedImage.Height,
                                                   imgWSQ,              //WSQ Image Buffer
                                                   out lenWSQ,      //WSQ image file Length.		
                                                   qualidade            //quality value 0.1 - 7.0 ex)float  m_WSQquality = 0.75; // 15:1
                                                   );

                File.WriteAllBytes(@"..\ImagemNova.wsq", imgWSQ);
                funcSucesso(true);
            }
            catch (Exception e){
                funcFalha(true);
                MessageBox.Show(e.ToString());
            }*/


        }
    }
}

