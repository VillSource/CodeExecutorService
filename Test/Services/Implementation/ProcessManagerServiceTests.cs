using Xunit;
using CodeExecutorService.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using System.Diagnostics;

namespace CodeExecutorService.Services.Implementation.Tests
{
    public class ProcessManagerServiceTests
    {
        [Fact]
        public void AddNewProcessTest()
        {
            var service = new ProcessManagerService();
            var pid = "123";
            var startInfo = new ProcessStartInfo()
            {
                FileName = "echo",
                Arguments = "Test 1"
            };
            var expectedKeys = new string[] { pid };
            var expectedCount = 1;

            service.AddNewProcess(pid, startInfo);
            var actualKeys = service.AllProcessesID.ToArray();
            var actualCount = service.CountProcesseses;

            Assert.Equal(expectedKeys, actualKeys);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void KillAllProcessTest()
        { 
            var service = new ProcessManagerService();
            var pid1 = "123";
            var pid2 = "123456";
            var startInfo = new ProcessStartInfo()
            {
                FileName = "echo",
                Arguments = "Test 1"
            };
            var expectedKeys = Array.Empty<string>();
            var expectedCount = 0;

            service.AddNewProcess(pid1, startInfo);
            service.AddNewProcess(pid2, startInfo);

            service.KillAllProcess();
            var actualKeys = service.AllProcessesID.ToArray();
            var actualCount = service.CountProcesseses;

            Assert.Equal(expectedKeys, actualKeys);
            Assert.Equal(expectedCount, actualCount); 
        }

        [Fact]
        public void KillProcessTest()
        { 
            var service = new ProcessManagerService();
            var pid1 = "123";
            var pid2 = "123456";
            var startInfo = new ProcessStartInfo()
            {
                FileName = "echo",
                Arguments = "Test 1"
            };
            var expectedKeys = new string[] { pid2 };
            var expectedCount = 1;

            service.AddNewProcess(pid1, startInfo);
            service.AddNewProcess(pid2, startInfo);

            service.KillProcess(pid1);
            var actualKeys = service.AllProcessesID.ToArray();
            var actualCount = service.CountProcesseses;

            Assert.Equal(expectedKeys, actualKeys);
            Assert.Equal(expectedCount, actualCount); 
        }

        [Fact]
        public void StartProcessTest()
        {
            var service = new ProcessManagerService();
            var pid = "123";
            var expectedOutput = "Test 1";
            var startInfo = new ProcessStartInfo()
            {
                FileName = "PowerShell",
                Arguments = $"""echo '{expectedOutput}'"""
            };

            StringBuilder actualOutput = new();

            service.AddNewProcess(pid, startInfo);
            service.StartProcess(pid, onOutput:ch=>actualOutput.Append(ch));
            var actualKeys = service.AllProcessesID.ToArray();
            var actualCount = service.CountProcesseses;

            Assert.Equal(expectedOutput, actualOutput.ToString().Trim());
        }

        [Fact]
        public void WritLineToProcessTest()
        {
            var service = new ProcessManagerService();
            var pid = "123";
            var input = "hello world";
            var expectedOutput = $"Enter your first name: {input}\n{input}";
            var startInfo = new ProcessStartInfo()
            {
                FileName = "PowerShell",
                Arguments = $""" -c "Read-Host -Prompt 'Enter your first name' """
            };

            StringBuilder actualOutput = new();

            
            service.AddNewProcess(pid, startInfo);
            service.StartProcess(pid,
                onOutput:ch=>actualOutput.Append(ch),
                sendInput:wt=>wt.WriteLine(input)
            ); 
            var actualKeys = service.AllProcessesID.ToArray();
            var actualCount = service.CountProcesseses;

            Assert.Equal(expectedOutput, actualOutput.ToString().Trim()); 
        }
    }
}