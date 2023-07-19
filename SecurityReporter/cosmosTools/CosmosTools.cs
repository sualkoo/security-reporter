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
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("-------------------------------");
        Console.WriteLine("| Cosmos Tools - DB Generator |");
        Console.WriteLine("-------------------------------");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public async Task Run()
    {
        ItemsGenerator Generator = new ItemsGenerator();

        while (command != "quit")
        {
            consoleInput = Console.ReadLine();
            command = CommandFromInput(consoleInput);
            if (command == "add")
            {
                amount = SecondArgumentAsInteger(consoleInput);
                await Generator.AddItemsToDatabaseAsync(amount);
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
}
