using QuickStartProject.Domain.Auxilary;
using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Web.UI.Models.Log
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