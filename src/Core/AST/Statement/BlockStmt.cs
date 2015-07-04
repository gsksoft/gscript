//------------------------------------------------------------------------------
// <copyright file="BlockStmt.cs">
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

    public class BlockStmt : Statement
    {
        public List<Statement> Statements { get; private set; }

        public BlockStmt(List<Statement> statements)
        {
            Statements = statements;
        }

        public override object Eval(ExecutionContext context)
        {
            object result = null;
            ExecutionContext newContext = context.CreateContextWithChildScope();
            foreach (var s in Statements)
            {
                result = s.Eval(newContext);
            }
            return result;
        }
    }
}
