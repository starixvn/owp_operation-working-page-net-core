using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace OWP.MessageManagement.Models;
public class MessageModel
{
    public int MessageId { get; set; }

    public bool IsViewed { get; set; }

    public string MessageContent { get; set; }

    [JsonConverter(typeof(DateTimeWithoutGmtConverter))]
    public DateTime CreatedDate { get; set; }

    public bool IsImportant { get; set; }

    public int MessageType { get; set; }

    public int SystemMessageType { get; set; }
}

public class DateTimeWithoutGmtConverter : IsoDateTimeConverter
{
    public DateTimeWithoutGmtConverter()
    {
        DateTimeFormat = "yyyy-MM-dd HH:mm:ss.FFF";
    }
}