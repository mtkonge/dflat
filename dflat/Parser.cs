namespace DFLAT;

class Parser {
    private Lexer lexer;

    public Parser(Lexer lexer) {
        this.lexer = lexer;
    }

    public Expression parseExpr() {
        return lexer.peek().type switch {
            TokenType.If => parseIf(),
            TokenType.While => parseWhile(),
            TokenType.For => parseFor(),
            TokenType.LBrace => parseBlock(),
            _ => parseOperations(),
        };
    }

    private Expression parseIf() {
        return expressionError("if not implemented");
    }

    private Expression parseWhile() {
        return expressionError("while not implemented");
    }

    private Expression parseFor() {
        return expressionError("for not implemented");
    }

    private Expression parseBlock() {
        return expressionError("block not implemented");
    }

    private Expression parseOperations() {
        return expressionError("operations not implemented");
    }

    private Expression parseOperand() {
        return current().type switch {
            TokenType.Id => parseId(),
            TokenType.Int => parseInt(),
            TokenType.Float => parseFloat(),
            TokenType.Char => parseChar(),
            TokenType.String => parseString(),
            TokenType.False => parseBool(),
            TokenType.True => parseBool(),
            TokenType.Null => parseNull(),
            _ => expressionError("expected value"),
        };
    }

    private Expression parseId() => new IdExpression { value = current().value };

    private Expression parseInt() => new IntExpression { value = long.Parse(current().value) };

    private Expression parseFloat() => new FloatExpression { value = double.Parse(current().value) };

    private Expression parseChar() {
        // TODO escape chars
        return new CharExpression { value = current().value[1] };
    }

    private Expression parseString() {
        // TODO escape chars
        var value = current().value;
        return new StringExpression { value = value.Substring(1, value.Length - 2) };
    }

    private Expression parseBool() {
        return new BoolExpression { value = current().type == TokenType.True };
    }

    private Expression parseNull() => new NullExpression();

    private Token current() => this.lexer.peek();

    private void step() {
        this.lexer.next();
    }

    private bool currentIs(TokenType type) => current().type == type;

    private bool eat(TokenType type) {
        var isType = currentIs(type);
        if (isType)
            this.lexer.next();
        return isType;
    }

    private Expression expressionError(string message) {
        return new ErrorExpression {
            line = lexer.peek().line,
            column = lexer.peek().column,
            message = message,
        };
    }
}
