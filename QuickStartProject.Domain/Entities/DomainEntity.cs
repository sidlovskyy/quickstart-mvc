using System;

namespace QuickStartProject.Domain.Entities
{
    public class DomainEntity<TId>
    {
        public virtual TId Id { get; protected set; }

        public virtual bool IsNew
        {
            get { return Id.Equals(default(TId)); }
        }
    }

    public class LongIdDomainEntity : DomainEntity<long>
    {
    }

    public class IntIdDomainEntity : DomainEntity<int>
    {
    }

    public class GuidIdDomainEntity : DomainEntity<Guid>
    {
    }
}