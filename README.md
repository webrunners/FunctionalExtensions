FunctionalExtensions
====================

This project includes some types and functions that support concepts from functional programming.
Types:
* Option&lt;T&gt;
* Choice&lt;T1, T2&gt;.

### Examples
#### Option Applicative
```c#
var result = Fun.Create((decimal x, decimal y) => x/y)
    .ReturnOption()
    .OnExceptionNone()
    .Curry()
    .ToOption()
    .Do(() => Console.Write("Enter a flaoting point number: "))
    .Apply(ReadDecimal())
    .Do(() => Console.Write("Enter a flaoting point number: "))
    .Apply(ReadDecimal())
    .Match(
        x => String.Format("Result = {0}", x.ToString("F")),
        () => "An error occurred.");

Console.WriteLine(result);
```

### Validation Framework (Fluent API)

It also includes a simple validation framework that makes use of the Choice type.
There is a fluent API on top of the validation framework.

#### Grammar Diagram

![Fluent Validation API](https://raw.githubusercontent.com/webrunners/FunctionalExtensions/develop/FunctionalExtensions/SolutionItems/FluentGrammar/Validate.png "Fluent Validation API")
