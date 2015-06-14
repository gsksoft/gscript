//------------------------------------------------------------------------------
// <copyright file="Interpreter.cs">
//     Copyright (c) gsksoft. All rights reserved.
// </copyright>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Interpreter
    {
        public object Execute(string source)
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Scan(source);

            Parser parser = new Parser();
            var program = parser.Parse(tokens);

            return program.Eval(Scope.Global);
        }
    }
}
