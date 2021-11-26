using OWP.AutoAssignment.Enum;

namespace OWP.AutoAssignment.Models.PublicApi
{
    public class StatusRequestModel
    {
        public string RequestId { get; set; }

        public StatusRequest Status { get; set; }
    }
}