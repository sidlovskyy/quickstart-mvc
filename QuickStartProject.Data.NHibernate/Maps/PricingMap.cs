using FluentNHibernate.Mapping;
using Logfox.Domain.Entities;

namespace Logfox.Data.NHibernate.Maps
{
    public class PricingMap: ClassMap<Pricing>
    {
        public PricingMap()
        {
            Id(x => x.Id);
            Map(x => x.Value).Length(200).Not.Nullable();
            References(x => x.TimeUnit, "TimeUnitId").Not.Nullable();
            References(x => x.StorageUnit, "StorageUnitId").Not.Nullable();
        }
    }
}