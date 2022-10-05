using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Database.Objetos
{
    class ObToken
    {
        public string user { get; set; }
        public string pass { get; set; }
        public string projeto { get; set; }

        public string ip { get; set; }

        public string mac { get; set; }

       
    }

    public class ObTipo
    {
        public int Operacao { get; set; }
        public ObTipo(int op)
        {
            Operacao = op;
        }
    }

    public class ObProxy
    {
        public string userProxy { get; set; }
        public string passProxy { get; set; }
    }
}
