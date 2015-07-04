//------------------------------------------------------------------------------
// <copyright file="DefVarStmt.cs">
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

    public class DefVarStmt : Statement
    {
        public VarType Type { get; private set; }

        public string Name { get; private set; }

        public Expression Initializer { get; private set; }

        public DefVarStmt(VarType type, string name, Expression initializer)
        {
            Type = type;
            Name = name;
            Initializer = initializer;
        }

        public override object Eval(ExecutionContext context)
        {
            object value = Initializer == null ? null : Initializer.Eval(context);
            if (value != null)
            {
                ThrowExceptionOnTypeError(value);
            }

            context.Scope.DefineVariable(Name, value);
            return null;
        }

        private void ThrowExceptionOnTypeError(object value)
        {
            if (Type == VarType.Integer && value.GetType() == typeof(int)) { }
            else if (Type == VarType.Boolean && value.GetType() == typeof(bool)) { }
            else
            {
                throw new GScriptException();
            }
        }
    }
}
