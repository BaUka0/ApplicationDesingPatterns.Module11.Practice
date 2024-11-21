using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_11
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Task 2
            var accountingSystem = new AccountingSystem();
            var catalog = new Catalog();
            var librarian = new Librarian(accountingSystem, catalog);

            catalog.AddBook(new Book("Война и мир", "Л. Толстой", "Роман", "978-5-17-097415-3"));
            catalog.AddBook(new Book("Преступление и наказание", "Ф. Достоевский", "Роман", "978-5-17-097416-0"));

            var reader = new Reader("Иван", "Иванов", 12345);

            librarian.IssueBook(catalog.SearchBooks("Война и мир").First(), reader);
            librarian.ReturnBook(catalog.SearchBooks("Война и мир").First(), reader);

            Console.WriteLine("\nИстория выдачи:");
            foreach (var transaction in accountingSystem.GetIssueHistory(reader))
            {
                Console.WriteLine($"{transaction.Item1.Title} - {transaction.Item2} - {transaction.Item3}");
            }
        }
    }
}
