//------------------------------------------------------------------------------
// <copyright file="GScriptIO.cs">
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

    public class GScriptIO
    {
        private Action<object> m_outputAction;

        private GScriptIO(Action<object> outputAction)
        {
            m_outputAction = outputAction;
        }

        public static GScriptIO Create()
        {
            return new GScriptIO(Console.WriteLine);
        }

        public static GScriptIO Create(Action<object> outputAction)
        {
            return new GScriptIO(outputAction);
        }

        public void Output(object o)
        {
            m_outputAction(o);
        }
    }
}
