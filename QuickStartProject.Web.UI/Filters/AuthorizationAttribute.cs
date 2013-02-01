using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;

namespace QuickStartProject.Web.UI.Filters
{
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        public IRepository<User, Guid> UserRepository { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            string currentUserEmail = httpContext.User.Identity.Name;
            User currentUser = UserRepository.GetOne(r => r.Email == currentUserEmail);
            return currentUser != null || base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var routeData = new RouteValueDictionary
                                {
                                    {"action", "Logon"},
                                    {"controller", "Account"},
                                    {"returnUrl", filterContext.HttpContext.Request.Url}
                                };
            filterContext.Result = new RedirectToRouteResult(routeData);
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}