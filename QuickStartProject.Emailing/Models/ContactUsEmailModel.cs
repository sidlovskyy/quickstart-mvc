using Logfox.Domain.Entities;

namespace Logfox.Emailing.Models
{
    public class ContactUsEmailModel : EmailModel
    {
		public static ContactUsEmailModel Create()
		{
			return new ContactUsEmailModel("ContactUsEmail");
		}

	    internal ContactUsEmailModel(string templateName) : base(templateName) { }

	    public string SenderName { get; set; }

	    public string Message { get; set; }

	    public override EmailType Type
	    {
		    get { return EmailType.ContactUs; }
	    }
    }
}
