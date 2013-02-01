using DataAnnotationsExtensions.ClientValidation;
using QuickStartProject.Web.UI.App_Start;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof (RegisterClientValidationExtensions), "Start")]

namespace QuickStartProject.Web.UI.App_Start
{
    public static class RegisterClientValidationExtensions
    {
        public static void Start()
        {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();
        }
    }
}