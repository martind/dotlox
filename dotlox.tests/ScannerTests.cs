using System;
using Xunit;
using dotlox;
using System.Collections.Generic;
using System.Linq;

namespace dotlox.tests
{
    public class ScannerTests {
        [Fact]
        public void ScanSimpleTokens() {
            var sut = new Scanner("() {} , . + - / ;");

            var expected = new List<Token> {
                new Token(TokenType.LEFT_PAREN, "(", null, 1),
                new Token(TokenType.RIGHT_PAREN, ")", null, 1),
                new Token(TokenType.LEFT_BRACE, "{", null, 1),
                new Token(TokenType.RIGHT_BRACE, "}", null, 1),
                new Token(TokenType.COMMA, ",", null, 1),
                new Token(TokenType.DOT, ".", null, 1),
                new Token(TokenType.PLUS, "+", null, 1),
                new Token(TokenType.MINUS, "-", null, 1),
                new Token(TokenType.SLASH, "/", null, 1),
                new Token(TokenType.SEMICOLON, ";", null, 1),
                new Token(TokenType.EOF, string.Empty, null, 1)
            };

            Assert.Equal(expected, sut.ScanTokens());
        }

        [Fact]
        public void ScanCompositeTokens() {
            var sut = new Scanner("!= >= <= ==");

            var expected = new List<Token> {
                new Token(TokenType.BANG_EQUAL, "!=", null, 1),
                new Token(TokenType.GREATER_EQUAL, ">=", null, 1),
                new Token(TokenType.LESS_EQUAL, "<=", null, 1),
                new Token(TokenType.EQUAL_EQUAL, "==", null, 1),
                new Token(TokenType.EOF, string.Empty, null, 1)
            };

            Assert.Equal(expected, sut.ScanTokens());
        }

        [Fact]
        public void SkipComments() {
            var sut = new Scanner("! * // a comment ({ })");

            var expected = new List<Token> {
                new Token(TokenType.BANG, "!", null, 1),
                new Token(TokenType.STAR, "*", null, 1),
                new Token(TokenType.EOF, string.Empty, null, 1)
            };

            Assert.Equal(expected, sut.ScanTokens());
        }

        [Fact]
        public void ScanString() {
            var sut = new Scanner("\"hello world!\"");

            var expected = new List<Token> {
                new Token(TokenType.STRING, "\"hello world!\"", "hello world!", 1),
                new Token(TokenType.EOF, string.Empty, null, 1)
            };

            Assert.Equal(expected, sut.ScanTokens());
        }

        [Fact]
        public void ScanNumber() {
            var sut = new Scanner("123 123.45");

            var expected = new List<Token> {
                new Token(TokenType.NUMBER, "123", 123d, 1),
                new Token(TokenType.NUMBER, "123.45", 123.45, 1),
                new Token(TokenType.EOF, string.Empty, null, 1)
            };

            Assert.Equal(expected, sut.ScanTokens());
        }

        [Fact]
        public void ScanKeyword() {
            var sut = new Scanner("if for while and");

            var expected = new List<Token> {
                new Token(TokenType.IF, "if", null, 1),
                new Token(TokenType.FOR, "for", null, 1),
                new Token(TokenType.WHILE, "while", null, 1),
                new Token(TokenType.AND, "and", null, 1),
                new Token(TokenType.EOF, string.Empty, null, 1)
            };

            Assert.Equal(expected, sut.ScanTokens());
        }

        [Fact]
        public void ScanIdentifier() {
            var sut = new Scanner("foo bar baz");

            var expected = new List<Token> {
                new Token(TokenType.IDENTIFIER, "foo", "foo", 1),
                new Token(TokenType.IDENTIFIER, "bar", "bar", 1),
                new Token(TokenType.IDENTIFIER, "baz", "baz", 1),
                new Token(TokenType.EOF, string.Empty, null, 1)
            };

            Assert.Equal(expected, sut.ScanTokens());
        }

        [Fact]
        public void ScanExpression() {
            var sut = new Scanner("var foo = nada");

            var expected = new List<Token> {
                new Token(TokenType.VAR, "var", null, 1),
                new Token(TokenType.IDENTIFIER, "foo", "foo", 1),
                new Token(TokenType.EQUAL, "=", null, 1),
                new Token(TokenType.NADA, "nada", null, 1),
                new Token(TokenType.EOF, string.Empty, null, 1)
            };

            Assert.Equal(expected, sut.ScanTokens());
        }

        [Fact]
        public void ScanMultilineCommment() {
            var sut = new Scanner(
                "var i = \"foobar\"\n" +
                "/*\n" + 
                " * a nice comment\n" +
                " */\n" +
                "print i\n"
            );

            var expected = new List<Token> {
                new Token(TokenType.VAR, "var", null, 1),
                new Token(TokenType.IDENTIFIER, "i", "i", 1),
                new Token(TokenType.EQUAL, "=", null, 1),
                new Token(TokenType.STRING, "\"foobar\"", "foobar", 1),
                new Token(TokenType.PRINT, "print", null, 5),
                new Token(TokenType.IDENTIFIER, "i", "i", 5),
                new Token(TokenType.EOF, string.Empty, null, 6)
            };

            Assert.Equal(expected, sut.ScanTokens());
        }
    }
}
