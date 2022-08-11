namespace OWP.Utils
{
    using System.Xml;
    using System.Xml.Serialization;
    public static class BuildLogXmlHelper
    {
        public static List<T> Deserialize<T>(string xml)
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