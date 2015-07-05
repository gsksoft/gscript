//------------------------------------------------------------------------------
// <copyright file="TokenType.cs" company="gsksoft">
//     Copyright gsksoft. All rights reserved.
// </copyright>
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
        Function,

        // punctuations
        LCurly,     // {
        RCurly,     // }
        LParen,     // (
        RParen,     // )
        Colon,      // :
        Semi,       // ;
        Comma,      // ,
        RightArrow, // ->

        // operators
        Assign,     // =

        And,        // and
        Or,         // or
        Not,        // not

        EQ,         // ==
        NE,         // <>
        LT,         // <
        GT,         // >
        LE,         // <=
        GE,         // >=

        Plus,       // +
        Minus,      // -
        Mul,        // *
        Div,        // /
    }
}
