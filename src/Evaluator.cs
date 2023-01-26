using System;

namespace DFLAT;
class Evaluator {
    public long evaluateExpression(Expression expression) {
        return expression.type() switch {
            ExpressionType.Int => evaluateInt((IntExpression) expression),

            ExpressionType.Binary => evaluateBinary((BinaryExpression) expression),
            _ => throw new Exception($"Evaluator.evaluateExpression: failed to match: {expression.type()}"),
        };
    }

    private long evaluateInt(IntExpression expression) {
        return expression.value;
    }

    private long evaluateBinary(BinaryExpression expression) {
        return expression.binaryType switch {
            BinaryType.Add => evaluateExpression(expression.left) + evaluateExpression(expression.right),
            BinaryType.Subtract => evaluateExpression(expression.left) - evaluateExpression(expression.right),
            _ => throw new Exception($"Evaluator.evaluateBinary: failed to match: {expression.type()}"),
        };
    }
}
