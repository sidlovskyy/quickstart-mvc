using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Logfox.Domain.Entities;

namespace Logfox.Web.UI.Api.v1.Models
{
    [DataContract(Name = "Log")]
    public class LogEntryModel
    {
        public LogEntryModel()
        {
            Level = LogLevel.Error;
        }

        [Required]
        [DataMember(IsRequired=true)] 
        public LogLevel Level { get; set; }

        [Required]
        [DataMember(IsRequired=true)] 
        public string Message { get; set; }

        [DataMember]
        public string OS { get; set; }

        [DataMember]
        public string DeviceType { get; set; }

        [DataMember]
        public string DeviceId { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }
    }
}
