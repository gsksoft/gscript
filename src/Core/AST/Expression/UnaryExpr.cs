//------------------------------------------------------------------------------
// <copyright file="UnaryExpr.cs">
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

    public class UnaryExpr : Expression
    {
        public Expression Expression { get; private set; }

        public UnaryOperator Operator { get; private set; }

        public UnaryExpr(Expression expression, UnaryOperator op)
        {
            Expression = expression;
            Operator = op;
        }

        public override object Eval(ExecutionContext context)
        {
            object result = null;
            if (Operator == UnaryOperator.Not)
            {
                bool value = TypeHelper.GetValue<bool>(Expression.Eval(context));
                result = !value;
            }
            else if (Operator == UnaryOperator.Negative)
            {
                int value = TypeHelper.GetValue<int>(Expression.Eval(context));
                result = -value;
            }
            else
            {
                throw new GScriptException();
            }

            return result;
        }
    }
}
