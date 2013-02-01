namespace Logfox.Domain.Entities
{
    public class TimeUnit : IntIdDomainEntity
    {
        private string _value;

        public virtual string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public TimeUnit() { }

        public TimeUnit( string value)
        {
            _value = value;
        }
    }
}