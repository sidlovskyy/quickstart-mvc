using FluentNHibernate.Mapping;
using Logfox.Domain.Entities;

namespace Logfox.Data.NHibernate.Maps
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