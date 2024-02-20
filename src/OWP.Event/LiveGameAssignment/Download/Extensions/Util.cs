namespace OWP.Event.LiveGameAssignment.Download.Extensions;

public static class Util
{
    public static void Iter<T>(this IEnumerable<T> source, Action<T> iterator)
    {
        foreach (var e in source)
        {
            iterator(e);
        }
    }
}
