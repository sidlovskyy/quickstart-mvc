using System;
using System.Web.Mvc;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;

namespace QuickStartProject.Web.UI.Filters
{
    public class BindCurrentUserToAttribute : ActionFilterAttribute
    {
        private readonly string _bindToVariable;
        public IRepository<User, Guid> UserRepository;

        public BindCurrentUserToAttribute()
            : this("currentUser")
        {
        }

        public BindCurrentUserToAttribute(string bindToVariable)
        {
            _bindToVariable = bindToVariable;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //TODO: temporary fix. IoC should resolve it automatically
            UserRepository = DependencyResolver.Current.GetService<IRepository<User, Guid>>();

            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                User currentUser = GetCurrentUser(filterContext);
                AddOrUpdateCurrentUserParam(filterContext, currentUser);
            }

            base.OnActionExecuting(filterContext);
        }

        private User GetCurrentUser(ActionExecutingContext filterContext)
        {
            string userEmail = filterContext.HttpContext.User.Identity.Name;
            User currentUser = UserRepository.GetOne(user => user.Email == userEmail);
            return currentUser;
        }

        private void AddOrUpdateCurrentUserParam(ActionExecutingContext filterContext, User currentUser)
        {
            if (filterContext.ActionParameters.ContainsKey(_bindToVariable))
            {
                filterContext.ActionParameters[_bindToVariable] = currentUser;
            }
            else
            {
                filterContext.ActionParameters.Add(_bindToVariable, currentUser);
            }
        }
    }
}