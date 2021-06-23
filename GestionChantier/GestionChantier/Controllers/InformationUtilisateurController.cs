using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace GestionChantier.Controllers
{
    public abstract class InformationUtilisateurController : Controller
    {
        // GET: InformationUtilisateur
        public int GetUserID()
        {
#if DEBUG
            return 1190;
#endif
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;

            if (!claims.Any())
                return -2;

            int idEmployer = Convert.ToInt32(claims.ElementAt(9).Value);
            return idEmployer;
        }

        public string GetUserName()
        {
            int userID = GetUserID();

            if (userID == -2)
                return "Inconnu";
            else
            {
                using (HorizonContext horizonContext = new HorizonContext())
                {
                    return horizonContext.TabGroupe.Where(x => x.IdBottin == userID).Select(x => x.usager_nt).FirstOrDefault() ?? "Inexistant dans table groupe (Bottin ID : " + userID.ToString() + ")";
                }
            }
        }

        public string GetUserCompleteName()
        {
            int userID = GetUserID();

            if (userID == -2)
                return "Inconnu";
            else
            {
                using (HorizonContext horizonContext = new HorizonContext())
                {
                    return horizonContext.TabBottin.Where(x => x.ID == userID).Select(x => x.Prenom + " " + x.Nom).FirstOrDefault() ?? "Inexistant dans bottin (Bottin ID : " + userID.ToString() + ")";
                }
            }
        }

        public string GetUserPhoneNumber()
        {
            int userID = GetUserID();

            if (userID == -2)
                return "Inconnu";
            else
            {
                using (HorizonContext horizonContext = new HorizonContext())
                {
                    return horizonContext.TabBottin.Where(x => x.ID == userID).Select(x => x.MobileTelephone).FirstOrDefault() ?? "Inexistant dans bottin (Bottin ID : " + userID.ToString() + ")";
                }
            }
        }

        public string GetUserEmail()
        {
            int userID = GetUserID();

            if (userID == -2)
                return "Inconnu";
            else
            {
                using (HorizonContext horizonContext = new HorizonContext())
                {
                    return horizonContext.TabGroupe.Where(x => x.IdBottin == userID).Select(x => x.Courriel).FirstOrDefault() ?? "Inexistant dans table groupe (Bottin ID : " + userID.ToString() + ")";
                }
            }
        }

        public bool IsUserDirecteurProjet()
        {
            int usrId = GetUserID();
            using (HorizonContext contextHorizon = new HorizonContext())
            {
                return contextHorizon.TabGroupe.Where(x => x.IdBottin == usrId).Select(x => x.Est_DirecteurProjet).FirstOrDefault() ?? false;
            }
        }

        public bool IsUserContremaitre()
        {
            int usrId = GetUserID();
            using (HorizonContext contextHorizon = new HorizonContext())
            {
                return contextHorizon.TabGroupe.Where(x => x.IdBottin == usrId).Select(x => x.Est_Contremaitre).FirstOrDefault() ?? false;
            }
        }
        public bool IsUserChargeProjet()
        {
            int usrId = GetUserID();
            using (HorizonContext contextHorizon = new HorizonContext())
            {
                return contextHorizon.TabGroupe.Where(x => x.IdBottin == usrId).Select(x => x.Est_ChargeProjet).FirstOrDefault() ?? false;
            }
        }

        public bool IsAstAdmin()
        {
            int usrId = GetUserID();
            using (HorizonContext contextHorizon = new HorizonContext())
            {
                return contextHorizon.TabGroupe.Where(x => x.IdBottin == usrId).Select(x => x.AdminAST).FirstOrDefault() ?? false;
            }
        }
    }
}