using System;
using System.ComponentModel.DataAnnotations;
using QuickStartProject.Domain.Entities;

namespace QuickStartProject.Web.UI.Models.Application
{
    public class ApplicationViewModel
    {
        public ApplicationViewModel()
        {
        }

        public ApplicationViewModel(Domain.Entities.Application application)
        {
            Id = application.Id;
            Name = application.Name;
            Description = application.Description;
            LogLevel = application.LogLevel;
            CreatedDate = application.CreatedDate;
            OperatingSystem = application.OperatingSystem;
            IsUpdating = !application.IsNew;
        }

        [Display(Name = "Application id")]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Application name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Log level")]
        public LogLevel LogLevel { get; set; }

        [Display(Name = "Created")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Operating system")]
        public AppOperatingSystem OperatingSystem { get; set; }

        public bool IsUpdating { get; set; }
    }
}