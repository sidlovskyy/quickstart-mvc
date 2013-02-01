using System;

namespace QuickStartProject.Domain.Entities
{
    public class LogEntry : LongIdDomainEntity
    {
        private Application _application;
        private DateTime _createdDate;
        private string _deviceId;
        private string _deviceType;
        private LogLevel _level;
        private string _message;
        private string _os;

        protected LogEntry()
        {
            _level = LogLevel.Error;
        }

        public LogEntry(Application app, LogLevel level, string message, string os, string deviceType, string deviceId)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            _level = level;
            _message = message;
            _os = os;
            _deviceType = deviceType;
            _deviceId = deviceId;
            _application = app;
            _createdDate = DateTime.UtcNow;
        }

        public virtual LogLevel Level
        {
            get { return _level; }
            protected set { _level = value; }
        }

        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
            protected set { _createdDate = value; }
        }

        public virtual Application Application
        {
            get { return _application; }
            protected set { _application = value; }
        }

        public virtual string Message
        {
            get { return _message; }
            protected set { _message = value; }
        }

        public virtual string OS
        {
            get { return _os; }
            protected set { _os = value; }
        }

        public virtual string DeviceType
        {
            get { return _deviceType; }
            protected set { _deviceType = value; }
        }

        public virtual string DeviceId
        {
            get { return _deviceId; }
            protected set { _deviceId = value; }
        }
    }
}