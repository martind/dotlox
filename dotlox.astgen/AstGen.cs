using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotlox.astgen
{
    class AstGen
    {
        static void Main(string[] args)
        {
            var ast = defineAst("Expr", new[] {
                "Binary : Expr left, Token oper, Expr right",
                "Unary : Token oper, Expr right",
                "Grouping : Expr expression",
                "Literal : object value"
            });

            Console.WriteLine(ast);
        }

        static string defineAst(string baseName, IEnumerable<string> types)
        {
            var buffer = new StringBuilder();
            buffer.AppendLine("// THIS FILE IS AUTO-GENERATED");
            buffer.AppendLine("using System.Collections.Generic;");
            buffer.AppendLine();
            buffer.AppendLine("namespace dotlox {");

            defineVisitor(buffer, baseName, types);

            buffer.AppendLine($"    public abstract class {baseName} {{");
            buffer.AppendLine("        public abstract R Accept<R>(IVisitor<R> visitor);");
            buffer.AppendLine("    }");
            buffer.AppendLine();


            foreach (var type in types)
            {
                var klass = type.Split(':')[0].Trim();
                var fields = type.Split(':')[1].Trim();
                buffer.AppendLine();
                defineType(buffer, baseName, klass, fields);
            }

            buffer.AppendLine("}");

            return buffer.ToString();
        }

        static void defineType(StringBuilder buffer, string baseName, string klass, string fields)
        {
            buffer.AppendLine($"    public class {klass} : {baseName} {{");
            buffer.AppendLine($"        public {klass}({fields}) {{");

            foreach (var field in fields.Split(',').Select(f => f.Trim()))
            {
                var name = field.Split(' ')[1].Trim();
                buffer.AppendLine($"            {name.ToTitleCase()} = {name};");
            }

            buffer.AppendLine("        }");
            buffer.AppendLine();

            buffer.AppendLine("        public override R Accept<R>(IVisitor<R> visitor) {");
            buffer.AppendLine($"            return visitor.Visit{klass}{baseName}(this);");
            buffer.AppendLine("        }");
            buffer.AppendLine();

            foreach (var field in fields.Split(',').Select(f => f.Trim()))
            {
                var type = field.Split(' ')[0].Trim();
                var name = field.Split(' ')[1].Trim().ToTitleCase();
                buffer.AppendLine($"        public {type} {name} {{ get; }}");
            }

            buffer.AppendLine("    }");
        }

        static void defineVisitor(StringBuilder buffer, string baseName, IEnumerable<string> types)
        {
            buffer.AppendLine("    public interface IVisitor<R> {");

            foreach (var type in types)
            {
                var typeName = type.Split(':')[0].Trim();
                buffer.AppendLine($"        R Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
            }

            buffer.AppendLine("    }");
            buffer.AppendLine();
        }
    }

    static class Utils
    {
        public static string ToTitleCase(this string value)
        {
            var valueArray = value.ToCharArray();
            return string.Join("", char.ToUpper(valueArray[0]), value.Substring(1));
        }
    }
}
