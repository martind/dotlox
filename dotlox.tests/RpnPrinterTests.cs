using Xunit;

namespace dotlox.tests
{
    public class RpnPrinterTests
    {
        [Fact]
        public void TransformToRPNNotation()
        {
            var expr = new Binary(
                new Grouping(
                    new Binary(
                        new Literal("1"),
                        new Token(TokenType.PLUS, "+", null, 1),
                        new Literal("2")
                    )
                ),
                new Token(TokenType.STAR, "*", null, 1),
                new Grouping(
                    new Binary(
                        new Literal("4"),
                        new Token(TokenType.MINUS, "-", null, 1),
                        new Literal("3")
                    )
                )
            );

            var sut = new RpnPrinter();

            Assert.Equal("1 2 + 4 3 - *", sut.Print(expr));
        }
    }
}