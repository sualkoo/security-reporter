using cosmosTools;

class CosmosTools
{
    private string? consoleInput;
    private string? command;
    private int amount;

    static async Task Main(string[] args)
    {
        CosmosTools tool = new CosmosTools();
        await tool.Run();
    }

    public CosmosTools()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("-------------------------------");
        Console.WriteLine("| Cosmos Tools - DB Generator |");
        Console.WriteLine("-------------------------------");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public async Task Run()
    {

        ItemsGenerator Generator = new ItemsGenerator(PrimaryKeyCheck());

        while (command != "quit")
        {
            consoleInput = Console.ReadLine();
            command = CommandFromInput(consoleInput);
            if (command == "add")
            {
                amount = SecondArgumentAsInteger(consoleInput);
                await Generator.AddItemsToDatabaseAsync(amount);
            }
            else if (command == "addroles")
            {
                amount = SecondArgumentAsInteger(consoleInput);
                await Generator.AddRolesToDatabaseAsync(amount);
            }
            else if (command == "clearroles")
            {
                await Generator.ClearRoleDatabaseAsync();
            }
            else if (command == "clear")
            {
                await Generator.ClearDatabaseAsync();
            }
            else if (command == "help")
            {
                Generator.Help();
            }
            else if (command == "quit")
            {
                break;
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid command!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }

        }
    }

    private string CommandFromInput(string input)
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

    private int SecondArgumentAsInteger(string input)
    {
        string[] inputWords = input.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        if (inputWords.Length < 2)
        {
            throw new ArgumentException("Input must contain at least two elements separated by whitespace.");
        }

        if (!int.TryParse(inputWords[1], out int secondArgument))
        {
            throw new ArgumentException("Second argument is not a valid number.");
        }

        return secondArgument;
    }

    private string PrimaryKeyCheck()
    {
        this.ConsoleMessage("Do you want to use your own Primary Key [y/n] ? ");

        while (consoleInput != "y" || consoleInput != "n" || consoleInput != "Y" || consoleInput != "N")
        {
            string? consoleInput = Console.ReadLine();
            string? key;

            if (consoleInput == "y" || consoleInput == "Y")
            {
                this.ConsoleMessage("Your primary key: ");
                key = Console.ReadLine();
                this.ConsoleMessage("Primary Key set to: " + key);

                return key;
            }
            else if (consoleInput == "n" || consoleInput == "N")
            {
                this.ConsoleMessage("Used default Primary Key.");
                break;
            }
        }
        return "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    }

    private void ConsoleMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine();
        Console.WriteLine(message);
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
    }
}
