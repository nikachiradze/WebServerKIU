namespace WebServerKIU.Http;

public class HttpRequest
{
    public string Method { get; set; }
    public string Url { get; set; }
    public string Version { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();

    public static HttpRequest Parse(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new ArgumentException("Request is empty or null.");

        var lines = raw.Split(new[] { "\r\n" }, StringSplitOptions.None);
        if (lines.Length == 0)
            throw new FormatException("Invalid HTTP request format.");

        var requestLineParts = lines[0].Split(' ');
        if (requestLineParts.Length < 3)
            throw new FormatException("Request line must contain Method, URL, and HTTP version.");

        var request = new HttpRequest
        {
            Method = requestLineParts[0],
            Url = requestLineParts[1],
            Version = requestLineParts[2]
        };

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
                break;

            var separatorIndex = line.IndexOf(':');
            if (separatorIndex > 0)
            {
                var key = line.Substring(0, separatorIndex).Trim();
                var value = line.Substring(separatorIndex + 1).Trim();
                request.Headers[key] = value;
            }
        }

        return request;
    }
}