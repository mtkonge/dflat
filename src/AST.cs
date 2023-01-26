using System;
using System.Linq;

namespace DFLAT;

struct Error {
    public int line, column;
    public string message;

    public override string ToString() => $"error: {message} at {line}:{column}";
}

enum TypeType {
    Error,
    Id,
}

interface Type {
    TypeType type();
}

struct ErrorType : Type {
    public Error error;

    public TypeType type() => TypeType.Error;
}

struct IdType : Type {
    public string value;

    public TypeType type() => TypeType.Id;
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

struct IdPattern : Pattern {
    public string value;

    public PatternType type() => PatternType.Id;
    public override string ToString() => value;
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

struct IfExpression : Expression {
    public Expression condition;
    public Expression body;

    public ExpressionType type() => ExpressionType.If;
    public override string ToString() => $"if {condition} {body}";
}

struct IfElseExpression : Expression {
    public Expression condition;
    public Expression truthy;
    public Expression falsy;

    public ExpressionType type() => ExpressionType.IfElse;
    public override string ToString() => $"if {condition} {truthy} else {falsy}";
}

struct WhileExpression : Expression {
    public Expression condition;
    public Expression body;

    public ExpressionType type() => ExpressionType.While;
    public override string ToString() => $"while {condition} {body}";
}

struct ForExpression : Expression {
    public Pattern subject;
    public Expression value;
    public Expression body;

    public ExpressionType type() => ExpressionType.For;
    public override string ToString() => $"for {subject} in {value} {body}";
}

struct BlockExpression : Expression {
    public Statement[] statements;
    public Expression? result;

    public ExpressionType type() => ExpressionType.Block;
    public override string ToString() {
        var stringified = "{ ";
        stringified += string.Join("", statements.Select((s) => s.ToString() + "; "));
        if (result != null)
            stringified += $"{result} ";
        stringified += "}";
        return stringified;
    }
}

enum AssignType {
    Assign,
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulus,
}

struct AssignExpression : Expression {
    public Expression subject;
    public Expression value;
    public AssignType assignType;

    public ExpressionType type() => ExpressionType.Assign;
    public override string ToString() => $"({subject} {assignTypeToString()} {value})";

    private string assignTypeToString() => assignType switch {
        AssignType.Assign => "=",
        AssignType.Add => "+=",
        AssignType.Subtract => "-=",
        AssignType.Multiply => "*=",
        AssignType.Divide => "/=",
        AssignType.Modulus => "%=",
    };
}

enum BinaryType {
    Exponentation,
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulus,
    Lt,
    LtEqual,
    Gt,
    GtEqual,
    In,
    Equal,
    NotEqual,
    And,
    Or,
}

struct BinaryExpression : Expression {
    public Expression left;
    public Expression right;
    public BinaryType binaryType;

    public ExpressionType type() => ExpressionType.Binary;
    public override string ToString() => $"({left} {binaryTypeToString()} {right})";

    private string binaryTypeToString() => binaryType switch {
        BinaryType.Exponentation => "**",
        BinaryType.Add => "+",
        BinaryType.Subtract => "-",
        BinaryType.Multiply => "*",
        BinaryType.Divide => "/",
        BinaryType.Modulus => "%",
        BinaryType.Lt => "<",
        BinaryType.LtEqual => "<=",
        BinaryType.Gt => ">",
        BinaryType.GtEqual => ">=",
        BinaryType.In => "in",
        BinaryType.Equal => "==",
        BinaryType.NotEqual => "!=",
        BinaryType.And => "and",
        BinaryType.Or => "or",
    };
}

enum UnaryType {
    Not,
    Negate,
}

struct UnaryExpression : Expression {
    public Expression subject;
    public UnaryType unaryType;

    public ExpressionType type() => ExpressionType.Unary;
    public override string ToString() => $"({unaryTypeToString()} {subject})";

    private string unaryTypeToString() => unaryType switch {
        UnaryType.Not => "not",
        UnaryType.Negate => "-",
    };
}

struct CallExpression : Expression {
    public Expression subject;
    public Expression[] arguments;

    public ExpressionType type() => ExpressionType.Call;
    public override string ToString() => $"{subject}({string.Join(", ", arguments.Select((a) => a.ToString()))})";
}

struct MemberExpression : Expression {
    public Expression subject;
    public string value;

    public ExpressionType type() => ExpressionType.Member;
    public override string ToString() => $"{subject}.{value}";
}

struct IndexExpression : Expression {
    public Expression subject;
    public Expression value;

    public ExpressionType type() => ExpressionType.Index;
    public override string ToString() => $"{subject}[{value}]";
}

struct IdExpression : Expression {
    public string value;

    public ExpressionType type() => ExpressionType.Id;
    public override string ToString() => value.ToString();
}

struct IntExpression : Expression {
    public long value;

    public ExpressionType type() => ExpressionType.Int;
    public override string ToString() => value.ToString();
}

struct FloatExpression : Expression {
    public double value;

    public ExpressionType type() => ExpressionType.Float;
    public override string ToString() => value.ToString();
}

struct CharExpression : Expression {
    public char value;

    public ExpressionType type() => ExpressionType.Char;
    public override string ToString() => value.ToString();
}

struct StringExpression : Expression {
    public string value;

    public ExpressionType type() => ExpressionType.String;
    public override string ToString() => $"\"{value.ToString()}\"";
}

struct BoolExpression : Expression {
    public bool value;

    public ExpressionType type() => ExpressionType.Bool;
    public override string ToString() => value.ToString().ToLower();
}

struct NullExpression : Expression {
    public ExpressionType type() => ExpressionType.Null;
    public override string ToString() => "null";
}

struct Parameter {
    public Pattern subject;
    public Type? type;
    public Expression? value;

    public override string ToString() {
        string stringified = $"{subject}";
        if (type != null)
            stringified += $": {type}";
        if (value != null)
            stringified += $" = {value}";
        return stringified;
    }

    public bool isError() =>
        subject.type() == PatternType.Error
        || type?.type() == TypeType.Error
        || value?.type() == ExpressionType.Error;

    public Error error() {
        if (subject.type() == PatternType.Error)
            return ((ErrorPattern) subject).error;
        else if (type?.type() == TypeType.Error)
            return ((ErrorType) type).error;
        else if (value?.type() == ExpressionType.Error)
            return ((ErrorExpression) value).error;
        else
            throw new Exception("panic");
    }
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

struct ExpressionStatement : Statement {
    public Expression value;

    public StatementType type() => StatementType.Expression;
    public override string ToString() => value?.ToString() ?? throw new Exception("panic");
}

struct Field {
    public Parameter subject;
}

struct Method {
    public string subject;
    public Parameter[] parameters;
    public Type returnType;
    public Expression body;
}

struct ClassStatement : Statement {
    public string subject;
    public Field[] fields;
    public Method[] methods;

    public StatementType type() => StatementType.Class;
}

struct FnStatement : Statement {
    public string subject;
    public Parameter[] parameters;
    public Type returnType;
    public Expression body;

    public StatementType type() => StatementType.Fn;
}

struct LetStatement : Statement {
    public Parameter subject;

    public StatementType type() => StatementType.Let;
    public override string ToString() => $"let {subject}";
}

struct ReturnStatement : Statement {
    public Expression? value;

    public StatementType type() => StatementType.Return;
}

struct BreakStatement : Statement {
    public Expression? value;

    public StatementType type() => StatementType.Break;
}

struct ContinueStatement : Statement {
    public Expression? value;

    public StatementType type() => StatementType.Continue;
}
