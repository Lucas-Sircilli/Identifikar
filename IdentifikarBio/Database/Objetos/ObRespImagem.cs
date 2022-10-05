using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Database.Objetos
{
    class ObRespImagem
    {
        public ObImagemAssinatura Assinatura { get; set; }

        public List<ObRespImagemBiometria> Biometrias { get; set; }

        public ObImagemFoto Foto { get; set; }

    }

    public class ObRespImagemAssinatura
    {
        public int? IdCadastro { get; set; }
        public string cpf { get; set; }
        public string renach { get; set; }
        public byte[] foto { get; set; }

      //  public int indice { get; set; }

        // falta imagem no formato text
        public DateTime? data_hora_cadastro { get; set; }
    }

    public class DigitalBioIndice
    {
        public int id_entidades { get; set; }
        public int id_candidatos { get; set; }
        public string cpf { get; set; }
        public string renach { get; set; }
        public int indice { get; set; }
        public string local_gravacao { get; set; }
        public string caminho { get; set; }
        public string coletado { get; set; }
        public int? minucias { get; set; }
        public int? id_operadores_cadastro { get; set; }
        public string data_hora_cadastro { get; set; }
    }

    public class DigitalBio
    {
        public string cpf { get; set; }
        public string renach { get; set; }
        public List<DigitalBioIndice> digitais { get; set; }
    }

    public class ObRespImagemBiometria
    {
        public string cpf { get; set; }
        public string renach { get; set; }
        public string indice { get; set; }
        public string data_hora_cadastro { get; set; }
        public byte[] foto { get; set; }
        public string coletado { get; set; }
        public int? minucias { get; set; }

        public string erro { get; set; }

        public int? formato { get; set; }
    }

    public class ObRespImagemFoto
    {
        public int? IdCadastro { get; set; }
        public string cpf { get; set; }
        public string renach { get; set; }
        public byte[] foto { get; set; }
        public DateTime? data_hora_cadastro { get; set; }
    }

    public class ObRetornoErro
    {
        public int codigo { get; set; }
        public string message { get; set; }
    }

}
    

