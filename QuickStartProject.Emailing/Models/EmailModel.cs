using System.Configuration;
using System.Net.Configuration;
using Logfox.Domain.Entities;
using Email = Postal.Email;

namespace Logfox.Emailing.Models
{
	public abstract class EmailModel : Email
	{
		private string _from;

		internal EmailModel(string templateName) : base(templateName) { }
		
		public string From
		{
			get
			{
				if (_from != null)
				{
					return _from;
				}

				var smtpSection = (SmtpSection) ConfigurationManager.GetSection("system.net/mailSettings/smtp");
				return smtpSection.From;
			}
			set { _from = value; }
		}

		public string To { get; set; }

		public string Subject { get; set; }

		public string CC { get; set; }

		public abstract EmailType Type { get; }
	}
}
