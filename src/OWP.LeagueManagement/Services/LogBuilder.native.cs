namespace OWP.LeagueManagement.Service
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using Models;
    using OWP.Extensions;
    using OWP.Utils;

    public static class LogBuilder
    {
        public static IList<LogDisplayModel> Build(IList<UserLogModel> userLog)
        {
            var logDisplayModel = new List<LogDisplayModel>();
            foreach (var log in userLog)
            {
                var productBeforeChange = !string.IsNullOrEmpty(log.DataOld) ? BuildLogXmlHelper.Deserialize<LogModelDeserialize>(log.DataOld).FirstOrDefault() : new LogModelDeserialize();
                var productAfterChange = !string.IsNullOrEmpty(log.Data) ? BuildLogXmlHelper.Deserialize<LogModelDeserialize>(log.Data).FirstOrDefault() : new LogModelDeserialize();
                logDisplayModel.Add(new LogDisplayModel
                {
                    Product = productBeforeChange.Product ?? productAfterChange.Product,
                    Action = log.Action,
                    OldValue = productBeforeChange.Value,
                    NewValue = productAfterChange.Value,
                    ModifyDate = log.ModifiedDate.ToGmt8().ToString("MM/dd/yyyy hh:mm tt"),
                    ModifyBy = log.ModifiedBy
                });
            }
            return logDisplayModel;
        }
    }

    [XmlRoot(ElementName = "z")]
    public class LogModelDeserialize
    {
        [XmlAttribute("p")]
        public string Product { get; set; }

        [XmlAttribute("vl")]
        public string Value { get; set; }
    }
}