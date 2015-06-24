//------------------------------------------------------------------------------
// <copyright file="ReplConsole.cs">
//     Copyright (c) gsksoft. All rights reserved.
// </copyright>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Repl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    internal static class ReplConsole
    {
        private static TextWriter s_stdOut;

        static ReplConsole()
        {
            s_stdOut = Console.Out;
        }

        public static string GetConsoleOutput(Action action)
        {
            StringBuilder outputBuilder = new StringBuilder();
            using (StringWriter writer = new StringWriter(outputBuilder))
            {
                Console.SetOut(writer);
                action();
                Console.SetOut(s_stdOut);
            }

            return outputBuilder.ToString();
        }

        public static string ReadLine(ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            string line = Console.ReadLine();
            Console.ResetColor();
            return line;
        }

        public static void WriteLine(object value, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            s_stdOut.WriteLine(value);
            Console.ResetColor();
        }

        public static void Write(object value, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            s_stdOut.Write(value);
            Console.ResetColor();
        }
    }
}
