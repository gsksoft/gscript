//------------------------------------------------------------------------------
// <copyright file="ScriptRepl.cs">
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

    using Gsksoft.GScript.Core;
    using Gsksoft.GScript.Core.AST;

    internal static class ScriptRepl
    {
        public static void Run()
        {
            Lexer lexer = new Lexer();
            Parser parser = new Parser();
            ExecutionContext context = ExecutionContext.CreateContext(
                GScriptIO.Create(PrintOutput), Scope.CreateGlobalScope());

            while (true)
            {
                StringBuilder inputBuilder = new StringBuilder();
                inputBuilder.AppendLine(Read());

                object result = null;
                while (true)
                {
                    var tokens = lexer.Scan(inputBuilder.ToString());
                    CompilationUnit compilationUnit;
                    ParseStatus status = parser.TryParse(tokens, out compilationUnit);
                    if (status == ParseStatus.Completed)
                    {
                        result = compilationUnit.Eval(context);
                        break;
                    }
                    else if (status == ParseStatus.Incomplete)
                    {
                        inputBuilder.AppendLine(Read());
                    }
                    else if (status == ParseStatus.Inconclusive)
                    {
                        string line = Read();
                        if (string.IsNullOrEmpty(line))
                        {
                            tokens = lexer.Scan(inputBuilder.ToString());
                            compilationUnit = parser.Parse(tokens);
                            result = compilationUnit.Eval(context);
                            break;
                        }

                        inputBuilder.AppendLine(line);
                    }
                    else if (status == ParseStatus.Error)
                    {
                        result = null;
                        PrintError();
                        break;
                    }
                }

                PrintResult(result);
            }
        }

        private static string Read()
        {
            ReplConsole.Write("> ");
            return ReplConsole.ReadLine();
        }

        private static void PrintError()
        {
            ReplConsole.Write("< ");
            ReplConsole.WriteLine("Error", ConsoleColor.Red);
        }

        private static void PrintResult(object result)
        {
            if (result != null)
            {
                ReplConsole.Write("< ");
                ReplConsole.WriteLine(result);
            }
        }

        private static void PrintOutput(object output)
        {
            PrintOutput(output.ToString());
        }

        private static void PrintOutput(string output)
        {
            if (!string.IsNullOrEmpty(output))
            {
                string[] printLines = output
                    .TrimEnd(new[] { '\r', '\n' })
                    .Split('\n');
                foreach (var printLine in printLines)
                {
                    ReplConsole.Write("< ");
                    ReplConsole.WriteLine(printLine);
                }
            }
        }
    }
}
