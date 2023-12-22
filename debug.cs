using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vlg
{
    internal class debug
    {
        static DateTime start = DateTime.Now;
        static DateTime now = DateTime.Now;

        public static void log(string inp) {
            now = DateTime.Now;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"LOG [{(now - start).TotalMilliseconds} ms]: {inp}");

            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static void good(string inp) {
            now = DateTime.Now;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"LOG [{(now - start).TotalMilliseconds} ms]: {inp}");

            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static void alert(string inp) {
            now = DateTime.Now;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ALERT [{(now - start).TotalMilliseconds} ms]: {inp}");
        }
    }
}
