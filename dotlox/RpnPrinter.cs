using System.Text;

namespace dotlox
{
    public class RpnPrinter : IVisitor<string>
    {
        public string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public string VisitBinaryExpr(Binary expr)
        {
            var result = new StringBuilder();
            result.Append(expr.Left.Accept(this))
                  .Append(" ")
                  .Append(expr.Right.Accept(this))
                  .Append(" ")
                  .Append(expr.Oper.Lexeme);
            return result.ToString();
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            return expr.Expression.Accept(this);
        }

        public string VisitLiteralExpr(Literal expr)
        {
            if (expr.Value == null) return "nada";
            return expr.Value.ToString();
        }

        public string VisitUnaryExpr(Unary expr)
        {
            var result = new StringBuilder();
            result.Append(expr.Oper.Lexeme)
                  .Append(expr.Right.Accept(this));
            return result.ToString();
        }
    }
}