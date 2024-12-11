// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World! from C#");

static void ProcessLine(string line)
{
    Console.WriteLine("[" + line + "]");
    string[] words = line.Split(" ");
    string currentOperator = "none";
    float result = 0;
    foreach (string word in words)
    {
        Console.WriteLine(word);

        bool isNumber = float.TryParse(word, out float number);

        if (isNumber)
        {
            Console.WriteLine("number: " + number);
            if (currentOperator == "none")
            {
                Console.WriteLine(" ! No Operator ! ");
                result = number;
            }
            else if (currentOperator == "add")
            {
                Console.WriteLine(" ! Add ! ");
                result += number;
            }
            else if (currentOperator == "subtract")
            {
                Console.WriteLine(" ! Subtract ! ");
                result -= number;
            }
            else if (currentOperator == "multiply")
            {
                Console.WriteLine(" ! Multiply ! ");
                result *= number;
            }
            else if (currentOperator == "divide")
            {
                Console.WriteLine(" ! Divide ! ");
                result /= number;
            }
            else
            {
                Console.WriteLine(" ! Unknown Operator ! ");
            }

            currentOperator = "none";
            Console.WriteLine("result: " + result);
        }
        else
        {
            Console.WriteLine(" ! Not a Number ! ");
            if (currentOperator != "none")
            {
                Console.WriteLine(" ! Operator already set ! ");
            }
            else
            if (word == "+")
            {
                currentOperator = "add";
            }
            else if (word == "-")
            {
                currentOperator = "subtract";
            }
            else if (word == "*")
            {
                currentOperator = "multiply";
            }
            else if (word == "/")
            {
                currentOperator = "divide";
            }
            else
            {
                Console.WriteLine(" ! Unknown Operator ! ");
            }
        }

    }
}

while (true)
{
    Console.Write("Enter a line of text or 'exit': ");
    string? line = Console.ReadLine();
    if (line == null || line == "exit")
    {
        Console.WriteLine("Exiting...");
        break;
    }
    ProcessLine(line);
}