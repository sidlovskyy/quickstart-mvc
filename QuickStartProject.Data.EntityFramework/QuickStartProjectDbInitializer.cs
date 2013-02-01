using System.Data.Entity;

namespace QuickStartProject.Data.EntityFramework
{
	public class WebScrapperDbInitializer : DropCreateDatabaseIfModelChanges<QuickStartProjectDbContext> {} //MigrateDatabaseToLatestVersion<QuickStartProject, Configuration> { }
}