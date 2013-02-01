using FluentNHibernate.Mapping;
using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Data.NHibernate.Maps
{
    public class TimeUnitMap : ClassMap<TimeUnit>
    {
        public TimeUnitMap()
        {
            Id(x => x.Id);
            Map(x => x.Value).Not.Nullable();
        }
    }
}