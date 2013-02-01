using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Emailing.Models
{
    public class ContactUsEmailModel : EmailModel
    {
        internal ContactUsEmailModel(string templateName) : base(templateName)
        {
        }

        public string SenderName { get; set; }

        public string Message { get; set; }

        public override EmailType Type
        {
            get { return EmailType.ContactUs; }
        }

        public static ContactUsEmailModel Create()
        {
            return new ContactUsEmailModel("ContactUsEmail");
        }
    }
}