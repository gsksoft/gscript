//------------------------------------------------------------------------------
// <copyright file="ValueReturnedException.cs">
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

    public class ValueReturnedException : GScriptException
    {
        public object ReturnValue { get; private set; }

        public ValueReturnedException()
        {
            ReturnValue = null;
        }

        public ValueReturnedException(object value)
        {
            ReturnValue = value;
        }
    }
}
