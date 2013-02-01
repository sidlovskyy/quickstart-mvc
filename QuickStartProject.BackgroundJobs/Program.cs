using System.ServiceProcess;

namespace Logfox.BackgroundJobs
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] 
            { 
                new LogfoxService() 
            };
			ServiceBase.Run(ServicesToRun);
		}
	}
}
