using QuickStartProject.Emailing.Models;

namespace QuickStartProject.Emailing
{
    public interface IMailingService
    {
        void SentContactUsEmail(ContactUsEmailModel model);
    }
}