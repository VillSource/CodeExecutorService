namespace CodeExecutorService.Services.CodeRunners.Interfaces
{
    public interface ICodeRunnerFactory
    {
        ICodeRunner CreateCodeRunner(in string language);
    }
}
