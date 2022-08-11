namespace OWP.Extensions;

public static class IntExtensions
{
    public static int? ConvertNullableInt(this string text)
    {
        int i;
        if (int.TryParse(text, out i)) return i;
        return null;
    }
}