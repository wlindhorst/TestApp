using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() != 0)
            {
                LineProcessor proc = new LineProcessor();
                proc.ParseArgs(args);
            }
            else
            {
                Console.WriteLine("One or more arguments required.");
            }
        }
    }
}
