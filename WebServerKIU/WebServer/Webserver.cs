using System.Net;
using System.Net.Sockets;
using WebServerKIU.WebServer;

namespace WebServerApp.Server;

public class WebServer
{
    private readonly TcpListener _listener;
    private readonly FileHandler _handler;


    public WebServer(string ip, int port, FileHandler handler)
    {
        _listener = new TcpListener(IPAddress.Parse(ip), port);
        _handler = handler;
    }

    public async Task Start()
    {
        _listener.Start();
        Console.WriteLine($"[Server] Started on {_listener.LocalEndpoint}");

        while (true)
        {
            try
            {
                TcpClient client = _listener.AcceptTcpClient();

                async void ThreadStart()
                {
                    try
                    {
                        ClientHandler clientHandler = new ClientHandler(client, _handler);
                        await clientHandler.HandleAsync();
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        client.Close();
                    }
                }

                Thread thread = new Thread(ThreadStart);

                thread.Start();
            }
            catch (Exception ex)
            {
                // Optionally add some delay or shutdown logic if needed
            }
        }
    }

    public void Stop()
    {
        _listener.Stop();
    }
}