using System.Diagnostics;
using System.Text;

namespace CodeExecutorService.SubProcess
{
    public sealed class SubProcessIOHandelService
    {
        private static volatile SubProcessIOHandelService instance;
        private static object syncRoot = new ();

        private Dictionary<string, Process> ActivedProcess;

        private SubProcessIOHandelService() 
        {
            ActivedProcess = new Dictionary<string, Process>();
        }

        public static SubProcessIOHandelService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SubProcessIOHandelService();
                    }
                }
                return instance;
            }
        }


        private void Push(string key, Process process) {
            lock (syncRoot)
            {
                if (ActivedProcess.ContainsKey(key))
                {
                    ActivedProcess[key].Kill();
                    ActivedProcess[key] = process;
                    return;
                } 
                ActivedProcess.Add(key, process);
            }
        }

        public Process? Get(string key)
        {
            lock (syncRoot)
            {
                if (!ActivedProcess.ContainsKey(key)) return null;
                return ActivedProcess[key];
            }
        }

        private void Delete(string key)
        {
            lock (syncRoot)
            { 
                if (ActivedProcess.ContainsKey(key))
                {
                    ActivedProcess[key].Kill();
                    ActivedProcess.Remove(key);
                }
            }
        }
        public Process NewProcess(string connectionID,ProcessStartInfo startInfo, Action<string> action )
        {
            Delete(connectionID);
            // Redirect the input, output, and error streams
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;


            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            // Set the encoding for the output and error streams
            startInfo.StandardOutputEncoding = Encoding.UTF8;
            startInfo.StandardErrorEncoding = Encoding.UTF8;

            Process process = new();
            process.StartInfo = startInfo;

            Push(connectionID, process);
            WatchProcessOutPut(connectionID,action);
            return process;
        }

        private void WatchProcessOutPut(string connnectionID,Action<string> action)
        {
            Process? process = Get(connnectionID);
            if (process == null)
                return;

            //process.StartInfo.resiv

            process.Start();

            StreamReader outputReader = process.StandardOutput;
            StreamReader errorReader = process.StandardError;

            Thread outputThread = new Thread(() =>
            {
                while (!outputReader.EndOfStream)
                {
                    char line = (char)outputReader.Read();
                    action($"{line}");
                    //Console.WriteLine(SendOutPut);
                    Console.Write(line);
                }
            });
            outputThread.Start();

            Thread errorThread = new Thread(() =>
            {
                while (!errorReader.EndOfStream)
                {
                    char line = (char)errorReader.Read();
                    //SendOutPut.Invoke(connnectionID, $"> {line}");
                    action($"{line}");
                    Console.Error.Write(line);
                }
            });
            errorThread.Start();

            process.WaitForExit();

            outputThread.Join();
            errorThread.Join();

            Delete(connnectionID); 
        }

        private void processOutput(string connnectionID, string v)
        {

        }

        public void WriteLine(string connectionID,  string line)
        {
            Process? process = Get(connectionID);
            if (process == null)
                return;

            process.StandardInput.WriteLine(line);
            Console.WriteLine(line);
        }

    }
}
