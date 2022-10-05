using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Database.Objetos
{
    public class ObCadastro
    {

        public string cpf { get; set; }
        public string renach { get; set; }
        public string excecao_digital { get; set; }
        public string sexo { get; set; }
        public string nome { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public int? id_municipios { get; set; }
        public string uf { get; set; }
       public string telefone { get; set; }
        public string telefone2 { get; set; }
        public string email { get; set; }
        public string registro { get; set; }
        public DateTime? registro_expedicao { get; set; }
        public string registro_emissor { get; set; }
        public string registro_uf { get; set; }
        public DateTime? nascimento { get; set; }
        public string nome_pai { get; set; }
        public string nome_mae { get; set; }

        
        public int? id_candidatos { get; set; }
        public string Municipio { get; set; }
        public string DDDTelefone { get; set; }
        public string DDDCelular { get; set; }
        public DateTime? DataHoraCadastro { get; set; }
        
        

       // public ObCadastroAssinatura Assinatura { get; set; }

       // public List<ObCadastroBiometria> Biometrias { get; set; }

      //  public ObCadastroFoto Foto { get; set; }

    }

   /* public class ObCadastroAssinatura
    {
        public int IdCadastro { get; set; }
        public string CPF { get; set; }
        public string RENACH { get; set; }
        public byte[] Imagem { get; set; }
    }

    public class ObCadastroBiometria
    {
        public int IdCadastro { get; set; }
        public string CPF { get; set; }
        public string RENACH { get; set; }

        public int IndicaDedo { get; set; }
        public byte[] Imagem { get; set; }
    }

    public class ObCadastroFoto
    {
        public int IdCadastro { get; set; }
        public string CPF { get; set; }
        public string RENACH { get; set; }
        public byte[] Imagem { get; set; }
    }*/
}
