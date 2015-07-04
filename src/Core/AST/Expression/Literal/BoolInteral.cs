//------------------------------------------------------------------------------
// <copyright file="BoolInteral.cs">
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

    public class BoolInteral : LiteralExpr
    {
        public bool Value { get; private set; }

        public BoolInteral(bool value)
        {
            Value = value;
        }

        public override object Eval(ExecutionContext context)
        {
            return Value;
        }
    }
}
