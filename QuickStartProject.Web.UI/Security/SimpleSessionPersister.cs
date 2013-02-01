using System.Web;

namespace QuickStartProject.Web.UI.Security
{
    public static class SimpleSessionPersister
    {
        private const string UsernameSessionVar = "username";

        public static string Username
        {
            get
            {
                if (HttpContext.Current == null) return string.Empty;
                if (HttpContext.Current.Session[UsernameSessionVar] != null)
                {
                    return HttpContext.Current.Session[UsernameSessionVar] as string;
                }
                return null;
            }
            set { HttpContext.Current.Session[UsernameSessionVar] = value; }
        }
    }
}