using System;
using System.Linq;
using Logfox.Common;
using Logfox.Domain.Entities;
using Logfox.Domain.Repository;

namespace Logfox.BackgroundJobs.Jobs
{	
    internal class ClearLogsJob : JobBase
    {
        private IRepository<User, Guid> _userRepository;
        private IRepository<LogEntry, long> _logRepository;        

        //NOTE: to make this job run really fast we may create stored procedure for it.
        public override void Execute()
        {
	        ResolveDependencies();

            var users = _userRepository.All();
            foreach (var user in users)
            {
                int? retentionPeriodInDays = user.RetensionPeriodInDays;
                if (retentionPeriodInDays != null)
                {
                    ProcessApplicationsForUser(user, retentionPeriodInDays.Value);
                }
            }
        }

	    private void ResolveDependencies()
	    {
			_userRepository = DependencyResolver.Resolve<IRepository<User, Guid>>();
			_logRepository = DependencyResolver.Resolve<IRepository<LogEntry, long>>();
	    }

	    private void ProcessApplicationsForUser(User user, int retentionPeriodInDays)
        {
            try
            {
                Log.Info("Start processing log deletion for user {0} (id: {1})", user.Name, user.Id);
                
                TryProcessApplicationForUser(user, retentionPeriodInDays);
                
                Log.Info("End processing log deletion for user {0} (id: {1})", user.Name, user.Id);
            }
            catch(Exception ex)
            {
                Log.Error(
                    string.Format("Exception while processing log deletion for user {0} (id: {1})", user.Name, user.Id),
                    ex);
            }
        }

        private void TryProcessApplicationForUser(User user, int retentionPeriodInDays)
        {
            DateTime clearBeforeDate = DateTime.Now.AddDays(-retentionPeriodInDays);
            foreach (var application in user.Applications)
            {
                ProcessLogsForApplication(application, clearBeforeDate);
            }
        }

        private void ProcessLogsForApplication(Application app, DateTime clearBeforeDate)
        {
            try
            {
                Log.Info("Start processing log deletion for app id {0} (user id: {1})", app.Id, app.Owner.Id);
                
                TryProcessLogsForApplication(app, clearBeforeDate);

                Log.Info("End processing log deletion for app id {0} (user id: {1})", app.Id, app.Owner.Id);
            }
            catch (Exception ex)
            {
                Log.Error(
                    string.Format("Exception while processing log deletion for app id {0} (user id: {1})", app.Id, app.Owner.Id),
                    ex);
            }
        }

        private void TryProcessLogsForApplication(Application application, DateTime clearBeforeDate)
        {
            long[] lodIdsToRemove = application.Logs
                .Where(log => log.CreatedDate < clearBeforeDate)
                .Select(log => log.Id)
                .ToArray();

            Log.Info("{0} logs are being deleted.", lodIdsToRemove.Length);

            foreach (long logId in lodIdsToRemove)
            {
                _logRepository.Delete(logId);
            }
        }

        protected override string Name
        {
            get { return "Clear Logs"; }
        }

        protected override string CronSchedule
        {
            get { return GetAppConfigValue("LogFox.ClearlogsJobSchedule"); }
        }
    }
}
