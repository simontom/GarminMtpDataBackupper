using System;
using System.Runtime.CompilerServices;

namespace GarminMtpDataBackupper
{
    public static class Logger
    {
        public static void Info(string message)
        {
            Console.WriteLine($"INFO: {message}");
        }

        public static void Warning(string message)
        {
            Console.WriteLine($"WARNING: {message}");
        }

        public static void Error(
            string message,
            Exception exception = null,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = "")
        {
            Console.WriteLine($"ERROR:\n\tMessage: {message}\n\tException: {exception?.GetType()} - {exception?.Message}\n\tFile: {callerFilePath} : {callerLineNumber} : {callerMemberName}");
        }
    }
}
