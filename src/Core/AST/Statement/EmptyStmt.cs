﻿//------------------------------------------------------------------------------
// <copyright file="EmptyStmt.cs">
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

    public class EmptyStmt : Statement
    {
        public EmptyStmt()
        {
        }

        public override object Eval(ExecutionContext context)
        {
            return null;
        }
    }
}
