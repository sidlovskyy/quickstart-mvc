using System.Security.Principal;

namespace QuickStartProject.Web.UI.Security
{
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name)
        {
            Name = name;
        }

        #region IIdentity Members

        public string Name { get; private set; }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(Name); }
        }

        #endregion
    }
}