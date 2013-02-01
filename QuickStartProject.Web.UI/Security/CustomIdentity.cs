using System.Security.Principal;

namespace Logfox.Web.UI.Security
{
    public class CustomIdentity : IIdentity
    {
        public string Name { get; private set; }

        public CustomIdentity(string name)
        {
            Name = name;
        }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(Name); }
        }
    }
}