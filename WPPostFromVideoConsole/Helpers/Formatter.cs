using System.Text.RegularExpressions;

namespace WPPostFromVideoConsole.Helpers;

public class Formatter
{
    public static string StripHtml(string input)
    {
        return Regex.Replace(input, "<.*?>", string.Empty);
    }
}