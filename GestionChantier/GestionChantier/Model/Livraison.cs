using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GestionChantier.Model
{
    public class Livraison
    {
        [DisplayName("Nom du fichier")]
        public string NomFichier { get; set; }

        public string Path { get; set; }

        [DisplayName("Nom de l'utilisateur")]
        public string NomCompletUtilisateur { get; set; }


    }
}