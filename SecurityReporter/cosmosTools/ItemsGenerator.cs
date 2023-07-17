using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmosTools
{
    internal class ItemsGenerator
    {
        private string[]? consoleInput;
        private string? command;
        
        public ItemsGenerator(string[] args)
        {
            Console.WriteLine("-------------------");
            Console.WriteLine("DB Generator");
            Console.WriteLine("-------------------");

            this.consoleInput = args;

            if (this.consoleInput != null)
            {
                command = CommandFromInput(this.consoleInput);
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

        private string CommandFromInput(string[] input) {

            string inputString = string.Join(" ", input);

            int firstWhitespaceIndex = inputString.IndexOfAny(new[] { ' ', '\t' });

            if (firstWhitespaceIndex == -1)
            {
                return inputString;
            }

            string firstWord = inputString.Substring(0, firstWhitespaceIndex);

            return firstWord;
        }
    }
    
}
