namespace WPPostFromVideoConsole.Helpers;

public static class HttpHelper
{
    private static readonly HttpClient HttpClient = new HttpClient();

    public static async void DownloadFileAsync(string uri
        , string outputPath)
    {
        Uri uriResult;

        if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
            throw new InvalidOperationException("URI is invalid.");

        if (!File.Exists(outputPath))
            throw new FileNotFoundException("File not found."
                , nameof(outputPath));

        byte[] fileBytes = await HttpClient.GetByteArrayAsync(uri);
        File.WriteAllBytes(outputPath, fileBytes);
    }
}