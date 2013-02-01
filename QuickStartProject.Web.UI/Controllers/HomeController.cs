using System.Web.Mvc;

namespace QuickStartProject.Web.UI.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}