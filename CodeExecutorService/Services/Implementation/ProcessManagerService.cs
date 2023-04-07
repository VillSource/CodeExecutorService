using CodeExecutorService.Services.Interfaces;
using System.Diagnostics;
using System.Text;

namespace CodeExecutorService.Services.Implementation
{
    public class ProcessManagerService : IProcessManagerService
    {
        private readonly IDictionary<string,Process> _processes;

        public ProcessManagerService()
        {
            _processes = new Dictionary<string,Process>();
        }

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
        }

        public bool KillAllProcess()
        {
            foreach ((string processID, Process process) in _processes)
            {
                process.Close();
                _processes.Remove(processID);
            }
            return true;
        }

        public bool KillProcess(string processID)
        {
            if (!_processes.ContainsKey(processID))
                return false;

            _processes[processID].Close();
            _processes.Remove(processID);
            return true;
        }

        public void StartProcess(string processID, Action<char>? onOutput = null, Action<char>? onError = null, Action<StreamWriter>? sendInput = null)
        {
            bool processIsNull = ! _processes.TryGetValue(processID, out Process? process); 
            if (processIsNull)
                return; 

            process!.Start();

            StreamReader outputReader = process.StandardOutput;
            StreamReader errorReader = process.StandardError;

            Thread outputThread = new (() =>
            {
                while (!outputReader.EndOfStream)
                {
                    char ch = (char)outputReader.Read();
                    onOutput?.Invoke(ch);
                }
            });
            outputThread.Start();

            Thread errorThread = new (() =>
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

        public void WritLineToProcess(string processID, string line)
        {
            bool processIsReady =  _processes.TryGetValue(processID, out Process? process); 
            if (processIsReady)
                return; 

            process!.StandardInput.WriteLine(line);

        }
    }
}
