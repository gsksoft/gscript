//------------------------------------------------------------------------------
// <copyright file="CompilationUnit.cs">
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

    public class CompilationUnit : Node
    {
        public List<Statement> Statements { get; private set; }

        public CompilationUnit(List<Statement> statements)
        {
            Statements = statements;
        }

        public override object Eval(ExecutionContext context)
        {
            object result = null;
            try
            {
                foreach (var s in Statements)
                {
                    result = s.Eval(context);
                }
            }
            catch (ValueReturnedException ex)
            {
                return ex.ReturnValue;
            }

            return result;
        }
    }
}
