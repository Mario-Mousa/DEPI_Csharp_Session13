using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASSLINQ; // namespace of the provided ListGenerators.cs (Product, Customer, Order, ListGenerators)

namespace Session13
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            do
            {
                Console.WriteLine();
                Console.WriteLine("==== Session 13 - LINQ Assignment 02 ====");
                Console.WriteLine("1 - Element Operators");
                Console.WriteLine("2 - Aggregate Operators");
                Console.WriteLine("3 - Set Operators");
                Console.WriteLine("4 - Partitioning Operators");
                Console.WriteLine("5 - Quantifiers");
                Console.WriteLine("6 - Grouping Operators");
                Console.WriteLine("0 - Exit");
                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                    choice = -1;

                switch (choice)
                {
                    case 1: ElementOperators(); break;
                    case 2: AggregateOperators(); break;
                    case 3: SetOperators(); break;
                    case 4: PartitioningOperators(); break;
                    case 5: Quantifiers(); break;
                    case 6: GroupingOperators(); break;
                    case 0: Console.WriteLine("Goodbye!"); break;
                    default: Console.WriteLine("Invalid choice."); break;
                }

            } while (choice != 0);
        }

        // Reads dictionary_english.txt into an array of strings (one word per line)
        static string[] ReadDictionary()
        {
            return File.ReadAllLines("dictionary_english.txt");
        }

        // ======================= ELEMENT OPERATORS =======================
        static void ElementOperators()
        {
            #region Element - Q1: First product out of stock
            Console.WriteLine("-- Element Q1 --");
            Product firstOutOfStock = ListGenerators.ProductList.First(p => p.UnitsInStock == 0);
            Console.WriteLine(firstOutOfStock);
            #endregion

            #region Element - Q2: First product with price > 1000 (or null)
            Console.WriteLine("-- Element Q2 --");
            Product? expensive = ListGenerators.ProductList.FirstOrDefault(p => p.UnitPrice > 1000);
            Console.WriteLine(expensive == null ? "null" : expensive.ToString());
            #endregion

            #region Element - Q3: Second number greater than 5
            Console.WriteLine("-- Element Q3 --");
            int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            int secondGreaterThan5 = arr.Where(n => n > 5).ElementAt(1);
            Console.WriteLine(secondGreaterThan5); // 8
            #endregion
        }

        // ======================= AGGREGATE OPERATORS =======================
        static void AggregateOperators()
        {
            int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region Aggregate - Q1: Count of odd numbers
            Console.WriteLine("-- Aggregate Q1 --");
            Console.WriteLine(arr.Count(n => n % 2 != 0)); // 5
            #endregion

            #region Aggregate - Q2: Customers and how many orders each has
            Console.WriteLine("-- Aggregate Q2 --");
            var customersOrders = ListGenerators.CustomerList
                .Select(c => new { c.CustomerName, OrdersCount = c.Orders.Length });

            foreach (var item in customersOrders)
                Console.WriteLine($"{item.CustomerName} : {item.OrdersCount} orders");
            #endregion

            #region Aggregate - Q3: Categories and how many products each has
            Console.WriteLine("-- Aggregate Q3 --");
            var categoriesCount = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key, ProductsCount = g.Count() });

            foreach (var item in categoriesCount)
                Console.WriteLine($"{item.Category} : {item.ProductsCount} products");
            #endregion

            #region Aggregate - Q4: Total of the numbers in the array
            Console.WriteLine("-- Aggregate Q4 --");
            Console.WriteLine(arr.Sum()); // 45
            #endregion

            // Read dictionary_english.txt into an array of strings first
            string[] words = ReadDictionary();

            #region Aggregate - Q5: Total number of characters of all words
            Console.WriteLine("-- Aggregate Q5 --");
            Console.WriteLine(words.Sum(w => w.Length));
            #endregion

            #region Aggregate - Q6: Length of the shortest word
            Console.WriteLine("-- Aggregate Q6 --");
            Console.WriteLine(words.Min(w => w.Length));
            #endregion

            #region Aggregate - Q7: Length of the longest word
            Console.WriteLine("-- Aggregate Q7 --");
            Console.WriteLine(words.Max(w => w.Length));
            #endregion

            #region Aggregate - Q8: Average length of the words
            Console.WriteLine("-- Aggregate Q8 --");
            Console.WriteLine(words.Average(w => w.Length));
            #endregion

            #region Aggregate - Q9: Total units in stock for each category
            Console.WriteLine("-- Aggregate Q9 --");
            var unitsPerCategory = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key, TotalUnits = g.Sum(p => p.UnitsInStock) });

            foreach (var item in unitsPerCategory)
                Console.WriteLine($"{item.Category} : {item.TotalUnits}");
            #endregion

            #region Aggregate - Q10: Cheapest price in each category
            Console.WriteLine("-- Aggregate Q10 --");
            var cheapestPerCategory = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key, CheapestPrice = g.Min(p => p.UnitPrice) });

            foreach (var item in cheapestPerCategory)
                Console.WriteLine($"{item.Category} : {item.CheapestPrice}");
            #endregion

            #region Aggregate - Q11: Products with the lowest price in each category (LET)
            Console.WriteLine("-- Aggregate Q11 --");
            var lowestProducts =
                from p in ListGenerators.ProductList
                group p by p.Category into g
                let minPrice = g.Min(p => p.UnitPrice)
                select new { Category = g.Key, Products = g.Where(p => p.UnitPrice == minPrice) };

            foreach (var item in lowestProducts)
            {
                Console.WriteLine(item.Category);
                foreach (Product p in item.Products)
                    Console.WriteLine("   " + p);
            }
            #endregion

            #region Aggregate - Q12: Highest price in each category
            Console.WriteLine("-- Aggregate Q12 --");
            var highestPerCategory = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key, HighestPrice = g.Max(p => p.UnitPrice) });

            foreach (var item in highestPerCategory)
                Console.WriteLine($"{item.Category} : {item.HighestPrice}");
            #endregion

            #region Aggregate - Q13: Products with the highest price in each category
            Console.WriteLine("-- Aggregate Q13 --");
            var highestProducts =
                from p in ListGenerators.ProductList
                group p by p.Category into g
                let maxPrice = g.Max(p => p.UnitPrice)
                select new { Category = g.Key, Products = g.Where(p => p.UnitPrice == maxPrice) };

            foreach (var item in highestProducts)
            {
                Console.WriteLine(item.Category);
                foreach (Product p in item.Products)
                    Console.WriteLine("   " + p);
            }
            #endregion

            #region Aggregate - Q14: Average price of each category's products
            Console.WriteLine("-- Aggregate Q14 --");
            var averagePerCategory = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key, AveragePrice = g.Average(p => p.UnitPrice) });

            foreach (var item in averagePerCategory)
                Console.WriteLine($"{item.Category} : {item.AveragePrice:0.00}");
            #endregion
        }

        // ======================= SET OPERATORS =======================
        static void SetOperators()
        {
            #region Set - Q1: Unique category names
            Console.WriteLine("-- Set Q1 --");
            var categories = ListGenerators.ProductList.Select(p => p.Category).Distinct();
            Console.WriteLine(string.Join(", ", categories));
            #endregion

            var productLetters = ListGenerators.ProductList.Select(p => p.ProductName[0]);
            var customerLetters = ListGenerators.CustomerList.Select(c => c.CustomerName[0]);

            #region Set - Q2: Unique first letters from both product and customer names
            Console.WriteLine("-- Set Q2 --");
            Console.WriteLine(string.Join(", ", productLetters.Union(customerLetters)));
            #endregion

            #region Set - Q3: Common first letters in both
            Console.WriteLine("-- Set Q3 --");
            Console.WriteLine(string.Join(", ", productLetters.Intersect(customerLetters)));
            #endregion

            #region Set - Q4: Product first letters that are not customer first letters
            Console.WriteLine("-- Set Q4 --");
            Console.WriteLine(string.Join(", ", productLetters.Except(customerLetters)));
            #endregion

            #region Set - Q5: Last three characters of every name (with duplicates)
            Console.WriteLine("-- Set Q5 --");
            // Safe helper: names shorter than 3 characters are returned as they are
            Func<string, string> lastThree = name => name.Length <= 3 ? name : name.Substring(name.Length - 3);

            var lastThrees = ListGenerators.CustomerList.Select(c => lastThree(c.CustomerName))
                .Concat(ListGenerators.ProductList.Select(p => lastThree(p.ProductName)));

            Console.WriteLine(string.Join(", ", lastThrees));
            #endregion
        }

        // ======================= PARTITIONING OPERATORS =======================
        static void PartitioningOperators()
        {
            // "Washington" customers are the ones whose Region is "WA"
            var washingtonOrders = ListGenerators.CustomerList
                .Where(c => c.Region == "WA")
                .SelectMany(c => c.Orders);

            #region Partitioning - Q1: First 3 orders from customers in Washington
            Console.WriteLine("-- Partitioning Q1 --");
            foreach (Order o in washingtonOrders.Take(3))
                Console.WriteLine(o);
            #endregion

            #region Partitioning - Q2: All but the first 2 orders from customers in Washington
            Console.WriteLine("-- Partitioning Q2 --");
            foreach (Order o in washingtonOrders.Skip(2))
                Console.WriteLine(o);
            #endregion

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region Partitioning - Q3: Take until a number is less than its position
            Console.WriteLine("-- Partitioning Q3 --");
            Console.WriteLine(string.Join(", ", numbers.TakeWhile((n, index) => n >= index))); // 5, 4
            #endregion

            #region Partitioning - Q4: Start from the first element divisible by 3
            Console.WriteLine("-- Partitioning Q4 --");
            Console.WriteLine(string.Join(", ", numbers.SkipWhile(n => n % 3 != 0))); // 3, 9, 8, 6, 7, 2, 0
            #endregion

            #region Partitioning - Q5: Start from the first element less than its position
            Console.WriteLine("-- Partitioning Q5 --");
            Console.WriteLine(string.Join(", ", numbers.SkipWhile((n, index) => n >= index))); // 1, 3, 9, 8, 6, 7, 2, 0
            #endregion
        }

        // ======================= QUANTIFIERS =======================
        static void Quantifiers()
        {
            #region Quantifiers - Q1: Any word contains "ei"
            Console.WriteLine("-- Quantifiers Q1 --");
            string[] words = ReadDictionary();
            Console.WriteLine(words.Any(w => w.Contains("ei")));
            #endregion

            #region Quantifiers - Q2: Categories with at least one product out of stock
            Console.WriteLine("-- Quantifiers Q2 --");
            var someOutOfStock = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Where(g => g.Any(p => p.UnitsInStock == 0));

            foreach (var group in someOutOfStock)
            {
                Console.WriteLine(group.Key);
                foreach (Product p in group)
                    Console.WriteLine("   " + p);
            }
            #endregion

            #region Quantifiers - Q3: Categories where all products are in stock
            Console.WriteLine("-- Quantifiers Q3 --");
            var allInStock = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Where(g => g.All(p => p.UnitsInStock > 0));

            foreach (var group in allInStock)
            {
                Console.WriteLine(group.Key);
                foreach (Product p in group)
                    Console.WriteLine("   " + p);
            }
            #endregion
        }

        // ======================= GROUPING OPERATORS =======================
        static void GroupingOperators()
        {
            #region Grouping - Q1: Group numbers by remainder when divided by 5
            Console.WriteLine("-- Grouping Q1 --");
            List<int> numbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            var byRemainder = numbers.GroupBy(n => n % 5).OrderBy(g => g.Key);

            foreach (var group in byRemainder)
            {
                Console.WriteLine($"Numbers with a remainder of {group.Key} when divided by 5:");
                foreach (int n in group)
                    Console.WriteLine(n);
            }
            #endregion

            #region Grouping - Q2: Group dictionary words by first letter
            Console.WriteLine("-- Grouping Q2 --");
            string[] words = ReadDictionary();

            var byFirstLetter = words.GroupBy(w => w[0]).OrderBy(g => g.Key);

            foreach (var group in byFirstLetter)
            {
                Console.WriteLine($"Letter '{group.Key}' : {group.Count()} words");

                // The dictionary has ~370,000 words, so only the count is printed.
                // To print every word of the group, uncomment the loop below:
                // foreach (string w in group) Console.WriteLine("   " + w);
            }
            #endregion

            #region Grouping - Q3: Group anagrams using a custom comparer
            Console.WriteLine("-- Grouping Q3 --");
            string[] arr = { "from", "salt", "earn", "last", "near", "form" };

            var anagramGroups = arr.GroupBy(w => w, new AnagramComparer());

            foreach (var group in anagramGroups)
            {
                foreach (string w in group)
                    Console.WriteLine(w);

                Console.WriteLine("....");
            }
            #endregion
        }
    }
}