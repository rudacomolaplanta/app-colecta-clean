using desafiocoaniquem.Services;
using Microsoft.AspNetCore.Mvc;

namespace desafiocoaniquem.Controllers
{
    public class ViewsController : Controller
    {
        public static IConfiguration? _config;
        private readonly IMailService _mailService;
        public ViewsController(IConfiguration iConfig, ILogger<CoaniquemController> logger, IMailService _MailService)
        {
            _config = iConfig;
            _mailService = _MailService;
        }

        //Rutas default configuradas en el program.cs
        public ActionResult Index()
        {
            Random r = new Random();
            ViewData.Add("random", r.Next(0, 2));

            if (GlobalConfiguration.IsColecta)
            {
                //Si es fecha colecta
                return View("ColectaIndex");
            }
            else
            {
                //Sino es fecha colecta
                return View();
            }
        }

        //[HttpGet("tarjetaprepagolosheroes")] public IActionResult tarjetaprepagolosheroes() { return View(); }
        [HttpGet("coaniquem/exito")] public ActionResult Exito(string? i, string? v) { ViewData["i"] = i; ViewData["v"] = v; return View(); }
        [HttpGet("coaniquem/error")] public ActionResult Error() { return View(); }

        //Vistas de Colecta
        [HttpGet("colecta")] public IActionResult ColectaIndex() { if (GlobalConfiguration.IsColecta) { return View(); } else { return RedirectToAction("Index"); } }
        [HttpGet("consulta")] public ActionResult ColectaConsulta() { if (GlobalConfiguration.IsColecta) { return View(); } else { return RedirectToAction("Index"); } }
        [HttpGet("alcancia/{*id}")] public ActionResult ColectaAlcancia(string id) { if (id is not null && GlobalConfiguration.IsColecta) { ViewBag.ID = id; return View(); } else { return RedirectToAction("Index"); } }
    }
}
