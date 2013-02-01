using System;
using System.Net.Http;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;
using QuickStartProject.Web.UI.Api.v1.Models;

namespace QuickStartProject.Web.UI.Api.v1
{
    public class ApplicationController : BaseApiController
    {
        public ApplicationController(IRepository<Application, Guid> applicationRepository)
            : base(applicationRepository)
        {
        }

        public HttpResponseMessage Get(string acc, string app)
        {
            Guid accId;
            Guid appId;
            if (Guid.TryParse(acc, out accId) && Guid.TryParse(app, out appId))
            {
                Application application = ApplicationRepository.GetById(appId);
                if ((application != null) && (application.Owner.Id == accId))
                {
                    var applicationModel = new ApplicationModel
                                               {
                                                   Name = application.Name,
                                                   Description = application.Description,
                                                   Level = application.LogLevel,
                                                   CreatedDate = application.CreatedDate,
                                               };
                    return Result(applicationModel);
                }
            }
            return NotFound("Account or it's application is not found");
        }
    }
}