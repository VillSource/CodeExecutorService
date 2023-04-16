using CodeExecutorService.Constants;
using CodeExecutorService.Services.CodeRunners.Interfaces;
using CodeExecutorService.Services.FileSavers.Interfaces;
using CodeExecutorService.Services.ProcessManagers.Interfaces;
using System.Diagnostics;

namespace CodeExecutorService.Services.CodeRunners
{
    public class PythonRunner : ICodeRunner
    {
        private readonly IProcessManagerService _processManagerService;
        private readonly IFileSaverFactory _fileSaverFactory;
        private readonly IFileSaver _fileSaver;

        private static readonly string _workingDir = "/config/sourcecode";
        private static readonly string _remoteUsername
             = Environment.GetEnvironmentVariable(SLAVE_ENV.USER_NAME)
            ?? "anirut";
        private static readonly string _serverName 
             = Environment.GetEnvironmentVariable(SLAVE_ENV.PYTHON_HOST)
            ?? string.Empty;


        public PythonRunner(IFileSaverFactory fileSaverFactory, IProcessManagerService processManagerService)
        {
            _fileSaverFactory = fileSaverFactory;
            _processManagerService = processManagerService;
            _fileSaver = _fileSaverFactory.CreateStrategy();
        }

        public void Execute(string IOconnectionID, string code, Action<char>? onOutput = null, Action<char>? onError = null, Action<StreamWriter>? setInput = null)
            
        {
            string name = _fileSaver.SaveSourceCode(code, ".py");
            ProcessStartInfo startInfo = new()
            {
                FileName = "ssh",
                Arguments = $"-q {_remoteUsername}@{_serverName} python {Path.Join(_workingDir,name)}"
            };

            _processManagerService.AddNewProcess(IOconnectionID, startInfo);
            _processManagerService.StartProcess(IOconnectionID, onOutput, onError, setInput);
        }
    }
}
