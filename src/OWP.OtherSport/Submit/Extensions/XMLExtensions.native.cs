using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OWP.OtherSport.Submit.Extensions
{
    public static class XMLExtensions
    {
        public static string Serialize(object existObject)
        {
            var serializer = new XmlSerializer(existObject.GetType());
            var fileStream = new FileStream("file", FileMode.Create);
            var writer = new XmlTextWriter(fileStream, Encoding.Unicode);
            serializer.Serialize(writer, existObject);
            writer.Close();
            return writer.ToString();
        }
    }
}
