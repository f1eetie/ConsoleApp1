using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic; // For .FileIO.TextFieldParser
//using Microsoft.VisualBasic.FileIO; // For .TextFieldParser
using System.Xml;
using System.Text.RegularExpressions;

// Here is ANOTHER 13 comment added on the GitHub website

namespace ConsoleApp1
{
    class Demo
    {

        public static string StaticString; // static, so cannot be referenced by instance. Must be referenced by class name.

        public static string StaticProperty { get; set; } // Auto-implemented STATIC property. static, so cannot be referenced by instance. Must be referenced by class name.

        public string NonStaticProperty { get; set; } // Auto-implemented NON-STATIC property
        public (string FirstName, string Surname) Name { get; set; } // Tuple property



        public void RunDemo()
        {
            Console.WriteLine("\n\nDemo.RunDemo(): Basics:");
            Console.WriteLine("Demo.RunDemo(): Hello, world!");

            // --- Properties, C#7.0 Null-coalescing operator ??
            Console.WriteLine("\n\nDemo.RunDemo(): Properties:");
            Console.WriteLine($"Demo.RunDemo(): StaticProperty={StaticProperty ?? "[Null]"}"); // Null-coalescing operator ??
            StaticProperty = "StaticProperty value"; // Set STATIC property
            Console.WriteLine($"Demo.RunDemo(): StaticProperty={StaticProperty}"); // Get STATIC property

            StaticString = "StaticString";
            Console.WriteLine($"Demo.RunDemo(): StaticString={StaticString}");
            StaticString = "StaticString2";
            Console.WriteLine($"Demo.RunDemo(): StaticString={StaticString}");


            // --- Nulls and strings
            Console.WriteLine("\n\nDemo.RunDemo(): Nulls and strings:");
            string s;
            s = null; // This is needed for following line to compile
            Console.WriteLine($"s={s ?? "[Null]"}");


            // --- C#7.0 "out" variables, C#7.0 local function
            Console.WriteLine("\n\nDemo.RunDemo(): Out variables:");
            void doubleTriple(int x, out int _dbl, out int _trpl) // Local function. "out" variables.
            {
                _dbl = 2 * x;
                _trpl = 3 * x;
            }
            doubleTriple(5, out int dbl, out int trpl); // "out" variables; declare here, but scope extends out, so can use them below...
            Console.WriteLine($"dbl={dbl}, trpl={trpl}");


            // --- Lambda expressions, C#6 string interpolation
            Console.WriteLine("\n\nDemo.RunDemo(): Basic lambda expressions:");
            Func<int, int> times2 = (int x) => (x * 2); // Lambda expression
            int a, aTimes2;
            a = 2;
            aTimes2 = times2(a);
            Console.WriteLine($"a={a}, aTimes2={aTimes2}"); // String interpolation


            // --- C#7.0 Tuples, C#7.0 tuple "discards"
            Console.WriteLine("\n\nDemo.RunDemo(): Tuples:");
            (int squared, int cubed) = squareAndCube(2); // Call function returning a tuple
            Console.WriteLine($"squared={squared}, cubed={cubed}");
            (int aa, int bb) = (2, 3); // You can use tuples for multi-assignments
            Console.WriteLine($"Before tuple swap : aa={aa}, bb={bb}");
            (aa, bb) = (bb, aa); // Swap aa , bb
            Console.WriteLine($"After  tuple swap : aa={aa}, bb={bb}");
            (int twoSquared, _) = squareAndCube(2); // "Discard" character _ means discard that component of the tuple
            (int threeSquared, _) = squareAndCube(3);
            Console.WriteLine($"twoSquared={twoSquared}, threeSquared={threeSquared}");


            // --- C#7.0 Digit separator
            Console.WriteLine("\n\nDemo.RunDemo(): Digit separator:");
            int bigInt = 10_000_000; // Digit separator _
            Console.WriteLine($"bigInt={bigInt}");


            // --- C#7.0 Local functions
            Console.WriteLine("\n\nDemo.RunDemo(): Local functions:");
            int localFunction(int y) // Local (nested) function
            {
                return y * 4;
            }
            Console.WriteLine($"Calling localFunction(4) = {localFunction(4)}"); // Calling local function (using string interpolation)


            // --- CSV files
            Console.WriteLine("\n\nDemo.RunDemo(): CSV (Reading):");
            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(@"c:\work\DB_Homebase_DebitNote.csv"))
            {
                Console.WriteLine($"\n\nReading from a CSV file:\n");
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");
                int line = 0;
                while (!parser.EndOfData)
                {
                    line++;
                    //Processing row
                    string[] fields = parser.ReadFields();
                    if (fields.Count() < 2) continue;
                    string field1 = fields[0];
                    string field2 = fields[1];
                    Console.WriteLine($"Line {line} : Field1={field1}, Field2={field2}");
                    foreach (string field in fields)
                    {
                        Console.WriteLine($"Line {line} : Read field from CSV file: {field}");
                    }
                }
            }

