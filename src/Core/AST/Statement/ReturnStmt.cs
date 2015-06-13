//------------------------------------------------------------------------------
// <copyright file="ReturnStmt.cs">
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

    public class ReturnStmt : Statement
    {
        public Expression Expression { get; private set; }

        public ReturnStmt(Expression expression)
        {
            Expression = expression;
        }

        public override object Eval(Scope scope)
        {
            // TODO: skip unexecuted statements
            return Expression != null ? Expression.Eval(scope) : null;
        }
    }
}
