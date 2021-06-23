using Model;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace GestionChantier.Controllers
{
    public class PackingSlipController : PhotographerController
    {
        public PackingSlipController()
        {
            this.PhotoFolder = @"\\bruel-pdc.bruneau.local\bibliotheque\";
        }

        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public override bool EnvoyerPhoto(string ASTPicture, string ProjetName)
        {
            try
            {
                int idEmployer = GetUserID();
                TabBottin Contremaitre = new HorizonContext().TabBottin.Where(x => x.ID == idEmployer).FirstOrDefault();
                string nomContremaitre = Contremaitre.Prenom.Trim() + " " + Contremaitre.Nom.Trim();

                string saveFolder = PhotoFolder + @"\" + ProjetName + @"\PackingSlip\" + nomContremaitre + @"\";
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


    }
}