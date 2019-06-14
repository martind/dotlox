using Xunit;

namespace dotlox.tests
{
    public class AstPrinterTests
    {
        [Fact]
        public void TransformToPrefixForm()
        {
            var expr = new Binary(
                new Unary(
                    new Token(TokenType.MINUS, "-", null, 1),
                    new Literal("123")
                ),
                new Token(TokenType.STAR, "*", null, 1),
                new Grouping(new Literal(123.45))
            );

            AstPrinter sut = new AstPrinter();
            
            Assert.Equal("(* (- 123) (group 123.45))", sut.Print(expr));
        }
    }
}