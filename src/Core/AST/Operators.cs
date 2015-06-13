//------------------------------------------------------------------------------
// <copyright file="Operators.cs">
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

    public enum UnaryOperator
    {
        Not,
        Negative
    }

    public enum BinaryOperator
    {
        And,
        Or,

        EQ,
        NE,
        LT,
        GT,
        LE,
        GE,

        Plus,
        Minus,
        Mul,
        Div
    }
}
