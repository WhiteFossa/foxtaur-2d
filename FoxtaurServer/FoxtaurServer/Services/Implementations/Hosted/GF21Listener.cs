using System.Net;
using System.Net.Sockets;
using System.Text;
using NLog;

namespace FoxtaurServer.Services.Implementations.Hosted;

/// <summary>
/// Listener for GF21 trackers
/// </summary>
public class GF21Listener : IHostedService
{
    private Logger _logger = LogManager.GetCurrentClassLogger();
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var listenIpAddress = IPAddress.Any; // TODO: Move to config
        var listenEndPoint = new IPEndPoint(listenIpAddress, 10000); // TODO: Move to config
        
        var listener = new Socket
        (
            listenEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        listener.Bind(listenEndPoint);
        listener.Listen(1024); // TODO: Move to config - Amount of listening threads

        var processClientsConnectionsThread = new Thread(async () => await ProcessClientsConnections(listener).ConfigureAwait(false));
        processClientsConnectionsThread.Start();
    }

    /// <summary>
    /// Thread, awaiting for clients connections
    /// </summary>
    private async Task ProcessClientsConnections(Socket listener)
    {
        while (true)
        {
            var clientSocket = await listener.AcceptAsync();

            var clientThread = new Thread(async () => await ProcessClientConnection(clientSocket).ConfigureAwait(false));
            clientThread.Start();
        }
    }

    /// <summary>
    /// Thread, processing one client connection
    /// </summary>
    private async Task ProcessClientConnection(Socket clientSocket)
    {
        while (true)
        {
            var buffer = new byte[1_024];
            var receivedBytes = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);

            if (receivedBytes == 0)
            {
                // It seems that client disconnected
                clientSocket.Close();
                break;
            }
            
            var receivedString = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
            
            _logger.Info($"Received: { receivedString }");
            
            // Test
            if (receivedString.Contains("TRVAP"))
            {
                var currentTime = DateTime.UtcNow;
                
                var response = $"TRVBP00{ currentTime.Year }{currentTime.Month:00}{currentTime.Day:00}{currentTime.Hour:00}{currentTime.Minute:00}{currentTime.Second:00}#";
                _logger.Info($"Sent: { response }");
                
                var responseBytes = Encoding.UTF8.GetBytes(response);
                await clientSocket.SendAsync(responseBytes, 0);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}