using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_11
{
    public interface IAccountingSystem
    {
        void RegisterBorrow(Book book, Reader reader, DateTime borrowDate);
        void RegisterReturn(Book book, Reader reader, DateTime returnDate);
        List<Tuple<Book, DateTime, DateTime>> GetIssueHistory(Reader reader);
    }
    public interface ICatalog
    {
        List<Book> SearchBooks(string query);
        List<Book> FilterBooks(string author, string genre);
    }
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }

        public Book(string title, string author, string genre, string isbn)
        {
            Title = title;
            Author = author;
            Genre = genre;
            ISBN = isbn;
        }
    }

    public class Reader
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TicketNumber { get; set; }

        public Reader(string firstName, string lastName, int ticketNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            TicketNumber = ticketNumber;
        }
    }
    public class Librarian
    {
        private IAccountingSystem accountingSystem;
        private ICatalog catalog;

        public Librarian(IAccountingSystem accountingSystem, ICatalog catalog)
        {
            this.accountingSystem = accountingSystem;
            this.catalog = catalog;
        }

        public void IssueBook(Book book, Reader reader)
        {
            accountingSystem.RegisterBorrow(book, reader, DateTime.Now);
            Console.WriteLine($"{reader.FirstName} {reader.LastName} взял книгу: {book.Title}");
        }

        public void ReturnBook(Book book, Reader reader)
        {
            accountingSystem.RegisterReturn(book, reader, DateTime.Now);
            Console.WriteLine($"{reader.FirstName} {reader.LastName} вернул книгу: {book.Title}");
        }
    }
    public class Catalog : ICatalog
    {
        private List<Book> books = new List<Book>();

        public void AddBook(Book book) { books.Add(book); }

        public List<Book> SearchBooks(string query)
        {
            return books.Where(b => b.Title.ToLower().Contains(query.ToLower()) ||
                                     b.Author.ToLower().Contains(query.ToLower()) ||
                                     b.Genre.ToLower().Contains(query.ToLower()) ||
                                     b.ISBN.ToLower().Contains(query.ToLower())).ToList();
        }

        public List<Book> FilterBooks(string author, string genre)
        {
            return books.Where(b => (string.IsNullOrEmpty(author) || b.Author.ToLower().Contains(author.ToLower())) &&
                                     (string.IsNullOrEmpty(genre) || b.Genre.ToLower().Contains(genre.ToLower()))).ToList();
        }
    }
    public class AccountingSystem : IAccountingSystem
    {
        private List<Tuple<Book, Reader, DateTime, DateTime?>> transactions = new List<Tuple<Book, Reader, DateTime, DateTime?>>();

        public void RegisterBorrow(Book book, Reader reader, DateTime borrowDate)
        {
            transactions.Add(new Tuple<Book, Reader, DateTime, DateTime?>(book, reader, borrowDate, null));
        }

        public void RegisterReturn(Book book, Reader reader, DateTime returnDate)
        {
            var transaction = transactions.FirstOrDefault(t => t.Item1 == book && t.Item2 == reader && t.Item4 == null);
            if (transaction != null)
            {
                transactions.Remove(transaction);
                transactions.Add(new Tuple<Book, Reader, DateTime, DateTime?>(book, reader, transaction.Item3, returnDate));
            }
        }

        public List<Tuple<Book, DateTime, DateTime>> GetIssueHistory(Reader reader)
        {
            return transactions.Where(t => t.Item2 == reader)
                               .Select(t => new Tuple<Book, DateTime, DateTime>(t.Item1, t.Item3, t.Item4 ?? DateTime.Now))
                               .ToList();
        }
    }

    internal class Task2
    {
    }
}
