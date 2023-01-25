using System.Collections.Generic;

namespace DFLAT;

class Lexer : TokenIterator {
    private const string idChars = "abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ_";
    private readonly string text;
    private int index;
    private int column;
    private int line;

    private readonly Queue<Token> tokens = new() { };

    private bool done() => index >= this.text.Length;

    private void step() {
        index++;
        if (!done()) {
            if (this.text[index - 1] == '\n') {
                line++;
                column = 1;
            } else {
                column++;
            }
        }
    }

    private Token makeNumber() {
        var value = this.text[index].ToString();
        step();
        var dots = 0;
        while (!done() && (char.IsDigit(this.text[index]) || text[index] == '.')) {
            if (this.text[index] == '.' && dots < 1) {
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

    private TokenType identifierTokenType(string value) {
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

    private Token makeNameOrKeyword() {
        var value = this.text[index].ToString();
        step();
        while (!done() && (idChars.Contains(this.text[index]) || char.IsDigit(this.text[index]))) {
            value += this.text[index];
            step();
        }

        return new Token(identifierTokenType(value), value, column, line);
    }

    private Token singleChar(TokenType type) {
        var t = new Token(type, this.text[index].ToString(), column, line);
        step();
        return t;
    }

    private Token makeChar() {
        var value = text[index].ToString();
        step();
        if (done())
            return new Token(TokenType.Error, "unexpected end of char literal", column, line);
        if (text[index] == '\\') {
            value += text[index];
            step();
            if (done())
                return new Token(TokenType.Error, "unexpected end of char literal", column, line);
        }
        value += text[index];
        step();
        if (done())
            return new Token(TokenType.Error, "unexpected end of char literal", column, line);
        if (text[index] != '\'')
            return new Token(TokenType.Error, "expected `'` at end of char literal", column, line);
        value += text[index];
        step();
        return new Token(TokenType.Char, value, column, line);
    }

    private Token makeString() {
        var value = text[index].ToString();
        step();
        var escaped = false;
        while (!done() && !(!escaped && text[index] == '\"')) {
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

    private Token makeSingleOrDouble(
        TokenType caseSingle, TokenType caseDouble, char second) {
        var value = text[index].ToString();
        step();
        if (!done() && text[index] == second) {
            var t = new Token(caseDouble, value + text[index], column, index);
            step();
            return t;
        } else {
            return new Token(caseSingle, value, column, index);
        }
    }

    private Token makeSingleOrTwoDouble(TokenType caseSingle,
        TokenType caseDoubleA, char secondA,
        TokenType caseDoubleB, char secondB) {
        var value = text[index].ToString();
        var singleOrDoubleA = makeSingleOrDouble(caseSingle, caseDoubleA, secondA);
        if (singleOrDoubleA.type == caseSingle && !done()
            && text[index] == secondB) {
            var t = new Token(caseDoubleB, value + text[index], column, line);
            step();
            return t;
        } else {
            return singleOrDoubleA;
        }
    }

    private void tokenize() {
        if (this.text.Length == 0) {
            tokens.Enqueue(new Token(TokenType.Eof, "", column, line));
            return;
        }
        while (index < this.text.Length) {
            if (idChars.Contains(this.text[index])) {
                tokens.Enqueue(makeNameOrKeyword());
            } else if (char.IsDigit(this.text[index])) {
                tokens.Enqueue(makeNumber());
            } else if (this.text[index] == ' ') {
                step();
            } else {
                switch (this.text[index]) {
                    case '\'':
                        tokens.Enqueue(makeChar());
                        break;
                    case '"':
                        tokens.Enqueue(makeString());
                        break;
                    case '+':
                        tokens.Enqueue(makeSingleOrDouble(
                            TokenType.Plus, TokenType.PlusEqual, '='));
                        break;
                    case '-':
                        tokens.Enqueue(makeSingleOrTwoDouble(TokenType.Minus, TokenType.MinusEqual, '=', TokenType.ThinArrow, '>'));
                        break;
                    case '*':
                        tokens.Enqueue(makeSingleOrTwoDouble(TokenType.Asterisk, TokenType.AsteriskEqual, '=', TokenType.Exponentation, '*'));
                        break;
                    case '/':
                        pushSlashOrComment();
                        break;
                    case '%':
                        tokens.Enqueue(makeSingleOrDouble(
                            TokenType.Percent, TokenType.PercentEqual, '='));
                        break;
                    case '!':
                        tokens.Enqueue(makeSingleOrDouble(
                            TokenType.Error, TokenType.ExclamationEqual, '='));
                        break;
                    case '&':
                        tokens.Enqueue(singleChar(TokenType.Ampersand));
                        break;
                    case '<':
                        tokens.Enqueue(makeSingleOrDouble(
                            TokenType.Lt, TokenType.LtEqual, '='));
                        break;
                    case '>':
                        tokens.Enqueue(makeSingleOrDouble(
                            TokenType.Gt, TokenType.GtEqual, '='));
                        break;
                    case '=':
                        tokens.Enqueue(makeSingleOrDouble(
                            TokenType.Equal, TokenType.DoubleEqual, '='));
                        break;
                    case '(':
                        tokens.Enqueue(singleChar(TokenType.LParen));
                        break;
                    case ')':
                        tokens.Enqueue(singleChar(TokenType.RParen));
                        break;
                    case '{':
                        tokens.Enqueue(singleChar(TokenType.LBrace));
                        break;
                    case '}':
                        tokens.Enqueue(singleChar(TokenType.RBrace));
                        break;
                    case '[':
                        tokens.Enqueue(singleChar(TokenType.LBracket));
                        break;
                    case ']':
                        tokens.Enqueue(singleChar(TokenType.RBracket));
                        break;
                    case ',':
                        tokens.Enqueue(singleChar(TokenType.Comma));
                        break;
                    case ':':
                        tokens.Enqueue(singleChar(TokenType.Colon));
                        break;
                    case ';':
                        tokens.Enqueue(singleChar(TokenType.Semicolon));
                        break;
                    case '.':
                        tokens.Enqueue(singleChar(TokenType.Dot));
                        break;
                    default:
                        var errorMsg = "unexpected char '" + text[index] + "'";
                        tokens.Enqueue(new Token(TokenType.Error, errorMsg, column, line));
                        break;
                }
            }
        }
    }

    private void pushSlashOrComment() {
        var value = text[index].ToString();
        var currentColumn = this.column;
        var currentLine = this.line;
        step();
        if (!done() && text[index] == '/') {
            while (!done() && text[index] != '\n')
                step();
        } else if (!done() && text[index] == '*') {
            step();
            if (done())
                tokens.Enqueue(new Token(TokenType.Error, "unexpected end of multiline comment", column, line));
            var last = text[index];
            step();
            var nesting = 0;
            while (!done() && !(last == '*' && text[index] == '/' && nesting != 0)) {
                if (last == '/' && text[index] == '*') {
                    nesting++;
                } else if (last == '*' && text[index] == '/') {
                    nesting--;
                }
                last = text[index];
                step();
            }
            if (done() || last != '*' || text[index] != '/' || nesting != 0) {
                tokens.Enqueue(new Token(TokenType.Error, "unexpected end of multiline comment", column, line));
            }
            step();
        } else if (!done() && text[index] == '=') {
            tokens.Enqueue(new Token(TokenType.SlashEqual, value, currentColumn, currentLine));
            step();
        } else {
            tokens.Enqueue(new Token(TokenType.Slash, value, currentColumn, currentLine));
        }
    }

    public Lexer(string text) {
        this.text = text;
        this.index = 0;
        this.column = 1;
        this.line = 1;
        tokenize();
    }

    public Token next() => this.tokens.Dequeue();
    public Token peek() {
        if (this.tokens.Count > 0)
            return this.tokens.Peek();
        else
            return new Token(TokenType.Eof, "", line, column);
    }
    public Token[] collect() {
        var tokens = new List<Token>();
        while (peek().type != TokenType.Eof)
            tokens.Add(next());
        return tokens.ToArray();
    }

}
