using CodeExecutorService.Services.ProcessManagers.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CodeExecutorService.Hubs
{
    public class IODeliver : Hub
    {
        private readonly ILogger<IODeliver> _logger;
        private readonly IProcessManagerService _processManagerService;

        public IODeliver
        (
            ILogger<IODeliver> logger,
            IProcessManagerService processManagerService
        )
        {
            _logger = logger;
            _processManagerService = processManagerService;
        }

        public void ProcessOutput(string connectionID, string line) {
            Clients.All.SendAsync("processoutput", line).Wait();
        }

        public void UserInput(string line) {
            _processManagerService.WriteLineToProcess(Context.ConnectionId, line);
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"{Context.ConnectionId} : conncted");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"{Context.ConnectionId} : disconncted");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
