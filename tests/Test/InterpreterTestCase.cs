//------------------------------------------------------------------------------
// <copyright file="InterpreterTestCase.cs">
//     Copyright (c) gsksoft. All rights reserved.
// </copyright>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    
    using Gsksoft.GScript.Core;

    internal class InterpreterTestCase
    {
        private string m_name { get; set; }

        private string m_code { get; set; }

        public InterpreterTestCase(string name, string code)
        {
            m_name = name;
            m_code = code;
        }

        public void ShouldOutput(string expected)
        {
            Console.Write(m_name + " ");

            StringBuilder outputBuilder = new StringBuilder();
            using (StringWriter writer = new StringWriter(outputBuilder))
            {
                var stdOut = Console.Out;
                Console.SetOut(writer);
                new Interpreter().Execute(m_code);
                Console.SetOut(stdOut);
            }

            string actual = outputBuilder.ToString().TrimEnd(new[] { '\r', '\n' });
            if (actual == expected)
            {
                WriteLine("[Passed]", ConsoleColor.Green);
            }
            else
            {
                WriteLine("[Failed]", ConsoleColor.Red);
                Console.WriteLine(" (expected: {0}, actual: {1})", expected, actual);
            }
        }

        private static void WriteLine(string s, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(s);
            Console.ResetColor();
        }
    }
}
