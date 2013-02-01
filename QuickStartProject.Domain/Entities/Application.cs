using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Logfox.Domain.Entities
{
    public class Application : GuidIdDomainEntity
    {
        private string _name;
        private User _owner;
        private DateTime _createdDate;
        private LogLevel _logLevel;
        //private Guid _ownerId;
        private AppOperatingSystem _operatingSystem;
	    private ICollection<LogEntry> _logs;

	    protected Application() {}

        public Application(User owner, string name, AppOperatingSystem operatingSystem)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            _owner = owner;
            //_ownerId = owner.Id;
            _name = name;
            _operatingSystem = operatingSystem;
            _logLevel = LogLevel.Error;            
            _createdDate = DateTime.UtcNow;
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Description { get; set; }

        public virtual LogLevel LogLevel
        {
            get { return _logLevel; }
            set { _logLevel = value; }
        }

        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
            protected set { _createdDate = value; }
        }

        //TODO: investigate why it's not working
        /*public virtual Guid OwnerId
        {
            get { return _ownerId; }
            protected set { _ownerId = value; }
        }*/

        public virtual User Owner
        {
            get { return _owner; }
            protected set { _owner = value; }
        }

        public virtual AppOperatingSystem OperatingSystem
        {
            get { return _operatingSystem; }
            set { _operatingSystem = value; }
        }

        public virtual ICollection<LogEntry> Logs
        {
	        get { return _logs ?? (_logs = new Collection<LogEntry>()); }
	        protected set { _logs = value; }
        }
    }
}
