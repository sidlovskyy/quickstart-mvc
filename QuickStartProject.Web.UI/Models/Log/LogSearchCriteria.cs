using System.Collections.Generic;
using Logfox.Domain.Entities;
using System;

namespace Logfox.Web.UI.Models.Log
{
    public class LogSearchCriteria
    {
        public Guid? App { get; set; }
        public LogLevel? Level { get; set; }
        public string OS { get; set; }
        public string DeviceType { get; set; }
        public string DeviceId { get; set; }
        public DateTime? Before { get; set; }
        public DateTime? After { get; set; }
        public string Text { get; set; }

        public List<string> AvailableOperatingSystems { get; set; }
        public List<string> AvailableDeviceTypes { get; set; }
        public List<string> AvailableDeviceIds { get; set; }        
        public Dictionary<Guid, string> AvailableApplications { get; set; }
    }
}