// See https://aka.ms/new-console-template for more information

using WebServerKIU.WebServer;

public class Program
{
    public static void Main(string[] args)
    {
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "webroot"));
        WebServer server = new WebServer("127.0.0.1", 13000, new FileHandler(projectRoot));
        server.Start();
    }
}