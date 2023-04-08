using CodeExecutorService.Hubs;
using CodeExecutorService.Models;
using CodeExecutorService.Services.CodeRunners.Interfaces;
using CodeExecutorService.Services.FileSavers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CodeExecutorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunController : ControllerBase
    {
        private readonly IFileSaverFactory _fileSaverFactory;
        private readonly ICodeRunnerFactory _codeRunnerFactory;
        private readonly IHubContext<IODeliver> _hubContext;

        public RunController(IFileSaverFactory fileSaverFactory, ICodeRunnerFactory codeRunnerFactory, IHubContext<IODeliver> hubContext)
        {
            _fileSaverFactory = fileSaverFactory;
            _codeRunnerFactory = codeRunnerFactory;
            _hubContext = hubContext;
        }

        [HttpPost]
        public ActionResult RunCode([FromQuery] string connectionID, [FromForm] CodeInfo codeInfo)
        {
            ICodeRunner codeRunner = _codeRunnerFactory.CreateCodeRunner(codeInfo.Language);

            codeRunner.Execute(connectionID, codeInfo.Code, ch=>SendOutputTo(connectionID,ch), ch=>SendOutputTo(connectionID,ch));
            return Ok();
        }

        private void SendOutputTo(string connectionID, char data)
        { 
            _hubContext.Clients.Client(connectionID).SendAsync("processoutput", $"{data}").Wait();
        }
    }
}
