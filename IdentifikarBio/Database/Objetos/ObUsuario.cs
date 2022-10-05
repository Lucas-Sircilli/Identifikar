using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Database.Objetos
{
    public class ObUsuario
    {
        public int id_usuario { get; set; }
        public string username { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
    }
}
