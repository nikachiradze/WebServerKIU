using System.Text;

namespace WebServerKIU.Http;

public class HttpResponse
{
    public string StatusCode { get; set; } = "200 OK";
    public string ContentType { get; set; } = "text/html";
    public byte[] Content { get; set; } = [];

    public byte[] ToBytes()
    {
        var builder = new StringBuilder();
        builder.Append("HTTP/1.1 ").Append(StatusCode).Append("\r\n");
        builder.Append("Content-Type: ").Append(ContentType).Append("\r\n");
        builder.Append("Content-Length: ").Append(Content.Length).Append("\r\n");
        builder.Append("\r\n");

        var headerBytes = Encoding.UTF8.GetBytes(builder.ToString());
        var result = new byte[headerBytes.Length + Content.Length];

        Buffer.BlockCopy(headerBytes, 0, result, 0, headerBytes.Length);
        Buffer.BlockCopy(Content, 0, result, headerBytes.Length, Content.Length);

        return result;
    }

    public void WriteTo(Stream stream)
    {
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);

        writer.Write(Encoding.UTF8.GetBytes($"HTTP/1.1 {StatusCode}\r\n"));
        writer.Write(Encoding.UTF8.GetBytes($"Content-Type: {ContentType}\r\n"));
        writer.Write(Encoding.UTF8.GetBytes($"Content-Length: {Content.Length}\r\n\r\n"));

        if (Content.Length > 0)
        {
            writer.Write(Content);
        }

        writer.Flush();
    }
}