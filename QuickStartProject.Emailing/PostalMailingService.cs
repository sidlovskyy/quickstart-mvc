using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;
using QuickStartProject.Emailing.Models;

namespace QuickStartProject.Emailing
{
    public class PostalMailingService : PostalMailingServiceBase, IMailingService
    {
        public PostalMailingService(IRepository<Email, int> emailRepository) : base(emailRepository)
        {
        }

        #region IMailingService Members

        public void SentContactUsEmail(ContactUsEmailModel model)
        {
            SendEmail(model);
        }

        #endregion
    }
}