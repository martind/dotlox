using System.Text;

namespace dotlox
{
    public class AstPrinter : IVisitor<string>
    {
        public string Print(Expr expr) {
            return expr.Accept(this);
        }

        public string VisitBinaryExpr(Binary expr)
        {
            return paren(expr.Oper.Lexeme, expr.Left, expr.Right);
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            return paren("group", expr.Expression);
        }

        public string VisitLiteralExpr(Literal expr)
        {
            if (expr.Value == null) return "nada";
            return expr.Value.ToString();
        }

        public string VisitUnaryExpr(Unary expr)
        {
            return paren(expr.Oper.Lexeme, expr.Right);
        }

        private string paren(string lexeme, params Expr[] exprs)
        {
            var output = new StringBuilder();

            output.Append($"({lexeme}");
            foreach (var expr in exprs) {
                output.Append(" ").Append(expr.Accept(this));
            }
            output.Append(")");
            return output.ToString();
        }
    }
}