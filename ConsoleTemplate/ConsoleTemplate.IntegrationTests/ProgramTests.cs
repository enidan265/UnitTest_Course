using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleTemplate.IntegrationTests
{
    public class ProgramTests
    {
        private readonly string testApp = "ConsoleTemplate";

        public Task<string?> RunApplication()
        {
            var mode = "Release";
#if DEBUG
            mode = "Debug";
#endif
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = @$"..\..\..\..\{testApp}\bin\{mode}\net6.0\{testApp}.exe";
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.Arguments = "Test";
            processStartInfo.EnvironmentVariables.Add("TEST", "Test");

            var process = Process.Start(processStartInfo);

            return Task.Run(() =>
            {
                var output = process?.StandardOutput.ReadLine();

                return output;
            });
        }

        [Fact]
        public void Prints_Env_Vars()
        {
            //Arrange
            var expected = "TestTest";

            //Act
            var result = RunApplication();

            //Assert
            Assert.Equal(expected, result.Result);

        }
    }
}
