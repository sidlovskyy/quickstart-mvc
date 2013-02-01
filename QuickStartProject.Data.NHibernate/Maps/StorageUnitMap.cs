using FluentNHibernate.Mapping;
using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Data.NHibernate.Maps
{
    public class StorageUnitMap : ClassMap<StorageUnit>
    {
        public StorageUnitMap()
        {
            Id(x => x.Id);
            Map(x => x.Value).Not.Nullable();
        }
    }
}