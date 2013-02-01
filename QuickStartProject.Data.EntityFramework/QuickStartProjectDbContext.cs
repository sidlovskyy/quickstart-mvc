using System.Data.Entity;
using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Data.EntityFramework
{
	public class QuickStartProjectDbContext : DbContext
	{
        public QuickStartProjectDbContext() : this("DefaultConnection") { }

        public QuickStartProjectDbContext(string connectionStringOrName) : base(connectionStringOrName) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Email> Emails { get; set; }
        public DbSet<Email> Images { get; set; }
	}
}