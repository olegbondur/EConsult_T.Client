using System;

namespace EConsult_T.ConsoleClient
{
    class Printer
    {
        protected ConsoleColor color;

        public Printer(ConsoleColor color)
        {
            this.color = color;
        }

        public virtual void Print(string str)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.Red;
        }
    }
}
