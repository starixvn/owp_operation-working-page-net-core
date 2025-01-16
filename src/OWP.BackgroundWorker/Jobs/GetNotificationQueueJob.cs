using OWP.BackgroundWorker.Handler;
using ILogger = Fanex.Logging.ILogger;

namespace OWP.BackgroundWorker.Jobs;

[DisallowConcurrentExecution]
public class GetNotificationQueueJob(ILogger logger, ISender sender) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var underMaintenanceTime = true;
            if (underMaintenanceTime)
            {
                return;
            }

            await sender.Send(new NotificationQueueRequest());
        }
        catch (Exception e)
        {
            var message = $"Error in NotifyJob: {e.Message}.";
            await logger.ErrorAsync(message);
        }
    }
}
