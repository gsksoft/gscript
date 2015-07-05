//------------------------------------------------------------------------------
// <copyright file="DefFuncStmt.cs">
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

    using Parameter = Function.Parameter;

    public class DefFuncStmt : Statement
    {
        public FunctionType Type { get; private set; }

        public string Name { get; private set; }

        public Function Function { get; private set; }

        public DefFuncStmt(FunctionType type, string name, Function function)
        {
            Type = type;
            Name = name;
            Function = function;
        }

        public override object Eval(ExecutionContext context)
        {
            ThrowExceptionOnTypeNotMatch();
            context.Scope.DefineVariable(Name, Function);
            return null;
        }

        private void ThrowExceptionOnTypeNotMatch()
        {
            if (Function.ReturnType != Type.ReturnType)
            {
                throw new GScriptException();
            }

            var @params = Function.Parameters;
            var paramTypes = Type.ParameterTypes;
            if (@params.Count != paramTypes.Count)
            {
                throw new GScriptException();
            }

            for (int i = 0; i < @params.Count; i++)
            {
                if (@params[i].Type != paramTypes[i])
                {
                    throw new GScriptException();
                }
            }
        }
    }
}
