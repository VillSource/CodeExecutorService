using CodeExecutorService.Services.FileSavers.Interfaces;

namespace CodeExecutorService.Services.FileSavers
{
    public class WindowsFileSaverStrategy : IFileSaver
    {
        public string Path { get; set; } = Directory.GetCurrentDirectory();

        public string SaveSourceCode(string sourceCode, string extension = "")
        {
            ToExtension(ref extension);
            Console.WriteLine($"""
                save : {sourceCode}
                name : x{extension}
                to   : {Path}
                """);
            return string.Empty;
        }

        private static void ToExtension(ref string extension)
        {
            if (!extension.StartsWith('.'))
            {
                extension = string.Concat(".", extension);
            }
        }
    }
}
