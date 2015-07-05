//------------------------------------------------------------------------------
// <copyright file="Function.cs">
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

    public class FunctionType
    {
        public List<VarType> ParameterTypes { get; set; }

        public VarType ReturnType { get; set; }
    }

    public class Function
    {
        public class Parameter
        {
            public VarType Type { get; set; }

            public string Name { get; set; }
        }

        public List<Parameter> Parameters { get; set; }

        public VarType ReturnType { get; set; }

        public BlockStmt Body { get; set; }
    }
}
