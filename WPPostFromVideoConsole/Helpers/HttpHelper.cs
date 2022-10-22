namespace WPPostFromVideoConsole.Helpers;

public static class HttpHelper
{
    private static readonly HttpClient HttpClient = new();

    public static async void DownloadFileAsync(string uri
        , string outputPath)
    {
        if (!Uri.TryCreate(uri, UriKind.Absolute, out _))
            throw new InvalidOperationException("URI is invalid.");

        if (!File.Exists(outputPath))
            throw new FileNotFoundException("File not found."
                , nameof(outputPath));

        var fileBytes = await HttpClient.GetByteArrayAsync(uri);
        await File.WriteAllBytesAsync(outputPath, fileBytes);
    }
}