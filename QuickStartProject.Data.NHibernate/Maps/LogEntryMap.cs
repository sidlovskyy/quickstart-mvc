using FluentNHibernate.Mapping;
using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Data.NHibernate.Maps
{
    public class LogEntryMap : ClassMap<LogEntry>
    {
        public LogEntryMap()
        {
            Id(x => x.Id);
            Map(x => x.Level).CustomType<int>().Not.Nullable();
            Map(x => x.OS).Length(200).Nullable();
            Map(x => x.DeviceType).Length(200).Nullable();
            Map(x => x.DeviceId).Length(200).Nullable();
            Map(x => x.Message).Length(3000).Not.Nullable();
            References(x => x.Application, "ApplicationId").Not.Nullable();
            Map(x => x.CreatedDate).Not.Nullable();
        }
    }
}