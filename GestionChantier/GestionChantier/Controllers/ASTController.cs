using System.Web.Mvc;

namespace GestionChantier.Controllers
{
    public class ASTController : PhotographerController
    {
        public ASTController()
        {
            this.PhotoFolder = @"\\bruel-pdc.bruneau.local\Ressource Humaine\Privé\CNESST et SST Bruneau\AST\";
        }

        //[Authorize]
        public ActionResult Index()
        {
            ViewBag.ASTAdmin = this.IsAstAdmin();
            return View();
        }


    }
}