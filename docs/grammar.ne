
statement ->
    | class
    | fn
    | let
    | return
    | break
    | continue

class -> "class" class_derivees "{" members "}"

class_derivees -> (":" type ("," type):* ",":?):?

members -> specified_member:*

specified_member -> member

member ->
    | method
    | field

method ->
    "pub":? "fn" Id
    "(" method_parameters ")"
    ("->" type):? fn_body_expression

method_parameters -> "self" ("," parameters):? | parameters

field -> "pub":? parameter ";"

fn -> "fn" Id fn_definition

fn_definition -> "(" parameters ")" ("->" type):? fn_body_expression

fn_body ->
    | fn_body_expression
    | block

fn_body_expression -> "=>" expression ";"

parameters -> (parameter ("," parameter) ",":?):?

let -> "let" parameter

return -> "return" expression:?

break -> "break" expression:?

continue -> "continue" expression:?

parameter -> pattern type:? expression:?

pattern ->
    | Id

type ->
    | Id

arguments -> (expression ("," expression):* ",":?):?

expression ->
    | if
    | while
    | for
    | block
    | operation

if -> "if" expression block ("else" block):?

while -> "while" expression block

for -> "for" pattern "in" expression block

block -> "{" block_statements "}"

block_statements -> (statements ";"):* statement:?

operation -> prec1
prec1 -> prec2
prec2 ->
    | prec3 "=" prec2
    | prec3 "+=" prec2
    | prec3 "-=" prec2
    | prec3 "*=" prec2
    | prec3 "/=" prec2
    | prec3 "%=" prec2
    | prec3

prec3 ->
    | prec3 "or" prec4
    | prec4

prec4 ->
    | prec4 "and" prec5
    | prec5

prec5 -> prec6
prec6 -> prec7
prec7 -> prec8
prec8 ->
    | prec8 "==" prec9
    | prec8 "!=" prec9
    | prec9

prec9 ->
    | prec9 "<" prec10
    | prec9 "<=" prec10
    | prec9 ">" prec10
    | prec9 "<=" prec10
    | prec9 "in" prec10
    | prec10

prec10 -> prec11
prec11 ->
    | prec11 "+" prec12
    | prec11 "-" prec12
    | prec12

prec12 ->
    | prec12 "*" prec13
    | prec12 "/" prec13
    | prec12 "%" prec13
    | prec13

prec13 ->
    | prec14 Exponentation prec13
    | prec14

prec14 ->
    | "not" prec14
    | "-" prec14
    | prec15

prec15 -> prec16
prec16 -> prec17
prec17 ->
    | prec17 "." Id
    | prec17 "[" expression "]"
    | prec17 "(" arguments "]"
    | prec18
    
prec18 ->
    | "(" expression ")"
    | atom

atom ->
    | Id
    | Int
    | Float
    | Char
    | String
    | bool
    | Null

bool -> False | True

### tokens ###

Id -> /[a-zA-Z_][a-zA-Z0-9_]*/
Int -> /[0-9]+/
Float -> /[0-9]\.[0-9]/
Char -> /'\.'/ # + escaped chars
String -> /".*?"/ # + escaped chars

Null -> "null"
False -> "false"
True -> "true"
If -> "if"
Else -> "else"
While -> "while"
For -> "for"
Break -> "break"
Continue -> "continue"
Colon -> "colon"
Comma -> "comma"
Semicolon -> "semicolon"
Dot -> "dot"
Fn -> "fn"
Return -> "return"
Mut -> "mut"
Let -> "let"
Class -> "class"
New -> "new"
Pub -> "pub"
Not -> "not"
And -> "and"
Or -> "or"
In -> "in"

LParen -> "("
RParen -> ")"
LBrace -> "{"
RBrace -> "}"
LBracket -> "["
RBracket -> "]"

DoubleEqual -> "=="
Equal -> "="
Exponentation -> "**"
PlusEqual -> "+="
MinusEqual -> "-="
AsteriskEqual -> "*="
SlashEqual -> "/="
PercentEqual -> "%="
ThinArrow -> "->"
Ampersand -> "&"
Plus -> "+"
Minus -> "-"
Asterisk -> "*"
Slash -> "/"
Percent -> "%"
LtEqual -> "<="
Lt -> "<"
GtEqual -> ">="
Gt -> ">"
ExclamationEqual -> "!="
