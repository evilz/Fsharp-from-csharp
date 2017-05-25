namespace Fsharp.Lib

open System
open System.Runtime.CompilerServices

/// Example of a simple class
type Product(id, name, price) = 

    /// immutable Id property
    member this.Id = id

    /// mutable Name property
    member val Name = name with get,set

    /// mutable Price property
    member val Price = price with get,set

    /// secondary constructor
    new(id,name) = Product(id,name,Product.DefaultPrice)

    /// True if price > 10.00
    member this.IsExpensive = this.Price > 10.00

    /// Example of method
    member this.CanBeSoldTo(countryCode) = 
        match countryCode with
        | "US" 
        | "CA" 
        | "UK" -> true
        | "RU" -> false
        | _  -> false   //all others
    
    /// Example of static property
    static member DefaultPrice = 9.99
    
/// Definition of a Person
type Person = {
    FirstName: string
    mutable LastName: string
    DateOfBirth: DateTime
    }
    with 
    member this.FullName = 
        this.FirstName + " " + this.LastName

    member this.IsBirthday() = 
        DateTime.Today.Month = this.DateOfBirth.Month 
        && DateTime.Today.Day = this.DateOfBirth.Day


type Color = Red | Green | Blue
type ColorEnum = Red=1 | Green=2 | Blue=3


type CheckNumber = CheckNumber of int
type CardType = MasterCard | Visa
type CardNumber = CardNumber of string

/// PaymentMethod is cash, check or card
[<NoComparisonAttribute>]
type PaymentMethod = 
    /// Cash needs no extra information
    | Cash
    /// Check needs a CheckNumber 
    | Check of CheckNumber 
    /// CreditCard needs a CardType and CardNumber 
    | CreditCard of CardType * CardNumber 

// to be used in C#
[<Extension>]
type IntExtension =
    [<Extension>]
    static member IsEven2(i: int) = i % 2 = 0



module PureFsharp =
    open System.Runtime.CompilerServices

    let IntPatternMatching x = 
        match x with
        | 1 -> "iphone"
        | 2 -> "nexus"
        | 3 -> "winphone"
        | 4 -> "Xiaomi"
        // example of guard 
        | e when x%2 = 0 -> "even" 
        // wildcard
        | _ -> "other"


    /// demonstrates some type-testing pattern matching
    let TypeTesting obj = 
        match box obj with
        | :? string as s -> 
            sprintf "Obj is string with value %s" s
        | :? int as i -> 
            sprintf "Obj is int with value %i" i
        | :? Person as p -> 
            sprintf "Obj is Person with name %s" p.FullName
        | _ -> 
        sprintf "Obj is something else"

    let GetOptionType = 
        Some "aaa"

    let UseOptionType o = 
        match o with
        | Some a -> "has a value"
        | None -> "no value"

    // not visible in C#
    type System.Int32 with
        member this.IsEven = this % 2 = 0


    
    // active patterns
    let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd

    let EvenOrOdd n = 
        match n with
        | Even -> sprintf "%i is even" n
        | Odd -> sprintf "%i is odd" n
    
    let UseTuples (a , b) c = 
        ( a + 1, b - 1, c * c)
   