using System;
using System.Web.Mvc;
using QuickStartProject.Common;
using QuickStartProject.Emailing;
using QuickStartProject.Emailing.Models;
using QuickStartProject.Web.UI.Models.ContactUs;

namespace QuickStartProject.Web.UI.Controllers
{
    public class ContactUsController : BaseController
    {
        private readonly IMailingService _mailingService;

        public ContactUsController(IMailingService mailingService)
        {
            _mailingService = mailingService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new ContactUsViewModel());
        }

        [HttpPost]
        public ActionResult Index(ContactUsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                MergeModelState();
                return RedirectToAction("Index");
            }

            ContactUsEmailModel contactUs = ToContactUsEmailModel(model);

            try
            {
                _mailingService.SentContactUsEmail(contactUs);

                ShowInformation("Your message was sent successfully.");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Log.Error(
                    "Exception occured while sending contact use email from {0} (email: {1}) user. Subject {2}. Message {3}. Exception {4}",
                    model.SenderName,
                    model.SenderEmail,
                    model.Subject,
                    model.Message,
                    ex);

                ShowError("There was an error while sending your email. Please try again later.");
                return RedirectToAction("Index");
            }
        }

        private ContactUsEmailModel ToContactUsEmailModel(ContactUsViewModel model)
        {
            string contactUsEmail = GetAppSettingsValue("QuickStartProject.ContactUsEmail");
            string subject = string.Format("QuickStartProject Contact Us Email (User: '{0}') - {1}", model.SenderName,
                                           model.Subject);

            ContactUsEmailModel contactUs = ContactUsEmailModel.Create();
            contactUs.SenderName = model.SenderName;
            contactUs.From = model.SenderEmail;
            contactUs.To = contactUsEmail;
            contactUs.Subject = subject;
            contactUs.Message = model.Message;
            return contactUs;
        }
    }
}