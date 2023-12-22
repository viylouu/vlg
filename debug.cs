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
        public static int frame = 1;

        public static void log(string inp) {
            now = DateTime.Now;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"LOG (FRAME {frame}) [{(now - start).TotalMilliseconds} ms]: {inp}");

            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static void good(string inp) {
            now = DateTime.Now;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"YAY (FRAME {frame}) [{(now - start).TotalMilliseconds} ms]: {inp}");

            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static void alert(string inp) {
            now = DateTime.Now;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ALERT (FRAME {frame}) [{(now - start).TotalMilliseconds} ms]: {inp}");
        }

        public static void msg(string inp) {
            now = DateTime.Now;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"MESSAGE (FRAME {frame}) [{(now - start).TotalMilliseconds} ms]: {inp}");

            Console.ForegroundColor = ConsoleColor.Red;
        }
    }
}
