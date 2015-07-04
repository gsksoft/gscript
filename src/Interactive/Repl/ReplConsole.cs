//------------------------------------------------------------------------------
// <copyright file="ReplConsole.cs">
//     Copyright (c) gsksoft. All rights reserved.
// </copyright>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Interactive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    internal static class ReplConsole
    {
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
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void Write(object value, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }
    }
}
