using System;
using System.Collections.Generic;
using System.Text;

namespace DFLAT {

    class Parser {
        private Lexer lexer;

        public Parser(Lexer lexer) {
            this.lexer = lexer;
        }

        public Expression parseExpr() {
            return lexer.current().type switch {
                TokenType.If => this.parseIf(),
                TokenType.While => this.parseWhile(),
                TokenType.For => this.parseFor(),
                TokenType.LBrace => this.parseBlock(),
                _ => this.parseOperations(),
            };
        }

        private Expression parseIf() {
            return this.error("if not implemented");
        }

        private Expression parseWhile() {
            return this.error("while not implemented");
        }

        private Expression parseFor() {
            return this.error("for not implemented");
        }

        private Expression parseBlock() {
            return this.error("block not implemented");
        }

        private Expression parseOperations() {
            return this.error("operations not implemented");
        }

        private error(string message) {
            return new ErrorExpression {
                line = lexer.current().line,
                column = lexer.current().column,
                message = message,
            };
        }
    }

}
