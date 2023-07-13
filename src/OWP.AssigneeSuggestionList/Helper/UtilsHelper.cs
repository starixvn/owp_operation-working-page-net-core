namespace OWP.AssigneeSuggestionList.Helper
{
    public static class UtilsHelper
    {
        public static DateTime SelectTime(DateTime src, DateTime dest)
        => src < dest
        ? src
        : dest;
    }
}