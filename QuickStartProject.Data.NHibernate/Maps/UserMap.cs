using FluentNHibernate.Mapping;
using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Data.NHibernate.Maps
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Nullable().Length(100);
            Map(x => x.Email).UniqueKey("Email").Not.Nullable().Length(200);
            Map(x => x.Password).Not.Nullable().Length(120);
            Map(x => x.Salt).Not.Nullable().Length(120);
            Map(x => x.Company).Not.Nullable().Length(200);
            Map(x => x.RetensionPeriodInDays).Nullable();
            HasMany(x => x.Applications).Inverse().KeyColumn("OwnerId").Cascade.All();
        }
    }
}