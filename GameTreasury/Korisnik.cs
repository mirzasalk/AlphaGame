using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTreasury
{
    public class Korisnik
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public DateTime Clanarina { get; set; } // Promenili smo tip u DateTime
        public string PlaceneIgrice { get; set; }
        public bool EmailVerifikacija { get; set; }
        public string OcenjeneIgre { get; set; }
    }
}
