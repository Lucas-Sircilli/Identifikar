using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Database.Objetos
{
    
    public class ObParametros
    {
        public uint ScannerIdDispositivo { get; set; }
        public uint ScannerCapturaQualidade { get; set; }
        public uint ScannerTimeOut { get; set; }
        public uint ScannerChecaRuido { get; set; }
        public uint ScannerParadaQualidade { get; set; }
        public uint CameraIdDispositivo { get; set; }
        public uint CameraCapturaResolucao { get; set; }
        public int CameraLargura { get; set; }
        public int CameraAltura { get; set; }
    }
   
}
