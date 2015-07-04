//------------------------------------------------------------------------------
// <copyright file="ExecutionContext.cs">
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

    public class ExecutionContext
    {
        public ExecutionStatus Status { get; internal set; }

        public GScriptIO IO { get; private set; }

        public Scope Scope { get; private set; }

        private ExecutionContext() { }

        public static ExecutionContext CreateContext(GScriptIO io, Scope scope)
        {
            return new ExecutionContext() { IO = io, Scope = scope };
        }

        public ExecutionContext CreateContextWithChildScope()
        {
            Scope childScope = new Scope(Scope);
            return CreateContext(IO, childScope);
        }
    }
}
