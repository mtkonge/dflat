using System;
using System.Collections.Generic;
using System.Text;

namespace dflat
{
    interface Lexer
    {
        Token next();
    }

    class LexerImplementation : Lexer
    {
        private const string DIGITS = "1234567890";
        private const string ID_CHARS = "abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ_";
        private string text;
        private int index;
        private int column;
        private int line;

        private Queue<Token> tokens = new Queue<Token> { };

        private bool done()
        {
            return index >= this.text.Length;
        }

        private void step()
        {
            index++;
            if (!done())
            {
                if (this.text[index - 1] == '\n')
                {
                    line++;
                    column = 1;
                }
                else
                {
                    column++;
                }
            }
        }

        private Token make_number()
        {
            string value = this.text[index].ToString();
            step();
            int dots = 0;
            while (!done() && (char.IsDigit(this.text[index])) || text[index] == '.')
            {
                if (this.text[index] == '.' && dots < 1)
                {
                    if (dots >= 1)
                        break;
                    dots++;
                }
                value += this.text[index];
                step();
            }
            var type = value.Contains('.') ? TokenType.Float : TokenType.Int;

            return new Token(type, value, column, line);
        }

        private TokenType identifier_token_type(string value)
        {
            if (value == "if")
                return TokenType.If;
            else if (value == "while")
                return TokenType.While;
            else if (value == "break")
                return TokenType.Break;
            else if (value == "fn")
                return TokenType.Fn;
            else if (value == "for")
                return TokenType.For;
            else if (value == "else")
                return TokenType.Else;
            else if (value == "continue")
                return TokenType.Continue;
            else if (value == "and")
                return TokenType.And;
            else if (value == "or")
                return TokenType.Or;
            else if (value == "in")
                return TokenType.In;
            else if (value == "not")
                return TokenType.Not;
            else if (value == "pub")
                return TokenType.Pub;
            else if (value == "new")
                return TokenType.New;
            else if (value == "class")
                return TokenType.Class;
            else if (value == "return")
                return TokenType.Return;
            else if (value == "let")
                return TokenType.Let;
            else if (value == "mut")
                return TokenType.Mut;
            else if (value == "false")
                return TokenType.False;
            else if (value == "true")
                return TokenType.True;
            else
                return TokenType.Id;
        }

        private Token make_name_or_keyword()
        {
            string value = this.text[index].ToString();
            step();
            while (!done() && (ID_CHARS.Contains(this.text[index])))
            {
                value += this.text[index];
                step();
            }

            return new Token(identifier_token_type(value), value, column, line);
        }

        private Token single_char(TokenType type)
        {
            var t = new Token(type, this.text[index].ToString(), column, line);
            step();
            return t;
        }

        private Token make_char()
        {
            string value = text[index].ToString();
            step();
            if (done())
            {
                return new Token(TokenType.Error, "unexpected end of char literal", column, line);
            }
            value += text[index];
            if (text[index] == '\\')
            {
                step();
                if (done())
                {
                    return new Token(TokenType.Error, "unexpected end of char literal", column, line);
                }
                value += text[index];
            }
            step();
            if (done())
            {
                return new Token(TokenType.Error, "unexpected end of char literal", column, line);
            }
            if (text[index] != '\'')
            {
                return new Token(TokenType.Error, "expected `'` at end of char literal", column, line);
            }
            value += text[index];
            step();
            return new Token(TokenType.Char, value, column, line);
        }

        private Token make_string()
        {
            string value = text[index].ToString();
            step();
            bool escaped = false;
            while (!done() && !(!escaped && text[index] == '\"'))
            {
                if (escaped)
                    escaped = false;
                else if (text[index] == '\\')
                    escaped = true;
                value += text[index];
                step();
            }
            if (done() || text[index] != '\"')
                return new Token(TokenType.Error, "expected `\"` at end of string literal", column, line);
            value += text[index];
            step();
            return new Token(TokenType.String, value, column, line);
        }

        private Token make_single_or_double(
            TokenType case_single, TokenType case_double, char second)
        {
            string value = text[index].ToString();
            step();
            if (!done() && text[index] == second)
            {
                var t = new Token(case_double, value + text[index], column, index);
                step();
                return t;
            }
            else
            {
                return new Token(case_single, value, column, index);
            }
        }

        private Token make_single_or_two_double(TokenType case_single,
            TokenType case_double_a, char second_a,
            TokenType case_double_b, char second_b)
        {
            var value = text[index].ToString();
            var single_or_double_a = make_single_or_double(case_single, case_double_a, second_a);
            if (single_or_double_a.type == case_single && !done()
                && text[index] == second_b)
            {
                var t = new Token(case_double_b, value + text[index], column, line);
                step();
                return t;
            }
            else
            {
                return single_or_double_a;
            }

        }


