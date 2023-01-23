using System;
using System.Collections.Generic;
using System.Text;

namespace dflat {

    interface Lexer {
        Token next();
    }
}
