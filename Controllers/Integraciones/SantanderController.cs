using Microsoft.AspNetCore.Mvc;

namespace desafiocoaniquem.Controllers.Integraciones
{
    public class SantanderController : Controller
    {
        // GET: Santander
        //private static readonly ILog Log = LogManager.GetLogger(typeof(CoaniquemController));

        public ActionResult Init(string m, string r, string c, string e) //r: Referencia, ch: Canal, m:Monto
        {
            return Redirect(GlobalConfiguration.SantanderByPassURL + "/Init?m=" + m + "&r=" + r + "&c=" + c + "&e=" + e);
        }

        public ActionResult Response(string TX)
        {
            return Redirect(GlobalConfiguration.SantanderByPassURL + "/Response?TX=" + TX);
        }

        public ActionResult Error(string TX)
        {
            return Redirect(GlobalConfiguration.SantanderByPassURL + "/Error?TX=" + TX);
        }
    }
}