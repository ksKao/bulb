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
