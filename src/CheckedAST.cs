
namespace DFLAT.Checked;

struct Error {
    public int line, column;
    public string message;

    public override string ToString() =>
        $"error: {message} at {line}:{column}";
}

enum TypeType {
    Error,
    Int,
    Float,
    Char,
    String,
    Bool,
    Null,
}

interface Type {
    TypeType type();
}

struct ErrorType : Type {
    public Error error;

    public TypeType type() => TypeType.Error;
}


enum PatternType {
    Error,
    Id,
}

interface Pattern {
    PatternType type();
}


struct ErrorPattern : Pattern {
    public Error error;

    public PatternType type() => PatternType.Error;
}

enum ExpressionType {
    Error,
    If,
    IfElse,
    While,
    For,
    Block,
    Assign,
    Binary,
    Unary,
    Call,
    Member,
    Index,
    Id,
    Int,
    Float,
    Char,
    String,
    Bool,
    Null,
}

interface Expression {
    ExpressionType type();
}

struct ErrorExpression : Expression {
    public Error error;

    public ExpressionType type() => ExpressionType.Error;
    public override string ToString() => error.ToString();
}

enum StatementType {
    Error,
    Expression,
    Class,
    Fn,
    Let,
    Return,
    Break,
    Continue,
}

interface Statement {
    StatementType type();
}

struct ErrorStatement : Statement {
    public Error error;

    public StatementType type() => StatementType.Error;
}

