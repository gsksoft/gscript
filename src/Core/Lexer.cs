//------------------------------------------------------------------------------
// <copyright file="Lexer.cs">
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

    public class Lexer
    {
        private static readonly Dictionary<string, TokenType> s_keywordMapping;

        private string m_source;
        private int m_length;
        private int m_position;

        static Lexer()
        {
            s_keywordMapping = new Dictionary<string, TokenType>();
            s_keywordMapping["if"] = TokenType.If;
            s_keywordMapping["else"] = TokenType.Else;
            s_keywordMapping["while"] = TokenType.While;
            s_keywordMapping["define"] = TokenType.Define;
            s_keywordMapping["def"] = TokenType.Define;
            s_keywordMapping["let"] = TokenType.Let;
            s_keywordMapping["print"] = TokenType.Print;
            s_keywordMapping["return"] = TokenType.Return;
            s_keywordMapping["ret"] = TokenType.Return;
            s_keywordMapping["true"] = TokenType.True;
            s_keywordMapping["false"] = TokenType.False;
            s_keywordMapping["integer"] = TokenType.Int;
            s_keywordMapping["int"] = TokenType.Int;
            s_keywordMapping["boolean"] = TokenType.Bool;
            s_keywordMapping["bool"] = TokenType.Bool;
            s_keywordMapping["and"] = TokenType.And;
            s_keywordMapping["or"] = TokenType.Or;
            s_keywordMapping["not"] = TokenType.Not;
        }

        public List<Token> Scan(string source)
        {
            m_source = source;
            m_length = source.Length;
            m_position = 0;

            List<Token> tokens = new List<Token>();

            Token token;
            do
            {
                token = GetNextToken();
                tokens.Add(token);
            } while (token.Type != TokenType.EOF);

            return tokens;
        }

        private Token GetNextToken()
        {
            SkipWhiteSpaceChars();

            char c = PeekChar();
            if (c == '\0')
            {
                return GetEofToken();
            }

            if (Char.IsLetter(c) || CharEx.IsUnderscore(c))
            {
                return GetKeywordOrIdentifierToken();
            }

            if (Char.IsDigit(c))
            {
                return GetNumberToken();
            }

            return GetSymbolToken();
        }

        private Token GetEofToken()
        {
            return new Token() { Type = TokenType.EOF };
        }

        private Token GetKeywordOrIdentifierToken()
        {
            StringBuilder valueBuilder = new StringBuilder();

            char c = PeekChar();
            do
            {
                valueBuilder.Append(c);
                Forward();
                c = PeekChar();
            } while (Char.IsLetterOrDigit(c) || CharEx.IsUnderscore(c));

            string tokenValue = valueBuilder.ToString();

            Token token = new Token();
            token.Type = LookupTokenType(tokenValue);
            token.Value = tokenValue;
            return token;
        }

        private TokenType LookupTokenType(string tokenValue)
        {
            return s_keywordMapping.ContainsKey(tokenValue) ? s_keywordMapping[tokenValue] : TokenType.Id;
        }

        private Token GetNumberToken()
        {
            StringBuilder valueBuilder = new StringBuilder();

            char c = PeekChar();
            do
            {
                valueBuilder.Append(c);
                Forward();
                c = PeekChar();
            } while (Char.IsDigit(c));

            string tokenValue = valueBuilder.ToString();

            Token token = new Token();
            token.Type = TokenType.IntLiteral;
            token.Value = tokenValue;
            return token;
        }

        private Token GetSymbolToken()
        {
            Token token = new Token();
            char c = PeekChar();
            switch (c)
            {
                case '{':
                    Forward();
                    token.Type = TokenType.LCurly;
                    break;
                case '}':
                    Forward();
                    token.Type = TokenType.RCurly;
                    break;
                case '(':
                    Forward();
                    token.Type = TokenType.LParen;
                    break;
                case ')':
                    Forward();
                    token.Type = TokenType.RParen;
                    break;
                case ':':
                    Forward();
                    token.Type = TokenType.Colon;
                    break;
                case ';':
                    Forward();
                    token.Type = TokenType.Semi;
                    break;
                case '=':
                    Forward();
                    c = PeekChar();
                    if (c == '=')
                    {
                        Forward();
                        token.Type = TokenType.EQ;
                    }
                    else
                    {
                        token.Type = TokenType.Assign;
                    }
                    break;
                case '<':
                    Forward();
                    c = PeekChar();
                    if (c == '=')
                    {
                        Forward();
                        token.Type = TokenType.LE;
                    }
                    else if (c == '>')
                    {
                        Forward();
                        token.Type = TokenType.NE;
                    }
                    else
                    {
                        token.Type = TokenType.LT;
                    }
                    break;
                case '>':
                    Forward();
                    c = PeekChar();
                    if (c == '=')
                    {
                        Forward();
                        token.Type = TokenType.GE;
                    }
                    else
                    {
                        token.Type = TokenType.GT;
                    }
                    break;
                case '+':
                    Forward();
                    token.Type = TokenType.Plus;
                    break;
                case '-':
                    Forward();
                    token.Type = TokenType.Minus;
                    break;
                case '*':
                    Forward();
                    token.Type = TokenType.Mul;
                    break;
                case '/':
                    Forward();
                    token.Type = TokenType.Div;
                    break;
                default:
                    throw new GScriptException();
            }
            return token;
        }

        private void SkipWhiteSpaceChars()
        {
            while (Char.IsWhiteSpace(PeekChar()))
            {
                Forward();
            }
        }

        private char PeekChar()
        {
            return m_position < m_length ? m_source[m_position] : '\0';
        }

        private void Forward()
        {
            ++m_position;
        }

        public static class CharEx
        {
            public static bool IsUnderscore(char c)
            {
                return c == '_';
            }

            public static bool IsPoint(char c)
            {
                return c == '.';
            }
        }
    }
}
