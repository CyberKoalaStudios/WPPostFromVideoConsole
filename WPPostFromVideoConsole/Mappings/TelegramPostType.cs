namespace WPPostFromVideoConsole.Mappings;

public enum TelegramPostMode
{
    InstantView,
    InlineButton
}

public static class TelegramPostType
{
    public static readonly Dictionary<byte, TelegramPostMode> TelegramPostMap = new()
    {
        { 0, TelegramPostMode.InstantView },
        { 1, TelegramPostMode.InlineButton }
    };
}