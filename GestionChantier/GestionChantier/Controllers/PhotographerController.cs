using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace GestionChantier.Controllers
{
    public abstract class PhotographerController : InformationUtilisateurController
    {
        protected string PhotoFolder { get; set; }

        [HttpPost]
        public virtual bool EnvoyerPhoto(string ASTPicture, string ProjetName)
        {
            try
            {
                int idEmployer = GetUserID();
                TabBottin Contremaitre = new HorizonContext().TabBottin.Where(x => x.ID == idEmployer).FirstOrDefault();
                string nomContremaitre = Contremaitre.Prenom.Trim() + " " + Contremaitre.Nom.Trim();

                string saveFolder = PhotoFolder + nomContremaitre + @"\" + ProjetName + @"\";
                Directory.CreateDirectory(saveFolder);


                ASTPicture = ASTPicture.Remove(0, ASTPicture.IndexOf(",") + 1);//.Replace("data:image/png;base64,", "");
                ASTPicture = ASTPicture.Replace(" ", "+");

                byte[] data = Convert.FromBase64String(ASTPicture);

                using (MemoryStream memstr = new MemoryStream(data))
                {
                    Image img = Image.FromStream(memstr);
                    img.Save(saveFolder + ProjetName + "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "_" + DateTime.Now.Hour.ToString() + "H" + DateTime.Now.Minute.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }

            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public ActionResult ObtenirProjets()
        {
            using (HorizonContext contextHorizon = new HorizonContext())
            {
                int usrId = GetUserID();

                List<int> lstProjetList = contextHorizon.TabListeEmploye.Where(x => x.IdEmploye == usrId).Select(x => x.IdProjet).ToList();

                //Pour l'instant seul les contremaitres, chargés de projets, directeur de projet sont assigné à des projets donc pour les autres on affiche tout!
                if (IsUserChargeProjet() || IsUserContremaitre() || IsUserDirecteurProjet())
                {
                    var lstProjets = new HorizonContext().TabProjets.Where(x => x.IdStatut > 300 && x.IdStatut < 800 && (x.IdAdjoint == usrId || x.IdChargeProjet == usrId
                    || x.IdContremaitre == usrId || x.IdContremaitre2 == usrId || x.IdDirecteurProjet == usrId) || lstProjetList.Contains(x.ID)).OrderByDescending(x => x.NumeroProjet).ToList();

                    if (lstProjets.Count == 0)
                    {
                        var lstProjets2 = new HorizonContext().TabProjets.Where(x => x.IdStatut > 300 && x.IdStatut < 800 || lstProjetList.Contains(x.ID)).OrderByDescending(x => x.NumeroProjet).ToList();
                        return Json(new { success = true, responseText = "Requête traitée avec succès!", data = lstProjets2 });
                    }
                    else
                    {
                        return Json(new { success = true, responseText = "Requête traitée avec succès!", data = lstProjets });
                    }
                }
                else
                {
                    var lstProjets2 = new HorizonContext().TabProjets.Where(x => x.IdStatut > 300 && x.IdStatut < 800 || lstProjetList.Contains(x.ID)).OrderByDescending(x => x.NumeroProjet).ToList();
                    return Json(new { success = true, responseText = "Requête traitée avec succès!", data = lstProjets2 });
                }
            }

        }

        [HttpPost]
        public ActionResult ObtenirDateInactiviteProjet(int idProjet)
        {
            DateTime dateDebutInactivite = new HorizonContext().TabProjets.Find(idProjet).DebutASTInactif.GetValueOrDefault();
            DateTime dateFinInactivite = new HorizonContext().TabProjets.Find(idProjet).FinASTInactif.GetValueOrDefault();

            string dateDebutInactiviteReturn = (dateDebutInactivite == DateTime.MinValue) ? "" : dateDebutInactivite.ToString("yyyy-MM-dd");
            string dateFinInactiviteReturn = (dateFinInactivite == DateTime.MinValue) ? "" : dateFinInactivite.ToString("yyyy-MM-dd");

            return Json(new { success = true, responseText = "Requête traitée avec succès!", dateDebutInactivite = dateDebutInactiviteReturn, dateFinInactivite = dateFinInactiviteReturn });
        }

        [HttpPost]
        public ActionResult UpdateDateInactiviteProjet(int idProjet, string dateDebutInactivite, string dateFinInactivite)
        {
            DateTime? DTdateDebutInactivite = (dateDebutInactivite != "") ? DateTime.Parse(dateDebutInactivite) : DateTime.MinValue;
            DateTime? DTdateFinInactivite = (dateFinInactivite != "") ? DateTime.Parse(dateFinInactivite) : DateTime.MinValue;

            using (HorizonContext context = new HorizonContext())
            {
                context.TabProjets.Find(idProjet).DebutASTInactif = (DTdateDebutInactivite != DateTime.MinValue) ? DTdateDebutInactivite : null;
                context.TabProjets.Find(idProjet).FinASTInactif = (DTdateFinInactivite != DateTime.MinValue) ? DTdateFinInactivite : null;

                context.SaveChanges();
            };

            return Json(new { success = true, responseText = "Requête traitée avec succès!" });
        }

        [HttpPost]
        public ActionResult GetNbPhotoForToday(int idProjet)
        {
            using (HorizonContext context = new HorizonContext())
            {
                string nomProjet = context.TabProjets.Find(idProjet).NumeroProjet;

                int nbPhotoFound = Directory.GetFiles(PhotoFolder, "*" + nomProjet + "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "*", SearchOption.AllDirectories).Count();
                return Json(new { success = true, responseText = "Requête traitée avec succès!", nbPhotoFound = nbPhotoFound });
            }
        }

        [HttpPost]
        public ActionResult IsPhotoTakenByUserToday(int idProjet)
        {
            using (HorizonContext context = new HorizonContext())
            {
                int idEmployer = GetUserID();
                TabBottin Contremaitre = new HorizonContext().TabBottin.Where(x => x.ID == idEmployer).FirstOrDefault();
                string nomContremaitre = Contremaitre.Prenom.Trim() + " " + Contremaitre.Nom.Trim();
                string nomProjet = context.TabProjets.Find(idProjet).NumeroProjet;
                bool isPhotoTaken = false;
                if (Directory.Exists(PhotoFolder + nomContremaitre + "\\" + nomProjet + "\\"))
                    isPhotoTaken = Directory.GetFiles(PhotoFolder + nomContremaitre + "\\" + nomProjet + "\\", "*" + nomProjet + "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "*").Any();
                return Json(new { success = true, responseText = "Requête traitée avec succès!", isPhotoTaken = isPhotoTaken });
            }
        }


    }
}