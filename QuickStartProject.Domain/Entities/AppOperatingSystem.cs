using System.ComponentModel;

namespace QuickStartProject.Domain.Entities
{
    public enum AppOperatingSystem
    {
        Other = 0,
        Windows = 1,
        Linux = 2,
        MacOs = 3,
        Android = 4,
        iOS = 5,
        [Description("Windows Phone")] WindowsPhone = 6
    }
}