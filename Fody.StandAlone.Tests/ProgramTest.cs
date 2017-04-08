using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fody.StandAlone.Tests
{
    public class ProgramTest
    {
        public static IEnumerable<string[]> Source
        {
            get
            {
                yield return new[]
                {
                    @"-f", @"c:\users\laurent\documents\visual studio 2017\Projects\Thirdparty.Wrapper\FodyWeavers.xml",
                    "-t",@"c:\users\laurent\documents\visual studio 2017\Projects\Thirdparty.Wrapper\bin\Debug\AssemblyToInstrument.dll",
                    "-r",@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.dll"
                };

                yield return new[]
                {
                    @"-fc:\users\laurent\documents\visual studio 2017\Projects\Thirdparty.Wrapper\FodyWeavers.xml",
                    @"-tc:\users\laurent\documents\visual studio 2017\Projects\Thirdparty.Wrapper\bin\Debug\AssemblyToInstrument.dll",
                    @"-rC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.dll"
                };
                yield return new[]
                {
                    @"--FodyWeaverXml", @"c:\users\laurent\documents\visual studio 2017\Projects\Thirdparty.Wrapper\FodyWeavers.xml",
                    "--TargetAssembly",@"c:\users\laurent\documents\visual studio 2017\Projects\Thirdparty.Wrapper\bin\Debug\AssemblyToInstrument.dll",
                    "--References",@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.dll"
                };

                yield return new[]
                {
                    @"--FodyWeaverXml=c:\users\laurent\documents\visual studio 2017\Projects\Thirdparty.Wrapper\FodyWeavers.xml",
                    @"--TargetAssembly=c:\users\laurent\documents\visual studio 2017\Projects\Thirdparty.Wrapper\bin\Debug\AssemblyToInstrument.dll",
                    @"--References=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.dll"
                };
            }
        }

        [TestCaseSource("Source")]
        public void Test(string[] strings)
        {          
            TestImpl(strings);
        }

        [Test]
        public void Test5()
        {
            var strings = new[]
            {
                @"--FodyWeaverXml=c:\users\laurent\documents\visual studio 2017\Projects\Thirdparty.Wrapper\FodyWeavers.xml",
                @"--References=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.dll"
            };

            Assert.Throws<WrongCommandLineParametersException>(() => TestImpl(strings));
        }

        private static void TestImpl(string[] strings)
        {
            var commandLineParameters = Program.GetCommandLineParameters(strings);
            var lineParameters = GetExpectedValue();

            Assert.That(commandLineParameters.FodyWeaverConfigPath, Is.EqualTo(lineParameters.FodyWeaverConfigPath));
            Assert.That(commandLineParameters.References, Is.EqualTo(lineParameters.References));
            Assert.That(commandLineParameters.TargetAssembly, Is.EqualTo(lineParameters.TargetAssembly));
        }

        private static CommandLineParameters GetExpectedValue()
        {
            using (var manifestResourceStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Fody.StandAlone.Tests.ExpectedCommandLineParameter1.json"))
            {
               return JsonSerializer.Create()
                    .Deserialize<CommandLineParameters>(new JsonTextReader(new StreamReader(manifestResourceStream)));
            }
        }
    }
}
