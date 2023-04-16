namespace CodeExecutorService.Services.CodeRunners.Interfaces
{
    public interface ICodeRunner
    {
        void Execute(string IOconnectionID, string code, Action<char>? onOutput = null, Action<char>? onError = null, Action<StreamWriter>? sendInput = null);
    }
}
