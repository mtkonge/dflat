using System;
using System.Collections.Generic;
using System.Text;

namespace dflat {

    class Parser {
        private Lexer lexer;

        public Parser(Lexer lexer) {
            this.lexer = lexer;
        }

        public Expression parseExpr() {
            lexer.current().type switch {
                TokenType.If => this.parseIf(),
                TokenType.While => this.parseWhile(),
                TokenType.For => this.parseFor(),
                TokenType.LBrace => this.parseBlock(),
                _ => throw new ParserError("unexpected token"),
            };
        }

        private Expression parseIf() {
            return new ErrorExpression {
                line = lexer.current().line,
                column = lexer.current().column,
                message = "if not implemented",
            };
        }

        private Expression parseWhile() {
            return new ErrorExpression {
                line = lexer.current().line,
                column = lexer.current().column,
                message = "while not implemented",
            };
        }

        private Expression parseFor() {
            return new ErrorExpression {
                line = lexer.current().line,
                column = lexer.current().column,
                message = "for not implemented",
            };
        }

        private Expression parseBlock() {
            return new ErrorExpression {
                line = lexer.current().line,
                column = lexer.current().column,
                message = "block not implemented",
            };
        }

        private Expression parseOperations() {
            return new ErrorExpression {
                line = lexer.current().line,
                column = lexer.current().column,
                message = "operations not implemented",
            };
        }
    }

}
