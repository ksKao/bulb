<p align="center">
  <img width="287" height="113" alt="bulb_logo" src="https://github.com/user-attachments/assets/008555fd-9019-4cd6-85f2-43bdcbdb3eac" />
</p>

Bulb is a lightweight, interpreted scripting language built with C#. It is inspired by JavaScript/TypeScript and supports essential constructs for scripting tasks.

## Quick Start
Download the latest release from the [Bulb Releases Page](https://github.com/ksKao/bulb/releases) and select the executable for your operating system.

Run the Bulb interpreter using the following command:
```bash
<executable_name> <path_to_script>
``` 

Example: 
```bash
win-x64 ./index.bulb
```

## Grammars and Syntax

### Data Types
Bulb supports the following basic data types:
- `number`: Represents numeric values (integers or floating-point).
- `bool`: Represents boolean values (`true` or `false`).
- `string`: Represents a sequence of characters enclosed in double quotes.


### Console Output
Use the `print` statement to output values to the console:
```js
print "Hello World";
```

### Variable Declaration
Variables in Bulb are statically typed. You must assign a value at the time of declaration:
```js
let num = 0;
let isAdmin = true;
let message = "Hello World";
```

### Binary Operators
Bulb supports the following binary operators:

Arithmetic operators: `+`, `-`, `*`, `/` for numbers.

Boolean operators: `&&`, `||`, `!`, for booleans.

String concatenation: The `+` operator also concatenates strings. Concatenating different types to strings is also possible.
```js
let num1 = 2;
let num2 = 3;

print num1 + num2;

let hello = "Hello";
let world = "World";

print hello + " " + world;

print true && false;
```

### If Statements
If statements must be enclosed in curly braces, even if the body contains a single line:
```js
let a = 3;

if (a > 5) {
  print "a is bigger than 5";
} else if (a == 5) {
  print "a is equal to 5";
} else {
  print "a is smaller than 5";
}
```

### Loops
Both `while` and `for` loops are supported.
```js
let i = 0;

while (i < 5) {
  print i;
  i++;
}
```

```js
for (let i = 0; i < 5; i++) {
  print i;
}
```

### Scopes
Variables declared in the outer scope cannot be redeclared in the inner scope. For example, the following code will result in an error:
```js
let a = 10;
{
  let a = 10; // error!
}
```

### Functions
Function syntax is similar to TypeScript. Both parameter and return types are required:
```ts
function sum(a: number, b: number): number {
  return a + b;
}

function printNum(num: number): void {
  print num;
}

printNum(sum(1, 2));
```

### Comments
Comments are also supported, but multiline comments are not.
```js
// this is a comment
print x;
```

### Methods

Bulb provides several built-in helper methods on core data types to make common operations easier.

#### `number` methods

* **`toString(): string`**
  Converts the number to its string representation.
* **`toFixed(precision: number): string`**
  Returns a string version of the number rounded to the given number of decimal places.

#### `string` methods

* **`length`** → `number`
  The number of characters in the string.
* **`isNumber(): bool`**
  Returns `true` if the string can be safely parsed as a number.
* **`toNumber(): number`**
  Returns a number typed value for the string, will throw if the string contains invalid character(s).
* **`toUpper(): string`**
  Returns the string in uppercase.
* **`toLower(): string`**
  Returns the string in lowercase.
* **`charAt(index: number): string`**
  Returns the character at the given index.

### Built-in Functions

Bulb also includes a small set of built-in functions to support interactive programs.

```js
let message = prompt();
print "Your message is '" + message + "'";

message = prompt("Enter another message: ");
print "Your second message is '" + message + "'";
```

**Output**

```
hello
Your message is 'hello'
Enter another message: world
Your second message is 'world'
```

## Examples

### Fibonacci Calculator
```ts
function fib(n: number): number {
  if (n == 0) {
    return 0;
  } else if (n == 1) {
    return 1;
  } else {
    return fib(n - 1) + fib(n - 2);
  }
}

for (let i = 0; i < 10; i++) {
  print fib(i);
}
```

Output:
```
0
1
1
2
3
5
8
13
21
34
```

### Simple Calculator Program
```ts
while (true) {
	let operator = prompt("Enter an operator (+, -, *, /, or 'quit' to quit): ");

	if (operator.toLower() == "quit") {
		print "Thank you for using the calculator :)";
		return;
	}

	let num1Str = prompt("Enter the first number: ");

	if (!num1Str.isNumber()) {
		print num1Str + " is not a valid number";
		continue;
	}

	let num2Str = prompt("Enter the second number: ");

	if (!num2Str.isNumber()) {
		print num2Str + " is not a valid number";
		continue;
	}

	let num1 = num1Str.toNumber();
	let num2 = num2Str.toNumber();

	let answer = 0;
	if (operator == "+") {
		answer = num1 + num2;
	} else if (operator == "-") {
		answer = num1 - num2;
	} else if (operator == "*") {
		answer = num1 * num2;
	} else if (operator == "/") {
		if (num2 == 0) {
			print "Cannot divide by 0";
			continue;
		}

		answer = num1 / num2;
	} else {
		print operator + " is not a valid operator";
		continue;
	}

	print "The result is: " + answer;
}
```

Output: 
```
Enter an operator (+, -, *, /, or 'quit' to quit): +
Enter the first number: 1
Enter the second number: 5
The result is: 6
Enter an operator (+, -, *, /, or 'quit' to quit): -
Enter the first number: -2
Enter the second number: 3
The result is: -5
Enter an operator (+, -, *, /, or 'quit' to quit): *
Enter the first number: 5
Enter the second number: 1.5
The result is: 7.5
Enter an operator (+, -, *, /, or 'quit' to quit): /
Enter the first number: 2
Enter the second number: 0
Cannot divide by 0
Enter an operator (+, -, *, /, or 'quit' to quit): /
Enter the first number: 5
Enter the second number: 2
The result is: 2.5
Enter an operator (+, -, *, /, or 'quit' to quit): quit
Thank you for using the calculator :)
```