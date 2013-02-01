using System.Data.Entity;
using QuickStartProject.Data.EntityFramework.Migrations;

namespace QuickStartProject.Data.EntityFramework
{
    public class QuickStartProjectDbInitializer : MigrateDatabaseToLatestVersion<QuickStartProjectDbContext, Configuration> { }
}