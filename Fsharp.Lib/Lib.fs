namespace Fsharp.Lib

open System
open System.Runtime.CompilerServices
open System.Threading.Tasks

// ──────────────────────────────────────────────
// 1. CLASS – a simple product with properties and methods
// ──────────────────────────────────────────────

/// A simple class with primary constructor, mutable/immutable properties, and methods.
type Product(id: int, name: string, price: float) =

    /// Immutable identifier.
    member _.Id = id

    /// Mutable product name.
    member val Name = name with get, set

    /// Price with public getter and private setter (F# 10 access-modifier feature).
    member val Price = price with public get, private set

    /// Secondary constructor that uses a default price.
    new(id, name) = Product(id, name, Product.DefaultPrice)

    /// True when the price exceeds 10.00.
    member this.IsExpensive = this.Price > 10.00

    /// Returns true if the product can be sold to the given country code.
    member _.CanBeSoldTo(countryCode: string) =
        match countryCode with
        | "US" | "CA" | "UK" -> true
        | _ -> false

    /// A shared default price.
    static member DefaultPrice = 9.99

    override this.ToString() =
        $"Product(Id={this.Id}, Name={this.Name}, Price={this.Price})"

// ──────────────────────────────────────────────
// 2. RECORD – immutable-by-default data structure
// ──────────────────────────────────────────────

/// An F# record. Records have structural equality and pretty printing built in.
type Person =
    { FirstName: string
      mutable LastName: string
      DateOfBirth: DateTime }

    /// Full display name.
    member this.FullName = $"{this.FirstName} {this.LastName}"

    /// True if today is the person's birthday.
    member this.IsBirthday() =
        DateTime.Today.Month = this.DateOfBirth.Month
        && DateTime.Today.Day = this.DateOfBirth.Day

// ──────────────────────────────────────────────
// 3. DISCRIMINATED UNIONS & ENUMS
// ──────────────────────────────────────────────

/// A simple discriminated union (like a sealed class hierarchy in C#).
type Color =
    | Red
    | Green
    | Blue

/// An F# enum backed by int values.
type ColorEnum =
    | Red = 1
    | Green = 2
    | Blue = 3

type CheckNumber = CheckNumber of int

type CardType =
    | MasterCard
    | Visa

type CardNumber = CardNumber of string

/// Discriminated union modelling payment methods.
[<NoComparison>]
type PaymentMethod =
    | Cash
    | Check of CheckNumber
    | CreditCard of CardType * CardNumber

// ──────────────────────────────────────────────
// 4. RESULT TYPE – for error handling without exceptions
// ──────────────────────────────────────────────

/// Validation helpers that return Result<'T, string>.
module Validation =

    /// Validates that a string is not null or whitespace.
    let validateNonEmpty (fieldName: string) (value: string) =
        if String.IsNullOrWhiteSpace value then
            Error $"{fieldName} must not be empty"
        else
            Ok value

    /// Validates that an integer is positive.
    let validatePositive (fieldName: string) (value: int) =
        if value <= 0 then
            Error $"{fieldName} must be positive"
        else
            Ok value

// ──────────────────────────────────────────────
// 5. EXTENSION METHODS – usable from C#
// ──────────────────────────────────────────────

/// Extension methods on System.Int32, callable from C#.
[<Extension>]
type IntExtensions =

    /// Returns true if the integer is even.
    [<Extension>]
    static member IsEven(value: int) = value % 2 = 0

    /// Returns true if the integer is a positive number.
    [<Extension>]
    static member IsPositive(value: int) = value > 0

// ──────────────────────────────────────────────
// 6. MODULE – pure functions, pattern matching, active patterns, async
// ──────────────────────────────────────────────

module PureFsharp =

    // — Pattern matching on integers —

    let IntPatternMatching x =
        match x with
        | 1 -> "one"
        | 2 -> "two"
        | 3 -> "three"
        | n when n % 2 = 0 -> "even"
        | _ -> "other"

    // — Type-test pattern matching —

    let TypeTesting (obj: obj) =
        match obj with
        | :? string as s -> $"Obj is string with value {s}"
        | :? int as i -> $"Obj is int with value {i}"
        | :? Person as p -> $"Obj is Person with name {p.FullName}"
        | _ -> "Obj is something else"

    // — Option type —

    let GetOptionType = Some "hello"

    let UseOptionType opt =
        match opt with
        | Some _ -> "has a value"
        | None -> "no value"

    // — Active patterns —

    let (|Even|Odd|) input =
        if input % 2 = 0 then Even else Odd

    let EvenOrOdd n =
        match n with
        | Even -> $"{n} is even"
        | Odd -> $"{n} is odd"

    // — Tuples —

    let UseTuples (a, b) c = (a + 1, b - 1, c * c)

    // — Collections —

    /// Squares every element, then keeps only even results.
    let SquareAndFilterEven (numbers: int list) =
        numbers
        |> List.map (fun n -> n * n)
        |> List.filter (fun n -> n % 2 = 0)

    /// Returns the sum of a sequence of integers.
    let SumAll (numbers: int seq) = Seq.sum numbers

    // — Async / Task interop —

    /// An async computation that simulates work and returns a greeting.
    let GreetAsync (name: string) : Task<string> =
        task {
            do! Task.Delay(100)
            return $"Hello, {name}!"
        }

    /// Demonstrates F# 10 'and!' for concurrent task awaiting in task CEs.
    let FetchBothAsync (nameA: string, nameB: string) : Task<string * string> =
        task {
            let! greetA = GreetAsync nameA
            and! greetB = GreetAsync nameB
            return (greetA, greetB)
        }

    // — ValueOption optional parameters (F# 10) —

    /// Uses [<Struct>] on optional parameter for zero-allocation ValueOption.
    let Greet (name: string) ([<System.Runtime.InteropServices.Optional; System.Runtime.InteropServices.DefaultParameterValue(null: string)>] greeting: string) =
        let g = if isNull greeting then "Hello" else greeting
        $"{g}, {name}!"

    /// Demonstrates ValueOption for optional values without heap allocation.
    let TryParseInt (input: string) : int voption =
        match System.Int32.TryParse(input) with
        | true, v -> ValueSome v
        | _ -> ValueNone

    // — Result helpers for C# interop —

    /// Wraps an F# Result into a tuple (bool success, T value, string error) for easy C# consumption.
    let TryValidateName (name: string) : struct (bool * string * string) =
        match Validation.validateNonEmpty "Name" name with
        | Ok v -> struct (true, v, null)
        | Error e -> struct (false, null, e)

    /// Wraps an F# Result into a tuple for positive int validation.
    let TryValidatePositive (fieldName: string) (value: int) : struct (bool * int * string) =
        match Validation.validatePositive fieldName value with
        | Ok v -> struct (true, v, null)
        | Error e -> struct (false, 0, e)
   