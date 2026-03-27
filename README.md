# F# from C# — Interoperability Samples

A practical guide showing how to **consume an F# library from a C# application**. Each example demonstrates a different F# feature and how it looks when called from C#.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download) (or later)

## Quick Start

```bash
# Clone the repository
git clone https://github.com/evilz/Fsharp-from-csharp.git
cd Fsharp-from-csharp

# Build the solution
dotnet build

# Run the C# console app
dotnet run --project CsharpApp
```

## Project Structure

```
Fsharp-from-csharp.sln
├── Fsharp.Lib/          # F# class library (net9.0)
│   ├── Lib.fs           # All F# types and functions
│   └── Fsharp.Lib.fsproj
└── CsharpApp/           # C# console app (net9.0)
    ├── Program.cs       # Demonstrates calling every F# feature
    └── CsharpApp.csproj
```

## What's Covered

### 1. Classes

F# classes with primary constructors, mutable/immutable properties, methods, and secondary constructors map directly to regular .NET classes.

| F# | C# usage |
|---|---|
| `type Product(id, name, price)` | `new Product(1, "Phone")` |
| `member _.Id` (immutable) | `product.Id` |
| `member val Name with get, set` (mutable) | `product.Name = "Tablet"` |
| `member this.IsExpensive` | `product.IsExpensive` |
| `member _.CanBeSoldTo(code)` | `product.CanBeSoldTo("US")` |

### 2. Records & Structural Equality

F# records are immutable data types with **built-in structural equality** — two records with the same field values are equal. They also get automatic `ToString()`.

```csharp
var p1 = new Person("John", "Doe", new DateTime(1990, 6, 15));
var p2 = new Person("John", "Doe", new DateTime(1990, 6, 15));
p1.Equals(p2) // true — structural equality!
p1.FullName   // "John Doe"
```

### 3. Discriminated Unions & Enums

**Discriminated unions** (DUs) are type-safe algebraic data types. In C# they appear as a class hierarchy with static factory methods.

```csharp
// Simple union
Color.Red

// Enum
ColorEnum.Blue

// Union with data
PaymentMethod.NewCreditCard(CardType.Visa, CardNumber.NewCardNumber("4111-..."))
```

### 4. Pattern Matching

F# pattern matching on integers, types, and guards is exposed as regular functions.

```csharp
PureFsharp.IntPatternMatching(1)  // "one"
PureFsharp.IntPatternMatching(8)  // "even" (guard: n % 2 = 0)
PureFsharp.TypeTesting("hello")   // "Obj is string with value hello"
PureFsharp.TypeTesting(42)        // "Obj is int with value 42"
```

### 5. Option Type

`FSharpOption<T>` is F#'s way to represent optional values (no nulls!).

```csharp
var some = FSharpOption<string>.Some("test");
var none = FSharpOption<string>.None;
PureFsharp.UseOptionType(some) // "has a value"
PureFsharp.UseOptionType(none) // "no value"
```

### 6. Extension Methods

F# extension methods decorated with `[<Extension>]` work seamlessly in C#.

```csharp
4.IsEven()      // true
7.IsEven()      // false
5.IsPositive()  // true
```

### 7. Active Patterns

Active patterns let you define custom matching logic. From C# they are called as ordinary functions.

```csharp
PureFsharp.EvenOrOdd(4) // "4 is even"
PureFsharp.EvenOrOdd(7) // "7 is odd"
```

### 8. Tuples

F# struct/value tuples deconstruct naturally in modern C#.

```csharp
var (a, b, c) = PureFsharp.UseTuples(3, 5, 10);
// a=4, b=4, c=100
```

### 9. Collections

F# list/sequence functions can be called from C# by converting between .NET and F# collection types.

```csharp
var input = ListModule.OfSeq([1, 2, 3, 4, 5]);
var result = PureFsharp.SquareAndFilterEven(input); // [4, 16]
var sum = PureFsharp.SumAll([10, 20, 30]);          // 60
```

### 10. Async / Task

F# `task { }` computation expressions return `Task<T>`, so they integrate with C# `async`/`await` with zero friction.

```csharp
var greeting = await PureFsharp.GreetAsync("World");
// "Hello, World!"
```

### 11. Result Type / Validation

F# `Result<'T, 'E>` doesn't map cleanly to C#, so the library exposes a **tuple-based wrapper** `(bool success, T value, string error)` for easy consumption.

```csharp
var (ok, value, _)   = PureFsharp.TryValidateName("Alice"); // ok=true
var (ok2, _, error)  = PureFsharp.TryValidateName("");       // ok=false, error="Name must not be empty"
```

## Tips for F# / C# Interop

- **Records** give you free equality, comparison, and formatting — great for DTOs.
- **Discriminated unions** are powerful but verbose to use from C# — consider wrapping complex cases.
- **`Option<T>`** → use `FSharpOption<T>.Some/None`; or wrap with nullable helpers.
- **`Result<T,E>`** → expose as tuples or custom result classes for idiomatic C#.
- **`task { }`** in F# returns `Task<T>` directly — perfect for `await`.
- **Extension methods** need `[<Extension>]` on both the type and the method.
- **Modules** compile to static classes — call with `ModuleName.FunctionName()`.

## License

[MIT](LICENSE)
