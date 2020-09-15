using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace GestionChantier.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            //TODO : Changer ca pour un role
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            if (claims.ElementAt(9).Value != "1190" && claims.ElementAt(9).Value != "4")
            {
                ViewBag.InfoProjet = "invisible";
                ViewBag.Punch = "invisible";
            }
            else {
                ViewBag.InfoProjetURL = "/Home/RedirectToInfoProjet";
                ViewBag.PunchURL = "/Home/RedirectToPunch";
            }

            return View();
        }

        //[Authorize]
        public ActionResult RedirectToPricer()
        {
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["PricerURL"]);
        }

        //[Authorize]
        public ActionResult RedirectToPunch()
        {
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["PunchURL"]);
        }

        //[Authorize]
        public ActionResult RedirectToInfoProjet()
        {
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["InfoProjetURL"]);
        }

    }
}