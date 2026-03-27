using Fsharp.Lib;
using Microsoft.FSharp.Core;

// Helper to print section headers
void WriteTitle(string title) => Console.WriteLine($"\n===== {title} =====\n");

// ──────────────────────────────────────────────
// 1. CLASS
// ──────────────────────────────────────────────
WriteTitle("Class");

var product = new Product(1, "Phone");
Console.WriteLine(product);
Console.WriteLine($"  Id          : {product.Id}");
Console.WriteLine($"  Price       : {product.Price}");
Console.WriteLine($"  IsExpensive : {product.IsExpensive}");
Console.WriteLine($"  CanSellTo US: {product.CanBeSoldTo("US")}");
Console.WriteLine($"  CanSellTo JP: {product.CanBeSoldTo("JP")}");

// ──────────────────────────────────────────────
// 2. RECORD – structural equality
// ──────────────────────────────────────────────
WriteTitle("Record & Structural Equality");

var person1 = new Person("John", "Doe", new DateTime(1990, 6, 15));
var person2 = new Person("John", "Doe", new DateTime(1990, 6, 15));

Console.WriteLine($"  person1          : {person1}");
Console.WriteLine($"  person1 == person2: {person1.Equals(person2)}");
Console.WriteLine($"  FullName         : {person1.FullName}");
Console.WriteLine($"  IsBirthday       : {person1.IsBirthday()}");

// Mutate the mutable field
person1.LastName = "Smith";
Console.WriteLine($"  After mutation   : {person1.FullName}");
Console.WriteLine($"  Still equal?     : {person1.Equals(person2)}");

// ──────────────────────────────────────────────
// 3. DISCRIMINATED UNIONS & ENUMS
// ──────────────────────────────────────────────
WriteTitle("Discriminated Unions & Enums");

Console.WriteLine($"  Color union : {Color.Red}");
Console.WriteLine($"  Color enum  : {ColorEnum.Blue}");

var payment = PaymentMethod.NewCreditCard(
    CardType.Visa,
    CardNumber.NewCardNumber("4111-1111-1111-1111"));
Console.WriteLine($"  Payment     : {payment}");

// ──────────────────────────────────────────────
// 4. PATTERN MATCHING
// ──────────────────────────────────────────────
WriteTitle("Pattern Matching");

Console.WriteLine($"  Match 1 -> {PureFsharp.IntPatternMatching(1)}");
Console.WriteLine($"  Match 3 -> {PureFsharp.IntPatternMatching(3)}");
Console.WriteLine($"  Match 8 -> {PureFsharp.IntPatternMatching(8)}");

Console.WriteLine($"  TypeTest(person)  -> {PureFsharp.TypeTesting(person1)}");
Console.WriteLine($"  TypeTest(\"hello\") -> {PureFsharp.TypeTesting("hello")}");
Console.WriteLine($"  TypeTest(42)      -> {PureFsharp.TypeTesting(42)}");

// ──────────────────────────────────────────────
// 5. OPTION TYPE
// ──────────────────────────────────────────────
WriteTitle("Option Type");

var someValue = PureFsharp.GetOptionType;         // FSharpOption<string>
Console.WriteLine($"  Option value : {someValue.Value}");
Console.WriteLine($"  UseOption(Some) : {PureFsharp.UseOptionType(FSharpOption<string>.Some("test"))}");
Console.WriteLine($"  UseOption(None) : {PureFsharp.UseOptionType(FSharpOption<string>.None)}");

// ──────────────────────────────────────────────
// 6. EXTENSION METHODS
// ──────────────────────────────────────────────
WriteTitle("Extension Methods");

Console.WriteLine($"  4.IsEven()     : {4.IsEven()}");
Console.WriteLine($"  7.IsEven()     : {7.IsEven()}");
Console.WriteLine($"  5.IsPositive() : {5.IsPositive()}");
Console.WriteLine($"  -1.IsPositive(): {(-1).IsPositive()}");

// ──────────────────────────────────────────────
// 7. ACTIVE PATTERNS
// ──────────────────────────────────────────────
WriteTitle("Active Patterns");

Console.WriteLine($"  EvenOrOdd(4) -> {PureFsharp.EvenOrOdd(4)}");
Console.WriteLine($"  EvenOrOdd(7) -> {PureFsharp.EvenOrOdd(7)}");

// ──────────────────────────────────────────────
// 8. TUPLES
// ──────────────────────────────────────────────
WriteTitle("Tuples");

var (a, b, c) = PureFsharp.UseTuples(3, 5, 10);
Console.WriteLine($"  UseTuples(3, 5, 10) -> ({a}, {b}, {c})");

// ──────────────────────────────────────────────
// 9. COLLECTIONS
// ──────────────────────────────────────────────
WriteTitle("Collections");

var input = Microsoft.FSharp.Collections.ListModule.OfSeq([1, 2, 3, 4, 5]);
var result = PureFsharp.SquareAndFilterEven(input);
Console.WriteLine($"  SquareAndFilterEven([1..5]) -> [{string.Join(", ", result)}]");

var sum = PureFsharp.SumAll([10, 20, 30]);
Console.WriteLine($"  SumAll([10, 20, 30])       -> {sum}");

// ──────────────────────────────────────────────
// 10. ASYNC / TASK
// ──────────────────────────────────────────────
WriteTitle("Async / Task");

var greeting = await PureFsharp.GreetAsync("World");
Console.WriteLine($"  GreetAsync(\"World\") -> {greeting}");

// ──────────────────────────────────────────────
// 11. RESULT / VALIDATION
// ──────────────────────────────────────────────
WriteTitle("Result / Validation");

var (ok1, value1, _) = PureFsharp.TryValidateName("Alice");
Console.WriteLine($"  Validate 'Alice' -> success={ok1}, value={value1}");

var (ok2, _, error2) = PureFsharp.TryValidateName("");
Console.WriteLine($"  Validate ''      -> success={ok2}, error={error2}");

var (ok3, value3, _) = PureFsharp.TryValidatePositive("Age", 25);
Console.WriteLine($"  Validate Age=25  -> success={ok3}, value={value3}");

var (ok4, _, error4) = PureFsharp.TryValidatePositive("Age", -1);
Console.WriteLine($"  Validate Age=-1  -> success={ok4}, error={error4}");

if (!Console.IsInputRedirected)
{
    Console.WriteLine("\nDone! Press any key to exit...");
    Console.ReadKey();
}
else
{
    Console.WriteLine("\nDone!");
}
