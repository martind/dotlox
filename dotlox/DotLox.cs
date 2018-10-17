using System;
using System.IO;
using System.Text;

using static dotlox.ExitCodes;

namespace dotlox
{
    class DotLox
    {
        private static bool hadError;

        static void Main(string[] args)
        {
            if (args.Length > 1) {
                Console.WriteLine("Usage: dotlox [script]");
                Environment.Exit(EX_USAGE);
            } else if (args.Length == 1) {
                RunFile(args[0]);
            } else {
                RunPrompt();
            }
        }

        private static void RunFile(string path) {
            var source = File.ReadAllText(path, Encoding.UTF8);
            Run(source);

            if (hadError) {
                Environment.Exit(EX_DATAERR);
            }
        }

        private static void RunPrompt() {
            for (;;) {
                Console.Write("> ");
                Run(Console.ReadLine());
                hadError = false;
            }
        }

        private static void Run(string source) {
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();

            foreach (var token in tokens) {
                Console.WriteLine(token);
            }
        }

        public static void Error(int line, string message) {
            Report(line, string.Empty, message);
        }

        private static void Report(int line, string where, string message) {
            Console.WriteLine($"[line {line}] Error{where}: {message}");
            hadError = true;
        }
    }
}
