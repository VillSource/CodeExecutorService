using CodeExecutorService.Services.CodeRunners.Interfaces;
using CodeExecutorService.Services.FileSavers.Interfaces;
using CodeExecutorService.Services.ProcessManagers.Interfaces;
using System.Diagnostics;

namespace CodeExecutorService.Services.CodeRunners
{
    public class ShellRunner : ICodeRunner
    {
        private readonly IProcessManagerService _processManagerService;
        private readonly IFileSaverFactory _fileSaverFactory;
        private readonly IFileSaver _fileSaver;

        private static readonly string _workingDir = "/config/sourcecode";
        private static readonly string _remoteUsername = "anirut";
        private static readonly string _serverName = "python-slave";

        public ShellRunner(IFileSaverFactory fileSaverFactory, IProcessManagerService processManagerService)
        {
            _fileSaverFactory = fileSaverFactory;
            _processManagerService = processManagerService;
            _fileSaver = _fileSaverFactory.CreateStrategy();
        }

        public void Execute(string IOconnectionID, string code, Action<char>? onOutput = null, Action<char>? onError = null, Action<StreamWriter>? setInput = null)
            
        {
            code = code.Replace("\r\n", "\n").Replace("\r","\n");
            string name = _fileSaver.SaveSourceCode(code, ".sh");
            ProcessStartInfo startInfo = new()
            {
                FileName = "ssh",
                Arguments = $"{_remoteUsername}@{_serverName} /config/sourcecode/run.sh {name}"
            };

            _processManagerService.AddNewProcess(IOconnectionID, startInfo);
            _processManagerService.StartProcess(IOconnectionID, onOutput, onError, setInput);
        }
    }
}
