using cosmosTools;

class CosmosTools
{
    private string[]? consoleInput;
    private string? command;
    private int amount;

    static async Task Main(string[] args)
    {
        CosmosTools tool = new CosmosTools(args);
        await tool.Run();
    }

    public CosmosTools(string[] args)
    {
        consoleInput = args;
    }

    public async Task Run()
    {
        Console.WriteLine("Cosmos Tools");
        Console.WriteLine("-------------------");
        Console.WriteLine("DB Generator");
        Console.WriteLine("-------------------");

        if (consoleInput != null)
        {
            command = CommandFromInput(consoleInput);
            ItemsGenerator Generator = new ItemsGenerator();


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
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
        else
        {
            Console.WriteLine("Invalid input");
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
}
