using System.Threading;

namespace OWP.BackgroundWorker.Scheduler;

public class OWPScheduler(ISchedulerFactory schedulerFactory, IJobFactory jobFactory) : IHostedService
{
    private IScheduler Scheduler { get; set; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await CreateScheduler(cancellationToken);
        await RunJobSchedulerAsync(cancellationToken);
        await StartScheduler(cancellationToken);
    }

    private async Task StartScheduler(CancellationToken cancellationToken)
    {
        await Scheduler.Start(cancellationToken);
    }

    private async Task RunJobSchedulerAsync(CancellationToken cancellationToken)
    {
        await AddJobToSchedulerAsync(
            new JobInfo(typeof(GetNotificationQueueJob), CronExpressionJob.GET_NOTIFICATION_QUEUE), cancellationToken);

    }

    private async Task AddJobToSchedulerAsync(JobInfo jobInfo, CancellationToken cancellationToken)
    {
        var jobDetail = CreateJob(jobInfo);

        var trigger = CreateTrigger(jobInfo);

        await Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
    }

    private static IJobDetail CreateJob(JobInfo jobInfo)
        => JobBuilder
            .Create(jobInfo.Type)
            .WithIdentity($"sync{jobInfo.Type.Name}", $"group{jobInfo.Type.Name}")
            .Build();

    private static ITrigger CreateTrigger(JobInfo jobInfo)
        => TriggerBuilder
            .Create()
            .WithIdentity($"trigger{jobInfo.Type.Name}", $"group{jobInfo.Type.Name}")
            .WithDescription(jobInfo.Type.Name)
            .WithCronSchedule(jobInfo.CronExpression)
            .Build();

    private async Task CreateScheduler(CancellationToken cancellationToken)
    {
        Scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        Scheduler.JobFactory = jobFactory;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Scheduler?.Shutdown(cancellationToken)!;
    }
}