using System;
using System.Collections.Generic;

namespace DFLAT;

class Parser {
    private TokenIterator tokens;

    public Parser(TokenIterator tokens) {
        this.tokens = tokens;
    }

    public Expression parseExpression(bool allowAssignment = false) => parseOperations(0, allowAssignment);

    private Expression parseOperations(int minimumBindingPower = 0, bool allowAssignment = false) {
        var left = ((Func<Expression>) (() => {
            if (current().type == TokenType.LParen) {
                step();
                var value = parseOperations();
                if (value.type() == ExpressionType.Error)
                    return value;
                if (current().type != TokenType.RParen)
                    return errorExpression("expected ')'");
                step();
                return value;
            }
            var prefixOperator = current();
            var prefixBP = prefixBindingPower(prefixOperator.type);
            if (prefixBP != null) {
                step();
                var right = parseOperations((int) prefixBP);
                return new UnaryExpression { subject = right, unaryType = unaryType(prefixOperator.type) };
            }
            return parseOperand();
        }))();
        if (left.type() == ExpressionType.Error)
            return left;
        while (true) {
            var op = current();

            var postfixBP = postfixBindingPower(op.type);
            if (postfixBP != null) {
                if (postfixBP < minimumBindingPower)
                    break;
                if (current().type == TokenType.Dot) {
                    step();
                    if (current().type != TokenType.Id)
                        return errorExpression("expected Id");
                    var value = current().value;
                    step();
                    left = new MemberExpression {
                        subject = left,
                        value = value,
                    };
                } else if (current().type == TokenType.LBracket) {
                    step();
                    var right = parseOperations(0);
                    if (right.type() == ExpressionType.Error)
                        return right;
                    if (current().type != TokenType.RBracket)
                        return errorExpression("expected ']'");
                    step();
                    left = new IndexExpression {
                        subject = left,
                        value = right,
                    };
                } else if (current().type == TokenType.LParen) {
                    step();
                    var arguments = new List<Expression>();
                    if (current().type != TokenType.RParen) {
                        arguments.Add(parseExpression());
                        while (current().type == TokenType.Comma) {
                            step();
                            if (current().type == TokenType.RParen)
                                break;
                            arguments.Add(parseExpression());
                        }
                        if (current().type != TokenType.RParen)
                            return errorExpression("expected ')'");
                    }
                    step();
                    left = new CallExpression { subject = left, arguments = arguments.ToArray() };
                } else {
                    throw new Exception("panic");
                }
                continue;
            }

            var bindingPower = infixBindingPower(op.type);
            if (bindingPower != null) {
                var (leftBP, rightBP) = ((int, int)) bindingPower;
                if (leftBP < minimumBindingPower)
                    break;
                step();
                var right = parseOperations(rightBP);
                if (right.type() == ExpressionType.Error)
                    return right;
                var assignType = this.assignType(op.type);
                if (assignType != null) {
                    if (!allowAssignment)
                        return errorExpression("assignment not allowed");
                    left = new AssignExpression { subject = left, value = right, assignType = (AssignType) assignType };
                } else {
                    left = new BinaryExpression { left = left, right = right, binaryType = binaryType(op.type) };
                }
                continue;
            }

            break;
        }
        return left;
    }

    private int? prefixBindingPower(TokenType tokenType) {
        return tokenType switch {
            TokenType.Not => 27,
            TokenType.Minus => 27,
            _ => null,
        };
    }

    private int? postfixBindingPower(TokenType tokenType) {
        return tokenType switch {
            TokenType.Dot => 33,
            TokenType.LBracket => 33,
            TokenType.LParen => 33,
            _ => null,
        };
    }

    private (int, int)? infixBindingPower(TokenType tokenType) {
        return tokenType switch {
            TokenType.And => (7, 8),
            TokenType.Or => (5, 6),
            TokenType.In => (17, 18),
            TokenType.Exponentation => (26, 25),
            TokenType.Plus => (21, 22),
            TokenType.Minus => (21, 22),
            TokenType.Asterisk => (23, 24),
            TokenType.Slash => (23, 24),
            TokenType.Percent => (23, 24),
            TokenType.DoubleEqual => (15, 16),
            TokenType.LtEqual => (17, 18),
            TokenType.Lt => (17, 18),
            TokenType.GtEqual => (17, 18),
            TokenType.Gt => (17, 18),
            TokenType.ExclamationEqual => (15, 16),

            TokenType.Equal => (4, 3),
            TokenType.PlusEqual => (4, 3),
            TokenType.MinusEqual => (4, 3),
            TokenType.AsteriskEqual => (4, 3),
            TokenType.SlashEqual => (4, 3),
            TokenType.PercentEqual => (4, 3),
            _ => null,
        };
    }

