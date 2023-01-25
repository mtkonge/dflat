using System.Linq;

namespace DFLAT;

enum TypeType {
    Error,
    Id,
}

interface Type {
    TypeType type();
}

class ErrorType : Type {
    public int line, column;
    public string message;

    public TypeType type() => TypeType.Error;
}

class IdType : Type {
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

class ErrorPattern : Pattern {
    public int line, column;
    public string message;

    public PatternType type() => PatternType.Error;
}

class IdPattern : Pattern {
    public string value;

    public PatternType type() => PatternType.Id;
}

enum ExpressionType {
    Error,
    If,
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

class ErrorExpression : Expression {
    public int line, column;
    public string message;

    public ExpressionType type() => ExpressionType.Error;
    public override string ToString() => $"error: {message} at {line}:{column}";
}

class IfExpression : Expression {
    public Expression condition;
    public Expression body;

    public ExpressionType type() => ExpressionType.If;
}

class WhileExpression : Expression {
    public Expression condition;
    public Expression body;

    public ExpressionType type() => ExpressionType.While;
}

class ForExpression : Expression {
    public Parameter subject;
    public Expression value;
    public Expression body;

    public ExpressionType type() => ExpressionType.For;
}

class BlockExpression : Expression {
    public Statement[] statements;
    public Expression result;

    public ExpressionType type() => ExpressionType.Block;
}

enum AssignType {
    Assign,
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulus,
}

class AssignExpression : Expression {
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

class BinaryExpression : Expression {
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

class UnaryExpression : Expression {
    public Expression subject;
    public UnaryType unaryType;

    public ExpressionType type() => ExpressionType.Unary;
    public override string ToString() => $"({unaryTypeToString()} {subject})";

    private string unaryTypeToString() => unaryType switch {
        UnaryType.Not => "not",
        UnaryType.Negate => "-",
    };
}

class CallExpression : Expression {
    public Expression subject;
    public Expression[] arguments;

    public ExpressionType type() => ExpressionType.Call;
    public override string ToString() => $"{subject}({string.Join(", ", arguments.Select((a) => a.ToString()))})";
}

class MemberExpression : Expression {
    public Expression subject;
    public string value;

    public ExpressionType type() => ExpressionType.Member;
    public override string ToString() => $"{subject}.{value}";
}

class IndexExpression : Expression {
    public Expression subject;
    public Expression value;

    public ExpressionType type() => ExpressionType.Index;
    public override string ToString() => $"{subject}[{value}]";
}

class IdExpression : Expression {
    public string value;

    public ExpressionType type() => ExpressionType.Id;
    public override string ToString() => value.ToString();
}

class IntExpression : Expression {
    public long value;

    public ExpressionType type() => ExpressionType.Int;
    public override string ToString() => value.ToString();
}

class FloatExpression : Expression {
    public double value;

    public ExpressionType type() => ExpressionType.Float;
    public override string ToString() => value.ToString();
}

class CharExpression : Expression {
    public char value;

    public ExpressionType type() => ExpressionType.Char;
    public override string ToString() => value.ToString();
}

class StringExpression : Expression {
    public string value;

    public ExpressionType type() => ExpressionType.String;
    public override string ToString() => value.ToString();
}

class BoolExpression : Expression {
    public bool value;

    public ExpressionType type() => ExpressionType.Bool;
    public override string ToString() => value.ToString();
}

class NullExpression : Expression {
    public ExpressionType type() => ExpressionType.Null;
    public override string ToString() => "null";
}

class Parameter {
    public Pattern subject;
    public Type? type;
    public Expression? value;
}

enum StatementType {
    Error,
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

class ErrorStatement : Statement {
    public int line, column;
    public string message;

    public StatementType type() => StatementType.Error;
}

class Field {
    public Parameter subject;
}

class Method {
    public string subject;
    public Parameter[] parameters;
    public Type returnType;
    public Expression body;
}

class ClassStatement : Statement {
    public string subject;
    public Field[] fields;
    public Method[] methods;

    public StatementType type() => StatementType.Class;
}

class FnStatement : Statement {
    public string subject;
    public Parameter[] parameters;
    public Type returnType;
    public Expression body;

    public StatementType type() => StatementType.Fn;
}

class LetStatement : Statement {
    public Parameter subject;

    public StatementType type() => StatementType.Let;
}

class ReturnStatement : Statement {
    public Expression value;

    public StatementType type() => StatementType.Return;
}

class BreakStatement : Statement {
    public Expression value;

    public StatementType type() => StatementType.Break;
}

class ContinueStatement : Statement {
    public Expression value;

    public StatementType type() => StatementType.Continue;
}
