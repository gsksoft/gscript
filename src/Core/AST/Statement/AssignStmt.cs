//------------------------------------------------------------------------------
// <copyright file="AssignStmt.cs">
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

    public class AssignStmt : Statement
    {
        public string Name { get; private set; }

        public Expression Expression { get; private set; }

        public AssignStmt(string name, Expression expression)
        {
            Name = name;
            Expression = expression;
        }

        public override object Eval(ExecutionContext context)
        {
            object value = Expression.Eval(context);
            context.Scope.SetValue(Name, value);
            return null;
        }
    }
}
