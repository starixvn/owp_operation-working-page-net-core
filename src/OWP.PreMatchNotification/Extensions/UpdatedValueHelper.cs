namespace OWP.PreMatchNotification.Extensions
{
    using System.Text.RegularExpressions;

    public static class UpdatedValueHelper
    {
        private const string USER = "#<user>";

        public static string GetValue(string userName, string assignmentNote)
        {
            var newValue = string.IsNullOrEmpty(userName)
                            ? Regex.Replace(assignmentNote, USER, "", RegexOptions.IgnoreCase)
                            : Regex.Replace(assignmentNote, USER, $"#{userName}", RegexOptions.IgnoreCase);
            //newValue = Regex.Replace(newValue, "same", "", RegexOptions.IgnoreCase).Trim();
            return newValue;
        }
    }
}