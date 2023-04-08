using CodeExecutorService.Services.CodeRunners.Interfaces;
using CodeExecutorService.Services.FileSavers.Interfaces;
using CodeExecutorService.Services.ProcessManagers.Interfaces;
using System.Text.RegularExpressions;

namespace CodeExecutorService.Services.CodeRunners
{
    public partial class CodeRunnerFactory : ICodeRunnerFactory
    {
        private readonly IDictionary<string, Type> strategies;

        private readonly IProcessManagerService _processManagerService;
        private readonly IFileSaverFactory _fileSaverFactory;


        public CodeRunnerFactory(IProcessManagerService processManagerService, IFileSaverFactory fileSaverFactory)
        {
            strategies = new Dictionary<string, Type>();
            _processManagerService = processManagerService;
            _fileSaverFactory = fileSaverFactory;

            // Load all the types in the current assembly that implement ISourceCodeSavingStrategy
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ICodeRunner).IsAssignableFrom(p) && !p.IsInterface);

            // Add each type to the dictionary with its name as the key
            Regex suffix = SuffixRegex();
            foreach (Type type in types)
            {
                string name = suffix.Replace(type.Name, "").ToUpper();
                Console.WriteLine(name);
                strategies[name] = type;
            }
        }

        public ICodeRunner CreateCodeRunner(in string language)
        {
            if (!strategies.TryGetValue(language.ToUpper(), out Type? strategyType))
                throw new NotSupportedException($"{language.ToUpper()} is not supported");

            var codeRunner = Activator.CreateInstance(strategyType!, _fileSaverFactory, _processManagerService);
            return (ICodeRunner)codeRunner!;
        }

        [GeneratedRegex("[A-Z][a-z]*$")]
        private static partial Regex SuffixRegex();
    }
}
