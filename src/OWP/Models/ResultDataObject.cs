namespace OWP.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Newtonsoft.Json;

    public class ResultDataObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<string> Errors { get; } = new List<string>();

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic ExtraData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Data { get; set; }

        public bool IsSuccess => !Errors.Any() && Code == (int)ErrorCode.Ok;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public static ResultDataObject Ok => new ResultDataObject
        {
            Code = (int)ErrorCode.Ok
        };

        public void ThrowExceptionIfError()
        {
            if (IsSuccess)
            {
                return;
            }

            var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(Message)
            };
            throw new HttpResponseException(resp);
        }
    }
}