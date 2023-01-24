namespace DFLAT;

enum TokenType {
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
    Colon,
    Comma,
    Semicolon,
    Dot,
    Fn,
    Return,
    Mut,
    Let,
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
    Exponentation,
    PlusEqual,
    MinusEqual,
    AsteriskEqual,
    SlashEqual,
    PercentEqual,
    ThinArrow,
    Ampersand,
    Plus,
    Minus,
    Asterisk,
    Slash,
    Percent,
    LtEqual,
    Lt,
    GtEqual,
    Gt,
    ExclamationEqual,
}

struct Token {
    public readonly TokenType type;
    public string value;
    public int column, line;

    public Token(TokenType type, string value, int column, int line) {
        this.type = type;
        this.value = value;
        this.column = column;
        this.line = line;
    }
}

