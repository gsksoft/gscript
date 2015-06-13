//------------------------------------------------------------------------------
// <copyright file="Node.cs">
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

    public abstract class Node
    {
        public virtual object Eval(Scope scope)
        {
            throw new NotImplementedException();
        }
    }
}
