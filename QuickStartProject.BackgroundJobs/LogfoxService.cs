using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using Logfox.BackgroundJobs.Jobs;
using Logfox.Common;

namespace Logfox.BackgroundJobs
{
	partial class LogfoxService : ServiceBase
	{
		private readonly List<JobBase> _jobs = new List<JobBase>();

		public LogfoxService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
#if DEBUG
			Debugger.Launch();            
#endif			
			Log.Info("Log fox service starting...");

			RegisterJobs();

			RegisterDependencies();

			StartJobs();

            Log.Info("Log fox service started.");
		}		

		private void RegisterJobs()
		{
			_jobs.Add(new ClearLogsJob());
			_jobs.Add(new EmailSenderJob());
		}

		private void RegisterDependencies()
		{			
			DependencyResolver.SetupDependencies();
		}

		private void StartJobs()
		{
			foreach (JobBase job in _jobs)
			{
				job.Start();
			}
		}

		protected override void OnStop()
		{
			Log.Info("Log fox service stopping.");

			StopJobs();

            Log.Info("Log fox service stopped.");
		}

		private void StopJobs()
		{
			foreach (JobBase job in _jobs)
			{
				job.Stop();
			}
		}
	}
}
