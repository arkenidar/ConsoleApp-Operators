// process a math formula with the operators +, -, *, /, ^, %
ProcessFormulaVerbose("1 + 2 + 3"); // 6
ProcessFormulaVerbose("1 + 2 - 3"); // 0
ProcessFormulaVerbose("1 + 2 * 3"); // 9
ProcessFormulaVerbose("1 + 2 / 3"); // 1
ProcessFormulaVerbose("1 + 2 ^ 3"); // 27
ProcessFormulaVerbose("1 + 2 % 3"); // 0
ProcessFormulaVerbose("2,5 * 2"); // 5
ProcessFormulaVerbose("5 + ( -1 * ( 1 - ( 3 * 2 * 2 ) / 10 ) )"); // 6,1

double resultPartial = ProcessFormulaWords("1 + ( 6 / 3 )".Split(" "), 3, 5);
Console.WriteLine("Partial result of '1 + ( 6 / 3 )' : " + resultPartial);

while (true)
{
    Console.Write("Enter a supported math formula: ");
    string? line = Console.ReadLine();

    if (line == null || line == "exit")
    {
        Console.WriteLine("Exiting...");
        return;
    }

    ProcessFormulaVerbose(line);
}

static void ProcessFormulaVerbose(string line)
{
    try
    {
        var result = ProcessFormulaString(line);
        Console.WriteLine($"The result of '{line}' is '{result}'");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static double ProcessFormulaString(string line)
{
    string[] words = line.Split(" "); // split the line into words
    return ProcessFormulaWords(words, 0, words.Length - 1);
}

static double ProcessFormulaWords(string[] words, int startIndex, int endIndex)
{
    if (endIndex < startIndex) throw new IndexOutOfRangeException("Invalid index: endIndex < startIndex");

    string? currentOperator = null; // the current operator

    double? result = null; // the current result

    for (int currentIndex = startIndex; currentIndex <= endIndex; currentIndex++) // iterate over the words
    {

        string word = words[currentIndex];

        // check if the word is a number
        bool isNumber = double.TryParse(word, out double number);

        // if the word is a number
        if (isNumber || word == "(")
        {
            if (word == "(")
            {
                int closingIndex = -1; // the closing index of the parenthesis
                for (int searchClosingIndex = endIndex; searchClosingIndex >= startIndex; searchClosingIndex--)
                {
                    if (words[searchClosingIndex] == ")")
                    {
                        closingIndex = searchClosingIndex;
                        break;
                    }
                }
                if (closingIndex == -1)
                {
                    throw new InvalidOperationException("Invalid input: no closing parenthesis");
                }
                double resultPartial = ProcessFormulaWords(words, currentIndex + 1, closingIndex - 1);
                currentIndex = closingIndex;
                number = resultPartial;
            }

            // if there is no current operator
            if (currentOperator == null)
            {
                // if there is no result, set the result to the number

                // the result should not be already set
                if (result != null)
                {
                    // if the result is already set, throw an exception
                    throw new InvalidOperationException("Invalid input: the result is already set, an operator is expected");
                }

                // set the result to the number
                result = number;
            }
            // if there is a current operator
            else if (result != null) // if there is a result
            {
                if (currentOperator == "+")
                {
                    // add the number to the result
                    result += number;
                }
                else if (currentOperator == "-")
                {
                    // subtract the number from the result
                    result -= number;
                }
                else if (currentOperator == "*")
                {
                    // multiply the result by the number
                    result *= number;
                }
                else if (currentOperator == "/")
                {
                    // check if the number is zero
                    if (number == 0)
                    {
                        throw new DivideByZeroException(); // division by zero
                    }
                    // divide the result by the number
                    result /= number;
                }
                else if (currentOperator == "^")
                {
                    // raise the result to the power of the number
                    result = Math.Pow(result.Value, number);
                }
                else if (currentOperator == "%")
                {
                    // take the result modulo the number
                    result %= number;
                }
                else
                {
                    // if the operator is invalid, throw an exception
                    throw new InvalidOperationException($"Invalid operator: '{currentOperator}'");
                }

            }
            else
            {
                // if there is no result, throw an exception
                throw new InvalidOperationException("Invalid input: there is no result");
            }

            // reset the current operator
            currentOperator = null;
        }
        else
        {
            // if the word is not a number

            // if the word is an operator
            if (word == "+" || word == "-" || word == "*" || word == "/" || word == "^" || word == "%")
            {
                // if there is no current operator, set the current operator
                if (currentOperator == null)
                {
                    // set the current operator
                    currentOperator = word;
                }
                else
                {
                    // if the word is an operator and there is already a current operator, throw an exception
                    throw new InvalidOperationException($"Invalid input: the word is an operator and there is already a current operator: '{currentOperator}'");
                }
            }
            else
            {
                // if the word is not a number or an operator, throw an exception
                throw new InvalidOperationException($"Invalid input: the word is not a number or an operator: '{word}'");
            }
        }

    }

    // if there is no result, throw an exception
    if (result == null)
    {
        throw new InvalidOperationException("Invalid input: there is no result");
    }

    return result.Value; // return the result
}