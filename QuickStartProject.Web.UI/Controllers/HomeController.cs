using System.Web.Mvc;

namespace Logfox.Web.UI.Controllers
{
	public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
