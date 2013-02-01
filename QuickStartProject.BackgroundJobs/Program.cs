using System.ServiceProcess;

namespace QuickStartProject.BackgroundJobs
{
    internal static class Program
    {
        /// <summary>
        /// 	The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
                                {
                                    new QuickStartProjectService()
                                };
            ServiceBase.Run(ServicesToRun);
        }
    }
}