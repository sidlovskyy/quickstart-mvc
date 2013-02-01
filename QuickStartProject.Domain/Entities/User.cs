using System;

namespace QuickStartProject.Domain.Entities
{
    public class User : GuidIdDomainEntity
    {        
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
    }
}