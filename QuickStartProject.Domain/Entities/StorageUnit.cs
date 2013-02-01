namespace QuickStartProject.Domain.Entities
{
    public class StorageUnit : IntIdDomainEntity
    {
        private string _value;

        public StorageUnit()
        {
        }

        public StorageUnit(string value)
        {
            _value = value;
        }

        public virtual string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}