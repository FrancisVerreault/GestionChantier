using GestionChantier.Helper;
using GestionChantier.Model;
using LivraisonsContextV2.Contexts;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace GestionChantier.Controllers
{
    public class LivraisonController : InformationUtilisateurController
    {
        string LivraisonDocumentFolder = @"\\bruel-pdc.bruneau.local\bibliotheque\Livraison\";

        // GET: Livraison
        public ActionResult Index()
        {
            return View("Index", SearchLivraison(LivraisonDocumentFolder));
        }

        public ActionResult Create()
        {
            LivraisonViewModel viewModel = new LivraisonViewModel();
            viewModel.Date = DateTime.Now;
            viewModel.receiverEmail = this.GetUserEmail();
            viewModel.senderEmail = this.GetUserEmail();
            viewModel.senderPhone = this.GetUserPhoneNumber();
            viewModel.receiverPhone = this.GetUserPhoneNumber();
            viewModel.senderName = this.GetUserCompleteName();
            viewModel.receiverName = this.GetUserCompleteName();

            return View(viewModel);
        }

        public ActionResult CreatePDF(LivraisonViewModel model)
        {
            List<string> emails;
            if (model.receiverEmail != model.senderEmail)
                emails = new List<string> { model.senderEmail, model.receiverEmail };
            else
                emails = new List<string> { model.senderEmail };

            if (!emails.Contains("Jocelyn.Charron@BruneauElectrique.com"))
                emails.Add("Jocelyn.Charron@BruneauElectrique.com");

            string pdfPath = CreateNewCommandePDF(model);

            //Create entry in database(this database is a copy of Pricer, but it should not. The client ask me to do it so that we could easily list the commandes with the dashboard app)
            //There will be a lot of null in the database
            AddCommandeToDatabase(model, pdfPath);

            EmailHelper.SendEmailWithAttachment(emails.ToArray(), pdfPath, "Livraison");
            return Index();
        }

        private void AddCommandeToDatabase(LivraisonViewModel model, string pdfPath)
        {
            using (LivraisonContext context = new LivraisonContext())
            {
                //Nouveau produit qui est en fait la commande
                LivraisonsContextV2.Models.ProductBruneau livraison = new LivraisonsContextV2.Models.ProductBruneau();
                livraison.DescriptionFR = model.description;
                livraison.DrawingPath = pdfPath;

                context.Products.Add(livraison);
                context.SaveChanges();


                LivraisonsContextV2.Models.EntryListProduct entry = new LivraisonsContextV2.Models.EntryListProduct();
                entry.Date = model.Date;
                entry.Product = livraison;
                entry.QtyAsked = 1;


                context.EntriesListProduct.Add(entry);
                context.SaveChanges();

                LivraisonsContextV2.Models.ListLivraison lst = new LivraisonsContextV2.Models.ListLivraison();
                lst.IdBottin = this.GetUserID();
                lst.Name = this.GetUserCompleteName() + model.Date.ToString().Replace(":", "_");
                lst.ProductsEntries = new List<AbstractEntryListProduct>();
                lst.ProductsEntries.Add(entry);
                lst.lieuLivraison = model.lieuLivraison;
                lst.lieuRamassage = model.lieuRamassage;
                lst.receiverEmail = model.receiverEmail;
                lst.receiverName = model.receiverName;
                lst.receiverPhone = model.receiverPhone;
                lst.senderEmail = model.senderEmail;
                lst.senderName = model.senderName;
                lst.senderPhone = model.senderPhone;

                context.ListsProducts.Add(lst);
                context.SaveChanges();
            }
        }

        public string CreateNewCommandePDF(LivraisonViewModel model)
        {
            //Il faut un moyen pour vider les tempFolder de temps en temps donc c'est ici
            DirectoryInfo directory = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(@"~\Ressources\tempFolderLivraison\"));
            directory.Empty();

            Directory.CreateDirectory(LivraisonDocumentFolder + this.GetUserCompleteName() + @"\");

            string newPDFPath = LivraisonDocumentFolder + this.GetUserCompleteName() + @"\" + DateTime.Now.ToString().Replace(":", "_").Replace(" ", "_") + ".pdf";

            Dictionary<string, string> customBalises = new Dictionary<string, string>();
            customBalises.Add("{Date}", DateTime.Now.ToString("yyyy-MM-dd"));
            customBalises.Add("{senderName}", this.GetUserCompleteName());
            customBalises.Add("{senderEmail}", model.senderEmail);
            customBalises.Add("{senderPhone}", model.senderPhone);
            customBalises.Add("{receiverEmail}", model.receiverEmail);
            customBalises.Add("{receiverPhone}", model.receiverPhone);
            customBalises.Add("{description}", model.description);
            customBalises.Add("{lieuLivraison}", model.lieuLivraison);
            customBalises.Add("{lieuRamassage}", model.lieuRamassage);

            DocumentManipulator.PDFManipulator pDFManipulator = new DocumentManipulator.PDFManipulator(System.Web.HttpContext.Current.Server.MapPath(@"~\Ressources\Formulaire_livraison.pdf"), null, customBalises.ToList());
            pDFManipulator.FillDocumentToPdf(newPDFPath, false);

            return newPDFPath;
        }



        public FileResult GetCommandePDF(string path)
        {
            if (path.Contains(LivraisonDocumentFolder))
            {
                byte[] FileBytes = System.IO.File.ReadAllBytes(path);
                return File(FileBytes, "application/pdf");
            }

            return null;
        }

        public ActionResult Complete(string path)
        {
            if (path.Contains(LivraisonDocumentFolder))
            {
                System.IO.File.Move(path, path.Replace(".pdf", "_Complet.pdf"));
            }

            return Index();
        }

        static public void SaveStreamToFile(string fileFullPath, Stream stream)
        {
            if (stream.Length == 0) return;

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)stream.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }

        private List<Livraison> SearchLivraison(string sDir)
        {
            List<Livraison> livraisons = new List<Livraison>();

            foreach (string file in Directory.EnumerateFiles(sDir, "*.*", SearchOption.AllDirectories))
            {
                if (!file.Contains("Complet"))
                {
                    Livraison livraison = new Livraison();
                    livraison.NomFichier = Path.GetFileName(file);
                    livraison.NomCompletUtilisateur = Path.GetDirectoryName(file).Split('\\').Last();
                    livraison.Path = file;
                    livraisons.Add(livraison);
                }
            }
            return livraisons;
        }
    }
}

