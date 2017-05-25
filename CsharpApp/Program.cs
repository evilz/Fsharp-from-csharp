using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fsharp.Lib;
using Microsoft.FSharp.Core;
using Console = Colorful.Console;
using C = System.Drawing.Color;

namespace CsharpApp
{
    class Program
    {
        private static void WriteTitle(string title)
        {
            Console.WriteLine($"\n\n##### {title} #####\n", C.Lime);
        }
        static void Main(string[] args)
        {
            WriteTitle("Class");
            var obj = new Product(1,"phone");
            Console.WriteLine(obj);
            Console.WriteLine(obj.Id);
            Console.WriteLine(obj.IsExpensive);
            Console.WriteLine(obj.Price);
            Console.WriteLine(obj.CanBeSoldTo("fr"));

            WriteTitle("Record");
            var record = new Fsharp.Lib.Person("John", "Doe", new DateTime(1980, 01, 01));
            var record2 = new Fsharp.Lib.Person("John", "Doe", new DateTime(1980, 01, 01));
            Console.WriteLine(record);
            Console.WriteLine(record.Equals(record2));
            Console.WriteLine(record.LastName);
            record.LastName = "Buzz";
            Console.WriteLine(record.LastName);
            Console.WriteLine(record.Equals(record2));
            Console.WriteLine(record.FullName);
            Console.WriteLine(record.IsBirthday());

            WriteTitle("Enum & union type");
            Console.WriteLine(Color.Red);
            Console.WriteLine(ColorEnum.Blue);
            Console.WriteLine(PaymentMethod.NewCreditCard(CardType.Visa, CardNumber.NewCardNumber("09876543234")));

            WriteTitle("pattern matching");
            Console.WriteLine(PureFsharp.IntPatternMatching(3));
            Console.WriteLine(PureFsharp.TypeTesting(record));

            WriteTitle("Option Type");
            var option = PureFsharp.GetOptionType;
            var vlue = option.Value;
            Console.WriteLine(option);
            Console.WriteLine(vlue);
            var opt = FSharpOption<string>.Some("toto");
            Console.WriteLine(PureFsharp.UseOptionType(opt));

            WriteTitle("Extension method");
            var even = 1.IsEven2();
            Console.WriteLine(even);

            WriteTitle("Method using Active patterns");
            Console.WriteLine(PureFsharp.EvenOrOdd(3));

            WriteTitle("Tuples");
            var (a, b, c) = PureFsharp.UseTuples(3, 5, 10);
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);


            Console.ReadLine();
        }
    }
}
