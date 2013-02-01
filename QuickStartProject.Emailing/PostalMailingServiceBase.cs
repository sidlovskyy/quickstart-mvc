using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Postal;
using QuickStartProject.Common;
using QuickStartProject.Domain.Repository;
using QuickStartProject.Emailing.Models;
using Email = QuickStartProject.Domain.Entities.Email;

namespace QuickStartProject.Emailing
{
    public class PostalMailingServiceBase
    {
        private readonly IRepository<Email, int> _emailRepository;

        public PostalMailingServiceBase(IRepository<Email, int> emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public void SendEmail(EmailModel email)
        {
            string viewsPath = GetEmailsTemplatePath();

            var engines = new ViewEngineCollection();
            engines.Add(new FileSystemRazorViewEngine(viewsPath));

            Log.Info("Creating email from {0} to {1} with subject {2}", email.From, email.From, email.Subject);
            var service = new EmailService(engines);
            MailMessage data = service.CreateMailMessage(email);

            var emailContent = new Email(email.To, email.From, email.CC, data.Subject, data.Body, data.IsBodyHtml,
                                         email.Type);

            _emailRepository.Save(emailContent);
        }

        private static string GetEmailsTemplatePath()
        {
            // Get the path to the directory containing views
            string viewsPath = ConfigurationManager.AppSettings["QuickStartProject.EmailTemplatesPath"];

#if DEBUG
            if (string.IsNullOrWhiteSpace(viewsPath) && HttpContext.Current != null)
            {
                string webApplicationPath = HttpContext.Current.Server.MapPath("~");
                string currentDirectory = Path.GetDirectoryName(webApplicationPath);
                string solutionRootDirectory = GetDirectoryInPath(currentDirectory, "QuickStart");
                viewsPath = Path.Combine(solutionRootDirectory, @"QuickStartProject.Emailing\Templates");
            }
#endif

            return viewsPath;
        }

        private static string GetDirectoryInPath(string path, string directoryName)
        {
            if (!path.EndsWith(@"\"))
            {
                path = path + @"\";
            }

            string directoryEntrance = string.Format(@"\{0}\", directoryName);
            int indexOfDirectory = path.IndexOf(directoryEntrance, StringComparison.OrdinalIgnoreCase);
            if (indexOfDirectory < 0)
            {
                return string.Empty;
            }
            string result = path.Substring(0, indexOfDirectory + directoryEntrance.Length);
            return result;
        }
    }
}