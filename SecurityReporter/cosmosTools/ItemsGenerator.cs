using cosmosTools.Service;
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
        private int amount;

        public ICosmosToolsService CosmosService { get; }

       
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
                    amount = SecondArgumentAsInteger(this.consoleInput);
                    AddItemsToDatabase(amount);
                }
                if (command == "clear")
                {
                    ClearDatabase();
                }
                if (command == "help")
                {
                    Help();
                }
            }
            else 
            {
                Console.WriteLine("Ivalid input");
            }
        }

        private string CommandFromInput(string[] input) 
        {
            string inputString = string.Join(" ", input);

            int firstWhitespaceIndex = inputString.IndexOfAny(new[] { ' ', '\t' });

            if (firstWhitespaceIndex == -1)
            {
                return inputString;
            }

            string firstWord = inputString.Substring(0, firstWhitespaceIndex);

            return firstWord;
        }

        private int SecondArgumentAsInteger(string[] input)
        {
            string inputString = string.Join(" ", input);

            int firstWhitespaceIndex = inputString.IndexOfAny(new[] { ' ', '\t' });

            if (firstWhitespaceIndex == -1)
            {
                throw new ArgumentException("Input must contain at least two elements separated by whitespace.");
            }

            string secondArgumentString = inputString.Substring(firstWhitespaceIndex + 1);

            if (!int.TryParse(secondArgumentString, out int secondArgument))
            {
                throw new ArgumentException("Second argument is not a valid number.");
            }

            return secondArgument;
        }

        private void Help() 
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("add [number] --> add [number] of random items to database");
            Console.WriteLine("clear --> deletes all items from database");
            Console.WriteLine();
        }

        private async void ClearDatabase() 
        {
            bool result = await CosmosService.DeleteAllProjects();
            Console.WriteLine("Database has been cleared.");
            Console.WriteLine();
        }

        private void AddItemsToDatabase(int amount)
        { 
            Console.WriteLine("Added " + amount + " items.");
            Console.WriteLine();
        }
    }    
}
