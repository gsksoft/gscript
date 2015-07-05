//------------------------------------------------------------------------------
// <copyright file="FuncCallStmt.cs">
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

    public class FuncCallStmt : Statement
    {
        public FuncCallExpr Expression { get; private set; }

        public FuncCallStmt(FuncCallExpr expression)
        {
            Expression = expression;
        }

        public override object Eval(ExecutionContext context)
        {
            Expression.Eval(context);
            return null;
        }
    }
}
