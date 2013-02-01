using FluentNHibernate.Mapping;
using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Data.NHibernate.Maps
{
    public class ImageMap : ClassMap<Image>
    {
        public ImageMap()
        {
            Id(x => x.Id);
            Map(x => x.ContentType).Not.Nullable().Length(30);
            Map(x => x.Content).Length(100000000).Not.Nullable();
        }
    }
}