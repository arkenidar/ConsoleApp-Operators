// MathCalculator.ts
// tsc MathCalculator.ts -t esnext
export class MathCalculator {
    static processFormulaVerbose(line) {
        try {
            const result = MathCalculator.processFormulaString(line);
            console.log(`The result of '${line}' is '${result}'`);
        }
        catch (error) {
            const errorMessage = error instanceof Error ? error.message : String(error);
            console.error(`Error: ${errorMessage}`);
        }
    }
    static processFormulaString(line) {
        const words = line.split(" ");
        return MathCalculator.processFormulaWords(words, 0, words.length - 1);
    }
    static processFormulaWords(words, startIndex, endIndex) {
        if (endIndex < startIndex)
            throw new RangeError("Invalid index: endIndex < startIndex");
        let currentOperator = null;
        let result = null;
        for (let currentIndex = startIndex; currentIndex <= endIndex; currentIndex++) {
            const word = words[currentIndex];
            const isNumber = !isNaN(parseFloat(word));
            if (isNumber || word === "(") {
                let number;
                if (word === "(") {
                    let depth = 0;
                    let closingIndex = -1;
                    for (let searchClosingIndex = currentIndex; searchClosingIndex <= endIndex; searchClosingIndex++) {
                        if (words[searchClosingIndex] === "(")
                            depth++;
                        else if (words[searchClosingIndex] === ")")
                            depth--;
                        if (words[searchClosingIndex] === ")" && depth === 0) {
                            closingIndex = searchClosingIndex;
                            break;
                        }
                    }
                    if (closingIndex === -1) {
                        throw new Error("Invalid input: no closing parenthesis");
                    }
                    number = MathCalculator.processFormulaWords(words, currentIndex + 1, closingIndex - 1);
                    currentIndex = closingIndex;
                }
                else {
                    number = parseFloat(word);
                }
                if (currentOperator === null) {
                    if (result !== null) {
                        throw new Error("Invalid input: operator expected");
                    }
                    result = number;
                }
                else if (currentOperator === "sqrt") {
                    result = Math.sqrt(number);
                }
                else if (result !== null) {
                    switch (currentOperator) {
                        case "+":
                            result += number;
                            break;
                        case "-":
                            result -= number;
                            break;
                        case "*":
                            result *= number;
                            break;
                        case "/":
                            if (number === 0)
                                throw new Error("Division by zero");
                            result /= number;
                            break;
                        case "^":
                            result = Math.pow(result, number);
                            break;
                        case "%":
                            result %= number;
                            break;
                        default:
                            throw new Error(`Invalid operator: '${currentOperator}'`);
                    }
                }
                else {
                    throw new Error("Invalid input: no result available");
                }
                currentOperator = null;
            }
            else if (["+", "-", "*", "/", "^", "%", "sqrt"].includes(word)) {
                if (currentOperator !== null) {
                    throw new Error(`Invalid input: operator '${currentOperator}' already set`);
                }
                currentOperator = word;
            }
            else {
                throw new Error(`Invalid input: '${word}' is not a number or operator`);
            }
        }
        if (result === null) {
            throw new Error("Invalid input: no result computed");
        }
        return result;
    }
}
