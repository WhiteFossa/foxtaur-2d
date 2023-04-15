using System.Net;
using System.Net.Sockets;
using System.Text;
using FoxtaurServer.Services.Implementations.Hosted.Parsers;

namespace FoxtaurServer.Services.Implementations.Hosted;

/// <summary>
/// Listener for GF21 trackers
/// </summary>
public class GF21Listener : IHostedService
{
    private readonly ILogger _logger;

    private readonly IList<IGF21Parser> _parsers = new List<IGF21Parser>();

    public GF21Listener(ILogger<GF21Listener> logger,
        ILogger<GF21LoginPacketParser> loginPackageParserLogger,
        ILogger<GF21LocationPacketParser> locationPackageParserLogger)
    {
        _logger = logger;
        
        _parsers.Add(new GF21LoginPacketParser(loginPackageParserLogger));
        _parsers.Add(new GF21LocationPacketParser(locationPackageParserLogger));
    }
    
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
            var buffer = new byte[65535];
            var receivedBytes = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);

            if (receivedBytes == 0)
            {
                // It seems that client disconnected
                clientSocket.Close();
                break;
            }
            
            var messageFromTracker = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
            
            _logger.LogWarning($"Received: { messageFromTracker }");

            foreach (var parser in _parsers)
            {
                var parseResult = parser.Parse(messageFromTracker);

                if (parseResult.IsRecognized)
                {
                    _logger.LogWarning($"Sent: { parseResult.Response }");
                
                    var responseBytes = Encoding.UTF8.GetBytes(parseResult.Response);
                    await clientSocket.SendAsync(responseBytes, 0);
                }
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}