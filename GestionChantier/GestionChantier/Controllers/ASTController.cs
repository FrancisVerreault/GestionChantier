using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionChantier.Controllers
{
    public class ASTController : Controller
    {
        //string ASTFolderPath = @"\\PDC2K3R2\wwwroot\PhotoAST\";
        string ASTFolderPath = @"\\PDC2K3R2\Ressource Humaine\Privé\CNESST\AST\";
        Models.HorizonContext horizonContext = new Models.HorizonContext();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool EnvoyerAST(string ASTPicture, string ProjetName)
        {
            try
            {
                ASTPicture = ASTPicture.Replace("data:image/png;base64,", "");
                ASTPicture = ASTPicture.Replace(" ", "+");

                byte[] data = Convert.FromBase64String(ASTPicture);

                using (MemoryStream memstr = new MemoryStream(data))
                {
                    Image img = Image.FromStream(memstr);
                    img.Save(ASTFolderPath + ProjetName + "_" + DateTime.Now.Hour.ToString() + "H" + DateTime.Now.Minute.ToString() + "ms" + DateTime.Now.Millisecond.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
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
            //TODO : seulement récupérer les projets auxquels participe l'employé
            var lstProjetHorizon = horizonContext.Database.SqlQuery<Models.ProjetHorizon>("Select ID, NumeroProjet FROM dbo.TabProjets WHERE IdStatut != '800' and IdStatut != '900'").ToList();

            return Json(new { success = true, responseText = "Requête traitée avec succès!", data = lstProjetHorizon });
        }
    }
}