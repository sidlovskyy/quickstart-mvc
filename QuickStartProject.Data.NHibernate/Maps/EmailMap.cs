using FluentNHibernate.Mapping;
using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Data.NHibernate.Maps
{
    public class EmailMap : ClassMap<Email>
    {
        public EmailMap()
        {
            Id(x => x.Id);
            Map(x => x.To).Column("ToAddress").Not.Nullable().Length(1000);
            Map(x => x.From).Column("FromAddress").Not.Nullable().Length(100);
            Map(x => x.Cc).Nullable().Length(100);
            Map(x => x.Subject).Not.Nullable().Length(500);
            Map(x => x.Body).Not.Nullable().Length(50000);
            Map(x => x.IsSent).Not.Nullable();
            Map(x => x.IsForceSend).Not.Nullable();
            Map(x => x.IsHtml).Not.Nullable();
            Map(x => x.Type).CustomType<int>().Not.Nullable();
            Map(x => x.SubmitTime).Not.Nullable();
            Map(x => x.SendTime).Nullable();
            Map(x => x.SendAttempt).Not.Nullable();
        }
    }
}