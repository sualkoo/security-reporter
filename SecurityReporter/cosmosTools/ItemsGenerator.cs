using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmosTools
{
    internal class ItemsGenerator
    {
        private string? consoleInput;
        private string? command;
        
        public ItemsGenerator()
        {
            Console.WriteLine("-------------------");
            Console.WriteLine("DB Generator");
            Console.WriteLine("-------------------");

            consoleInput = Console.ReadLine();

            if (consoleInput != null)
            {
                command = getCommandFromInput(consoleInput);
                if (command == "add")
                {
                    Console.WriteLine("add command");
                }
                if (command == "clear")
                {
                    Console.WriteLine("clear command");
                }
                if (command == "help")
                {
                    Console.WriteLine("help command");
                }
            }
            else { }


        }

        public string CommandFromInput(string input) {
            
            int firstWhitespaceIndex = input.IndexOfAny(new[] { ' ', '\t' });

            if (firstWhitespaceIndex == -1)
            {
                return string.Empty;
            }
            string truncatedInput = input.Substring(0, firstWhitespaceIndex);

            return truncatedInput;
        }
    }
    
}
