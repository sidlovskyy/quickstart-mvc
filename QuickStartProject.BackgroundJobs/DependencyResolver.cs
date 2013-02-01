using System;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using QuickStartProject.Data.NHibernate;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;
using QuickStartProject.Emailing;

namespace QuickStartProject.BackgroundJobs
{
    public static class DependencyResolver
    {
        private static readonly object Locker = new object();
        private static IContainer _container;

        public static void SetupDependencies()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => Fluently.Configure()
                                      .Database(
                                          MsSqlConfiguration.MsSql2008.ConnectionString(
                                              c => c.FromConnectionStringWithKey("DefaultConnection")))
                                      .Mappings(
                                          m =>
                                          m.FluentMappings.AddFromAssemblyOf<NHRepository<DomainEntity<int>, int>>())
                                      .BuildSessionFactory())
                .SingleInstance();

            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession()).InstancePerDependency();

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

            _container = builder.Build();
        }

        public static T Resolve<T>()
        {
            if (_container == null)
            {
                throw new InvalidOperationException("Dependencies were not regitered yet.");
            }

            T resolvedObj;
            lock (Locker)
            {
                resolvedObj = _container.Resolve<T>();
            }
            return resolvedObj;
        }
    }
}