    private UnaryType unaryType(TokenType tokenType) {
        return tokenType switch {
            TokenType.Minus => UnaryType.Negate,
            TokenType.Not => UnaryType.Not,
            _ => throw new Exception("panic"),
        };
    }

    private AssignType? assignType(TokenType tokenType) {
        return tokenType switch {
            TokenType.Equal => AssignType.Assign,
            TokenType.PlusEqual => AssignType.Add,
            TokenType.MinusEqual => AssignType.Subtract,
            TokenType.AsteriskEqual => AssignType.Multiply,
            TokenType.SlashEqual => AssignType.Divide,
            TokenType.PercentEqual => AssignType.Modulus,
            _ => null,
        };
    }

    private BinaryType binaryType(TokenType tokenType) {
        return tokenType switch {
            TokenType.And => BinaryType.And,
            TokenType.In => BinaryType.In,
            TokenType.Or => BinaryType.Or,
            TokenType.Exponentation => BinaryType.Exponentation,
            TokenType.Plus => BinaryType.Add,
            TokenType.Minus => BinaryType.Subtract,
            TokenType.Asterisk => BinaryType.Multiply,
            TokenType.Slash => BinaryType.Divide,
            TokenType.Percent => BinaryType.Modulus,
            TokenType.DoubleEqual => BinaryType.Equal,
            TokenType.ExclamationEqual => BinaryType.NotEqual,
            TokenType.Lt => BinaryType.Lt,
            TokenType.LtEqual => BinaryType.LtEqual,
            TokenType.Gt => BinaryType.Gt,
            TokenType.GtEqual => BinaryType.GtEqual,
            _ => throw new Exception("panic"),
        };
    }

    private Expression parseOperand() {
        return tokens.peek().type switch {
            TokenType.If => parseIf(),
            TokenType.While => parseWhile(),
            TokenType.For => parseFor(),
            TokenType.LBrace => parseBlock(),
            _ => parseAtom(),
        };
    }

    private Expression parseIf() {
        return errorExpression("if not implemented");
    }

    private Expression parseWhile() {
        return errorExpression("while not implemented");
    }

    private Expression parseFor() {
        return errorExpression("for not implemented");
    }

    private Expression parseBlock() {
        return errorExpression("block not implemented");
    }

    private Expression parseAtom() {
        var value = current().type switch {
            TokenType.Id => parseId(),
            TokenType.Int => parseInt(),
            TokenType.Float => parseFloat(),
            TokenType.Char => parseChar(),
            TokenType.String => parseString(),
            TokenType.False => parseBool(),
            TokenType.True => parseBool(),
            TokenType.Null => parseNull(),
            _ => errorExpression("expected value"),
        };
        step();
        return value;
    }

    private Expression parseId() => new IdExpression { value = current().value };

    private Expression parseInt() => new IntExpression { value = long.Parse(current().value) };

    private Expression parseFloat() => new FloatExpression { value = double.Parse(current().value) };

    private Expression parseChar() {
        // TODO escape chars
        return new CharExpression { value = parseCharValue(current().value) };
    }

    private Expression parseString() {
        // TODO escape chars
        var value = current().value;
        return new StringExpression { value = parseStringValue(value) };
    }

    private Expression parseBool() {
        return new BoolExpression { value = current().type == TokenType.True };
    }

    private Expression parseNull() => new NullExpression();

    private Token current() => this.tokens.peek();

    private void step() { this.tokens.next(); }

    private bool currentIs(TokenType type) => current().type == type;

    private Expression errorExpression(string message) {
        return new ErrorExpression {
            line = tokens.peek().line,
            column = tokens.peek().column,
            message = message,
        };
    }

    private readonly Dictionary<char, char> charToEscapeCharMap = new(){
        {'n', '\n'},
        {'r', '\r'},
        {'t', '\t'},
        {'f', '\f'},
        {'v', '\v'},
    };

    private char parseCharValue(string message) {
        var withoutQuotes = message.Substring(1, message.Length - 2);
        var c = withoutQuotes[0];
        if (c == '\\') {
            c = withoutQuotes[1];
            if (this.charToEscapeCharMap.ContainsKey(c))
                c = charToEscapeCharMap[c];
        }
        return c;
    }

    private string parseStringValue(string message) {
        var withoutQuotes = message.Substring(1, message.Length - 2);
        var result = new System.Text.StringBuilder();
        for (int i = 0; i < withoutQuotes.Length; i++) {
            char c = withoutQuotes[i];
            if (c == '\\') {
                i++;
                c = withoutQuotes[i];
                if (this.charToEscapeCharMap.ContainsKey(c))
                    c = charToEscapeCharMap[c];
            }
            result.Append(c);
        }
        return result.ToString();
    }
}
