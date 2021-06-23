using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionChantier.Model
{
    public class LivraisonViewModel
    {
        public string description { get; set; }

        public string lieuRamassage { get; set; }

        public string senderEmail { get; set; }

        public string senderPhone { get; set; }

        public string senderName { get; set; }

        public string lieuLivraison { get; set; }

        public string receiverEmail { get; set; }

        public string receiverPhone { get; set; }

        public string receiverName { get; set; }

        public DateTime Date { get; set; }
    }
}