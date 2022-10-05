using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Database.Objetos
{
    class ObMunicipio
    {
        public int id_municipios { get; set; }
        public string municipio { get; set; }
    }
    
       class ObRespMunicipio
    {
        public string mensagem { get; set; }
        public int codigoRetorno { get; set; }
        public List<ObMunicipio> municipios { get; set; }
    }
}


