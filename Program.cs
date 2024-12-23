
// [ITA] programma CS per calcolo di formule matematica stile calcolatrice scientifica

// testing the capabilities
// [ITA] prove di usi mirati a fine di verificare correttezze di funzionamento

// Process a math formula with the operators +, -, *, /, ^, %
ProcessFormulaVerbose("1 + 2 + 3"); // 6
ProcessFormulaVerbose("1 + 2 - 3"); // 0
ProcessFormulaVerbose("1 + 2 * 3"); // 9
ProcessFormulaVerbose("1 + 2 / 3"); // 1
ProcessFormulaVerbose("1 + 2 ^ 3"); // 27
ProcessFormulaVerbose("1 + 2 % 3"); // 0
ProcessFormulaVerbose("2,5 * 2"); // 5

// Process a complex formula with nested operations
ProcessFormulaVerbose("5 + ( -1 * ( 1 - ( 3 * 2 * 2 ) / 10 ) )"); // 6,1

// Process formulas with functions like sqrt
ProcessFormulaVerbose("sqrt ( 9 + 16 )"); // 5
ProcessFormulaVerbose("( 3 ^ 2 ) + ( 4 ^ 2 )"); // 25
ProcessFormulaVerbose("sqrt ( ( 3 ^ 2 ) + ( 4 ^ 2 ) )"); // 5

// Process a formula and get a partial result
// The parameters represent the formula split into words, and the start and end indices for processing
double resultPartial = ProcessFormulaWords("1 + ( 6 / 3 )".Split(" "), 3, 5);
Console.WriteLine("Partial result of '1 + ( 6 / 3 )' : " + resultPartial);

// Start a loop to continuously accept user input for math formulas
// [ITA] ripetutamente chiede una formula di calcolo e prova a calcolarla se valida
while (true)
{
    Console.Write("Enter a supported math formula: ");
    string? line = Console.ReadLine();

    // Exit the program if user enters "exit" or null input
    if (line == null || line == "exit")
    {
        Console.WriteLine("Exiting...");
        return;
    }

    // Process the user's formula input
    ProcessFormulaVerbose(line);
}

// Processes a mathematical formula and displays the result or error message
// Parameters:
//   line: The mathematical expression to evaluate
static void ProcessFormulaVerbose(string line)
{
    try
    {
        // Attempt to evaluate the mathematical expression
        var result = ProcessFormulaString(line);
        Console.WriteLine($"The result of '{line}' is '{result}'");
    }
    catch (Exception ex)
    {
        // Display any errors that occur during processing
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static double ProcessFormulaString(string line)
{
    string[] words = line.Split(" "); // split the line into words
    return ProcessFormulaWords(words, 0, words.Length - 1);
}

// [ITA] calcola una sottoparte , utile per le coppie di parentesi ...
// ... che delimitano sottoparti
static double ProcessFormulaWords(string[] words, int startIndex, int endIndex)
{
    if (endIndex < startIndex) throw new IndexOutOfRangeException("Invalid index: endIndex < startIndex");

    string? currentOperator = null; // the current operator

    double? result = null; // the current result

    for (int currentIndex = startIndex; currentIndex <= endIndex; currentIndex++) // iterate over the words
    {

        // [ITA] scorri parte per parte ovvero parola per parola
        string word = words[currentIndex];

        // check if the word is a number
        bool isNumber = double.TryParse(word, out double number);

        // if the word is a number
        if (isNumber || word == "(")
        {
            // [ITA] numero o parentesi che calcolata produce numero

            if (word == "(")
            {
                int closingIndex = -1; // the closing index of the parenthesis
                // find the closing parenthesis
                int depth = 0;
                for (int searchClosingIndex = currentIndex; searchClosingIndex <= endIndex; searchClosingIndex += 1)
                {
                    if (words[searchClosingIndex] == "(")
                    {
                        depth++;
                    }
                    else if (words[searchClosingIndex] == ")")
                    {
                        depth--;
                    }
                    if (words[searchClosingIndex] == ")" && depth == 0) // if the closing parenthesis is found
                    {
                        closingIndex = searchClosingIndex;
                        break;
                    }
                }
                if (closingIndex == -1)
                {
                    throw new InvalidOperationException("Invalid input: no closing parenthesis");
                }
                // [ITA] dopo aver trovato la parentesi di chiusura ...
                // ... calcola il pezzo di formula da esse racchiuso
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
            else if (currentOperator == "sqrt")
            {
                // sqrt ( square root ) operator require only one operand
                // if the current operator is sqrt, calculate the square root of the number
                result = Math.Sqrt(number);
            }
            // if there is a result ( operations that require two operands )
            else if (result != null)
            {
                // [ITA] operatori che compiono operazioni sugli operandi

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
                else if (currentOperator == "^" && result != null)
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
            // [ITA] se non è numero o equivalente ( le parentesi lo sono ) ...
            // ... prova ad assicurarsi che sia un valido operatore supportato ...
            // ... e lo mette da parte per quando arriva il secondo operando ...
            // ... per gli operatori binari o l' unico operando
            // ... per sqrt ( radice quadrata ) , square root

            // if the word is not a number

            // if the word is an operator
            if (word == "+" || word == "-" || word == "*" || word == "/" || word == "^" || word == "%" || word == "sqrt")
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
