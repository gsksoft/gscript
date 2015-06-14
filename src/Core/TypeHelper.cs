//------------------------------------------------------------------------------
// <copyright file="TypeHelper.cs">
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

    internal static class TypeHelper
    {
        public static T GetValue<T>(object value) where T : struct
        {
            if (value.GetType() != typeof(T))
            {
                throw new GScriptException();
            }

            return (T)value;
        }
    }
}
