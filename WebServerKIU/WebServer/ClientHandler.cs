using System.Net.Sockets;
using System.Text;
using WebServerKIU.Http;
using WebServerKIU.WebServer;


namespace WebServerKIU.WebServer;

public class ClientHandler
{
    private readonly TcpClient _client;
    private readonly FileHandler _handler;

    public ClientHandler(TcpClient client, FileHandler handler)
    {
        _client = client;
        _handler = handler;
    }

    public async Task HandleAsync()
    {
        using var stream = _client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var writer = new BinaryWriter(stream, Encoding.UTF8);

        var requestBuilder = new StringBuilder();
        string? line;
        while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
        {
            requestBuilder.AppendLine(line);
        }

        var request = HttpRequest.Parse(requestBuilder.ToString());
        var context = new HttpContext(request);

    
// validation to be added
       await _handler.Handle(context);
        context.Response.WriteTo(stream);
        _client.Close();
    }
}