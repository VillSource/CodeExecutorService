using CodeExecutorService.SubProcess;
using Microsoft.AspNetCore.SignalR;

namespace CodeExecutorService.Hubs
{
    public class IODeliver : Hub
    {
        private readonly ILogger<IODeliver> _logger;
        private readonly SubProcessIOHandelService subProcessIOHandelService;

        public IODeliver
        (
            ILogger<IODeliver> logger,
            SubProcessIOHandelService spioService
        )
        {
            _logger = logger;
            subProcessIOHandelService = spioService;
        }

        public void ProcessOutput(string connectionID, string line) {
            //Clients.Clients(connectionID).SendAsync("processoutput",line).Wait();
            Clients.All.SendAsync("processoutput", line).Wait();
        }

        public void UserInput(string line) {
            subProcessIOHandelService.WriteLine(Context.ConnectionId, line);
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
