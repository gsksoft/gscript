//------------------------------------------------------------------------------
// <copyright file="Parser.cs">
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
    
    using Gsksoft.GScript.Core.AST;

    public class Parser
    {
        private static readonly Dictionary<TokenType, UnaryOperator> s_unaryOperatorMapping;
        private static readonly Dictionary<TokenType, BinaryOperator> s_binaryOperatorMapping;

        private List<Token> m_tokens;
        private int m_tokenCount;
        private int m_position;

        static Parser()
        {
            s_unaryOperatorMapping = new Dictionary<TokenType, UnaryOperator>();
            s_unaryOperatorMapping[TokenType.Not] = UnaryOperator.Not;
            s_unaryOperatorMapping[TokenType.Minus] = UnaryOperator.Negative;

            s_binaryOperatorMapping = new Dictionary<TokenType, BinaryOperator>();
            s_binaryOperatorMapping[TokenType.EQ] = BinaryOperator.EQ;
            s_binaryOperatorMapping[TokenType.NE] = BinaryOperator.NE;
            s_binaryOperatorMapping[TokenType.GT] = BinaryOperator.GT;
            s_binaryOperatorMapping[TokenType.LT] = BinaryOperator.LT;
            s_binaryOperatorMapping[TokenType.GE] = BinaryOperator.GE;
            s_binaryOperatorMapping[TokenType.LE] = BinaryOperator.LE;
            s_binaryOperatorMapping[TokenType.Plus] = BinaryOperator.Plus;
            s_binaryOperatorMapping[TokenType.Minus] = BinaryOperator.Minus;
            s_binaryOperatorMapping[TokenType.Mul] = BinaryOperator.Mul;
            s_binaryOperatorMapping[TokenType.Div] = BinaryOperator.Div;
        }

        private static UnaryOperator LookupUnaryOperator(TokenType type)
        {
            return s_unaryOperatorMapping[type];
        }

        private static BinaryOperator LookupBinaryOperator(TokenType type)
        {
            return s_binaryOperatorMapping[type];
        }

        public Node Parse(List<Token> tokens)
        {
            m_tokens = tokens;
            m_tokenCount = tokens.Count;
            m_position = 0;
            return ParseProgram();
        }

        private Program ParseProgram()
        {
            List<Statement> statements = new List<Statement>();
            while (PeekToken().Type != TokenType.EOF)
            {
                statements.Add(ParseStatement());
            }

            return new Program(statements);
        }

        private Statement ParseStatement()
        {
            Statement statement = null;
            Token token = PeekToken();
            switch (token.Type)
            {
                case TokenType.Semi:
                    statement = ParseEmptyStatement();
                    break;
                case TokenType.LCurly:
                    statement = ParseBlockStatement();
                    break;
                case TokenType.If:
                    statement = ParseIfStatement();
                    break;
                case TokenType.While:
                    statement = ParseWhileStatement();
                    break;
                case TokenType.Define:
                    statement = ParseDefineStatement();
                    break;
                case TokenType.Let:
                    statement = ParseAssignStatement();
                    break;
                case TokenType.Print:
                    statement = ParsePrintStatement();
                    break;
                case TokenType.Return:
                    statement = ParseReturnStatement();
                    break;
                default:
                    throw new GScriptException();
            }

            return statement;
        }

        private EmptyStmt ParseEmptyStatement()
        {
            ConsumeToken(TokenType.Semi);
            return new EmptyStmt();
        }

        private BlockStmt ParseBlockStatement()
        {
            List<Statement> statements = new List<Statement>();

            ConsumeToken(TokenType.LCurly);
            while (!TryConsumeToken(TokenType.RCurly))
            {
                statements.Add(ParseStatement());
            }

            return new BlockStmt(statements);
        }

        private IfStmt ParseIfStatement()
        {
            ConsumeToken(TokenType.If);
            ConsumeToken(TokenType.LParen);
            Expression condition = ParseExpression();
            ConsumeToken(TokenType.RParen);
            Statement thenStatement = ParseStatement();
            Statement elseStatement = null;
            if (TryConsumeToken(TokenType.Else))
            {
                elseStatement = ParseStatement();
            }

            return new IfStmt(condition, thenStatement, elseStatement);
        }

        private WhileStmt ParseWhileStatement()
        {
            ConsumeToken(TokenType.While);
            ConsumeToken(TokenType.LParen);
            Expression condition = ParseExpression();
            ConsumeToken(TokenType.RParen);
            Statement statement = ParseStatement();
            return new WhileStmt(condition, statement);
        }

        private Statement ParseDefineStatement()
        {
            return ParseDefineVariableStatement();
        }

        private DefVarStmt ParseDefineVariableStatement()
        {
            ConsumeToken(TokenType.Define);

            Token token = PeekToken();
            VarType varType = VarType.None;
            if (token.Type == TokenType.Int)
            {
                varType = VarType.Integer;
            }
            else if (token.Type == TokenType.Bool)
            {
                varType = VarType.Boolean;
            }
            else
            {
                throw new GScriptException();
            }

            Forward(); // type

            token = PeekToken();
            if (token.Type != TokenType.Id)
            {
                throw new GScriptException();
            }

            string varName = token.Value;
            Forward(); // id

            Expression initializer = null;
            if (TryConsumeToken(TokenType.Colon))
            {
                initializer = ParseExpression();
            }

            ConsumeToken(TokenType.Semi);

            return new DefVarStmt(varType, varName, initializer);
        }

        private AssignStmt ParseAssignStatement()
        {
            ConsumeToken(TokenType.Let);

            Token token = PeekToken();
            if (token.Type != TokenType.Id)
            {
                throw new GScriptException();
            }

            string name = token.Value;
            Forward();

            ConsumeToken(TokenType.Assign);
            Expression expr = ParseExpression();
            ConsumeToken(TokenType.Semi);

            return new AssignStmt(name, expr);
        }

        private PrintStmt ParsePrintStatement()
        {
            ConsumeToken(TokenType.Print);
            Expression expr = ParseExpression();
            ConsumeToken(TokenType.Semi);
            return new PrintStmt(expr);
        }

        private ReturnStmt ParseReturnStatement()
        {
            ConsumeToken(TokenType.Return);
            Expression expr = ParseExpression();
            ConsumeToken(TokenType.Semi);
            return new ReturnStmt(expr);
        }

        private Expression ParseExpression()
        {
            return ParseLogicalOrExpression();
        }

        private Expression ParseLogicalOrExpression()
        {
            Expression expr = ParseLogicalAndExpression();
            while (TryConsumeToken(TokenType.Or))
            {
                Expression expr_ = ParseLogicalAndExpression();
                expr = new BinaryExpr(expr, expr_, BinaryOperator.Or);
            }

            return expr;
        }

        private Expression ParseLogicalAndExpression()
        {
            Expression expr = ParseRelationalExpression();
            while (TryConsumeToken(TokenType.And))
            {
                Expression expr_ = ParseRelationalExpression();
                expr = new BinaryExpr(expr, expr_, BinaryOperator.And);
            }

            return expr;
        }

        private Expression ParseRelationalExpression()
        {
            Expression expr = ParseAdditiveExpression();

            while (true)
            {
                Token token = MatchTokenIn(
                    TokenType.EQ, TokenType.NE, TokenType.GT, TokenType.LT, TokenType.GE, TokenType.LE);
                if (token != null)
                {
                    Expression expr_ = ParseAdditiveExpression();
                    BinaryOperator optr = LookupBinaryOperator(token.Type);
                    expr = new BinaryExpr(expr, expr_, optr);
                }
                else
                {
                    break;
                }
            }

            return expr;
        }

        private Expression ParseAdditiveExpression()
        {
            Expression expr = ParseMultiplicativeExpression();

            while (true)
            {
                Token token = MatchTokenIn(TokenType.Plus, TokenType.Minus);
                if (token != null)
                {
                    Expression expr_ = ParseMultiplicativeExpression();
                    BinaryOperator optr = LookupBinaryOperator(token.Type);
                    expr = new BinaryExpr(expr, expr_, optr);
                }
                else
                {
                    break;
                }
            }

            return expr;
        }

        private Expression ParseMultiplicativeExpression()
        {
            Expression expr = ParseUnaryExpression();

            while (true)
            {
                Token token = MatchTokenIn(TokenType.Mul, TokenType.Div);
                if (token != null)
                {
                    Expression expr_ = ParseUnaryExpression();
                    BinaryOperator optr = LookupBinaryOperator(token.Type);
                    expr = new BinaryExpr(expr, expr_, optr);
                }
                else
                {
                    break;
                }
            }

            return expr;
        }

        private Expression ParseUnaryExpression()
        {
            Token token = MatchTokenIn(TokenType.Not, TokenType.Minus);
            if (token != null)
            {
                Expression expr = ParseUnaryExpression();
                UnaryOperator optr = LookupUnaryOperator(token.Type);
                return new UnaryExpr(expr, optr);
            }

            return ParsePrimaryExpression();
        }

        private Expression ParsePrimaryExpression()
        {
            Token token = PeekToken();
            if (token.Type == TokenType.Id)
            {
                Forward();
                return new NameExpr(token.Value);
            }

            if (token.Type == TokenType.IntLiteral)
            {
                Forward();
                int value = Convert.ToInt32(token.Value);
                return new IntLiteral(value);
            }

            if (token.Type == TokenType.True || token.Type == TokenType.False)
            {
                Forward();
                bool value = Convert.ToBoolean(token.Type.ToString());
                return new BoolInteral(value);
            }

            if (token.Type == TokenType.LParen)
            {
                Forward();
                Expression expr = ParseExpression();
                ConsumeToken(TokenType.RParen);
                return expr;
            }

            throw new GScriptException();
        }

        private void ConsumeToken(TokenType type)
        {
            Token token = PeekToken();
            if (token.Type == type)
            {
                Forward();
            }
            else
            {
                throw new GScriptException();
            }
        }

        private bool TryConsumeToken(TokenType type)
        {
            Token token = PeekToken();
            if (token.Type == type)
            {
                Forward();
                return true;
            }

            return false;
        }

        private Token MatchTokenIn(params TokenType[] types)
        {
            Token token = PeekToken();
            if (types.Any(type => token.Type == type))
            {
                Forward();
                return token;
            }

            return null;
        }

        private Token PeekToken(int offset = 0)
        {
            return m_position + offset < m_tokenCount ? m_tokens[m_position + offset] : null;
        }

        private void Forward(int offset = 1)
        {
            m_position += offset;
        }
    }
}