            Console.WriteLine("\n\nDemo.RunDemo(): CSV (Writing):");
            void writeCsv()
            {
                string csvEscape(string value)
                {
                    var mustQuote = value.Any(x => x == ',' || x == '\"' || x == '\r' || x == '\n');
                    if (!mustQuote) return value;
                    return $"\"{value.Replace("\"", "\"\"")}\""; // i.e.:  "   followed by value.Replace(" with "") followed by   "
                }
                // Set up array of lines, each being an array of strings:
                string[] csvFields1 = { "1Hello", "1Hello, World!", "1Hello\r\n\"friendly\" World!", "1\"Goodbye, Blue Sky\"\r\n\"Goodbye!\"" };
                string[] csvFields2 = { "2Hello", "2Hello, World!", "2Hello\r\n\"friendly\" World!", "2\"Goodbye, Blue Sky\"\r\n\"Goodbye!\"" };
                string[] csvFields3 = { "3Hello", "3Hello, World!", "3Hello\r\n\"friendly\" World!", "3\"Goodbye, Blue Sky\"\r\n\"Goodbye!\"" };
                string[][] csvFields = { csvFields1, csvFields2, csvFields3 };
                // Iterate over lines:
                using (var sw = new System.IO.StreamWriter(@"c:\work\VS2017_ExampleCsvWriter.csv"))
                {
                    foreach (string[] line in csvFields)
                    {
                        string csvLine = "";
                        foreach (string field in line)
                        {
                            string escapedField = csvEscape(field);
                            csvLine += escapedField + ',';
                        }
                        Console.WriteLine($"{csvLine}");
                        sw.WriteLine($"{csvLine}");
                        sw.Flush();
                    }
                } // using
            }
            writeCsv();


            // --- Very basic LINQ, Method syntax, followed by Query syntax
            Console.WriteLine("\n\nDemo.RunDemo(): LINQ (Method syntax):");
            int[] ints = { 0, 2, 3, 5, 6, 8, 10, 11, 13, 14, 16, 17, 19, 20, 21, 23, 24, 26, 27, 28, 29, 30, 31, 99, 100, 101, 102 };
            IEnumerable<int> intQueryMethodSyntax = ints.Where(ii => ii % 2 == 0)
                                                        .OrderByDescending(ii => ii); // Lambda expressions
            foreach (int ii in intQueryMethodSyntax) Console.WriteLine($"ii={ii}");

            Console.WriteLine("\n\nDemo.RunDemo(): LINQ (Query syntax):");
            IEnumerable<int> intQueryQuerySyntax = from ii in ints
                                                   where ii % 2 == 0
                                                   orderby ii descending
                                                   select ii;
            foreach (int ii in intQueryQuerySyntax) Console.WriteLine($"ii={ii}");


            Console.WriteLine("\n\nDemo.RunDemo(): Various LINQ expressions:");

            Console.WriteLine("\n\nDemo.RunDemo(): LINQ First 5 from the list:");
            IEnumerable<int> queryFirst5 = ints.Where((ii, index) => index < 5); // This overload of .Where() provides the 0-based "index"
            foreach (int ii in queryFirst5) Console.WriteLine($"ii={ii}");

            Console.WriteLine("\n\nDemo.RunDemo(): LINQ Projection, with lambda expression:");
            IEnumerable<int> sillyList = ints.Select((ii, index) => (ii * index)); // .Select() also has an overload providing "index"
            foreach (int ii in sillyList) Console.WriteLine($"{ii}");

            // LINQ Grouping
            Console.WriteLine("\n\nDemo.RunDemo(): LINQ Grouping by tens, Query Syntax:");
            var byTens = from ii in ints
                         group ii by (ii / 10) into jj // jj is IGroupable
                         select new { tensGroup = jj.Key, members = jj };
            foreach (var kk in byTens) // kk is anonymous type { int tensGroup , IGroupable members }
                foreach (var member in kk.members)
                    Console.WriteLine($"tensGroup={kk.tensGroup}, member={member}");


            Console.WriteLine("\n\nDemo.RunDemo(): LINQ Grouping in Method Syntax:");
            IEnumerable<IGrouping<int, int>> groups = ints.GroupBy(ii => ii / 10);
            foreach (var group in groups)
            {
                Console.WriteLine($"Group: {group.Key}");
                foreach (var ii in group)
                    Console.WriteLine($"Key={group.Key}, ii={ii}");
            }


            // --- Anonymous types
            Console.WriteLine("\n\nDemo.RunDemo(): Anonymous Types:");
            var xx = new { FirstPart = 1, SecondPart = 2 };
            Console.WriteLine($"xx = {xx}");
            Console.WriteLine($"xx.FirstPart={xx.FirstPart} , xx.SecondPart={xx.SecondPart}");
            Console.WriteLine($"Type is: {xx.GetType()}"); // Type is: <>f__AnonymousType0`2[System.Int32,System.Int32]


            // --- XML and LINQ
            Console.WriteLine("\n\nDemo.RunDemo(): XML and LINQ:");
            string xml = "<root xmlns=\"mynamespace\">" + // If XML has a namespace, need to use XmlNamespaceManager, below
                         "<nums>" +
                         "<num>1</num><num>2</num><num>3</num><num>4</num><num>5</num><num>6</num><num>7</num>" +
                         "</nums>" +
                         "</root>";
            XmlDocument xmlData = new XmlDocument(); // XML DOM, not true LINQ to XML
            xmlData.LoadXml(xml);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlData.NameTable);
            nsmgr.AddNamespace("mn", "mynamespace");

            Console.WriteLine($"xmlData.InnerXml() is:\n{xmlData.InnerXml}");
            Console.WriteLine("xmlData formatted:");
            xmlData.Save(Console.Out); // "Saves" document to console, pretty-printed
            Console.WriteLine("\nDisplaying all <num> node values:");
            XmlNodeList xmlNums = xmlData.SelectNodes("//mn:num", nsmgr); // num is in namespace "mynamespace", aliased as "mn" above
            foreach (XmlNode xmlNum in xmlNums) Console.WriteLine($"XML element: {xmlNum.InnerText}");
            Console.WriteLine("Querying XML using LINQ:");
            var xmlQuery = from XmlNode xn in xmlNums
                           where (int.Parse(xn.InnerText) % 3 == 0)
                           select xn;
            foreach (XmlNode xmlNum in xmlQuery) Console.WriteLine($"XML element: {xmlNum.InnerText}");

            Console.WriteLine("\nDemo.RunDemo(): LINQ to XML:");
            string xml2 = "<root>" +
                          "<nums>" +
                          "<num>1</num><num>2</num><num>3</num><num>4</num><num>5</num><num>6</num><num>7</num>" +
                          "</nums>" +
                          "</root>";
            //XElement.Load(@"c:\work\test.xml"); // XElement can also load from a file
            XElement root = XElement.Parse(xml2);
            IEnumerable<XElement> xmlQ = from XElement el in root.Elements("nums") // NB: We don't specify the root node "root"
                                                                 .Elements("num")
                                         where (int)el > 3 // Notice casting XElement to int
                                         select el;
            foreach (XElement el in xmlQ)
            {
                Console.WriteLine($"Got element with value: {el}");
            }
            string nullElement = (string)root.Element("NonExistentElement");
            Console.WriteLine($"Value of <NonExistentElement> is {nullElement ?? "[NULL]"}"); // ?? : null-coalescing operator


            // --- yield return

            Console.WriteLine("\n\nDemo.RunDemo(): IEnumerable function using yield return:");
            IEnumerable<int> MyRange(int lo, int hi)
            {
                for (int i = lo; i <= hi; i++) yield return i;
            }
            foreach (int i in MyRange(1, 12)) Console.WriteLine($"MyRange() returned: {i}");
            IEnumerable<int> oddsDoubledQ = MyRange(20, 30).Where(i => (i % 2 == 1))
                                                           .Select(i => i * 2)
                                                           .Reverse()
                                                           .OrderBy(i => i);
            foreach (int i in oddsDoubledQ) Console.WriteLine($"oddsDoubledQ returned: {i}");


            // --- Extension methods

            Console.WriteLine("\n\nDemo.RunDemo(): Extension methods:");
            // --- Extension methods on class
            string s1 = "Hello, World.";
            Console.WriteLine($"s1 = {s1}");
            Console.WriteLine($"s1.addQuotes() = {s1.addQuotes()}");

            // --- Extension methods on struct
            Point p = new Point(3, 4);
            Console.WriteLine($"struct Point p = {p}");                     // N.B.: .ToString() called implicitly
            Console.WriteLine($"struct Point p.reverse() = {p.reverse()}"); // N.B.: .ToString() called implicitly

            // --- Passing lambda expressions to functions
            int MyTransform(int operand, Func<int, int> transformer)
            {
                return transformer(operand);
            }
            Console.WriteLine($"MyTransform(5, x=>x*x) = {MyTransform(5, x => x * x)}");
            Console.WriteLine($"MyTransform(5, x=>2*x) = {MyTransform(5, x => 2 * x)}");


            // --- params keyword
            Console.WriteLine("\n\nDemo.RunDemo(): params keyword:");
            void UseParamsInt(params int[] list)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    Console.Write(list[i] + " ");
                }
                Console.WriteLine();
            }

            void UseParamsObj(params object[] list)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    Console.Write(list[i] + " ");
                }
                Console.WriteLine();
            }

            UseParamsInt(1, 2, 3, 4);
            UseParamsObj(1, 'a', "test");
            UseParamsObj();

            // An array argument can be passed, as long as the array
            // type matches the parameter type of the method being called
            int[] myIntArray = { 5, 6, 7, 8, 9 };
            UseParamsInt(myIntArray);

            object[] myObjArray = { 2, 'b', "test", "again" };
            UseParamsObj(myObjArray);

            // The following call does not cause an error, but the entire 
            // integer array becomes the first element of the params array.
            UseParamsObj(myIntArray);

            Console.WriteLine("\n\nDemo.RunDemo(): params keyword with generic extension method List.AddMulti():");
            List<int> listInt = new List<int> { 1, 2, 3, 4, 5, 6 }; // Notice syntax for Collection Initialiser
            listInt.AddMulti(7, 8, 9); // params keyword in this method's argument list allows variable number of args
            listInt.AddMulti<int>(10, 11, 12); // Explicit type specification - unnecessary here
            foreach (int i in listInt) Console.WriteLine($"listInt : {i}");


            // --- Boxing / unboxing
            Console.WriteLine("\n\nDemo.RunDemo(): Boxing / unboxing :");
            int t = 123;
            object o = (object)t;  // Explicit Boxing. Object o is stored on the heap, and is a REFERENCE VARIABLE.
            o = t;                 // Implicit Boxing. Same effect.

            // The object o can then be unboxed and assigned to integer variable i:
            t = -1;
            o = 123;
            t = (int)o;  // Unboxing
            Console.WriteLine($"After boxing and unboxing, value-type variable int t is: {t}");


            // --- Interfaces
            Console.WriteLine("\n\nDemo.RunDemo(): Interfaces:");
            Bunny babbit = new Bunny("Babbit");
            babbit.Feed();
            babbit.Stroke();

            // --- Regex matching, and in LINQ
            Console.WriteLine("\n\nDemo.RunDemo(): Regex matching:");
            string matchCandidate = "Martin Poyser";
            string matchPattern = "^Mar.*oys.*r$";
            Boolean matchResult = Regex.IsMatch(matchCandidate, matchPattern, RegexOptions.IgnoreCase);
            Console.WriteLine($"Match result for pattern {matchPattern} on {matchCandidate} is: {matchResult}");
            // In LINQ:
            string[] names = new string[] { "Martin", "Helen", "Charlotte", "Louise", "Hannah", "Debbie", "Sue", "April",
                                            "Laura", "Sarah", "Claire", "Diane"};
            IEnumerable<String> qNames = names.Where(name => Regex.IsMatch(name, "^H.*", RegexOptions.IgnoreCase));
            foreach (string name in qNames) Console.WriteLine($"Starting with H : {name}");
            foreach (string name in names.Where(name => Regex.IsMatch(name, ".*t.*", RegexOptions.IgnoreCase))) Console.WriteLine($"Contains t : {name}");


            // --- Functional programming : Closure
            Console.WriteLine("\n\nDemo.RunDemo(): Functional programming : Closure:");
            new FunctionalProgramming().Closure();


            Console.ReadLine();
        }

        // --- // Expression-bodied syntax. C#7.0 Tuples.
        private static (int squared, int cubed) squareAndCube(int x) => (x * x, x * x * x); // Expression-bodied syntax (uses =>). Returns a tuple.

    };

    public static class StringExtensionMethods // N.B.: public static class for extension methods
    {
        public static string addQuotes(this String s) // N.B. public static method for extension method
                                                      // N.B. "this" indicates extension method to class (System.)String
        {
            return "\"" + s + "\"";
        }
    }

    public static class PointExtensionMethods // N.B.: public static class for extension methods
    {
        public static Point reverse(this Point p)
        {
            return new Point(p.y, p.x);
        }
    }

    public static class ListExtensions
    {
        // GENERIC (type) extension method:
        public static void AddMulti<T>(this List<T> list, params T[] items) // Note params keyword allows variable number of args.
        {
            foreach (var listItem in items)
            {
                list.Add(listItem);
            }
        }
    }

    public struct Point
    {
        public int x { get; set; }
        public int y { get; set; }
        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public override string ToString() // N.B. "override" necessary because there's already a ValueType.ToString() method
        {
            return $"Point({x},{y})";
        }
    }

    interface IMammal
    {
        string Name { get; }
    }

    interface IStrokable : IMammal
    {
        void Stroke();
    }

    interface IFeedable : IMammal
    {
        void Feed();
    }

    class Bunny : IStrokable, IFeedable
    {
        public string Name { get; } // IMammal
        public Bunny(string name) => Name = name;

        public void Stroke() => Console.WriteLine($"{Name} is being stroked!");
        public void Feed() => Console.WriteLine($"{Name} is being fed!");
    }

    class FunctionalProgramming
    {
        public void Closure()
        {
            var funcs = new List<Func<int>>();
            for (var i = 0; i < 10; i++)
            {
                var j = i; // Copy i
                funcs.Add(() => j);
                // Value of j is captured here, and persists as function's return value even after j goes out of scope,
                // when execution leaves this for() block
            }

            foreach (var func in funcs) Console.WriteLine($"FunctionalProgramming.Closure() : func()={func()}"); // writes 0 to 9


            Func<int> NotClosureFunc;
            NotClosureFunc = () => 4; // NOT a closure, because not capturing and closing over any variable
            Console.WriteLine($"FunctionalProgramming.Closure() : NotClosureFunc()={NotClosureFunc()}");


            Func<int> ClosureFunc=null; // Need the =null else line that calls ClosureFunc() fails compile with CS0165 "Use of uninitialised local variable"
            for (int k = 3; k <= 3; k++) // Set up a "dummy block" to form the scope for k, m
            {
                int m = k; // Copy value of k to m, else ClosureFunc() ends up returning FINAL value of k=4! Would STILL be a closure, though.
                ClosureFunc = () => m; // Value of m is captured in a closure ClosureFunc, and will persist after this for() block
                Console.WriteLine($"FunctionalProgramming.Closure() : ClosureFunc()={ClosureFunc()}");
            }
            Console.WriteLine($"FunctionalProgramming.Closure() : ClosureFunc()={ClosureFunc()}");

        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            Demo demo = new Demo();
            Console.WriteLine("Main(): Main Program Starting:");
            Console.WriteLine("\n\nMain(): Non-static Properties:");
            demo.NonStaticProperty = "Non-static property value";
            Console.WriteLine($"Main(): NonStaticProperty={demo.NonStaticProperty}"); // Get NON-STATIC property

            Console.WriteLine("\n\nMain(): Static property:");
            //demo.StaticProperty = "Setting demo.StaticProperty"; // Cannot be done. Won't compile
            //demo.StaticString = "Setting demo.StaticString";     // Cannot be done. Won't compile
            Demo.StaticProperty = "Setting demo.StaticProperty"; // Can be done if you quality using TYPE (CLASS) name (Demo) rather than INSTANCE name (demo)
            Console.WriteLine($"Main(): Demo.StaticProperty={Demo.StaticProperty}");
            Console.WriteLine("\n\nMain(): Static variable:");
            Demo.StaticString = "Setting demo.StaticString";     // Can be done if you quality using TYPE (CLASS) name (Demo) rather than INSTANCE name (demo). BUT DON'T!
            Console.WriteLine($"Main(): Demo.StaticString={Demo.StaticString}");


            Console.WriteLine("\n\nMain(): Tuple property:");
            demo.Name = ("Martin", "Poyser");
            Console.WriteLine($"Main(): Name= FirstName:{demo.Name.FirstName}, Surname:{demo.Name.Surname}");
            Console.WriteLine($"Main(): Name={demo.Name}");

            Console.WriteLine("\n\nMain(): Run instance of demo: demo.RunDemo()");
            demo.RunDemo();
        }
    }
}

