using System.Collections.Generic;

namespace dotlox
{
    public class Scanner
    {
        private static readonly IDictionary<string, TokenType> keywords = new Dictionary<string, TokenType> {
            { "and", TokenType.AND },
            { "class", TokenType.CLASS },
            { "else", TokenType.ELSE },
            { "false", TokenType.FALSE },
            { "for", TokenType.FOR },
            { "fun", TokenType.FUN },
            { "if", TokenType.IF },
            { "nada", TokenType.NADA },
            { "or", TokenType.OR },
            { "print", TokenType.PRINT },
            { "return", TokenType.RETURN },
            { "super", TokenType.SUPER },
            { "this", TokenType.THIS },
            { "true", TokenType.TRUE },
            { "var", TokenType.VAR },
            { "while", TokenType.WHILE }
        };

        private readonly string source;
        private readonly ICollection<Token> tokens = new List<Token>();

        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            this.source = source;
        }

        public IEnumerable<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, string.Empty, null, line));
            return tokens;
        }

        private bool IsAtEnd() => current >= source.Length;

        private void ScanToken()
        {
            var ch = Advance();

            switch (ch)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;
                case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '/':
                    if (Match('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else if (Match('*'))
                    {
                        while (!IsAtEnd())
                        {
                            if (Peek() == '*' && PeekNext() == '/')
                            {
                                Advance(); // consume *
                                Advance(); // consume /
                                break;
                            }
                            if (Peek() == '\n')
                            {
                                line++;
                            }
                            Advance();
                        }
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\t':
                case '\r':
                    break;
                case '\n':
                    line++;
                    break;
                case '"': String(); break;

                default:
                    if (IsDigit(ch))
                    {
                        Number();
                    }
                    else if (IsAlpha(ch))
                    {
                        Identifier();
                    }
                    else
                    {
                        DotLox.Error(line, "Unexpected character.");
                    }

                    break;
            }
        }

        private char Advance()
        {
            current++;
            return source[current - 1];
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return source[current];
        }

        private char PeekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd() || source[current] != expected)
            {
                return false;
            }

            current++;
            return true;
        }

        private void AddToken(TokenType type, object literal = null)
        {
            var text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++;
                Advance();
            }

            if (IsAtEnd())
            {
                DotLox.Error(line, "Unclosed string");
                return;
            }

            Advance(); // consume closing "

            var value = source.Substring(start + 1, current - start - 2);
            AddToken(TokenType.STRING, value);
        }

        private bool IsDigit(char c) => c >= '0' && c <= '9';

        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance(); // consume the .

                while (IsDigit(Peek())) Advance();
            }

            AddToken(TokenType.NUMBER,
                double.Parse(source.Substring(start, current - start)));
        }

        private bool IsAlpha(char c) => (c >= 'a' && c <= 'z') ||
                                        (c >= 'A' && c <= 'Z') ||
                                         c == '_';

        private bool IsAlphanumeric(char c) => IsAlpha(c) || IsDigit(c);

        private void Identifier()
        {
            while (IsAlphanumeric(Peek())) Advance();

            var text = source.Substring(start, current - start);

            if (keywords.TryGetValue(text, out TokenType type))
            {
                AddToken(type);
            }
            else
            {
                AddToken(TokenType.IDENTIFIER, text);
            }
        }
    }
}
