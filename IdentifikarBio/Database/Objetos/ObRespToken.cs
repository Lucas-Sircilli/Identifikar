using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Database.Objetos
{
    class ObRespToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int tipo_operador { get; set; }
        public string user_name { get; set; }
        public bool admin { get; set; }
    }
}
