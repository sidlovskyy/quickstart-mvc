using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Logfox.Data.NHibernate;
using Logfox.Domain.Entities;
using Logfox.Domain.Repository;
using Logfox.Emailing;
using Logfox.Web.UI.App_Start;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Logfox.Web.UI
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

			RouteConfig.RegisterRoutes(RouteTable.Routes);			

			BundleConfig.RegisterBundles(BundleTable.Bundles);

			ResolveDependencies();
		}

		private void ResolveDependencies()
		{
			var builder = new ContainerBuilder();            

			// Register ISessionFactory as Singleton 
			builder.Register(x => Fluently.Configure()
				.Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHRepository<DomainEntity<int>, int>>())
				.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))			
				.BuildSessionFactory())
				.SingleInstance();

			// Register ISession as instance per web request
			builder.Register(x => x.Resolve<ISessionFactory>().OpenSession())
				.InstancePerHttpRequest();

			// Register all controllers
			builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Register API controllers using assembly scanning.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

			//Register repositories
            builder.RegisterType<IntIdNHRepository<Image>>().As<IRepository<Image, int>>();
            builder.RegisterType<GuidIdNHRepository<User>>().As<IRepository<User, Guid>>();            
            builder.RegisterType<LongIdNHRepository<LogEntry>>().As<IRepository<LogEntry, long>>();
            builder.RegisterType<GuidIdNHRepository<Application>>().As<IRepository<Application, Guid>>();
			builder.RegisterType<IntIdNHRepository<Email>>().As<IRepository<Email, int>>();

            builder.RegisterType<IntIdNHRepository<Pricing>>().As<IRepository<Pricing, int>>();
            builder.RegisterType<IntIdNHRepository<TimeUnit>>().As<IRepository<TimeUnit, int>>();
            builder.RegisterType<IntIdNHRepository<StorageUnit>>().As<IRepository<StorageUnit, int>>();

			builder.RegisterType<PostalMailingService>().As<IMailingService>();

			//builder.RegisterType<ExtensibleActionInvoker>().As<IActionInvoker>();
			//builder.RegisterType<SetUsernameViewModelAttribute>().As<IActionFilter>();

			//for filter support
			builder.RegisterFilterProvider();

		    IContainer container = builder.Build();
			// override default dependency resolver to use Autofac
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Set the dependency resolver implementation.
            var configuration = GlobalConfiguration.Configuration;
            var resolver = new AutofacWebApiDependencyResolver(container);
            configuration.DependencyResolver = resolver;

			//fill default database values
			FillDefaultDatbaseData();
		}

		private void FillDefaultDatbaseData()
		{
			//username
			const string userEmail = "logfox.dev@gmail.com";

			ISession dbSession = DependencyResolver.Current.GetService<ISession>();
			IRepository<User, Guid> userRepository = new GuidIdNHRepository<User>(dbSession);
			if(UserWithEmailExists(userRepository, userEmail))
			{
				return;
			}

			var user = new User(userEmail)
			{
				Name = "Igor Bublin",
				Company = "Mint.com",
				RetensionPeriodInDays = 3,
				//real password is 123
				Password = "YzpfonnQJMAF0Y5V0YmuJmv+Lkmn9tcz3ZK5Z++k7CxbJGmsinpo/orpW3DHJeFdICm9xmx+JMVNpXZZKuxNeA==",
				Salt = "YZGZV+i9lI3ZVe5lTKBArilVYv2Iy6+V66KKpMFTO7YDMp5eX7NvpJa3vssWKiITs3oKOln0St8rk3dZTZwzyQ==",				
			};

			var application = new Application(user, "Angry birds", AppOperatingSystem.Android)
			{
				Description = 
				"Angry Birds is a puzzle video game developed by Finnish computer game developer Rovio Entertainment. " + 
				"Inspired primarily by a sketch of stylized wingless birds, the game was first released for Apple's iOS in December 2009. " + 
				"Since that time, over 12 million copies of the game have been purchased from Apple's App Store, " + 
				"which has prompted the company to design versions for other touchscreen-based smartphones, " + 
				"such as those using the Android operating system, among others.",

				LogLevel = LogLevel.Info
			};
			user.Applications.Add(application);

			application.Logs.Add(new LogEntry(application, LogLevel.Warning, "Application started", "Android 2.1", "Nexus 7", null));
			application.Logs.Add(new LogEntry(application, LogLevel.Debug, "Application started", "Android 2.1", "Nexus 7", null));
			application.Logs.Add(new LogEntry(application, LogLevel.Info, "This is tests message from Android.", "Android 2.1", "Nexus 7", null));
			application.Logs.Add(new LogEntry(application, LogLevel.Error, "Application finished", "Windows Phone 7", "HTC Mozart", "device 1"));
			application.Logs.Add(new LogEntry(application, LogLevel.Fatal, "Application started", "Windows Phone 7", "HTC Mozart", "device 5"));
			application.Logs.Add(new LogEntry(application, LogLevel.Warning, "Application started", "Windows Phone 7", "HTC Mozart", "device 1"));
			application.Logs.Add(new LogEntry(application, LogLevel.Warning, "Application finished.", "Android 2.1", "Nexus 7", null));
			application.Logs.Add(new LogEntry(application, LogLevel.Info, "Application started", "Android 2.1", "Nexus 7", null));
			application.Logs.Add(new LogEntry(application, LogLevel.Error, "Application finished", "Windows Phone 7", "HTC Mozart", "device 5"));			
			application.Logs.Add(new LogEntry(
				application, 
				LogLevel.Info, 
				"Exception occured: org.omg.CORBA.MARSHAL: com.ibm.ws.pmi.server.DataDescriptor; IllegalAccessException  minor code: 4942F23E  completed", 
				"Android 2.1", 
				"Nexus 7", 
				null));

			userRepository.Save(user);

            // Inserting pricing info.
            var timeUnits = new[] 
            { 
                new TimeUnit("60"),
                new TimeUnit("120"),
                new TimeUnit("220"),
                new TimeUnit("440"),
                new TimeUnit("880") 
            };
            IRepository<TimeUnit, int> timeUnitRepository = new IntIdNHRepository<TimeUnit>(dbSession);
		    foreach (var timeUnit in timeUnits)
		    {
		        timeUnitRepository.Save(timeUnit);
		    }

            var storageUnits = new[] 
            { 
                new StorageUnit("10"),
                new StorageUnit("30"),
                new StorageUnit("60"),
                new StorageUnit("120"),
                new StorageUnit("240") 
            };
            IRepository<StorageUnit, int> storageUnitRepository = new IntIdNHRepository<StorageUnit>(dbSession);
            foreach (var storageUnit in storageUnits)
            {
                storageUnitRepository.Save(storageUnit);
            }

		    var priceItems = new[]
		    {
		        new Pricing(timeUnits[0], storageUnits[0], "100$"),
                new Pricing(timeUnits[1], storageUnits[0], "110$"),
                new Pricing(timeUnits[2], storageUnits[0], "120$"),
                new Pricing(timeUnits[3], storageUnits[0], "130$"),
                new Pricing(timeUnits[4], storageUnits[0], "140$"),

                new Pricing(timeUnits[0], storageUnits[1], "200$"),
                new Pricing(timeUnits[1], storageUnits[1], "210$"),
                new Pricing(timeUnits[2], storageUnits[1], "220$"),
                new Pricing(timeUnits[3], storageUnits[1], "230$"),
                new Pricing(timeUnits[4], storageUnits[1], "240$"),

                new Pricing(timeUnits[0], storageUnits[2], "300$"),
                new Pricing(timeUnits[1], storageUnits[2], "310$"),
                new Pricing(timeUnits[2], storageUnits[2], "320$"),
                new Pricing(timeUnits[3], storageUnits[2], "330$"),
                new Pricing(timeUnits[4], storageUnits[2], "340$"),

                new Pricing(timeUnits[0], storageUnits[3], "400$"),
                new Pricing(timeUnits[1], storageUnits[3], "410$"),
                new Pricing(timeUnits[2], storageUnits[3], "420$"),
                new Pricing(timeUnits[3], storageUnits[3], "430$"),
                new Pricing(timeUnits[4], storageUnits[3], "440$"),

                new Pricing(timeUnits[0], storageUnits[4], "500$"),
                new Pricing(timeUnits[1], storageUnits[4], "510$"),
                new Pricing(timeUnits[2], storageUnits[4], "520$"),
                new Pricing(timeUnits[3], storageUnits[4], "530$"),
                new Pricing(timeUnits[4], storageUnits[4], "540$"),
		    };
            IRepository<Pricing, int> priceItemRepository = new IntIdNHRepository<Pricing>(dbSession);
            foreach (var priceItem in priceItems)
            {
                priceItemRepository.Save(priceItem);
            }
		}

		private static bool UserWithEmailExists(IRepository<User, Guid> userRepository, string userEmail)
		{
			return userRepository.GetOne(u => u.Email == userEmail) != null;
		}
	}
}