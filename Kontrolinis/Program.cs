using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;

namespace Kontrolinis
{
    internal class Program
    {
        static void Main(string[] args)
        {         
            BookStore bookStore = InOut.ReadBooks("Knyga.txt");
            List<Book> sales = InOut.ReadSales("Parduota.txt");
            InOut.PrintBooks(bookStore, "Pradinė knygyno lentelė:");
            Console.WriteLine("");
            InOut.PrintSales(sales, "Pradinė knygų pardavimo lentelė:");
            bookStore.AddSalePrice(sales);          
            InOut.PrintSales(sales, "Papildoma knygų pardavimų lentelė:");
            InOut.PrintBooks(bookStore, "Papildyta knygyno lentelė:");
            Console.WriteLine("Turi dar surinkti: {0}", bookStore.Sum());
        }
        class Book
        {
            public string Distributor { get; set; }
            public string BookName { get; set; }
            public int Amount { get; set; }
            public decimal Price { get; set; }
            public string Sale { get; set; }      

            public Book(string distributor, string bookName, int amount, decimal price)
            {
                this.Distributor = distributor;
                this.BookName = bookName;
                this.Amount = amount;
                this.Price = price;
            }
            public Book(string sale)
            {
                this.Sale = sale;
                this.Price = 0;
                this.Amount = 1;
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

            public Book GetBook(int index)
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
           public void AddSalePrice(List<Book> books)
            {     
                for (int i = 0 ; i < books.Count ; i++ )
                {
                    int index = 0;
                    for (int j = 0; j < GetCount(); j++)
                    {
                        if (books[i].Sale == GetBook(j).BookName && GetBook(j).Amount > 0)
                        {
                            if (index != 0  && GetBook(j).Price > GetBook(index).Price)
                            {
                                index = j;
                            }
                            else if(index == 0)
                            {                        
                                index = j;
                            }
                        }
                    }
                    books[i].Price = GetBook(index).Price;
                    AllBooks[index].Amount--;
                }
            }
           public decimal Sum()
            {
                decimal sum = 0;
                for(int i = 0; i<GetCount();i++)
                {
                    sum += GetBook(i).Price * GetBook(i).Amount;
                }
                return sum;
            }
        }
        static class InOut
        {
            public static BookStore ReadBooks(string fileName)
            {
                BookStore bookStore = new BookStore();
                string[] Lines = File.ReadAllLines(fileName, Encoding.UTF8);
                foreach (string line in Lines)
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
            public static void PrintBooks(BookStore books, string header)
            {
                Console.WriteLine(header);        
                Console.WriteLine(new String('-', 51));
                Console.WriteLine("|{0, -10}|{1, -20}|{2, -8}|{3, -8}|", "Leidėjas", "Knygos pavadinimas", "Kiekis", "Kaina");
                Console.WriteLine(new String('-', 51));
                for (int i = 0; i < books.GetCount(); i++)
                {
                    Book book = books.GetBook(i);
                    Console.WriteLine(book.ToString());
                }
                Console.WriteLine(new String('-', 51));
            }
            public static List<Book> ReadSales(string fileName)
            {
                string[] Lines = File.ReadAllLines(fileName, Encoding.UTF8);
                List<Book> Books = new List<Book>();
                foreach (string line in Lines)
                {
                    string[] Values = line.Split(';');
                    string sale = Values[0];       
                    Book book = new Book(sale);
                    Books.Add(book);
                }
                return Books;
            }
            public static void PrintSales(List<Book> books, string header)
            {
                Console.WriteLine(header);
                Console.WriteLine(new String('-', 35));
                Console.WriteLine("|{0, -15}|{1, -8}|{2, -8}|", "Pavadinimas", "Kiekis", "Kaina");
                Console.WriteLine(new String('-', 35));
               foreach(Book book in books)
                {                  
                    Console.WriteLine("|{0, -15}|{1, 8}|{2, 8}|", book.Sale, book.Amount, book.Price);
                }
                Console.WriteLine(new String('-', 35));
                Console.WriteLine();
            }
        }
    }
}