        private void tokenize()
        {
            if (this.text.Length == 0)
            {
                tokens.Enqueue(new Token(TokenType.Eof, "", column, line));
                return;
            }

            while (index < this.text.Length)
            {
                if (char.IsDigit(this.text[index]))
                {
                    tokens.Enqueue(make_number());
                }
                else if (ID_CHARS.Contains(this.text[index]))
                {
                    tokens.Enqueue(make_name_or_keyword());
                }
                else if (this.text[index] == ' ')
                {
                    step();
                }
                else
                {
                    switch (this.text[index])
                    {
                        case '\'': tokens.Enqueue(make_char()); break;
                        case '"': tokens.Enqueue(make_string()); break;
                        case '+': tokens.Enqueue(single_char(TokenType.Plus)); break;
                        case '-': tokens.Enqueue(make_single_or_two_double(TokenType.Plus, TokenType.MinusEqual, '=', TokenType.ThinArrow, '>')); break;
                        case '*':
                            tokens.Enqueue(make_single_or_double(
                                TokenType.Asterisk, TokenType.Exponentation, '*'));
                            break;
                        case '/': push_slash_or_comment(); break;
                        case '%': tokens.Enqueue(single_char(TokenType.Percent)); break;
                        case '!':
                            tokens.Enqueue(make_single_or_double(
                                TokenType.Error, TokenType.ExclamationEqual, '='));
                            break;
                        case '&': tokens.Enqueue(single_char(TokenType.Ampersand)); break;
                        case '<':
                            tokens.Enqueue(make_single_or_double(
                                TokenType.Lt, TokenType.LtEqual, '='));
                            break;
                        case '>':
                            tokens.Enqueue(make_single_or_double(
                                TokenType.Gt, TokenType.GtEqual, '='));
                            break;
                        case '=':
                            tokens.Enqueue(make_single_or_double(
                                TokenType.Equal, TokenType.DoubleEqual, '='));
                            break;
                        case '(':
                            tokens.Enqueue(single_char(TokenType.LParen));
                            break;
                        case ')':
                            tokens.Enqueue(single_char(TokenType.RParen));
                            break;
                        case '{':
                            tokens.Enqueue(single_char(TokenType.LBrace));
                            break;
                        case '}':
                            tokens.Enqueue(single_char(TokenType.RBrace));
                            break;
                        case '[':
                            tokens.Enqueue(single_char(TokenType.LBracket));
                            break;
                        case ']':
                            tokens.Enqueue(single_char(TokenType.RBracket));
                            break;
                        case ',':
                            tokens.Enqueue(single_char(TokenType.Comma));
                            break;
                        case ':':
                            tokens.Enqueue(single_char(TokenType.Colon));
                            break;
                        case ';':
                            tokens.Enqueue(single_char(TokenType.Semicolon));
                            break;
                        case '.':
                            tokens.Enqueue(single_char(TokenType.Dot));
                            break;
                        default:
                            string errorMsg = "unexpected char '" + text[index] + "'";
                            tokens.Enqueue(new Token(TokenType.Error, errorMsg, column, line));
                            break;
                    }

                }
            }
        }
        private void push_slash_or_comment()
        {
            var value = text[index].ToString();
            var current_column = this.column;
            var current_line = this.line;
            step();
            if (!done() && text[index] == '/')
            {
                while (!done() && text[index] != '\n')
                    step();
            }
            else if (!done() && text[index] == '*')
            {
                step();
                if (done())
                    tokens.Enqueue(new Token(TokenType.Error, "unexpected end of multiline comment", column, line));
                var last = text[index];
                step();
                while (!done() && !(last == '*' && text[index] == '/'))
                {
                    last = text[index];
                    step();
                }
                if (done() || last != '*' || text[index] != '/')
                {
                    tokens.Enqueue(new Token(TokenType.Error, "unexpected end of multiline comment", column, line));
                }
                step();
            }
            else
            {
                tokens.Enqueue(new Token(TokenType.Slash, value, current_column, current_line));
            }
        }



        public LexerImplementation(string text)
        {
            this.text = text;
            this.index = 0;
            this.column = 1;
            this.line = 1;
            this.tokenize();
        }


        public Token next()
        {
            return this.tokens.Dequeue();
        }

        public Token current()
        {
            return this.tokens.Peek();
        }
    }
}
