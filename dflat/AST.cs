using System;
using System.Collections.Generic;
using System.Text;

namespace DFLAT {

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
        Int,
        Float,
        Char,
        String,
        Bool,
    }

    interface Expression {
        ExpressionType type();
    }

    class ErrorExpression : Expression {
        public int line, column;
        public string message;

        public ExpressionType type() => ExpressionType.Error;
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

        public ExpressionType type() => ExpressionType.Blcok;
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
    }

    enum BinaryType {
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
    }

    enum UnaryType {
        Not,
        Negate,
    }

    class UnaryExpression : Expression {
        public Expression subject;
        public UnaryType unaryType;

        public ExpressionType type() => ExpressionType.Unary;
    }

    class CallExpression : Expression {
        public Expression subject;
        public Expression[] arguments;

        public ExpressionType type() => ExpressionType.Call;
    }

    class MemberExpression : Expression {
        public Expression subject;
        public Expression value;

        public ExpressionType type() => ExpressionType.Member;
    }

    class IndexExpression : Expression {
        public Expression subject;
        public Expression value;

        public ExpressionType type() => ExpressionType.Index;
    }
    
    class IntExpression : Expression {
        public long value;

        public ExpressionType type() => ExpressionType.Int;
    }

    class FloatExpression : Expression {
        public double value;

        public ExpressionType type() => ExpressionType.Float;
    }

    class CharExpression : Expression {
        public char value;

        public ExpressionType type() => ExpressionType.Char;
    }

    class StringExpression : Expression {
        public string value;

        public ExpressionType type() => ExpressionType.String;
    }

    class BoolExpression : Expression {
        public bool value;

        public ExpressionType type() => ExpressionType.Bool;
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
}
