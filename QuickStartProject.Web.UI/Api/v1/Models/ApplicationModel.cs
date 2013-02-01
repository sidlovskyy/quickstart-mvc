using System;
using System.Runtime.Serialization;
using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Web.UI.Api.v1.Models
{
    [DataContract(Name = "Application")]
    public class ApplicationModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public LogLevel Level { get; set; }

        [DataMember]
        public virtual DateTime CreatedDate { get; set; }

        [DataMember]
        public virtual AppOperatingSystem OperatingSystem { get; set; }
    }
}