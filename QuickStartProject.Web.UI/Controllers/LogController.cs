using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using QuickStartProject.Domain.Auxilary;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;
using QuickStartProject.Web.UI.Extensions;
using QuickStartProject.Web.UI.Filters;
using QuickStartProject.Web.UI.Models.Log;

namespace QuickStartProject.Web.UI.Controllers
{
    [Authorization]
    public class LogController : BaseController
    {
        private const int LogsPerPage = 20;

        private readonly IRepository<Application, Guid> _applicationsRepository;
        private readonly IRepository<LogEntry, long> _logRepository;

        public LogController(
            IRepository<LogEntry, long> logRepository,
            IRepository<Application, Guid> applicationsRepository)
        {
            _logRepository = logRepository;
            _applicationsRepository = applicationsRepository;
        }

        [HttpGet]
        [BindCurrentUserTo("currentUser")]
        public ActionResult Index(User currentUser, LogSearchCriteria criteria, string orderby = "date", int page = 1)
        {
            IQueryable<LogEntry> allUserLogs = _logRepository.Query(log => log.Application.Owner.Id == currentUser.Id);
            IQueryable<LogEntry> logs = GetLogsForCriteria(allUserLogs, criteria);
            logs = ApplyOrdering(logs, orderby);
            PagedList<LogEntry> pagedLogs = logs.ToPagedList(page, LogsPerPage);

            var resultModel = new IndexViewModel();
            resultModel.Logs = pagedLogs;
            resultModel.SearchCriteria = criteria ?? new LogSearchCriteria();
            resultModel.SearchCriteria.AvailableOperatingSystems = GetOperatingSystemsAvailable(allUserLogs);
            resultModel.SearchCriteria.AvailableDeviceTypes = GetDeviceTypesAvailable(allUserLogs);
            resultModel.SearchCriteria.AvailableDeviceIds = GetDeviceIdsAvailable(allUserLogs);
            resultModel.SearchCriteria.AvailableApplications = GetAvailableApplications(currentUser);
            return View(resultModel);
        }

        [HttpGet]
        [BindCurrentUserTo("currentUser")]
        public ActionResult Export(User currentUser, LogSearchCriteria criteria, string orderby = "date")
        {
            IQueryable<LogEntry> logs = _logRepository.Query(log => log.Application.Owner.Id == currentUser.Id);
            logs = GetLogsForCriteria(logs, criteria);
            logs = ApplyOrdering(logs, orderby);

            var exportBody = new StringBuilder();
            exportBody.AppendLine("Application,Level,Message,OS,DeviceType,DeviceId,Posted");
            foreach (LogEntry log in logs)
            {
                string logLine = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                                               log.Application.Name,
                                               log.Level.GetDescription(),
                                               log.Message,
                                               log.OS,
                                               log.DeviceType,
                                               log.DeviceId,
                                               log.CreatedDate.ToString("mm/dd/yyyy hh:MM:ss tt"));

                exportBody.AppendLine(logLine);
            }

            byte[] exportBytes = GetBytes(exportBody.ToString());
            return File(exportBytes, "text/csv", "Export.csv");
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length*sizeof (char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        [HttpGet]
        [BindCurrentUserTo("currentUser")]
        public ActionResult Details(User currentUser, long id)
        {
            LogEntry log = _logRepository.GetById(id);
            if ((log == null) || !currentUser.IsOwnerOf(log))
            {
                ShowError("Log is not found or not available.");
                return RedirectToAction("Index");
            }

            return View(log);
        }

        private IQueryable<LogEntry> GetLogsForCriteria(IQueryable<LogEntry> logs, LogSearchCriteria criteria)
        {
            if (criteria == null)
            {
                return logs;
            }

            if (criteria.App != null)
            {
                Guid appId = criteria.App.Value;
                logs = logs.Where(log => log.Application.Id == appId);
            }

            if (criteria.Level != null)
            {
                LogLevel level = criteria.Level.Value;
                logs = logs.Where(log => log.Level >= level);
            }

            if (!string.IsNullOrWhiteSpace(criteria.OS))
            {
                string os = criteria.OS;
                logs = logs.Where(log => log.OS == os);
            }

            if (!string.IsNullOrWhiteSpace(criteria.DeviceType))
            {
                string deviceType = criteria.DeviceType;
                logs = logs.Where(log => log.DeviceType == deviceType);
            }

            if (!string.IsNullOrWhiteSpace(criteria.DeviceId))
            {
                string deviceId = criteria.DeviceId;
                logs = logs.Where(log => log.DeviceId == deviceId);
            }

            if (criteria.Before != null)
            {
                DateTime beforeDate = criteria.Before.Value.Date.AddDays(1).AddMilliseconds(-1);
                logs = logs.Where(log => log.CreatedDate <= beforeDate);
            }

            if (criteria.After != null)
            {
                DateTime afterDate = criteria.After.Value.Date;
                logs = logs.Where(log => log.CreatedDate >= afterDate);
            }

            if (!string.IsNullOrWhiteSpace(criteria.Text))
            {
                string searchText = criteria.Text;
                logs = logs.Where(log => log.Message.Contains(searchText));
            }

            return logs;
        }

        //TODO: this is not used for now
        private IQueryable<LogEntry> ApplyOrdering(IQueryable<LogEntry> logs, string orderby)
        {
            switch (orderby)
            {
                case "created":
                    logs = logs.OrderBy(log => log.CreatedDate);
                    break;
                case "app":
                    logs = logs.OrderBy(log => log.Application.Name);
                    break;
                case "level":
                    logs = logs.OrderBy(log => log.Level);
                    break;
                default:
                    logs = logs.OrderByDescending(log => log.CreatedDate);
                    break;
            }

            return logs;
        }

        private static List<string> GetOperatingSystemsAvailable(IQueryable<LogEntry> logs)
        {
            List<string> operatingSystems = logs
                .Select(log => log.OS)
                .Distinct()
                .ToList();

            operatingSystems.Sort();
            return operatingSystems;
        }

        private static List<string> GetDeviceTypesAvailable(IQueryable<LogEntry> allLogs)
        {
            List<string> deviceTypes = allLogs
                .Select(log => log.DeviceType)
                .Distinct()
                .ToList();

            deviceTypes.Sort();
            return deviceTypes;
        }

        private static List<string> GetDeviceIdsAvailable(IQueryable<LogEntry> allLogs)
        {
            List<string> deviceIds = allLogs
                .Select(log => log.DeviceId)
                .Distinct()
                .ToList();

            deviceIds.Sort();
            return deviceIds;
        }

        private Dictionary<Guid, string> GetAvailableApplications(User user)
        {
            Dictionary<Guid, string> applications = _applicationsRepository
                .Query(app => app.Owner.Id == user.Id)
                .ToDictionary(app => app.Id, app => app.Name);

            return applications;
        }
    }
}