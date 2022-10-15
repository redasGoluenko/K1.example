using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Kontrolinis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookStore bookStore = InOut.ReadBooks("Knyga.txt");
            InOut.PrintBooks(bookStore);
        }
        class Book
        {

            public string Distributor { get; set; }
            public string BookName { get; set; }
            public int Amount { get; set; }
            public decimal Price { get; set; }

            public Book(string distributor, string bookName, int amount, decimal price)
            {
                this.Distributor = distributor;
                this.BookName = bookName;
                this.Amount = amount;   
                this.Price = price;
            }

            public override bool Equals(object obj)
            {
                return obj is Book book &&
                       Distributor == book.Distributor &&
                       BookName == book.BookName &&
                       Amount == book.Amount &&
                       Price == book.Price;
            }

            public override int GetHashCode()
            {
                int hashCode = 2007465292;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Distributor);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BookName);
                hashCode = hashCode * -1521134295 + Amount.GetHashCode();
                hashCode = hashCode * -1521134295 + Price.GetHashCode();
                return hashCode;
            }

            public override string ToString()
            {
                string line;
                line = string.Format("|{0, -10}|{1, -20}|{2, 8}|{3, 8}|", Distributor, BookName, Amount, Price);
                return line;
            }
        }

        class BookStore
        {
          private List<Book> AllBooks = new List<Book>();

            public BookStore()
            {
                AllBooks = new List<Book>();
            }
            public Book GetByIndex(int index)
            {
                return this.AllBooks[index];
            }
            public int GetCount()
            {
                return AllBooks.Count;
            }
            public List<Book> Add(Book book)
            {
                 this.AllBooks.Add(book);
                return AllBooks;
            }
        }

        static class InOut
        {
        public static BookStore ReadBooks(string fileName)
            {
                BookStore bookStore = new BookStore();
                string[] Lines = File.ReadAllLines(fileName, Encoding.UTF8);
                foreach(string line in Lines)
                {
                    string[] Values = line.Split(';');
                    string distributor = Values[0];
                    string bookName = Values[1];
                    int amount = int.Parse(Values[2]);
                    decimal price = decimal.Parse(Values[3]);
                    Book book = new Book(distributor, bookName, amount, price);
                    bookStore.Add(book);
                }
                return bookStore;
            }
     
                public static void PrintBooks(BookStore books)
            {
                Console.WriteLine(new String('-', 51));
                Console.WriteLine("|{0, -10}|{1, -20}|{2, -8}|{3, -8}|", "Leidėjas", "Knygos pavadinimas", "Kiekis", "Kaina");
                Console.WriteLine(new String('-', 51));
                for(int i=0; i<books.GetCount();i++)
                {
                    Book book = books.GetByIndex(i);
                    Console.WriteLine(book.ToString());
                }
                Console.WriteLine(new String('-', 51));
            }


        }



    }
}
