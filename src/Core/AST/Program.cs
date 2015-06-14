//------------------------------------------------------------------------------
// <copyright file="Program.cs">
//     Copyright (c) gsksoft. All rights reserved.
// </copyright>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Core.AST
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Program : Node
    {
        public List<Statement> Statements { get; private set; }

        public Program(List<Statement> statements)
        {
            Statements = statements;
        }

        public override object Eval(Scope scope)
        {
            object result = null;
            foreach (var s in Statements)
            {
                result = s.Eval(scope);
            }

            return result;
        }
    }
}
