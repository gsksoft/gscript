//------------------------------------------------------------------------------
// <copyright file="Token.cs">
//     Copyright (c) gsksoft. All rights reserved.
// </copyright>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Token
    {
        public TokenType Type { get; set; }

        public string Value { get; set; }
    }
}
