namespace OWP.AssigneeSuggestionList.Helper
{
    public static class UtilsHelper
    {
        private static Random gen = new Random();

        private static DateTime InitialDate = new DateTime(1993, 7, 26);

        private static DateTime MaxDate = new DateTime(2000, 7, 26);

        public static DateTime RandomDate()
        {
            var range = (MaxDate - InitialDate).Days;
            return InitialDate.AddDays(gen.Next(range));
        }
    }
}