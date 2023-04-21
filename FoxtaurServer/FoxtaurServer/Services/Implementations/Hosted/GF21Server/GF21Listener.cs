using System.Net;
using System.Net.Sockets;
using System.Text;
using FoxtaurServer.Constants;
using FoxtaurServer.Models.Trackers;
using FoxtaurServer.Services.Abstract;
using FoxtaurServer.Services.Implementations.Hosted.Commands;
using FoxtaurServer.Services.Implementations.Hosted.Parsers;

namespace FoxtaurServer.Services.Implementations.Hosted;

/// <summary>
/// Listener for GF21 trackers
/// </summary>
public class GF21Listener : IHostedService
{
    /// <summary>
    /// Buffer, big enough to fit tracker message
    /// </summary>
    private const int ReadBufferSize = 65535;
    
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfigurationService _configurationService;

    private readonly IList<IGF21Parser> _parsers = new List<IGF21Parser>();

    private Queue<IGF21Command> _commandsToSend = new Queue<IGF21Command>();

    public GF21Listener(IServiceProvider serviceProvider,
        ILogger<GF21Listener> logger,
        ILogger<GF21LoginPacketParser> loginPacketParserLogger,
        ILogger<GF21LocationPacketParser> locationPacketParserLogger,
        ILogger<GF21ShutdownPacketParser> shutdownPacketParserLogger,
        ILogger<GF21HeartbeatPacketParser> heartbeatPacketParserLogger,
        ILogger<GF21SetStationarySleepPacketParser> setStationarySleepPacketParserLogger,
        ILogger<GF21ImsiIccidPacketParser> imsiIccidPacketParserLogger,
        IConfigurationService configurationService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configurationService = configurationService;

        var scope = _serviceProvider.CreateScope();
        
        var huntersLocationsService = scope.ServiceProvider.GetRequiredService<IHuntersLocationsService>();
        
        _parsers.Add(new GF21LoginPacketParser(loginPacketParserLogger));
        _parsers.Add(new GF21LocationPacketParser(locationPacketParserLogger, huntersLocationsService));
        _parsers.Add(new GF21ShutdownPacketParser(shutdownPacketParserLogger));
        _parsers.Add(new GF21HeartbeatPacketParser(heartbeatPacketParserLogger));
        _parsers.Add(new GF21SetStationarySleepPacketParser(setStationarySleepPacketParserLogger));
        _parsers.Add(new GF21ImsiIccidPacketParser(imsiIccidPacketParserLogger));
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var listenIpAddress = IPAddress.Any; // We are in docker, so listening everything
        var listenPort = int.Parse(await _configurationService.GetConfigurationString(GlobalConstants.GF21PortSettingName));
        var listeningThreadsCount = int.Parse(await _configurationService.GetConfigurationString(GlobalConstants.GF21ListenerThreadsCountSettingName));

        var listenEndPoint = new IPEndPoint(listenIpAddress, listenPort);
        
        var listener = new Socket
        (
            listenEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        listener.Bind(listenEndPoint);
        listener.Listen(listeningThreadsCount);

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

            var trackerContext = new TrackerContext();
            var clientThread = new Thread(async () => await ProcessClientConnection(clientSocket, trackerContext).ConfigureAwait(false));
            clientThread.Start();
        }
    }

    /// <summary>
    /// Thread, processing one client connection
    /// </summary>
    private async Task ProcessClientConnection(Socket clientSocket, TrackerContext trackerContext)
    {
        while (true)
        {
            var buffer = new byte[ReadBufferSize];
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
                var parseResult = await parser.ParseAsync(messageFromTracker, trackerContext).ConfigureAwait(false);

                if (parseResult.IsRecognized)
                {
                    if (parseResult.IsSendResponse)
                    {
                        _logger.LogWarning($"Sending packet response: { parseResult.Response }");
                    
                        var responseBytes = Encoding.UTF8.GetBytes(parseResult.Response);
                        await clientSocket.SendAsync(responseBytes, 0);
                    }

                    //// Special logic - when IMSI and ICCD packet received we need to start initialization sequence
                    //if (parser.GetType() == typeof(GF21ImsiIccidPacketParser))
                    //{
                        // First of all we need to disable sleepmode to avoid missing measurements when moving slowly
                        _commandsToSend.Enqueue(new GF21SetStationarySleepCommand(false));
                    //}
                }
            }
            
            // Sending one command per loop rotation
            if (_commandsToSend.Any())
            {
                var commandToSend = _commandsToSend.Dequeue();

                var commandMessage = await commandToSend.SendCommandAsync(trackerContext);
                
                _logger.LogWarning($"Sending command: { commandMessage }");
                
                var commandMessageBytes = Encoding.UTF8.GetBytes(commandMessage);
                await clientSocket.SendAsync(commandMessageBytes, 0);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}