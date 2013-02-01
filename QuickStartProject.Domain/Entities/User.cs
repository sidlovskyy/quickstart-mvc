using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QuickStartProject.Domain.Entities
{
    public class User : GuidIdDomainEntity
    {
        private ICollection<Application> _applications;
        private DateTime _createdDate;
        private string _email;

        public User()
        {
            _createdDate = DateTime.UtcNow;
        }

        public User(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }
            _email = email;
            _createdDate = DateTime.UtcNow;
        }

        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
            protected set { _createdDate = value; }
        }

        public virtual string Password { get; set; }
        public virtual string Salt { get; set; }
        public virtual string Name { get; set; }
        public virtual string Company { get; set; }
        public virtual int? RetensionPeriodInDays { get; set; }

        public virtual ICollection<Application> Applications
        {
            get { return _applications ?? (_applications = new Collection<Application>()); }
            protected set { _applications = value; }
        }

        public virtual bool IsOwnerOf(Application application)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }

            if (application.Owner == null)
            {
                throw new ArgumentException(string.Format("Application doesn't have owner. App id {0}", application.Id));
            }

            bool isOwner = application.Owner.Id == Id;
            return isOwner;
        }

        public virtual bool IsOwnerOf(LogEntry log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            if (log.Application == null)
            {
                throw new ArgumentException(string.Format("Log doesn't have application. Log id {0}", log.Id));
            }

            if (log.Application.Owner == null)
            {
                string msg = string.Format("Log application doesn't have owner. App id {0}, log id {1}",
                                           log.Application.Id, log.Id);
                throw new ArgumentException(msg);
            }

            bool isOwner = log.Application.Owner.Id == Id;
            return isOwner;
        }
    }
}