using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Service;

namespace Website.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [Route("")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [Route("resources/icon/{name}")]
        [HttpGet]
        public ActionResult IconOf(string name)
        {
            var pkId = PokemonGoApiHack.GetPokemonId(name);
            if (pkId < 0)
            {
                return HttpNotFound();
            }
            var file = Server.MapPath(
                $"/Content/icons/{pkId.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0')}.png");
            if (!System.IO.File.Exists(file))
            {
                return HttpNotFound();
            }
            return File(file, "image/png");
        }
    }
}
