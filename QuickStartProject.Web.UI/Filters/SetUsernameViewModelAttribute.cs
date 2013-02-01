using System;
using System.Web.Mvc;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;

namespace QuickStartProject.Web.UI.Filters
{
    public class SetUsernameViewModelAttribute : ActionFilterAttribute
    {
        public IRepository<User, Guid> UserRepository { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            //TODO: temporary fix. IoC should resolve it automatically
            UserRepository = DependencyResolver.Current.GetService<IRepository<User, Guid>>();

            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null)
            {
                string currentUserEmail = filterContext.HttpContext.User.Identity.Name;
                User currentUser = UserRepository.GetOne(r => r.Email == currentUserEmail);
                if (currentUser != null)
                {
                    viewResult.ViewBag.Username = currentUser.Name;
                }
            }
        }
    }
}