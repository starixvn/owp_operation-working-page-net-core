namespace OWP.PreMatchAssignment.Misc
{
    using Models;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Serialization;

    public interface LogFactory
    {
        string BuildColumn(string action, string data);

        string GetLogType(string action, string data);
    }

    public class LogFactoryImp : LogFactory
    {
        private const int hdp = 1;

        public string BuildColumn(string action, string data)
        {
            switch (action)
            {
                case LogActionConst.UpdateAssignment:
                    return BuildUpdateAssignment(data);

                case LogActionConst.UpdateRemark:
                    var remarkLog = Parse<UpdateRemarkLogModel>(data).First();
                    return remarkLog.Remark;

                case LogActionConst.ChangeStartTime:
                    if (string.IsNullOrEmpty(data))
                    {
                        return string.Empty;
                    }

                    var startTimeLog = Parse<UpdateStartTimeLogModel>(data).First();
                    return $"Date: {startTimeLog.Date:MM/dd/yyyy}, Start Time: {startTimeLog.StartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)}";

                default:
                    return string.Empty;
            }
        }

        public string GetLogType(string action, string data)
        {
            if (!action.Equals(LogActionConst.UpdateAssignment))
            {
                return action;
            }

            var assignmentLog = Parse<UpdateAssignmentLogModel>(data).First();

            if (assignmentLog.AssignmentType == 2)
            {
                return LogActionConst.UpdateOtherAssignment;
            }

            return LogActionConst.UpdateAssignment;
        }

        private string BuildUpdateAssignment(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            var assignmentLog = Parse<UpdateAssignmentLogModel>(data).First();
            var isTakeOver = assignmentLog.IsTakeOver ? " T.Over" : string.Empty;
            var type = assignmentLog.Bettype == hdp ? $"HDP{isTakeOver}" : $"O/U{isTakeOver}";
            return $"{type}: #{assignmentLog.Username}";
        }

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
    }
}