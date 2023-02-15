namespace OWP.AutoAssignment.Models.PublicApi;

using Enum;

public class StatusRequestModel
{
    public string RequestId { get; set; }

    public StatusRequest Status { get; set; }
}