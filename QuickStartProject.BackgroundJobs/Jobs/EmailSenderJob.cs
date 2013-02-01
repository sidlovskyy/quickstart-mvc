using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using QuickStartProject.Common;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;

namespace QuickStartProject.BackgroundJobs.Jobs
{
    internal class EmailSenderJob : JobBase
    {
        //This array indicates minutes when email will be resended after previous failures.
        //It should start from 0.
        private readonly int[] _resendingIntervalsInMinutes = new[] {0, 5, 30, 60, 300, 60*60};
        private IRepository<Email, int> _emailRepository;

        protected override string Name
        {
            get { return "Email Sending Job"; }
        }

        protected override string CronSchedule
        {
            get { return GetAppConfigValue("QuickStartProject.EmailSendingJobSchedule"); }
        }

        public override void Execute()
        {
            ResolveDependencies();

            foreach (Email email in GetNotSentAndForcedEmails())
            {
                Log.Info(
                    "Processing email from '{0}' to '{1}' with subject '{2}'. Submited at {3}. Attempts made before {4}",
                    email.From,
                    email.To,
                    email.Subject,
                    email.SubmitTime,
                    email.SendAttempt);

                if (ShouldBeSend(email))
                {
                    SendEmail(email);
                }
            }
        }

        private void ResolveDependencies()
        {
            _emailRepository = DependencyResolver.Resolve<IRepository<Email, int>>();
        }

        private IEnumerable<Email> GetNotSentAndForcedEmails()
        {
            return _emailRepository.Query(email => !email.IsSent || email.IsForceSend).ToList();
        }

        private bool ShouldBeSend(Email email)
        {
            if (email.IsForceSend)
            {
                return true;
            }

            if (email.SendAttempt >= _resendingIntervalsInMinutes.Length)
            {
                return false;
            }

            return email.SubmitTime.AddMinutes(_resendingIntervalsInMinutes[email.SendAttempt]) < DateTime.UtcNow;
        }

        public void SendEmail(Email email)
        {
            Log.Info("Sending email from '{0}' to '{1}' with subject '{2}'.",
                     email.From,
                     email.To,
                     email.Subject);

            try
            {
                TrySendEmail(email);

                email.IsSent = true;
                email.SendTime = DateTime.UtcNow;

                Log.Info("Email from '{0}' to '{1}' with subject '{2}' was sent successfully with attempt #{3}",
                         email.From,
                         email.To,
                         email.Subject,
                         email.SendAttempt);
            }
            catch (SmtpException ex)
            {
                Log.Error("Email sending failed. Exception {0}", ex.ToString());
                email.IsSent = false;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("Email sending failed. Exception {0}", ex.ToString());
                email.IsSent = false;
            }
            finally
            {
                email.SendAttempt++;
                email.IsForceSend = false;
                _emailRepository.Save(email);
            }
        }

        private static void TrySendEmail(Email email)
        {
            var mailToSend = new MailMessage();
            mailToSend.To.Add(email.To);
            mailToSend.From = new MailAddress(email.From);
            mailToSend.Subject = email.Subject;
            mailToSend.Body = email.Body;
            mailToSend.IsBodyHtml = email.IsHtml;

            var smtp = new SmtpClient();
            smtp.Send(mailToSend);
        }
    }
}