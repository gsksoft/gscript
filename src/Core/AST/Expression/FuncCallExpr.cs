//------------------------------------------------------------------------------
// <copyright file="FuncCallExpr.cs">
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

    public class FuncCallExpr : Expression
    {
        public Expression FuncExpr { get; private set; }

        public List<Expression> Arguments { get; private set; }

        public FuncCallExpr(Expression funcExpr, List<Expression> arguments)
        {
            FuncExpr = funcExpr;
            Arguments = arguments;
        }

        public override object Eval(ExecutionContext context)
        {
            Function func = FuncExpr.Eval(context) as Function;
            if (func == null)
            {
                throw new GScriptException();
            }

            ExecutionContext newContext = context.CreateContextWithChildScope();
            Scope scope = newContext.Scope;
            var @params = func.Parameters;
            if (Arguments.Count != @params.Count)
            {
                throw new GScriptException();
            }
            
            for (int i = 0; i < Arguments.Count; i++)
            {
                var arg = Arguments[i];
                object argVal = arg.Eval(context);
                var param = @params[i];
                ThrowExceptionOnTypeNotMatch(argVal, param.Type);
                scope.DefineVariable(param.Name, argVal);
            }

            object retVal = null;
            try
            {
                retVal = func.Body.Eval(newContext);
            }
            catch (ValueReturnedException ex)
            {
                retVal = ex.ReturnValue;
            }

            if (func.ReturnType == VarType.Void)
            {
                return null;
            }

            ThrowExceptionOnTypeNotMatch(retVal, func.ReturnType);
            return retVal;
        }

        private void ThrowExceptionOnTypeNotMatch(object value, VarType type)
        {
            if (type == VarType.Integer && value.GetType() == typeof(int)) { }
            else if (type == VarType.Boolean && value.GetType() == typeof(bool)) { }
            else
            {
                throw new GScriptException();
            }
        }
    }
}
