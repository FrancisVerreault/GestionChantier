using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace GestionChantier.Controllers
{
    public class PauseSecuriteController : PhotographerController
    {
        public PauseSecuriteController()
        {
            this.PhotoFolder = @"\\bruel-pdc.bruneau.local\Ressource Humaine\Privé\CNESST et SST Bruneau\Pause\";
        }

        //[Authorize]
        public ActionResult Index()
        {
            ViewBag.ASTAdmin = this.IsAstAdmin();
            return View();
        }
    }
}