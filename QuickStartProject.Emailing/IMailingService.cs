using Logfox.Emailing.Models;

namespace Logfox.Emailing
{
    public interface IMailingService
    {
	    void SentContactUsEmail(ContactUsEmailModel model);
    }
}
