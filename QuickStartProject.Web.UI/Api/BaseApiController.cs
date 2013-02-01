using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;

namespace QuickStartProject.Web.UI.Api
{
    public class BaseApiController : ApiController
    {
        protected readonly IRepository<Application, Guid> ApplicationRepository;

        public BaseApiController(IRepository<Application, Guid> applicationRepository)
        {
            ApplicationRepository = applicationRepository;
        }

        protected HttpResponseMessage Result<T>(T value)
        {
            return Request.CreateResponse(HttpStatusCode.OK, value);
        }

        protected HttpResponseMessage Result<T>(T value, Uri location)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, value);
            response.Headers.Location = location;
            return response;
        }

        protected HttpResponseMessage Result(Uri location)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Location = location;
            return response;
        }

        protected HttpResponseMessage Error(HttpStatusCode httpCode, string message)
        {
            var error = new HttpError(message);
            return Request.CreateResponse(httpCode, error);
        }

        protected HttpResponseMessage Error(HttpStatusCode httpCode, object value)
        {
            return Request.CreateResponse(httpCode, value);
        }

        protected HttpResponseMessage NotFound(string message = null)
        {
            var error = new HttpError(message ?? "Resource not found");
            return Request.CreateResponse(HttpStatusCode.NotFound, error);
        }

        protected HttpResponseMessage BadRequest()
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        protected HttpResponseMessage BadRequest(string message)
        {
            var error = new HttpError(message);
            return Request.CreateResponse(HttpStatusCode.BadRequest, error);
        }

        protected HttpResponseMessage BadRequest(object value)
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest, value);
        }

        protected bool AccountAndApplicationCodesValid(string accIdString, string appIdString)
        {
            bool isValid = false;
            Guid accId;
            Guid appId;
            if (Guid.TryParse(accIdString, out accId) && Guid.TryParse(appIdString, out appId))
            {
                Application application = ApplicationRepository.GetById(appId);
                isValid = (application != null) && (application.Owner.Id == accId);
            }
            return isValid;
        }
    }
}