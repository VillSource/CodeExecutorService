using System.Diagnostics;

namespace CodeExecutorService.Services.ProcessManagers.Interfaces
{
    public interface IProcessManagerService
    {
        /// <summary>
        /// Count all processes whish alived 
        /// </summary>
        int CountProcesseses { get; }

        /// <summary>
        /// Get all processes ID
        /// </summary>
        IEnumerable<string> AllProcessesID { get; }

        /// <summary>
        /// Add new process with ID
        /// </summary>
        /// <param name="processID">Create new process with this id</param>
        /// <param name="startInfo">Prosess infomatoin</param>
        void AddNewProcess(string processID, ProcessStartInfo startInfo);

        /// <summary>
        /// Start process by process ID.
        /// And set on ouput or on Error.
        /// </summary>
        /// <param name="processID">Process ID to start</param>
        /// <param name="onOutput">Action to inwoke</param>
        /// <param name="onError"></param>
        /// <param name="sendInput"></param>
        void StartProcess(string processID, Action<char>? onOutput = null, Action<char>? onError = null, Action<StreamWriter>? sendInput = null);

        /// <summary>
        /// Write input to a process
        /// </summary>
        /// <param name="processID">Process ID to write line</param>
        /// <param name="line">Data to write to process</param>
        void WriteLineToProcess(string processID, string line);

        /// <summary>
        /// Kill process if exist
        /// </summary>
        /// <param name="processID">Process ID to be killed</param>
        /// <returns></returns>
        bool KillProcess(string processID);

        /// <summary>
        /// Kill all alived process
        /// </summary>
        /// <returns>true if all process is killed otherwise false</returns>
        bool KillAllProcess();

    }
}
