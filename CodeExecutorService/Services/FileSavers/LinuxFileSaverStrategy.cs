using CodeExecutorService.Services.FileSavers.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CodeExecutorService.Services.FileSavers
{
    public class LinuxFileSaverStrategy : IFileSaver
    {
        public string Path { get; set; } = "/sourcecodes";

        public string SaveSourceCode(string sourceCode, string extension = "")
        {
            ToExtension(ref extension);
            HashContent(sourceCode, out string name);

            using StreamWriter file = new($"{Path}/{name}{extension}");
            file.WriteLine(sourceCode);

            Console.WriteLine($"""
                save : {sourceCode}
                name : {name}{extension}
                to   : {Path}
                """);
            return string.Concat(name,extension);
        }



        private static void ToExtension(ref string extension)
        {
            if (!extension.StartsWith('.') && !string.IsNullOrEmpty(extension))
            {
                extension = string.Concat(".", extension);
            }
        }

        private static void HashContent(in string content, out string result)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(content);
            byte[] hashBytes = SHA256.HashData(inputBytes);
            result = Convert.ToBase64String(hashBytes)
                .Replace("+","-")
                .Replace("/","_");
        }
    }
}
