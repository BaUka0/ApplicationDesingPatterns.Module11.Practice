using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Practice_111
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public List<Author> Authors { get; set; } = new List<Author>();
        public int PublicationYear { get; set; }
        public bool AvailabilityStatus { get; set; } = true;
        public void ChangeAvailabilityStatus()
        {
            AvailabilityStatus = !AvailabilityStatus;
        }
        public Book(int id, string title, string isbn, int publicationYear)
        {
            Id = id;
            Title = title;
            ISBN = isbn;
            PublicationYear = publicationYear;
        }
        public override string ToString()
        {
            return $"ID: {Id}, Title: {Title}, ISBN: {ISBN}, Year: {PublicationYear}, Available: {AvailabilityStatus}";
        }
    }
    public class Author
    {
        public string Name { get; set; }
    }

    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public abstract void BorrowBook(Library library, int bookId);
    }
    public class Reader : User
    {
        public override void BorrowBook(Library library, int bookId)
        {
            Book book = library.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null || !book.AvailabilityStatus)
            {
                Console.WriteLine("Книга недоступна.");
            }
            else
            {
                book.ChangeAvailabilityStatus();
                Console.WriteLine($"{Name} взял книгу: {book.Title}");
            }
        }
    }
    public class Librarian : User
    {
        public override void BorrowBook(Library library, int bookId) { }

        public void AddBook(Library library, Book book)
        {
            library.Books.Add(book);
            Console.WriteLine($"Книга '{book.Title}' добавлена.");
        }
        public void RemoveBook(Library library, int bookId)
        {
            Book bookToRemove = library.Books.FirstOrDefault(b => b.Id == bookId);
            if (bookToRemove == null)
            {
                Console.WriteLine("Книга не найдена.");
                return;
            }
            if (library.Loans.Any(l => l.BookId == bookId))
            {
                Console.WriteLine("Книга сейчас выдана. Нельзя удалить.");
                return;
            }
            library.Books.Remove(bookToRemove);
            Console.WriteLine($"Книга '{bookToRemove.Title}' удалена.");
        }
    }
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int ReaderId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public Loan(int bookId, int readerId)
        {
            BookId = bookId;
            ReaderId = readerId;
            LoanDate = DateTime.Now;
        }
    }
    public class Library
    {
        public List<Book> Books { get; set; } = new List<Book>();
        public List<User> Users { get; set; } = new List<User>();
        public List<Loan> Loans { get; set; } = new List<Loan>();

        public void IssueBook(int bookId, User user)
        {
            if (!(user is Reader))
            {
                Console.WriteLine("Только читатель может взять книгу.");
                return;
            }
            (user as Reader).BorrowBook(this, bookId);
        }


    }
    internal class Task1
    {
        static void Main(string[] args)
        {
            var library = new Library();
            var librarian = new Librarian { Id = 1, Name = "Библиотекарь", Email = "librarian@example.com" };
            var reader1 = new Reader { Id = 2, Name = "Читатель1", Email = "reader1@example.com" };
            library.Users.Add(librarian);
            library.Users.Add(reader1);
            librarian.AddBook(library, new Book(1, "Война и мир", "978-5-17-095742-5", 1869));
            librarian.AddBook(library, new Book(2, "Евгений Онегин", "978-5-17-110023-0", 1833));

            library.IssueBook(1, reader1);
            Console.WriteLine("Список книг:");
            foreach (var book in library.Books)
            {
                Console.WriteLine(book);
            }

            librarian.RemoveBook(library, 2);
            Console.WriteLine("Список книг после удаления:");
            foreach (var book in library.Books)
            {
                Console.WriteLine(book);
            }
        }
    }
}
