using System;
using System.Linq;
using System.Web.Mvc;
using Logfox.Domain.Auxilary;
using Logfox.Domain.Entities;
using Logfox.Domain.Repository;
using Logfox.Web.UI.Filters;
using Logfox.Web.UI.Models.Application;

namespace Logfox.Web.UI.Controllers
{
    [Authorization]
    public class ApplicationController : BaseController
    {
        private const string ApplicationNotAvailableMsg = "Application is not found or not available.";

        private readonly IRepository<Application, Guid> _applicationRepository;

        public ApplicationController(IRepository<Application, Guid> applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        [HttpGet]
        [BindCurrentUserTo("currentUser")]
        public ActionResult Index(User currentUser, int page = 1)
        {
            var applications = _applicationRepository
                .Query(app => app.Owner.Id == currentUser.Id)
                .OrderBy(app => app.CreatedDate)
                .ToPagedList(page);

            return View(applications);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Application", new ApplicationViewModel());
        }

        [HttpPost]
        [BindCurrentUserTo("currentUser")]
        public ActionResult Create(User currentUser, ApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                MergeModelStateOnNextCall();
                return RedirectToAction("Create");
            }

            var app = new Application(currentUser, model.Name, model.OperatingSystem)
            {
                Description = model.Description, 
                LogLevel = model.LogLevel
            };

            _applicationRepository.Save(app);

            ShowInformation("Application created successfully.");
            return RedirectToAction("Index");
        }        

        [HttpGet]
        [BindCurrentUserTo("currentUser")]
        public ActionResult Edit(User currentUser, Guid id)
        {
            Application application;
            if (!TryGetApplicationFor(currentUser, id, out application))
            {
                ShowError(ApplicationNotAvailableMsg);
                return RedirectToAction("Index");
            }

            return View("Application", new ApplicationViewModel(application));
        }        

        [HttpPost]
        [BindCurrentUserTo("currentUser")]
        public ActionResult Edit(User currentUser, ApplicationViewModel model)
        {
            if(!ModelState.IsValid)
            {
                MergeModelStateOnNextCall();
                return RedirectToAction("Edit");
            }

            Application application;
            if (!TryGetApplicationFor(currentUser, model.Id, out application))
            {
                ShowError(ApplicationNotAvailableMsg);
                return RedirectToAction("Index");
            }

            application.Name = model.Name;
            application.OperatingSystem = model.OperatingSystem;
            application.Description = model.Description;
            application.LogLevel = model.LogLevel;
            _applicationRepository.Save(application);

            ShowInformation("Application updated successfully.");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [BindCurrentUserTo("currentUser")]
        public ActionResult DeleteConfirm(User currentUser, Guid id)
        {
            Application application;
            if (!TryGetApplicationFor(currentUser, id, out application))
            {
                ShowError(ApplicationNotAvailableMsg);
                return RedirectToAction("Index");
            }

            return View(application);
        }

        [HttpPost]
        [BindCurrentUserTo("currentUser")]
        public ActionResult Delete(User currentUser, Guid id)
        {
            Application application;
            if (!TryGetApplicationFor(currentUser, id, out application))
            {
                ShowError(ApplicationNotAvailableMsg);
                return RedirectToAction("Index");
            }

            _applicationRepository.Delete(id);
            ShowInformation("Application was deleted successfully");
            return RedirectToAction("Index");
        }

        private bool TryGetApplicationFor(User user, Guid appId, out Application application)
        {
            application = _applicationRepository.GetById(appId);
            bool isApplicationFound = (application != null) && user.IsOwnerOf(application);
            if (!isApplicationFound)
            {
                application = null;
            }
            return isApplicationFound;
        }
    }
}
