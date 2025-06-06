using System.Net;
using System.Text;
using WebServerKIU.Http;


namespace WebServerKIU.WebServer;

public class FileHandler
{
    private readonly string _root;

    public FileHandler(string root)
    {
        _root = root;
    }

    public async Task Handle(HttpContext context)
    {
        var requestPath = context.Request.Url.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
        var path = Path.GetFullPath(Path.Combine(_root, requestPath));


        if (!path.StartsWith(Path.GetFullPath(_root)))
        {
            context.Response.StatusCode = "403 Forbidden";
            context.Response.ContentType = "text/html";
            context.Response.Content = LoadErrorPage();
            return;
        }

        if (File.Exists(path))
        {
            var contentType = GetContentType(path);
            if (contentType == null)
            {
                context.Response.StatusCode = "403 Forbidden";
                context.Response.ContentType = "text/html";
                context.Response.Content = LoadErrorPage();
                return;
            }

            context.Response.Content = await File.ReadAllBytesAsync(path);
            context.Response.ContentType = contentType;
        }
        else
        {
            // _logger.LogWarning("File not found: {FilePath}", path);
            context.Response.StatusCode = "404 Not Found";
            context.Response.ContentType = "text/html";
            context.Response.Content = LoadErrorPage();
        }
    }

    private string? GetContentType(string path)
    {
        return Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".html" => "text/html",
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".json" => "application/json",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            _ => null
        };
    }

    private byte[] LoadErrorPage()
    {
        string errorPagePath = Path.Combine(_root, "error.html");
        if (File.Exists(errorPagePath))
        {
            return File.ReadAllBytes(errorPagePath);
        }

        return Encoding.UTF8.GetBytes("<h1>Error</h1>");
    }
}