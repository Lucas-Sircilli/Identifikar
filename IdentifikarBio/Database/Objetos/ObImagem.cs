using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Database.Objetos
{
    public class ObRetorno
    {
        public string mensagem { get; set; }
        public int codigoRetorno { get; set; }
    }
    class ObImagem
    {
        public ObImagemAssinatura Assinatura { get; set; }

        public ObImagemBiometria BioCadastro { get; set; }

        public List<ObImagemBiometria> Biometrias { get; set; }

        public ObImagemFoto Foto { get; set; }

    }

    public class ObImagemAssinatura
    {
        public int? id_candidatos { get; set; }
        public string cpf { get; set; }
        public string renach { get; set; }
        
        public byte[] foto { get; set; }

        //public int? indice { get; set; }

        // falta imagem no formato text
        //public DateTime? DataHoraCadastro { get; set; }
    }

    public class ObImagemBiometria
    {
        public int id_candidatos { get; set; }
        public string cpf { get; set; }
        public string renach { get; set; }
        public int indice { get; set; }
        public int coletado { get; set; }
        public int minucias { get; set; }
        public byte[] arquivo { get; set; }
    }

    public class ObImagemFoto
    {
        public int id_candidatos { get; set; }
        public string cpf { get; set; }
        public string renach { get; set; }
        public byte[] foto { get; set; }
    }
}

