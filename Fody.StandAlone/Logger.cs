using System;

namespace Fody.StandAlone
{
    public class Logger : ILogger
    {
        public void SetCurrentWeaverName(string weaverName)
        {
            
        }

        public void ClearWeaverName()
        {
            
        }

        public void LogDebug(string message)
        {
            Console.WriteLine(message);
        }

        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        public void LogMessage(string message, MessageImportance level)
        {
            Console.WriteLine(message);
        }

        public void LogWarning(string message)
        {
            Console.WriteLine(message);
        }

        public void LogWarning(string message, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber)
        {
            Console.WriteLine(message);
        }

        public void LogError(string message, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber)
        {
            Console.WriteLine(message);
        }

        public void LogError(string message)
        {
            Console.WriteLine(message);
        }
    }
}