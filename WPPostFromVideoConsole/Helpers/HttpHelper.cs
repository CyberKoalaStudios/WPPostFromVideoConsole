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
    
    public static async Task DownloadImageAsync(string directoryPath, string fileName, Uri uri)
    {
        using var httpClient = new HttpClient();
        
        var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
        var fileExtension = Path.GetExtension(uriWithoutQuery);
        
        var path = Path.Combine(directoryPath, $"{fileName}{fileExtension}");
        Directory.CreateDirectory(directoryPath);
        
        var imageBytes = await httpClient.GetByteArrayAsync(uri);
        await File.WriteAllBytesAsync(path, imageBytes);
    }
    
}