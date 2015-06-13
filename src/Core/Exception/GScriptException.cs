//------------------------------------------------------------------------------
// <copyright file="GScriptException.cs">
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

    public class GScriptException : Exception
    {
        public GScriptException()
            : base("Unknown exception.")
        {
        }

        public GScriptException(string message)
            : base(message)
        {
        }
    }
}
