//------------------------------------------------------------------------------
// <copyright file="IntLiteral.cs">
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

    public class IntLiteral : LiteralExpr
    {
        public int Value { get; private set; }

        public IntLiteral(int value)
        {
            Value = value;
        }

        public override object Eval(Scope scope)
        {
            return Value;
        }
    }
}
