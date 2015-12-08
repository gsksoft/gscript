//------------------------------------------------------------------------------
// <copyright file="FuncInteral.cs">
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

    public class FuncInteral : LiteralExpr
    {
        public Function Value { get; private set; }

        public FuncInteral(Function value)
        {
            Value = value;
        }

        public override object Eval(ExecutionContext context)
        {
            return Value;
        }
    }
}
