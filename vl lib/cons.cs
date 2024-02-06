partial class cons {
    public static void lg(string inp) { txtcol(ConsoleColor.White); Console.WriteLine(inp); }
    public static void clr() { Console.Clear(); }
    public static void txtcol(ConsoleColor col) { Console.ForegroundColor = col; }

    public class dbg {
        static DateTime start = DateTime.Now;
        public static DateTime now = DateTime.Now;
        public static long frame = 0;

        public static void log(string inp) { //returns LOG ( 0 ms ) < fr 0 > ]: inp
            txtcol(ConsoleColor.White);
            lg($"LOG ( {(now - start).TotalMilliseconds} ms ) < fr {frame} > ]: {inp}"); 
        }

        public static void err(string inp) { //returns ERR ( 0 ms ) < fr 0 > ]: inp
            txtcol(ConsoleColor.Red);
            lg($"ERR ( {(now - start).TotalMilliseconds} ms ) < fr {frame} > ]: {inp}");
        }

        public static void alrt(string inp) { //returns ALR ( 0 ms ) < fr 0 > ]: inp
            txtcol(ConsoleColor.Yellow);
            lg($"ALR ( {(now - start).TotalMilliseconds} ms ) < fr {frame} > ]: {inp}");
        }
    }
}