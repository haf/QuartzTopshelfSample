using System;
using System.Threading;
using Common.Logging;
using Quartz;
using Quartz.Impl;
using Topshelf;
using log4net.Config;

namespace ConsoleApplication1
{
	class Program
	{
		private IScheduler _Sched;
		private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(typeof (Program));
		

		static void Main(string[] args)
		{
			XmlConfigurator.Configure();

			try
			{
				Run();
			}
			catch (Exception e)
			{
				LogManager.GetLogger(typeof(Program)).Error(e);
			}
		}

		private static void Run()
		{
			HostFactory.Run(x =>
			{
				x.Service<Program>(s =>
				{
					s.ConstructUsing(name => new Program());
					s.WhenStarted(p => p.Schedule());
					s.WhenPaused(p => p.Pause());
					s.WhenContinued(p => p.Continue());
					s.WhenStopped(p => p.Stop());
				});
				x.RunAsLocalService();

				x.SetDescription("My awesome service that checks new items in a database!");
				x.SetDisplayName("Logibit MailerJobService");
				x.SetServiceName("MailerJobService");
			});
		}

		private void Continue()
		{
			_Sched.ResumeAll();
		}

		private void Pause()
		{
			_Sched.Standby();
		}

		private void Stop()
		{
			ILog log = LogManager.GetLogger(typeof(Program));
			// shut down the scheduler
			log.Info("------- Shutting Down ---------------------");
			_Sched.Shutdown(true);
			log.Info("------- Shutdown Complete -----------------");
		}

		private void Schedule()
		{
			ILog log = LogManager.GetLogger(typeof(Program));

			log.Info("------- Initializing ----------------------");

			// First we must get a reference to a schedule
			ISchedulerFactory sf = new StdSchedulerFactory();
			_Sched = sf.GetScheduler();

			log.Info("------- Initialization Complete -----------");


			// computer a time that is on the next round minute
			DateTimeOffset runTime = DateBuilder.EvenSecondDate(DateTime.UtcNow);

			log.Info("------- Scheduling Job  -------------------");

			// define the job and tie it to our HelloJob class
			IJobDetail job = JobBuilder.Create<MailerJob>()
				.WithIdentity("job1", "group1")
				.Build();

			// Trigger the job to run on the next round minute
			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("trigger1", "group1")
				.StartAt(runTime)
				.Build();

			// Tell quartz to schedule the job using our trigger
			_Sched.ScheduleJob(job, trigger);
			log.Info(string.Format("{0} will run at: {1}", job.Key, runTime.ToString("r")));

			// Start up the scheduler (nothing can actually run until the 
			// scheduler has been started)
			_Sched.Start();
			log.Info("------- Started Scheduler -----------------");

			// wait long enough so that the scheduler as an opportunity to 
			// run the job!
			log.Info("------- Waiting 65 seconds... -------------");
		}
	}

	class MailerJob : IJob
	{
		private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(typeof (MailerJob));
		
		public void Execute(IJobExecutionContext context)
		{
			ILog log = LogManager.GetLogger(typeof(MailerJob));
			_Logger.Info("Nu jobbar jag hårt!");
			log.Info("Nu jobbar jag hårt!");
		}
	}
}
