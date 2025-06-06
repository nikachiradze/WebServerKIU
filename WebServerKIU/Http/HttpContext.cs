namespace WebServerKIU.Http;

public class HttpContext
{
    public HttpRequest Request { get; init; }
    public HttpResponse Response { get; init; }

    public HttpContext(HttpRequest request)
    {
        Request = request;
        Response = new HttpResponse();
    }
}