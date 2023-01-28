using System;
using DFLAT.Checked;

namespace DFLAT;

class Checker {

    public Expression checkExpression(Expression expression) {
        return expression.type() switch {
            _ => throw new Exception("todo fix"),
        };
    }

}