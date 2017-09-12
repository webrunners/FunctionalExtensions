Functional Extensions for C#
====================

[![Bulid status](https://ci.appveyor.com/api/projects/status/3gmwp78e7pcmaa57?svg=true)](https://ci.appveyor.com/project/cvk77/functionalextensions)

This project includes types and higher-order functions adopted from functional programming.
### Table of contents
* [Option&lt;T&gt;](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#optiont)  
    * [Creation of an instance of Option](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#creation-of-an-instance-of-option)  
    * [Extraction of the value from an instance of Option](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#extraction-of-the-value-from-an-instance-of-option)  
* [Choice&lt;T1, T2&gt;](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#choicet1t2) 
    * [Creation of an instance of Choice](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#creation-of-an-instance-of-choice)  
    * [Extraction of the value from an instance of Choice](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#extraction-of-the-value-from-an-instance-of-choice)  
* [Functions and higher-order functions](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#functions-and-higher-order-functions)  
    * [Fun.Create](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#funcreate)  
    * [ReturnOption](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#returnoption)  
    * [OnExceptionNone](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#onexceptionnone)  
    * [Parsing](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#parsing)  
    * [Curry](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#curry)  
* [Option Applicative](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#option-applicative)  
* [Linq query syntax](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#linq-query-syntax)  
    * [Option (with Linq query syntax)](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#option-with-linq-query-syntax)  
    * [Choice Applicative (with Linq query syntax)](https://github.com/webrunners/FunctionalExtensions/blob/develop/README.md#choice-applicative-with-linq-query-syntax)  
    
### Option&lt;T&gt;
The ``Option<T>`` type encapsulates an optional value. It is usefull when the actual value (of type ``T``) might not exist. ``Option`` is defined as a union type with two cases: ``Some`` and ``None``.

#### Creation of an instance of Option
```c#
var someInt = Option.Some(42);      // creates an instance of Some<int>
var noneInt = Option.None<int>();   // creates an instance of None<int>
```
```c#
var someInt = 5.ToOption();     // creates Some<int>

string s = null;
var noneString = s.ToOption();  // yields None<string>
```

##### Implicit Operator
```c#
Option<string> s = "hello"; // creates an instance of Some<string>
```

##### Nullable
```c#
int? i = 42;
var o = i.ToOption(); // creates an instance of Some<int>
```
##### Null
If ``null`` is passed as an argument to ``Option.Some<T>(T value)`` it will yield ``None``:
```c#
var none = Option.Some<string>(null); // the result is None<string>
```
#### Extraction of the value from an instance of Option
The extraction of the value should be easy but safe. It is supposed to be made difficult to extract the value when it is null (in case of ``None``) to prevent any kind of NullReferenceExceptions. Also the consumer should be forced to handle both cases of an exisitng value (Some) and non-existing value (None). (This concept is close to pattern matching from FP, e.g. F#.)

The value can be retrieved by calling the ``Match`` method. This method has the following signature: 
``TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)`` or in F# notation: ``((T -> TResult)*(unit -> TResult)) -> TResult``

```c#
Option.Some(49.9m)
    .Match(
        x => String.Format("Result = {0} %", x.ToString("F")),
        () => "An error occurred.");
```

##### DefaultIfNone
The `DefaultIfNone(T defaultValue)` extension method will return the inner value of an Option instance. If the Option is `None` a default value that is specified as an argument will be returned.
```c#
var x = Option.None<int>().DefaultIfNone(-1); // x will be -1
var y = Option.Some(42).DefaultIfNone(-1);    // y will be 42
```
### Choice&lt;T1,T2&gt;
`Choice<T1, T2>` is a type which represents either a value of type `T1` or a value of type `T2`.
#### Creation of an instance of Choice
Create an instance of `Choice` that represents Choice 1 of 2:
```c#
var c = Choice.NewChoice1Of2<decimal, string>(2.5m);
```
Create an instance of `Choice` that represents Choice 2 of 2:
```c#
var c = Choice.NewChoice2Of2<decimal, string>("An error occurred.");
```
##### ToChoice
An instance of `Choice` can be created from an instance of `Option`. If the Option is Some then the Choice will represent the value of the Option instance.
```c#
var c = Option.Some(42).ToChoice("No value specified."); // c represents 42
```
If the Option is None it will represent an alternative value which was passed as an argument.
```c#
var c = Option.None<int>().ToChoice("No value specified."); // c represents "No value specified"
```

#### Extraction of the value from an instance of Choice
The extraction of the inner value of the choice type can be done with `Match()` similar to the extraction of Option values.
```c#
var result = Choice.NewChoice1Of2<string, int>("world")
    .Match(
        onChoice1Of2: x => String.Format("Hello {0}", x),
        onChoice2Of2: x => String.Format("The number is {0}", x));
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
##### Parsing
There are no parsing function included. But it is very easy to create them if needed with any operation that might return null or throw an exception.
```c#
var parseInt = Fun.Create((string s) => Int32.Parse(s)).ReturnOption().OnExceptionNone();
var i = parseInt("sdfs"); // i will be Option.None<int>
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
             
