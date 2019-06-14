// THIS FILE IS AUTO-GENERATED
using System.Collections.Generic;

namespace dotlox {
    public interface IVisitor<R> {
        R VisitBinaryExpr(Binary expr);
        R VisitUnaryExpr(Unary expr);
        R VisitGroupingExpr(Grouping expr);
        R VisitLiteralExpr(Literal expr);
    }

    public abstract class Expr {
        public abstract R Accept<R>(IVisitor<R> visitor);
    }


    public class Binary : Expr {
        public Binary(Expr left, Token oper, Expr right) {
            Left = left;
            Oper = oper;
            Right = right;
        }

        public override R Accept<R>(IVisitor<R> visitor) {
            return visitor.VisitBinaryExpr(this);
        }

        public Expr Left { get; }
        public Token Oper { get; }
        public Expr Right { get; }
    }

    public class Unary : Expr {
        public Unary(Token oper, Expr right) {
            Oper = oper;
            Right = right;
        }

        public override R Accept<R>(IVisitor<R> visitor) {
            return visitor.VisitUnaryExpr(this);
        }

        public Token Oper { get; }
        public Expr Right { get; }
    }

    public class Grouping : Expr {
        public Grouping(Expr expression) {
            Expression = expression;
        }

        public override R Accept<R>(IVisitor<R> visitor) {
            return visitor.VisitGroupingExpr(this);
        }

        public Expr Expression { get; }
    }

    public class Literal : Expr {
        public Literal(object value) {
            Value = value;
        }

        public override R Accept<R>(IVisitor<R> visitor) {
            return visitor.VisitLiteralExpr(this);
        }

        public object Value { get; }
    }
}

