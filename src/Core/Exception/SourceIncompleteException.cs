//------------------------------------------------------------------------------
// <copyright file="SourceIncompleteException.cs">
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

    public class SourceIncompleteException : GScriptException
    {
        public SourceIncompleteException()
            : base("Unknown exception.")
        {
        }

        public SourceIncompleteException(string message)
            : base(message)
        {
        }
    }
}
