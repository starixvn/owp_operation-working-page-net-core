namespace OWP.OSLeagueSetting.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using Models;
    using OWP.Extensions;
    using AutoMapper;

    public static class LogBuilder
    {
        public static IList<LogDisplayModel> Build(IList<UserLogModel> userLog, IMapper mapper)
        {
            var logDisplayModel = new List<LogDisplayModel>();
            foreach (var log in userLog)
            {
                var productBeforeChange = !string.IsNullOrEmpty(log.DataOld) ? Deserialize<LogModelDeserialize>(log.DataOld).FirstOrDefault() : new LogModelDeserialize();
                var productAfterChange = !string.IsNullOrEmpty(log.Data) ? Deserialize<LogModelDeserialize>(log.Data).FirstOrDefault() : new LogModelDeserialize();
                logDisplayModel.Add(new LogDisplayModel
                {
                    OldValue = mapper.Map<OldDataLeagueSetting>(productBeforeChange),
                    NewValue = mapper.Map<NewDataLeagueSetting>(productAfterChange),
                    Action = log.Action,
                    ModifyDate = log.ModifiedDate.ToGmt8().ToString("MM/dd/yyyy hh:mm tt"),
                    ModifyBy = log.ModifiedBy
                });
            }
            return logDisplayModel;
        }
        private static List<T> Deserialize<T>(string xml)
        {
            var result = new List<T>();
            var serializer = new XmlSerializer(typeof(T));
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var rootElement = xmlDoc.ChildNodes[0];
            foreach (XmlElement childElement in rootElement)
            {
                using (var sr = new StringReader(childElement.OuterXml))
                {
                    result.Add((T)serializer.Deserialize(sr));
                }
            }

            return result;
        }
    }
}