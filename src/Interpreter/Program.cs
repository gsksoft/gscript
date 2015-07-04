//------------------------------------------------------------------------------
// <copyright file="Program.cs">
//     Copyright (c) gsksoft. All rights reserved.
// </copyright>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            var options = GetOptions(args);
            if (options != null && options.ContainsKey("file"))
            {
                Execute(options["file"]);
            }
            else
            {
                OutputHelpMessage();
            }
        }

        private static void Execute(string fileName)
        {
            string source = File.ReadAllText(fileName);
            new Core.Interpreter().Execute(source);
        }

        private static void OutputHelpMessage()
        {
            Console.WriteLine("Usage: GScript.Interpreter --file \"<filename>\"");
        }

        /// <summary>
        /// [ "--key1", "value1", "--key2", "value2" ] -> { "key1": "value1", "key2": "value2" }
        /// </summary>
        private static Dictionary<string, string> GetOptions(string[] args)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            string key = null;
            foreach (var arg in args)
            {
                bool isKey = arg.Length > 2 && arg.StartsWith("--");
                if (isKey)
                {
                    if (key != null)
                    {
                        options.Add(key, null);
                    }

                    key = arg.Substring(2).ToLower();
                }
                else
                {
                    if (key != null)
                    {
                        string value = arg;
                        options[key] = value;
                        key = null;
                    }
                    // ignore value without key
                }
            }

            if (key != null)
            {
                options.Add(key, null);
            }

            return options;
        }
    }
}
