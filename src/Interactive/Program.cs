//------------------------------------------------------------------------------
// <copyright file="Program.cs">
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

    class Program
    {
        static void Main(string[] args)
        {
            ReplConsole.WriteLine("GScript 1.0 Interactive Shell");
            ScriptRepl.Run();
        }
    }
}
