using System.Web.Mvc;
using QuickStartProject.Web.UI.Filters;

namespace QuickStartProject.Web.UI.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ModelStateMergeFilterAttribute());
            filters.Add(new SetUsernameViewModelAttribute());
        }
    }
}