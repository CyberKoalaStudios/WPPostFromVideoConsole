using System.Text.RegularExpressions;

namespace WPPostFromVideoConsole.Helpers;

public static class Formatter
{
    public static string StripHtml(string input)
    {
        return Regex.Replace(input, "<.*?>", string.Empty);
    }
    
    public static string Truncate(this string value, int maxChars)
    {
        return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
    }
}