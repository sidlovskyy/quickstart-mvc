using Logfox.Domain.Auxilary;
using Logfox.Domain.Entities;

namespace Logfox.Web.UI.Models.Log
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            SearchCriteria = new LogSearchCriteria();
        }

        public PagedList<LogEntry> Logs { get; set; }
        public LogSearchCriteria SearchCriteria { get; set; }
    }
}