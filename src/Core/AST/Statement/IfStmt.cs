//------------------------------------------------------------------------------
// <copyright file="IfStmt.cs">
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

    public class IfStmt : Statement
    {
        public Expression Condition { get; private set; }

        public Statement ThenStatement { get; private set; }

        public Statement ElseStatement { get; private set; }

        public IfStmt(Expression condition, Statement thenStatement, Statement elseStatement)
        {
            Condition = condition;
            ThenStatement = thenStatement;
            ElseStatement = elseStatement;
        }

        public override object Eval(ExecutionContext context)
        {
            bool value = TypeHelper.GetValue<bool>(Condition.Eval(context));
            if (value)
            {
                ThenStatement.Eval(context);
            }
            else if (ElseStatement != null)
            {
                ElseStatement.Eval(context);
            }

            return null;
        }
    }
}
