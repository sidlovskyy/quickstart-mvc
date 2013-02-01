using System.Security.Principal;

namespace QuickStartProject.Web.UI.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public CustomPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        #region IPrincipal Members

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return true;
        }

        #endregion
    }
}