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
#if !DEBUG
                    [Authorize]
#endif
        public ActionResult Index()
        {
            //TODO : Changer ca pour un role au lieu de mettre les id de verreault, beaudry, carlos
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            if (claims.Any() && claims.ElementAt(9).Value != "1190" && claims.ElementAt(9).Value != "4" && claims.ElementAt(9).Value != "1133")
            {
                ViewBag.InfoProjet = "invisible";
                ViewBag.Punch = "invisible";
            }
            else
            {
                //Comme ca on ne montre pas le url dans le html si le user a pas l'autorisation
                ViewBag.InfoProjetURL = "/Home/RedirectToInfoProjet";
                ViewBag.PunchURL = "/Home/RedirectToPunch";
                ViewBag.PunchOutilsURL = "/Home/RedirectToPunchOutils";
            }

            if (claims.Any() && claims.ElementAt(9).Value != "1190" && claims.ElementAt(9).Value != "4" && claims.ElementAt(9).Value != "1133" && claims.ElementAt(9).Value != "1191")
                ViewBag.LocationLoutec = "invisible";
            else
                ViewBag.LocationLoutec = "";


            return View();
        }

        [Authorize]
        public ActionResult RedirectToPricer()
        {
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["PricerURL"]);
        }

        [Authorize]
        public ActionResult RedirectToPricerOutils()
        {
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["PricerOutilsURL"]);
        }

        [Authorize]
        public ActionResult RedirectToPricerLoutec()
        {
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["PricerLoutecURL"]);
        }

        [Authorize]
        public ActionResult RedirectToDashboardLivraisons()
        {
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["DashboardLivraisons"]);
        }

        public ActionResult RedirectToPunch()
        {
            //Obliger de verifier la source de l'appel car sinon ca redemande l'authentification puisque c'est un site à part.
            string indexDefaultPath = System.Configuration.ConfigurationManager.AppSettings["GestionChantierURL"].ToString().Replace("Home/Index", "");
            if (Request.UrlReferrer.ToString() == System.Configuration.ConfigurationManager.AppSettings["GestionChantierURL"].ToString() || Request.UrlReferrer.ToString() == indexDefaultPath)
                return Redirect(System.Configuration.ConfigurationManager.AppSettings["PunchURL"]);
            else
                return Redirect(System.Configuration.ConfigurationManager.AppSettings["AuthentificateurURL"]);
        }

        public ActionResult RedirectToInfoProjet()
        {
            //Obliger de verifier la source de l'appel car sinon ca redemande l'authentification puisque c'est un site à part.
            string indexDefaultPath = System.Configuration.ConfigurationManager.AppSettings["GestionChantierURL"].ToString().Replace("Home/Index", "");
            if (Request.UrlReferrer.ToString() == System.Configuration.ConfigurationManager.AppSettings["GestionChantierURL"].ToString() || Request.UrlReferrer.ToString() == indexDefaultPath)
                return Redirect(System.Configuration.ConfigurationManager.AppSettings["InfoProjetURL"]);
            else
                return Redirect(System.Configuration.ConfigurationManager.AppSettings["AuthentificateurURL"]);
        }

    }
}