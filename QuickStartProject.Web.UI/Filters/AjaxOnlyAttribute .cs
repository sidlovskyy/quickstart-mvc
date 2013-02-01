using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace QuickStartProject.Web.UI.Filters
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            var headers = request.Headers;
            if (!headers.Contains("X-Requested-With") ||
                headers.GetValues("X-Requested-With").FirstOrDefault() != "XMLHttpRequest")
            {
                actionContext.Response = request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}