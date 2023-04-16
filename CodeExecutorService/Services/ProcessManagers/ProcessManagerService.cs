using CodeExecutorService.Services.ProcessManagers.Interfaces;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace CodeExecutorService.Services.ProcessManagers
{
    public class ProcessManagerService : IProcessManagerService
    {
        private readonly ILogger<ProcessManagerService> _logger;

        public ProcessManagerService(ILogger<ProcessManagerService> logger)
        {
            _logger = logger;
            _processes = new Dictionary<string, Process>();
        }

        private readonly IDictionary<string, Process> _processes;

        public int CountProcesseses => _processes.Count;

        public IEnumerable<string> AllProcessesID => _processes.Keys;

        public void AddNewProcess(string processID, ProcessStartInfo startInfo)
        {
            KillProcess(processID);

            // Redirect the input, output, and error streams
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            // Set the encoding for the output and error streams
            startInfo.StandardOutputEncoding = Encoding.UTF8;
            startInfo.StandardInputEncoding = Encoding.UTF8;

            Process process = new();
            process.StartInfo = startInfo;

            _processes.Add(processID, process);
            _logger.LogInformation("Create new process ID : {}", processID);
        }

        public bool KillAllProcess()
        {
            foreach ((string processID, Process process) in _processes)
            {
                process.Close();
                _processes.Remove(processID);
                _logger.LogInformation("Kill process ID : {}", processID);
            }
            return true;
        }

        public bool KillProcess(string processID)
        {
            if (!_processes.ContainsKey(processID))
                return false;

            _processes.TryGetValue(processID, out Process? process);
            process?.Kill();
            _processes.Remove(processID);
            _logger.LogInformation("Kill process ID : {}", processID);
            return true;
        }

        public void StartProcess(string processID, Action<char>? onOutput = null, Action<char>? onError = null, Action<StreamWriter>? sendInput = null)
        {
            bool processIsNull = !_processes.TryGetValue(processID, out Process? process);
            if (processIsNull)
                return;

            process!.Start();
            _logger.LogInformation("Start process ID : {}", processID);

            StreamReader outputReader = process.StandardOutput;
            StreamReader errorReader = process.StandardError;

            Thread outputThread = new(() =>
            {
                while (!outputReader.EndOfStream)
                {
                    char ch = (char)outputReader.Read();
                    onOutput?.Invoke(ch);
                }
            });
            outputThread.Start();

            Thread errorThread = new(() =>
            {
                while (!errorReader.EndOfStream)
                {
                    char ch = (char)errorReader.Read();
                    onError?.Invoke(ch);
                }
            });
            errorThread.Start();

            Task.Delay(1000).Wait();
            sendInput?.Invoke(process.StandardInput);

            process.WaitForExit();

            outputThread.Join();
            errorThread.Join();

            KillProcess(processID);
        }

        public void WriteLineToProcess(string processID, string line)
        {
            _processes.TryGetValue(processID, out Process? process);
            process?.StandardInput.WriteLine(line);
        }
    }
}
