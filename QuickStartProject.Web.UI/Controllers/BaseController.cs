using System.Configuration;
using System.Web.Mvc;
using QuickStartProject.Web.UI.Security;

namespace QuickStartProject.Web.UI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected void ShowError(string message)
        {
            TempData[Constants.ERROR] = message;
        }

        protected void ShowInformation(string message)
        {
            TempData[Constants.INFO] = message;
        }

        protected string GetAppSettingsValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!string.IsNullOrEmpty(SimpleSessionPersister.Username))
            {
                filterContext.HttpContext.User = new CustomPrincipal(new CustomIdentity(SimpleSessionPersister.Username));
            }
            base.OnAuthorization(filterContext);
        }

        protected void MergeModelStateOnNextCall()
        {
            TempData[Constants.MODEL_STATE] = ModelState;
        }
    }
}