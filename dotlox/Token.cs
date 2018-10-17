using System;

namespace dotlox {
    public class Token {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public object Literal { get; }
        public int Line { get; }

        public Token(TokenType type, string lexeme, object literal, int line) {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public override string ToString() => $"{Type} {Lexeme} {Literal} {Line}";

        public override bool Equals(object obj) {
            var other = obj as Token;

            if (other == null) {
                return false;
            }

            return Type == other.Type &&
                   ((Lexeme == null && other.Lexeme == null) || Lexeme == other.Lexeme) &&
                   ((Literal == null && other.Literal == null) || Literal.Equals(other.Literal)) &&
                   Line == other.Line;
        }

        public override int GetHashCode() {
            return Lexeme.GetHashCode();
        }
    }
}
