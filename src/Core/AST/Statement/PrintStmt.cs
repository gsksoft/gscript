//------------------------------------------------------------------------------
// <copyright file="PrintStmt.cs">
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

    public class PrintStmt : Statement
    {
        public Expression Expression { get; private set; }

        public PrintStmt(Expression expression)
        {
            Expression = expression;
        }

        public override object Eval(Scope scope)
        {
            Console.WriteLine(Expression.Eval(scope));
            return null;
        }
    }
}
