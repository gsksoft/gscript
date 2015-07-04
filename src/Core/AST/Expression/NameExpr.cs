//------------------------------------------------------------------------------
// <copyright file="NameExpr.cs">
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

    public class NameExpr : Expression
    {
        public string Name { get; private set; }

        public NameExpr(string name)
        {
            Name = name;
        }

        public override object Eval(ExecutionContext context)
        {
            return context.Scope.GetValue(Name);
        }
    }
}
