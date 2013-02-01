using System;
using System.Linq;
using System.Web.Mvc;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;
using QuickStartProject.Web.UI.Extensions;

namespace QuickStartProject.Web.UI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRepository<LogEntry, long> _logRepository;

        public DashboardController(IRepository<LogEntry, long> logRepository)
        {
            _logRepository = logRepository;
        }

        [HttpGet]
        public JsonResult AllTimesLogs()
        {
            var results = _logRepository.All()
                .GroupBy(log => log.Level, (level, logs) => new {Level = level, Count = logs.Count()})
                .OrderBy(logStatistic => logStatistic.Level)
                .ToList()
                .Select(log => new {Level = log.Level.GetDescription(), log.Count});

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TodaysLogs()
        {
            var todayDate = DateTime.UtcNow.Date;
            var results = _logRepository.Query(log => log.CreatedDate >= todayDate)
                .GroupBy(log => log.Level, (level, logs) => new {Level = level, Count = logs.Count()})
                .OrderBy(logStatistic => logStatistic.Level)
                .ToList()
                .Select(log => new {Level = log.Level.GetDescription(), log.Count});

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //TODO: change to levels per date chart
        //TODO: add zooming
        //TODO: fix intervals
        public JsonResult LastPeriodLogs()
        {
            var period = DateTime.UtcNow.Date.AddDays(-7);
            var results = _logRepository.Query(log => log.CreatedDate >= period)
                .GroupBy(log => log.CreatedDate.Date, (date, logs) => new {LogDate = date, Count = logs.Count()})
                .OrderBy(logStatistic => logStatistic.LogDate)
                .ToList()
                .Select(logStatistic => new {Date = logStatistic.LogDate.ToString("yyyy-MM-dd"), logStatistic.Count});

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}