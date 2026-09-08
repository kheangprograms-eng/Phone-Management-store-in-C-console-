using System;
using System.Collections.Generic;
using System.Linq;

namespace assingmenttest90
{
    // Model Class
    public class Phone
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }  
        public int Stock { get; set; }
        public int SoldCount { get; set; }

        public override string ToString() =>
            $"{Id}\t{Brand,-10} {Model,-15} ${Price,-10} Stock: {Stock}";
    }

    class Program
    {
        static List<Phone> inventory = new List<Phone>();
        static List<Phone> cart = new List<Phone>();
        static decimal totalRevenue = 0;

        // ---------- UI HELPERS ----------

        // Centers a single line of text
        static void CenterText(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            int width = Console.WindowWidth;
            int padding = Math.Max(0, (width - text.Length) / 2);
            Console.ForegroundColor = color;
            Console.WriteLine(new string(' ', padding) + text);
            Console.ResetColor();
        }

        static void CenterPrompt(string text, ConsoleColor color = ConsoleColor.Yellow)
        {
            int width = Console.WindowWidth;
            int padding = Math.Max(0, (width - text.Length) / 2);
            Console.ForegroundColor = color;
            Console.Write(new string(' ', padding) + text);
            Console.ResetColor();
        }

        // Draws a centered box with a title and a list of content lines
        static void DrawBox(string title, List<string> lines, ConsoleColor borderColor = ConsoleColor.Cyan, ConsoleColor textColor = ConsoleColor.White)
        {
            int contentWidth = Math.Max(title.Length, lines.Count == 0 ? 0 : lines.Max(l => l.Length)) + 4;
            int consoleWidth = Console.WindowWidth;
            int leftPad = Math.Max(0, (consoleWidth - (contentWidth + 2)) / 2);
            string padStr = new string(' ', leftPad);

            Console.ForegroundColor = borderColor;
            Console.WriteLine(padStr + "╔" + new string('═', contentWidth) + "╗");

            // Title centered inside box
            int titlePad = Math.Max(0, (contentWidth - title.Length) / 2);
            Console.WriteLine(padStr + "║" + new string(' ', titlePad) + title + new string(' ', contentWidth - titlePad - title.Length) + "║");

            Console.WriteLine(padStr + "╠" + new string('═', contentWidth) + "╣");

            Console.ForegroundColor = textColor;
            foreach (var line in lines)
            {
                Console.ForegroundColor = borderColor;
                Console.Write(padStr + "║ ");
                Console.ForegroundColor = textColor;
                Console.Write(line.PadRight(contentWidth - 2));
                Console.ForegroundColor = borderColor;
                Console.WriteLine(" ║");
            }

            Console.ForegroundColor = borderColor;
            Console.WriteLine(padStr + "╚" + new string('═', contentWidth) + "╝");
            Console.ResetColor();
        }

        // ---------- MAIN ----------
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            SeedData();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine();
                DrawBox("MNS PHONE STORE MANAGEMENT SYSTEM", new List<string>
                {
                    "[1] Customer Menu",
                    "[2] Admin Menu",
                    "[3] Sales Report",
                    "[4] Exit"
                }, ConsoleColor.Cyan, ConsoleColor.Green);

                Console.WriteLine();
                CenterPrompt("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CustomerMenu();
                        break;
                    case "2":
                        AdminMenu();
                        break;
                    case "3":
                        SalesReport();
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        CenterText("Invalid choice!", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void SeedData()
        {
            inventory.Add(new Phone { Id = 1, Brand = "Apple", Model = "iPhone 15", Price = 999, Stock = 10 });
            inventory.Add(new Phone { Id = 2, Brand = "Samsung", Model = "S24 Ultra", Price = 1199, Stock = 5 });
            inventory.Add(new Phone { Id = 3, Brand = "Google", Model = "Pixel 8", Price = 699, Stock = 8 });
        }

        // ---------- CUSTOMER PROCESSES ----------
        static void CustomerMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine();
                DrawBox("CUSTOMER MENU", new List<string>
                {
                    "[1] View All Phones",
                    "[2] Search Phone",
                    "[3] Add to Cart",
                    "[4] Checkout",
                    "[5] Back to Main Menu"
                }, ConsoleColor.Cyan, ConsoleColor.Green);

                Console.WriteLine();
                CenterPrompt("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewPhones();
                        break;
                    case "2":
                        SearchPhone();
                        break;
                    case "3":
                        AddToCart();
                        break;
                    case "4":
                        Checkout();
                        break;
                    case "5":
                        back = true;
                        break;
                }
            }
        }

        // phone viewing function
        static void ViewPhones()
        {
            Console.Clear();
            var lines = new List<string> { "ID   Brand      Model            Price      Stock" };
            lines.AddRange(inventory.Select(p => p.ToString().Replace("\t", "  ")));
            DrawBox("ALL PHONES", lines, ConsoleColor.Cyan, ConsoleColor.White);
            Console.WriteLine();
            CenterText("Press any key to continue...", ConsoleColor.DarkGray);
            Console.ReadKey();
        }

        static void SearchPhone()
        {
            Console.Clear();
            CenterPrompt("Enter Brand or Model to search: ");
            string query = Console.ReadLine().ToLower();
            var results = inventory.Where(p => p.Brand.ToLower().Contains(query) || p.Model.ToLower().Contains(query)).ToList();

            Console.WriteLine();
            if (results.Count == 0)
            {
                CenterText("No products found.", ConsoleColor.Red);
            }
            else
            {
                var lines = results.Select(p => p.ToString().Replace("\t", "  ")).ToList();
                DrawBox($"{results.Count} PRODUCT(S) FOUND", lines, ConsoleColor.Cyan, ConsoleColor.Green);
            }
            Console.WriteLine();
            CenterText("Press any key to continue...", ConsoleColor.DarkGray);
            Console.ReadKey();
        }

        static void AddToCart()
        {
            ViewPhones();
            Console.WriteLine();
            CenterPrompt("Enter ID to add to cart: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var phone = inventory.FirstOrDefault(p => p.Id == id);
                if (phone != null && phone.Stock > 0)
                {
                    cart.Add(phone);
                    Console.WriteLine();
                    CenterText("Added to cart!", ConsoleColor.Green);
                }
                else
                {
                    Console.WriteLine();
                    CenterText("Out of stock or invalid ID.", ConsoleColor.Red);
                }
            }
            Console.ReadKey();
        }

        static void Checkout()
        {
            Console.Clear();
            if (cart.Count == 0)
            {
                CenterText("Cart is empty!", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            decimal total = cart.Sum(p => p.Price);
            var lines = cart.Select(item => $"{item.Brand} {item.Model} - ${item.Price}").ToList();
            lines.Add("");
            lines.Add($"Total Paid: ${total}");

            foreach (var item in cart)
            {
                item.Stock--;
                item.SoldCount++;
                totalRevenue += item.Price;
            }

            DrawBox("RECEIPT", lines, ConsoleColor.Yellow, ConsoleColor.White);
            cart.Clear();

            Console.WriteLine();
            CenterText("Thank you for your purchase!", ConsoleColor.Green);
            Console.ReadKey();
        }

        // ---------- ADMIN PROCESSES ----------
        static void AdminMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine();
                DrawBox("ADMIN MENU", new List<string>
                {
                    "[1] Add Phone",
                    "[2] Update Phone",
                    "[3] Delete Phone",
                    "[4] View All",
                    "[5] Back"
                }, ConsoleColor.Magenta, ConsoleColor.White);

                Console.WriteLine();
                CenterPrompt("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CenterPrompt("Brand: "); string b = Console.ReadLine();
                        CenterPrompt("Model: "); string m = Console.ReadLine();
                        CenterPrompt("Price: "); decimal p = decimal.Parse(Console.ReadLine());
                        CenterPrompt("Stock: "); int s = int.Parse(Console.ReadLine());
                        inventory.Add(new Phone { Id = inventory.Max(x => x.Id) + 1, Brand = b, Model = m, Price = p, Stock = s });
                        CenterText("Phone added!", ConsoleColor.Green);
                        Console.ReadKey();
                        break;
                    case "2":
                        ViewPhones();
                        CenterPrompt("Enter ID to update stock: ");
                        int upId = int.Parse(Console.ReadLine());
                        var upPhone = inventory.First(x => x.Id == upId);
                        CenterPrompt("New Stock: ");
                        upPhone.Stock = int.Parse(Console.ReadLine());
                        break;
                    case "3":
                        ViewPhones();
                        CenterPrompt("Enter ID to delete: ");
                        int delId = int.Parse(Console.ReadLine());
                        inventory.RemoveAll(x => x.Id == delId);
                        CenterText("Phone deleted!", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                    case "4": ViewPhones(); break;
                    case "5": back = true; break;
                }
            }
        }

        // ---------- SALES REPORT ----------
        static void SalesReport()
        {
            Console.Clear();
            int totalSold = inventory.Sum(p => p.SoldCount);
            var bestSeller = inventory.OrderByDescending(p => p.SoldCount).FirstOrDefault();

            var lines = new List<string>
            {
                $"Total Phones Sold: {totalSold}",
                $"Total Revenue:     ${totalRevenue}",
                bestSeller != null && bestSeller.SoldCount > 0
                    ? $"Best Seller:       {bestSeller.Brand} {bestSeller.Model}"
                    : "Best Seller:       N/A"
            };

            DrawBox("SALES REPORT", lines, ConsoleColor.Yellow, ConsoleColor.White);

            Console.WriteLine();
            CenterText("Press any key to return...", ConsoleColor.DarkGray);
            Console.ReadKey();
        }
    }
}