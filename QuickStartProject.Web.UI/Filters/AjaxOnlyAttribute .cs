using System.Linq;
using System.Net;
using System.Net.Http;

namespace Logfox.Web.UI.Filters
{
    public class AjaxOnlyAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            var headers = request.Headers;
            if (!headers.Contains("X-Requested-With") || headers.GetValues("X-Requested-With").FirstOrDefault() != "XMLHttpRequest")
            {
                actionContext.Response = request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}