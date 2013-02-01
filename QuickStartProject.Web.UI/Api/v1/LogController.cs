using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using Logfox.Domain.Entities;
using Logfox.Domain.Repository;
using Logfox.Web.UI.Api.v1.Models;

namespace Logfox.Web.UI.Api.v1
{
    [KnownType(typeof(LogEntryModel))]
    public class LogController : BaseApiController
    {
        private readonly IRepository<LogEntry, long> _logRepository;        

        public LogController(
            IRepository<LogEntry, long> logRepository, 
            IRepository<Application, Guid> applicationRepository)
            :base(applicationRepository)
        {
            _logRepository = logRepository;            
        }

        //url for test
        //http://localhost:2324/api/v1/acc/EBFE8E99-F404-4D6E-87DF-B16B5AFC04FF/app/EBFE8E99-F404-4D6E-87DF-B16B5AFC04FE/logs/11
        public HttpResponseMessage Get(string acc, string app, long id)
        {
            if (!AccountAndApplicationCodesValid(acc, app))
            {
                return NotFound("Account or it's application is not found");
            }

            var logEntry = _logRepository.GetById(id);
            if(logEntry == null)
            {
                return NotFound(string.Format("Log with id {0} is not found", id));
            }

            var log = new LogEntryModel
            {
                CreatedDate = logEntry.CreatedDate,
                DeviceId = logEntry.DeviceId,
                DeviceType = logEntry.DeviceType,
                Level = logEntry.Level,
                Message = logEntry.Message,
                OS = logEntry.OS
            };

            return Result(log);
        }

        public HttpResponseMessage Post(string acc, string app, [FromBody] LogEntryModel log)
        {
            if (!AccountAndApplicationCodesValid(acc, app))
            {
                return NotFound("Account or it's application is not found");
            }

            if(log == null)
            {
                return BadRequest("Log is empty");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           

            var appId = new Guid(app);
            Application application = ApplicationRepository.GetById(appId);
            
            if(application.LogLevel > log.Level)
            {
                return BadRequest("The log was not saved as it's level is lower then configured for application.");
            }
            
            var logEntry = new LogEntry(application, log.Level, log.Message, log.OS, log.DeviceType, log.DeviceId);
            logEntry = _logRepository.Save(logEntry);

            string locationUrl = Url.Route(null, new { acc, app, id = logEntry.Id });
            var location = new Uri(Request.RequestUri, locationUrl);
            return Result(location);
        }
    }
}
