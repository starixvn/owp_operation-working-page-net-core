using OWP.BackgroundWorker.Handler;
using OWP.Services;
using ILogger = Fanex.Logging.ILogger;

namespace OWP.BackgroundWorker.Jobs;

[DisallowConcurrentExecution]
public class GetNotificationQueueJob(MaintenanceService maintenanceService, ILogger logger, ISender sender) : IJob
{

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var underMaintenanceTime = maintenanceService.IsUnderMaintenance();
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
