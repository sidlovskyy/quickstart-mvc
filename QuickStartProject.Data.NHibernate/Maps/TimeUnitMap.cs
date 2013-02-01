using FluentNHibernate.Mapping;
using Logfox.Domain.Entities;

namespace Logfox.Data.NHibernate.Maps
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