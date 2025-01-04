using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundrySimulator.UserInterface
{
    public static class ConsoleHelper
    {
        public static int DisplayMenu<T>(List<T> items, Func<T, string> displayFunc)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {displayFunc(items[i])}");
            }

            Console.Write("Choose an option: ");
            return int.Parse(Console.ReadLine()) - 1;
        }
    }
}
