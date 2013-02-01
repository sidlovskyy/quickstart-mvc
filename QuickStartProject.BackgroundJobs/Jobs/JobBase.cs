using System;
using System.Configuration;
using Logfox.Common;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Logfox.BackgroundJobs.Jobs
{	
    internal abstract class JobBase : IJob
    {
		public abstract void Execute();

		protected virtual void OnStart() { }        
		protected virtual void OnStop() { }

        protected abstract string Name { get; }
        protected abstract string CronSchedule { get; }

        public void Start()
        {
            Log.Info("{0} job starting...", Name);
            try
            {
                OnStart();

                ScheduleCronJob();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Exception while {0} job executing.", Name), ex);                
                throw;
            }            
        }

        private void ScheduleCronJob()
        {
			string cronSchedule = CronSchedule;
			if (string.IsNullOrWhiteSpace(cronSchedule))
			{
				Log.Warn("{0} job cron schedule is not found. Job will not be started.", Name);
				return;
			}

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler();
            IJobDetail jobDetail = new JobDetailImpl(Name, GetType());
            ITrigger trigger = new CronTriggerImpl(Name + "Trigger", "Default", CronSchedule);
            scheduler.ScheduleJob(jobDetail, trigger);

			scheduler.Start();

			Log.Info("{0} job started.", Name);
        }

        public void Execute(IJobExecutionContext context)
        {
            Log.Info("{0} job executing...", Name);
            try
            {
                Execute();

                DateTimeOffset? nextRunnigTime = context.NextFireTimeUtc;
                if (nextRunnigTime != null)
                {
                    Log.Info("{0} job executed. Next fire time {1}.", Name, nextRunnigTime.Value.ToLocalTime());
                }
                else
                {
                    Log.Info("{0} job executed. Job will not run more.");
                }
            }
            catch(Exception ex)
            {
                //we prevent failing all jobs here
                Log.Error(string.Format("Exception while {0} job executing.", Name), ex);
            }            
        }

        public void Stop()
        {
            Log.Info("{0} job stopping...", Name);
            try
            {
                OnStop();
            }
            catch (Exception ex)
            {
                //we prevent failing all jobs stopping here
                Log.Error(string.Format("Exception while {0} job stopping.", Name), ex);
            }
            Log.Info("{0} job stopped.", Name);
        }

        protected string GetAppConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        protected string GetRequiredAppConfigValue(string key)
        {
            string value = GetAppConfigValue(key);
            if(value == null)
            {
                throw new ConfigurationErrorsException(string.Format("{0} configuration item is not found.", key));
            }
            return value;
        }
    }
}
