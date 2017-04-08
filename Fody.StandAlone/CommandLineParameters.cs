using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace Fody.StandAlone
{
    public class CommandLineParameters
    {
        [Option('f', "FodyWeaverXml", Required = true, HelpText = "Fody config file path")]
        public string FodyWeaverConfigPath { get; set; }

        [Option('t', "TargetAssembly", Required = true, HelpText = "Target assembly path")]
        public string TargetAssembly { get; set; }

        [Option('r', "References", Required = true, HelpText = "References splitted by ;")]
        public string References { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        //[VerbOption("help", "Display help")]
        //public string StrType { get; set; }

        [HelpVerbOption]
        [HelpOption()]
        public string GetUsage()
        {
            var help = new HelpText(new EnglishSentenceBuilder(), Assembly.GetExecutingAssembly().FullName, "Marcello Faga, Laurent Nguyen");

            var commandLineParameters = new CommandLineParameters();
            //{
            //    FodyWeaverConfigPath= "c:\\users\\laurent\\documents\\visual studio 2017\\Projects\\Thirdparty.Wrapper\\FodyWeavers.xml",
            //    TargetAssembly= "c:\\users\\laurent\\documents\\visual studio 2017\\Projects\\Thirdparty.Wrapper\\bin\\Debug\\AssemblyToInstrument.dll",
            //    References= "C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.5.2\\System.dll"
            //};

            help.AddOptions(commandLineParameters);

            
            if (this.LastParserState?.Errors.Any() == true)
            {
                var errors = help.RenderParsingErrorsText(this, 2); // indent with two spaces

                if (!string.IsNullOrEmpty(errors))
                {
                    help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                    help.AddPreOptionsLine(errors);
                }
            }

            // ...
            return help;
        }

        private static string GetRequiredOrMandatoryLabel(bool required)
        {
            return required ? "Required" : "Mandatory";
        }
    }
}