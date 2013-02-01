namespace QuickStartProject.Domain.Entities
{
    public class Pricing : IntIdDomainEntity
    {
        private StorageUnit _storageUnit;
        private TimeUnit _timeUnit;
        private string _value;

        public Pricing()
        {
        }

        public Pricing(TimeUnit timeUnit, StorageUnit storageUnit, string value)
        {
            _timeUnit = timeUnit;
            _storageUnit = storageUnit;
            _value = value;
        }

        public virtual TimeUnit TimeUnit
        {
            get { return _timeUnit; }
            set { _timeUnit = value; }
        }

        public virtual StorageUnit StorageUnit
        {
            get { return _storageUnit; }
            set { _storageUnit = value; }
        }

        public virtual string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}