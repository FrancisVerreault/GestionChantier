using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionChantier.Model
{
    public class InformationUtilisateur
    {
        public string nom { get; set; }

        public string telephone { get; set; }

        public string courriel { get; set; }

        public string messageToShow { get; set; }

        public string courrielDirecteurProjet { get; set; }
    }
}