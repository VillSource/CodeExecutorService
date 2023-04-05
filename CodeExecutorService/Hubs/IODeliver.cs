using Microsoft.AspNetCore.SignalR;

namespace CodeExecutorService.Hubs
{
    public class IODeliver : Hub
    {
        private readonly ILogger<IODeliver> _logger;

        public IODeliver(ILogger<IODeliver> logger)
        {
            _logger = logger;
        }

        public void UserInput(string line) {
            string message = $"{Context.ConnectionId} > input : {line}";
            _logger.LogInformation(message);
        }
    }
}
