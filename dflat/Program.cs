using System;
using System.Linq;

namespace DFLAT;

internal class Program {
    static void test(string text) {
        Console.WriteLine($"===\ntext = \"{text}\"");
        var tokens = new Lexer(text).collect();
        Console.WriteLine($"tokens = [{string.Join(", ", tokens.Select((token) => token.type.ToString() + "(" + token.value + ")"))}]");
        var parser = new Parser(new Lexer(text));
        var ast = parser.parseExpression(true);
        Console.WriteLine($"ast = {ast}\n");
    }

    static void Main(string[] args) {
        Console.WriteLine("Hello World!");

        test("1 * 2 + 3 * 4 + add(1, 2)");
        test("1 + 2 + 3 + 4");
        test("1 ** 2 ** 3 ** 4");
        test("1 --- 4");

        test("a = \"hello world\"");
        test("\"hello world\\\"");
        test("\"hello world\\\"\"");

        test("abc123");
        test("123");
        test("123.456");
        test("'a'");
        test("'\\n'");
        test("'\\''");
        test("true");
        test("false");

        test("abc[123 + abc]");
        test("abc.abc");
        test("abc(abc, abc + 123)");

        test("abc[1][1]");
        test("abc.abc.abc");
        test("abc(123)(123)(abc)");

        Console.WriteLine("!dlroW olleH");
    }
}
