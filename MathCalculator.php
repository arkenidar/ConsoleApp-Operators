<?php

// to run this script in the command line, use the following command:
// php MathCalculator.php

// example run :
// enter a math formula (with spaces) : 5 + 2 * 3
// The result of '5 + 2 * 3' is '21'

class MathCalculator
{

    public static function processFormulaVerbose(string $line): void
    {
        try {
            $result = self::processFormulaString($line);
            echo "The result of '$line' is '$result'\n";
        } catch (Exception $e) {
            echo "Error: " . $e->getMessage() . "\n";
        }
    }

    public static function processFormulaString(string $line): float
    {
        $words = explode(" ", $line);
        return self::processFormulaWords($words, 0, count($words) - 1);
    }

    public static function processFormulaWords(array $words, int $startIndex, int $endIndex): float
    {
        if ($endIndex < $startIndex) {
            throw new RangeException("Invalid index: endIndex < startIndex");
        }

        $currentOperator = null;
        $result = null;

        for ($currentIndex = $startIndex; $currentIndex <= $endIndex; $currentIndex++) {
            $word = $words[$currentIndex];
            $isNumber = is_numeric($word);

            if ($isNumber || $word === "(") {
                if ($word === "(") {
                    $depth = 0;
                    $closingIndex = -1;

                    for ($searchClosingIndex = $currentIndex; $searchClosingIndex <= $endIndex; $searchClosingIndex++) {
                        if ($words[$searchClosingIndex] === "(") {
                            $depth++;
                        } elseif ($words[$searchClosingIndex] === ")") {
                            $depth--;
                        }

                        if ($words[$searchClosingIndex] === ")" && $depth === 0) {
                            $closingIndex = $searchClosingIndex;
                            break;
                        }
                    }

                    if ($closingIndex === -1) {
                        throw new InvalidArgumentException("Invalid input: no closing parenthesis");
                    }

                    $number = self::processFormulaWords($words, $currentIndex + 1, $closingIndex - 1);
                    $currentIndex = $closingIndex;
                } else {
                    $number = (float)$word;
                }

                if ($currentOperator === null) {
                    if ($result !== null) {
                        throw new InvalidArgumentException("Invalid input: operator expected");
                    }
                    $result = $number;
                } elseif ($currentOperator === "sqrt") {
                    $result = sqrt($number);
                } elseif ($result !== null) {
                    switch ($currentOperator) {
                        case "+":
                            $result += $number;
                            break;
                        case "-":
                            $result -= $number;
                            break;
                        case "*":
                            $result *= $number;
                            break;
                        case "/":
                            if ($number === 0) {
                                throw new DivisionByZeroError("Division by zero");
                            }
                            $result /= $number;
                            break;
                        case "^":
                            $result = pow($result, $number);
                            break;
                        case "%":
                            $result %= $number;
                            break;
                        default:
                            throw new InvalidArgumentException("Invalid operator: '$currentOperator'");
                    }
                } else {
                    throw new InvalidArgumentException("Invalid input: no result available");
                }

                $currentOperator = null;
            } elseif (in_array($word, ["+", "-", "*", "/", "^", "%", "sqrt"], true)) {
                if ($currentOperator !== null) {
                    throw new InvalidArgumentException("Invalid input: operator '$currentOperator' already set");
                }
                $currentOperator = $word;
            } else {
                throw new InvalidArgumentException("Invalid input: '$word' is not a number or operator");
            }
        }

        if ($result === null) {
            throw new InvalidArgumentException("Invalid input: no result computed");
        }

        return $result;
    }
}

while (true) {
    $ask = "enter a math formula (with spaces) : ";
    if (function_exists('readline')) {
        $line = readline('[readline] ' . $ask);
    } else {
        echo '[fgets] ' . $ask;
        $line = trim(fgets(STDIN));
    }
    if ($line === '' || $line === null || $line === false || $line === "exit") {
        break;
    }
    MathCalculator::processFormulaVerbose($line);
}
// php MathCalculator.php