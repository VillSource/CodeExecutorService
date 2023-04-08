namespace CodeExecutorService.Services.FileSavers.Interfaces
{
    public interface IFileSaver
    {
        string Path { get; set; }
        string SaveSourceCode(string sourceCode, string extension = "");
    }
}
