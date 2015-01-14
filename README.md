Functional Extensions for C#
====================
This project includes types and higher-order functions adopted from functional programming.
### Option&lt;T&gt;
The ``Option<T>`` type encapsulates an optional value. It is usefull when the actual value (of type ``T``) might not exist. ``Option`` is defined as a union type with two cases: ``Some`` and ``None``.

#### Creation of an instances of Option
```c#
var someInt = Option.Some(42);      // creates an instance of Some<int>
var noneInt = Option.None<int>();   // creates an instance of None<int>
```
```c#
var someInt = 5.ToOption();     // creates Some<int>

string s = null;
var noneString = s.ToOption();  // yields None<string>
```
If ``null`` is passed as an argument to ``Option.Some<T>(T value)`` it will yield ``None``:
```c#
var none = Option.Some<string>(null); // the result is None<string>
```
#### Extraction of the value from an instance of Option
The value can be retrieved by calling the ``Match`` method. This method has the following signature: 
``TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)`` or in F# notation: ``((T -> TResult)*(unit -> TResult)) -> TResult``
```c#
Option.Some(49.9m)
    .Match(
        x => String.Format("Result = {0} %", x.ToString("F")),
        () => "An error occurred.");
```
### Functions and higher-order functions
##### Fun.Create
``Fun.Create()`` helps with creating instances of ``Func``.

This
```c#
var f = new Func<decimal, decimal, decimal, Option<decimal>((x1, x2, x3) => (x1 + x2) / x3));
```
can be written as
```c#
var f = Fun.Create((decimal x1, decimal x2, decimal x3) => (x1 + x2) / x3));
```
##### ReturnOption
The extension method ``ReturnOption()`` takes the result from a fuction and returns it wrapped in the option type. If the result is null it returns ``None``.
```c#
var f = Fun.Create((int i) => i%2 == 0 ? "foo" : null)
    .ReturnOption(); // returns Option.None<string> if an odd number is passed as an argument
```
##### OnExceptionNone
The extension method ``OnException()`` internally wraps a ``try catch`` around the execution and returns ``None`` if an exception is thrown. It only extends functions with return type ``Option``.
```c#
var result = Fun.Create((decimal x, decimal y) => x/y)
    .ReturnOption()
    .OnExceptionNone(); // if 0 is passed as the second argument result will be None<decimal>
```
##### Curry
``Curry()`` returns the curried version of a function. (This is i.e necessary for the option type to act like an applicative functor.)
```c#
var result = Fun.Create((decimal x, decimal y) => x/y).Curry(); // yields Func<decimal, Func<decimal, decimal>>
```
### Option Applicative
```c#
var result = Fun.Create((decimal x, decimal y) => x/y)
    .ReturnOption()                                                 
    .OnExceptionNone()                                              
    .Curry()                // returns Func<decimal, Func<decimal, Option<decimal>>                                           
    .ToOption()             // lifts the function into the option type
    .Apply(ReadDecimal())   // applies the result from ReadDecimal() and returns Func<decimal, Option<decimal>>
    .Apply(ReadDecimal())   // applies the result from ReadDecimal() and returns Option<decimal>
    .Select(x => x * 100)   // maps a function over the inner value if option is Some
    .Match(
        x => String.Format("Result = {0} %", x.ToString("F")),
        () => "An error occurred.");

Console.WriteLine(result);
```
### Linq query syntax
Also the Linq query syntax is supported.
##### Option (with Linq query syntax)
```c#
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
##### Choice Applicative (with Linq query syntax)
```c#
(
    from v1 in ReadDecimal().ToChoice(Failure.Create(Error.CannotNotParse1StInput))
    join v2 in ReadDecimal().ToChoice(Failure.Create(Error.CannotNotParse2NdInput)) on 1 equals 1
    from result in Divide(v1, v2).ToChoice(Failure.Create(Error.CannotDivideByZero))
    select result
)
    .Match(
        x => Console.WriteLine("Result = {0}", x),
        errors => errors.ToList().ForEach(x => Console.WriteLine(x.GetDisplayName())));
```

### Validation Framework (Fluent API)

This project also includes a simple validation framework that makes use of the applicative character of the Choice type.
There is a fluent API on top of the validation framework.

#### Grammar Diagram

![Fluent Validation API](https://raw.githubusercontent.com/webrunners/FunctionalExtensions/develop/FunctionalExtensions/SolutionItems/FluentGrammar/Validate.png "Fluent Validation API")
