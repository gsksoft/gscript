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

    using Parameter = AST.Function.Parameter;

    public class Parser
    {
        private static readonly Dictionary<TokenType, UnaryOperator> s_unaryOperatorMapping;
        private static readonly Dictionary<TokenType, BinaryOperator> s_binaryOperatorMapping;
        private static readonly Dictionary<TokenType, VarType> s_varTypeMapping;

        private List<Token> m_tokens;
        private int m_tokenCount;
        private int m_position;

        private int m_currentLevel;
        private bool m_inconclusive;

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

            s_varTypeMapping = new Dictionary<TokenType, VarType>();
            s_varTypeMapping[TokenType.Int] = VarType.Integer;
            s_varTypeMapping[TokenType.Bool] = VarType.Boolean;
        }

        private static UnaryOperator LookupUnaryOperator(TokenType type)
        {
            if (s_unaryOperatorMapping.ContainsKey(type))
            {
                return s_unaryOperatorMapping[type];
            }

            throw new GScriptException();
        }

        private static BinaryOperator LookupBinaryOperator(TokenType type)
        {
            if (s_binaryOperatorMapping.ContainsKey(type))
            {
                return s_binaryOperatorMapping[type];
            }

            throw new GScriptException();
        }

        private static VarType LookupVarType(TokenType type)
        {
            if (s_varTypeMapping.ContainsKey(type))
            {
                return s_varTypeMapping[type];
            }

            throw new GScriptException();
        }

        public CompilationUnit Parse(List<Token> tokens)
        {
            m_tokens = tokens;
            m_tokenCount = tokens.Count;
            m_position = 0;
            m_currentLevel = 0;
            m_inconclusive = false;

            return ParseCompilationUnit();
        }

        public ParseStatus TryParse(List<Token> tokens, out CompilationUnit node)
        {
            node = null;
            try
            {
                node = Parse(tokens);
                return m_inconclusive ? ParseStatus.Inconclusive : ParseStatus.Completed;
            }
            catch (SourceIncompleteException)
            {
                return ParseStatus.Incomplete;
            }
            catch
            {
                return ParseStatus.Error;
            }
        }

        private CompilationUnit ParseCompilationUnit()
        {
            List<Statement> statements = new List<Statement>();
            while (PeekToken().Type != TokenType.EOF)
            {
                statements.Add(ParseStatement());
            }

            return new CompilationUnit(statements);
        }

        private Statement ParseStatement()
        {
            ++m_currentLevel;

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
                case TokenType.Id:
                    statement = ParseFuncCallStatement();
                    break;
                default:
                    if (token.Type == TokenType.EOF && m_currentLevel != 0)
                    {
                        throw new SourceIncompleteException();
                    }
                    else
                    {
                        throw new GScriptException();
                    }
            }

            --m_currentLevel;

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
            m_inconclusive = false;
            if (TryConsumeToken(TokenType.Else))
            {
                elseStatement = ParseStatement();
            }
            else
            {
                if (PeekToken().Type == TokenType.EOF)
                {
                    m_inconclusive = true;
                }
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
            ConsumeToken(TokenType.Define);
            Token token = PeekToken();
            if (token.Type == TokenType.LParen)
            {
                return ParseDefineFunctionStatement();
            }
            else
            {
                return ParseDefineVariableStatement();
            }
        }

        private DefVarStmt ParseDefineVariableStatement()
        {
            // ConsumeToken(TokenType.Define);

            Token token = PeekToken();
            VarType varType = LookupVarType(token.Type);

            Forward(); // type

            string varName = ParseId();

            Expression initializer = null;
            if (TryConsumeToken(TokenType.Colon))
            {
                initializer = ParseExpression();
            }

            ConsumeToken(TokenType.Semi);

            return new DefVarStmt(varType, varName, initializer);
        }

        private DefFuncStmt ParseDefineFunctionStatement()
        {
            // ConsumeToken(TokenType.Define);
            FunctionType funcType = ParseFunctionType();
            string funcName = ParseId();
            ConsumeToken(TokenType.Colon);
            Function func = ParseFunction();
            ConsumeToken(TokenType.Semi);
            return new DefFuncStmt(funcType, funcName, func);
        }

        private FunctionType ParseFunctionType()
        {
            FunctionType funcType = new FunctionType();

            ConsumeToken(TokenType.LParen);
            ConsumeToken(TokenType.Function);
            ConsumeToken(TokenType.LParen);

            var paramTypes = new List<VarType>();
            Token token = MatchTokenIn(
                TokenType.Int, TokenType.Bool);
            if (token != null)
            {
                var paramType = LookupVarType(token.Type);
                paramTypes.Add(paramType);
                while (TryConsumeToken(TokenType.Comma))
                {
                    token = PeekToken();
                    paramType = LookupVarType(token.Type);
                    paramTypes.Add(paramType);
                    Forward();
                }
            }

            funcType.ParameterTypes = paramTypes;

            ConsumeToken(TokenType.RParen);

            VarType returnType = VarType.Void;
            if (TryConsumeToken(TokenType.RightArrow))
            {
                token = PeekToken();
                returnType = LookupVarType(token.Type);
                Forward();
            }

            funcType.ReturnType = returnType;

            ConsumeToken(TokenType.RParen);

            return funcType;
        }

        private Function ParseFunction()
        {
            Function func = new Function();

            ConsumeToken(TokenType.Function);
            ConsumeToken(TokenType.LParen);

            var @params = new List<Parameter>();
            Token token = MatchTokenIn(
                TokenType.Int, TokenType.Bool);
            if (token != null)
            {
                VarType paramType = LookupVarType(token.Type);
                string varName = ParseId();
                @params.Add(new Parameter() { Type = paramType, Name = varName });

                while (TryConsumeToken(TokenType.Comma))
                {
                    token = PeekToken();
                    paramType = LookupVarType(token.Type);
                    Forward();

                    varName = ParseId();
                    @params.Add(new Parameter() { Type = paramType, Name = varName });
                }
            }

            func.Parameters = @params;

            ConsumeToken(TokenType.RParen);

            VarType returnType = VarType.Void;
            if (TryConsumeToken(TokenType.RightArrow))
            {
                token = PeekToken();
                returnType = LookupVarType(token.Type);
                Forward();
            }

            func.ReturnType = returnType;

            // support interactive parsing
            {
                ++m_currentLevel;
                BlockStmt body = ParseBlockStatement();
                func.Body = body;
                --m_currentLevel;
            }

            return func;
        }

        private string ParseId()
        {
            Token token = PeekToken();
            if (token.Type != TokenType.Id)
            {
                throw new GScriptException();
            }

            Forward();
            return token.Value;
        }

        private AssignStmt ParseAssignStatement()
        {
            ConsumeToken(TokenType.Let);

            string name = ParseId();

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

        private FuncCallStmt ParseFuncCallStatement()
        {
            NameExpr idExpr = new NameExpr(ParseId());

            ConsumeToken(TokenType.LParen);
            List<Expression> args = ParseArgumentList();
            FuncCallExpr expr = new FuncCallExpr(idExpr, args);
            ConsumeToken(TokenType.RParen);

            while (TryConsumeToken(TokenType.LParen))
            {
                args = ParseArgumentList();
                expr = new FuncCallExpr(expr, args);
                ConsumeToken(TokenType.RParen);
            }

            ConsumeToken(TokenType.Semi);

            return new FuncCallStmt(expr);
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

            return ParsePostfixExpression();
        }

        private Expression ParsePostfixExpression()
        {
            Expression expr = ParsePrimaryExpression();
            while (TryConsumeToken(TokenType.LParen))
            {
                List<Expression> args = ParseArgumentList();
                expr = new FuncCallExpr(expr, args);
                ConsumeToken(TokenType.RParen);
            }

            return expr;
        }

        private List<Expression> ParseArgumentList()
        {
            List<Expression> argList = new List<Expression>();

            Token token = PeekToken();
            if (token.Type == TokenType.RParen)
            {
                return argList;
            }

            Expression expr = ParseExpression();
            argList.Add(expr);

            while (TryConsumeToken(TokenType.Comma))
            {
                expr = ParseExpression();
                argList.Add(expr);
            }

            return argList;
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
                string msg = string.Format("{0} token expected", type);
                if (m_currentLevel > 0)
                {
                    throw new SourceIncompleteException(msg);
                }

                throw new GScriptException(msg);
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

        private Token PeekToken()
        {
            if (m_position >= m_tokenCount)
            {
                if (m_currentLevel != 0)
                {
                    throw new SourceIncompleteException();
                }

                return null;
            }

            return m_tokens[m_position];
        }

        private void Forward()
        {
            ++m_position;
        }
    }
}
