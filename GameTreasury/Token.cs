using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTreasury
{
    public class Token
    {
        public string Vrednost { get; set; }
        public DateTime DatumIsteka { get; set; }
        public Korisnik PovezaniKorisnik { get; set; }
    }
}
