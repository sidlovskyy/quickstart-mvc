using FluentNHibernate.Mapping;
using Logfox.Domain.Entities;

namespace Logfox.Data.NHibernate.Maps
{
    public class ApplicationMap : ClassMap<Application>
    {
        public ApplicationMap()
        {
            Id(x => x.Id);
            Map(x => x.LogLevel).CustomType<int>().Not.Nullable();
            Map(x => x.Name).Length(200).Not.Nullable();
            Map(x => x.Description).Length(2000).Nullable();
            Map(x => x.CreatedDate).Not.Nullable();
            //TODO: investigate why it's not working        
            //Map(x => x.OwnerId).Not.Nullable();
            Map(x => x.OperatingSystem).CustomType<int>().Not.Nullable();
            References(x => x.Owner, "OwnerId").Not.Nullable();
            HasMany(x => x.Logs).Inverse().KeyColumn("ApplicationId").Cascade.All();
        }
    }
}
