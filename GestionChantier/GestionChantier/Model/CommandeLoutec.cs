using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionChantier.Model
{
    public class CommandeLoutec
    {
        public int listProductId { get; set; }

        public string nom { get; set; }

        public string telephone { get; set; }

        public string courriel { get; set; }

        public string courrielDirecteurProjet { get; set; }

        public string adresseLivraison { get; set; }

        public string machines { get; set; }

        public DateTime dateHeureLivraison { get; set; }

        public int dureeLocationHeure { get; set; }

        public int dureeLocationJour { get; set; }

        public string po { get; set; }

        public string commentaire { get; set; }

        public string SolOuQuai { get; set; }

        public bool livraisonRapide { get; set; }
    }
}