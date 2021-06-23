using GestionChantier.Helper;
using GestionChantier.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace GestionChantier.Controllers
{
    public class LoutecController : InformationUtilisateurController
    {
        // GET: Loutec
        public ActionResult Location()
        {
            InformationUtilisateur infoUtilisateur = new InformationUtilisateur();
            infoUtilisateur.nom = GetUserCompleteName();
            infoUtilisateur.courriel = GetUserEmail();
            infoUtilisateur.telephone = GetUserPhoneNumber();
            infoUtilisateur.messageToShow = "";

            return View(infoUtilisateur);
        }

        [HttpPost]
        public ActionResult EnvoyerFormulaireLocation(CommandeLoutec commande)
        {
            Dictionary<string, string> customBalises = new Dictionary<string, string>();
            customBalises.Add("{nom}", commande.nom);
            customBalises.Add("{telephone}", commande.telephone);
            customBalises.Add("{courriel}", commande.courriel);
            customBalises.Add("{adresseLivraison}", commande.adresseLivraison);
            customBalises.Add("{machines}", commande.machines);
            customBalises.Add("{dateHeureLivraison}", (commande.livraisonRapide) ? "Le plus tôt possible" : commande.dateHeureLivraison.ToString());
            customBalises.Add("{dureeLocationJour}", commande.dureeLocationJour.ToString());
            customBalises.Add("{dureeLocationHeure}", commande.dureeLocationHeure.ToString());
            customBalises.Add("{po}", commande.po);
            customBalises.Add("{commentaire}", commande.commentaire);
            customBalises.Add("{quai}", (commande.SolOuQuai == "Quai") ? "X" : "");
            customBalises.Add("{sol}", (commande.SolOuQuai == "Sol") ? "X" : "");

            DocumentManipulator.PDFManipulator pdfManipulator = new DocumentManipulator.PDFManipulator(System.Web.HttpContext.Current.Server.MapPath(@"~\Ressources\LocationLoutecPDF.pdf"), null, customBalises.ToList());
            string commandeFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~\Ressources\tempFolder\LocationLoutec_" + DateTime.Now.ToString().Replace(":", "_").Replace(" ", "_") + ".pdf");
            pdfManipulator.FillDocumentToPdf(commandeFilePath);

            string returnValue;
            returnValue = "Commande envoyée!";

            List<string> ccEmails = new List<string>();
            ccEmails.Add(commande.courriel);
            ccEmails.Add(commande.courrielDirecteurProjet);
            if (!commande.courriel.Contains("mario.carrier"))
                ccEmails.Add("mario.carrier@bruneauelectrique.com");

            try
            {
                EmailHelper.SendEmailWithAttachmentForLoutec(commande.courriel, ccEmails.ToArray(), commandeFilePath, "Location outils Bruneau Électrique");

            }
            catch (Exception e)
            {

                returnValue = "Échec lors de l'envoie du courriel. \n" + e.Message;
            }


            InformationUtilisateur infoUtilisateur = new InformationUtilisateur();
            infoUtilisateur.nom = GetUserCompleteName();
            infoUtilisateur.courriel = GetUserEmail();
            infoUtilisateur.telephone = GetUserPhoneNumber();
            infoUtilisateur.messageToShow = returnValue;

            return View("Location", infoUtilisateur);
        }
    }
}