using System;
using System.IO;
using CommandLine;


namespace Fody.StandAlone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "help")
                {
                    Console.WriteLine(new CommandLineParameters().GetUsage());
                }
            }
            else
            {
                try
                {
                    var commandLineParameters = GetCommandLineParameters(args);
                    var weavingParameters = GetWeavingParameters(commandLineParameters);
                    new InnerWeaverWrapper(commandLineParameters.TargetAssembly).Weave(weavingParameters);
                }
                catch (WrongCommandLineParametersException e)
                {
                    Console.WriteLine(e.Usage);
                    throw;
                }
            }
        }

        private static WeavingParameters GetWeavingParameters(CommandLineParameters options)
        {
            WeavingParameters weavingParameters;
            using (var fodyXmlConfiguration = new FileStream(options.FodyWeaverConfigPath, FileMode.Open))
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                weavingParameters = WeavingParameters.GetWeavingParameters(fodyXmlConfiguration, currentDirectory, options.References);
            }
            return weavingParameters;
        }

        public static CommandLineParameters GetCommandLineParameters(string[] args)
        {
            var options = new CommandLineParameters();
            new Parser().ParseArgumentsStrict(args, options, () => OnFail(options));
            return options;
        }

        private static void OnFail(CommandLineParameters options)
        {
            throw new WrongCommandLineParametersException(options.GetUsage());
        }
    }

    public class WrongCommandLineParametersException : Exception
    {
        public string Usage { get; }


        public WrongCommandLineParametersException(string usage) : base()
        {
            Usage = usage;
        }
    }
}
