namespace CodeExecutorService.Services.FileSavers.Interfaces
{
    public interface IFileSaverFactory
    {
        IFileSaver CreateStrategy();
    }
}
