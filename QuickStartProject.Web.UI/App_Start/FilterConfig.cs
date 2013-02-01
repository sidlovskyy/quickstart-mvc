using System.Web.Mvc;
using Logfox.Web.UI.Filters;

namespace Logfox.Web.UI.App_Start
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