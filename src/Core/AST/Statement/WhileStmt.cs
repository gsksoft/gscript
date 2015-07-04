//------------------------------------------------------------------------------
// <copyright file="WhileStmt.cs">
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

    public class WhileStmt : Statement
    {
        public Expression Condition { get; private set; }

        public Statement Statement { get; private set; }

        public WhileStmt(Expression condition, Statement statement)
        {
            Condition = condition;
            Statement = statement;
        }

        public override object Eval(ExecutionContext context)
        {
            while (true)
            {
                bool value = TypeHelper.GetValue<bool>(Condition.Eval(context));
                if (!value)
                {
                    break;
                }

                Statement.Eval(context);
            }

            return null;
        }
    }
}
