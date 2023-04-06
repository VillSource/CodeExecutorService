using CodeExecutorService.Models;
using CodeExecutorService.SubProcess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace CodeExecutorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunController : ControllerBase
    {
        private readonly SubProcessIOHandelService _allProcess;

        public RunController(SubProcessIOHandelService allProcess)
        {
            _allProcess = allProcess;
        }

        [HttpPost]
        public IActionResult Run([FromQuery] string connectionID,[FromForm] CodeInfo codeInfo)
        {
            Savefile(codeInfo.Code);

            // command
            ProcessStartInfo startInfo = new(); 
            startInfo.FileName = "ssh";
            startInfo.Arguments = $"anirut@python-slave python sourcecode/py.py";

            _allProcess
                .NewProcess(connectionID, startInfo)
                .WaitForExit();

            return Ok(new
            {
                connectionID,
                codeInfo,
            });
        }

        private void Savefile(string content, string file = "py.py")
        {
            try
            {
                // On docker
                string path = $"""/sourcecodes/{file}""";
                using StreamWriter writer = new(path); 
                writer.Write(content);
            }catch (Exception ex)
            {
                // On windows 
                string path = $"""C:\Users\Anirut\Desktop\MyFnPj\CodeExecutorService\CompilerContainer\SourceCode\{file}""";
                using StreamWriter writer = new(path); 
                writer.Write(content);
            }
        }

    }
}
