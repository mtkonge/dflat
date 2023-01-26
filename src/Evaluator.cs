using System;

namespace DFLAT;

enum ValueType {
    Int,
    Bool,
    Float,
    Char,
    String,
    Null,
}

interface Value {
    ValueType type();
}

struct IntValue : Value {
    public ValueType type() => ValueType.Int;
    public long value;
}

struct BoolValue : Value {
    public ValueType type() => ValueType.Bool;
    public bool value;
}

struct FloatValue : Value {
    public ValueType type() => ValueType.Float;
    public double value;
}
struct CharValue : Value {
    public ValueType type() => ValueType.Char;
    public char value;
}

struct StringValue : Value {
    public ValueType type() => ValueType.String;
    public string value;
}

struct NullValue : Value {
    public ValueType type() => ValueType.Null;
}

class Evaluator {
    public Value evaluateExpression(Expression expression) {
        return expression.type() switch {
            ExpressionType.Int => evaluateInt((IntExpression) expression),
            ExpressionType.Float => evaluateFloat((FloatExpression) expression),
            ExpressionType.Char => evaluateChar((CharExpression) expression),
            ExpressionType.String => evaluateString((StringExpression) expression),
            ExpressionType.Bool => evaluateBool((BoolExpression) expression),
            ExpressionType.Null => evaluateNull((NullExpression) expression),
            ExpressionType.Binary => evaluateBinary((BinaryExpression) expression),
            _ => throw new Exception($"Evaluator.evaluateExpression: failed to match: {expression.type()}"),
        };
    }

    private IntValue evaluateInt(IntExpression expression) {

        return new IntValue { value = expression.value };
    }

    private FloatValue evaluateFloat(FloatExpression expression) {
        return new FloatValue { value = expression.value };
    }

    private CharValue evaluateChar(CharExpression expression) {
        return new CharValue { value = expression.value };
    }

    private StringValue evaluateString(StringExpression expression) {
        return new StringValue { value = expression.value };
    }

    private BoolValue evaluateBool(BoolExpression expression) {
        return new BoolValue { value = expression.value };
    }

    private NullValue evaluateNull(NullExpression expression) {
        return new NullValue { };
    }

    private Value evaluateBinary(BinaryExpression expression) {
        return expression.binaryType switch {
            // BinaryType.Add => evaluateExpression(expression.left) + evaluateExpression(expression.right),
            // BinaryType.Subtract => evaluateExpression(expression.left) - evaluateExpression(expression.right),
            // BinaryType.Multiply => evaluateExpression(expression.left) * evaluateExpression(expression.right),
            // BinaryType.Divide => evaluateExpression(expression.left) / evaluateExpression(expression.right),
            // BinaryType.Modulus => evaluateExpression(expression.left) % evaluateExpression(expression.right),
            // BinaryType.Exponentation => Math.Pow(evaluateExpression(expression.left), evaluateExpression(expression.right)),
            // BinaryType.Equal => evaluateExpression(expression.left) == evaluateExpression(expression.right),
            // BinaryType.NotEqual => evaluateExpression(expression.left) != evaluateExpression(expression.right),
            // BinaryType.And => (evaluateExpression(expression.left) && evaluateExpression(expression.right)),
            // BinaryType.Or => (evaluateExpression(expression.left) || evaluateExpression(expression.right)),
            // BinaryType.Lt => evaluateExpression(expression.left) < evaluateExpression(expression.right),
            // BinaryType.Gt => evaluateExpression(expression.left) > evaluateExpression(expression.right),
            // BinaryType.LtEqual => evaluateExpression(expression.left) <= evaluateExpression(expression.right),
            // BinaryType.GtEqual => evaluateExpression(expression.left) >= evaluateExpression(expression.right),
            _ => throw new Exception($"Evaluator.evaluateBinary: failed to match: {expression.type()}"),
        };
    }
}
