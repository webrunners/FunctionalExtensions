FunctionalExtensions
====================

This project includes some types and functions that support concepts from functional programming.

##### Types
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
    .Select(x => x * 100)
    .Match(
        x => String.Format("Result = {0} %", x.ToString("F")),
        () => "An error occurred.");

Console.WriteLine(result);
```
#### Option Monad
```c#
private static Option<decimal> Divide(decimal a, decimal b)
{
    try { return Option.Some(a / b); }
    catch (DivideByZeroException) { return Option.None<decimal>(); }
}

private static Option<decimal> ReadDecimal()
{
    decimal i;
    return decimal.TryParse(Console.ReadLine(), out i) ? Option.Some(i) : Option.None<decimal>();
}

var output =
    (
        from v1 in ReadDecimal()
        from v2 in ReadDecimal()
        from result in Divide(v1, v2)
        select result * 100
    )
        .Match(
            x => String.Format("Result = {0} %", x.ToString("F")),
            () => "An error occurred.");

Console.WriteLine(output);
```

### Validation Framework (Fluent API)

It also includes a simple validation framework that makes use of the Choice type.
There is a fluent API on top of the validation framework.

#### Grammar Diagram

![Fluent Validation API](https://raw.githubusercontent.com/webrunners/FunctionalExtensions/develop/FunctionalExtensions/SolutionItems/FluentGrammar/Validate.png "Fluent Validation API")
