using System.Configuration;
using System.Web.Mvc;

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

        protected void MergeModelState()
        {
            TempData[Constants.MODEL_STATE] = ModelState;
        }
    }
}