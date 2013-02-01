using Logfox.Domain.Entities;
using Logfox.Domain.Repository;
using Logfox.Emailing.Models;

namespace Logfox.Emailing
{
	public class PostalMailingService : PostalMailingServiceBase, IMailingService
	{
		public PostalMailingService(IRepository<Email, int> emailRepository) : base(emailRepository) { }

		public void SentContactUsEmail(ContactUsEmailModel model)
		{			
			SendEmail(model);
		}
	}
}
