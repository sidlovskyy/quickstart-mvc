namespace QuickStartProject.Domain.Entities
{
    public class Image : IntIdDomainEntity
    {
        public virtual byte[] Content { get; set; }
        public virtual string ContentType { get; set; }
    }
}