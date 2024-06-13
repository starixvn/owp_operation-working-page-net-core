namespace OWP.WorkingShift.ViewLog.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using Constants;
    using OWP.WorkingShift.ViewLog.Constants;
    using Utils;

    public interface LogService
    {
        List<T> Parse<T>(string xml);

        string GetActionName(int actionId);
    }

    public class LogServiceImp : LogService
    {
        public List<T> Parse<T>(string xml)
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

        public string GetActionName(int actionId)
        {
            return EnumUtils<ActionId>.GetDescription(actionId);
        }
    }
}
