using System;

namespace dflat
{
    enum TokenType
    {
        Eof,
        Error,

        Id,
        Int,
        Float,
        Char,
        String,

        Null,
        False,
        True,
        If,
        Else,
        While,
        For,
        Break,
        Continue,
        Fn,
        Return,
        Class,
        New,
        Pub,
        Not,
        And,
        Or,
        In,
        
        LParen,
        RParen,
        LBrace,
        RBrace,
        LBracket,
        RBracket,

        DoubleEqual,
        Equal,
        PlusEqual,
        MinusEqual,
        AsteriskEqual,
        SlashEqual,
        PercentEqual,
        Plus,
        LtEqual,
        Lt,
        GtEqual,
        Gt,
        ExclamationEqual,
    }

    struct Token
    {
        TokenType type;
        String value;
        int line, column;
    }

    interface Lexer
    {
        Token next();
    }

    class Parser
    {
        private Lexer lexer;

        Parser(Lexer lexer) {
            this.lexer = lexer;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
