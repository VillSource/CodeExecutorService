using CodeExecutorService.Services.FileSavers.Interfaces;

namespace CodeExecutorService.Services.FileSavers
{
    public class FileSaverFactory : IFileSaverFactory
    {
        public IFileSaver CreateStrategy()
        {
            var osVersion = Environment.OSVersion;

            if (osVersion.Platform == PlatformID.Win32NT)
                return WindowsStrategy();
            if (osVersion.Platform == PlatformID.Unix)
                return LinuxStrategy();
            if (osVersion.Platform == PlatformID.MacOSX)
                return MacStrategy();

            throw new NotSupportedException();
        }

        private static IFileSaver WindowsStrategy()
        {
            return new WindowsFileSaverStrategy();
        }
        private static IFileSaver LinuxStrategy()
        {
            return new LinuxFileSaverStrategy();
        }
        private static IFileSaver MacStrategy()
        {
            throw new NotImplementedException();
        }


    }
}
