using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using QuickStartProject.Data.NHibernate;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;
using QuickStartProject.Emailing;
using QuickStartProject.Web.UI.App_Start;

namespace QuickStartProject.Web.UI
{
    public class MvcApplication : HttpApplication
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
                                      .Database(
                                          MsSqlConfiguration.MsSql2008.ConnectionString(
                                              c => c.FromConnectionStringWithKey("DefaultConnection")))
                                      .Mappings(
                                          m =>
                                          m.FluentMappings.AddFromAssemblyOf<NHRepository<DomainEntity<int>, int>>())
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
            builder.RegisterType<IntIdNHRepository<Email>>().As<IRepository<Email, int>>();            

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
            const string userEmail = "QuickStartProject.dev@gmail.com";

            ISession dbSession = DependencyResolver.Current.GetService<ISession>();
            IRepository<User, Guid> userRepository = new GuidIdNHRepository<User>(dbSession);
            if (UserWithEmailExists(userRepository, userEmail))
            {
                return;
            }

            var user = new User(userEmail)
            {
                Name = "Igor Bublin",
                Company = "Mint.com",                               
                //real password is 123
                Password = "YzpfonnQJMAF0Y5V0YmuJmv+Lkmn9tcz3ZK5Z++k7CxbJGmsinpo/orpW3DHJeFdICm9xmx+JMVNpXZZKuxNeA==",
                Salt = "YZGZV+i9lI3ZVe5lTKBArilVYv2Iy6+V66KKpMFTO7YDMp5eX7NvpJa3vssWKiITs3oKOln0St8rk3dZTZwzyQ==",
            };
          
            userRepository.Save(user);
        }

        private static bool UserWithEmailExists(IRepository<User, Guid> userRepository, string userEmail)
        {
            return userRepository.GetOne(u => u.Email == userEmail) != null;
        }
    }
}