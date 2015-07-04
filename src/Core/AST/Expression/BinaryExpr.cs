//------------------------------------------------------------------------------
// <copyright file="BinaryExpr.cs">
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

    public class BinaryExpr : Expression
    {
        public Expression Left { get; private set; }

        public Expression Right { get; private set; }

        public BinaryOperator Operator { get; private set; }

        public BinaryExpr(Expression left, Expression right, BinaryOperator op)
        {
            Left = left;
            Right = right;
            Operator = op;
        }

        public override object Eval(ExecutionContext context)
        {
            object result = null;
            bool isLogicalOperator = Operator == BinaryOperator.And || Operator == BinaryOperator.Or;
            if (isLogicalOperator)
            {
                bool leftValue = TypeHelper.GetValue<bool>(Left.Eval(context));
                bool rightValue = TypeHelper.GetValue<bool>(Right.Eval(context));
                switch (Operator)
                {
                    case BinaryOperator.And:
                        result = leftValue && rightValue;
                        break;
                    case BinaryOperator.Or:
                        result = leftValue || rightValue;
                        break;
                    default:
                        throw new GScriptException(); // unreachable
                }
            }
            else
            {
                int leftValue = TypeHelper.GetValue<int>(Left.Eval(context));
                int rightValue = TypeHelper.GetValue<int>(Right.Eval(context));
                switch (Operator)
                {
                    case BinaryOperator.EQ:
                        result = leftValue == rightValue;
                        break;
                    case BinaryOperator.NE:
                        result = leftValue != rightValue;
                        break;
                    case BinaryOperator.GT:
                        result = leftValue > rightValue;
                        break;
                    case BinaryOperator.LT:
                        result = leftValue < rightValue;
                        break;
                    case BinaryOperator.GE:
                        result = leftValue >= rightValue;
                        break;
                    case BinaryOperator.LE:
                        result = leftValue <= rightValue;
                        break;
                    case BinaryOperator.Plus:
                        result = leftValue + rightValue;
                        break;
                    case BinaryOperator.Minus:
                        result = leftValue - rightValue;
                        break;
                    case BinaryOperator.Mul:
                        result = leftValue * rightValue;
                        break;
                    case BinaryOperator.Div:
                        result = leftValue / rightValue;
                        break;
                    default:
                        throw new GScriptException();
                }
            }

            return result;
        }
    }
}
