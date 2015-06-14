//------------------------------------------------------------------------------
// <copyright file="TokenType.cs" company="gsksoft">
//     Copyright gsksoft. All rights reserved.
// </copyright>
// <date>2015-05-31</date>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum TokenType
    {
        EOF,

        Id,

        // literals
        IntLiteral,

        // keywords
        If,
        Else,
        While,
        Define,
        Let,
        Print,
        Return,
        True,
        False,

        // types
        Int,
        Bool,

        // punctuations
        LCurly, // {
        RCurly, // }
        LParen, // (
        RParen, // )
        Colon,  // :
        Semi,   // ;

        // operators
        Assign, // =

        And,    // and
        Or,     // or
        Not,    // not

        EQ,     // ==
        NE,     // <>
        LT,     // <
        GT,     // >
        LE,     // <=
        GE,     // >=

        Plus,   // +
        Minus,  // -
        Mul,    // *
        Div,    // /
    }
}
