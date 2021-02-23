using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Lab5_SashaSrinivas.Controllers
{
    public class BooksController : Controller
    {
        [HttpGet]
        //this will load when just loading the empty form
        public ActionResult Create()
        {
            var book = new Models.Book();
            return View(book);
        }
        //this will load when a form is submitted via post (form data passed as model)
        [HttpPost]
        public ActionResult Create(Models.Book p)
        {//insert new <person> to people.xml
            string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\books.xml";
            XmlDocument doc = new XmlDocument();
            // create file if it doesn't exist
            if (System.IO.File.Exists(path))
            {
                doc.Load(path); //exists, so load
                XmlNode root = doc.SelectSingleNode("books"); //get root
                XmlElement book = CreateBookElement(doc, p);
                root.AppendChild(book); //append new person to <book>
            }
            else
            {
                //doesn't exist so add stuff
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("books");
                XmlNode book = CreateBookElement(doc, p);
                root.AppendChild(book); //append new person to <people>
                doc.AppendChild(root); //since it's a new file, it's not saved yet
                                       //append <people> the document
            }
            doc.Save(path);
            return View();

        }

        private XmlElement CreateBookElement(XmlDocument doc, Models.Book newBook)
        {
            var lastChild = doc.DocumentElement.LastChild;

            // get max id
            int maxId = Convert.ToInt32(lastChild["id"].InnerText);
            maxId++;
            string format = "0000";
           
            XmlElement book = doc.CreateElement("book");
            XmlNode bookId = doc.CreateElement("id");
            bookId.InnerText = maxId.ToString(format); 
            book.AppendChild(bookId);


            XmlNode title = doc.CreateElement("title");
            title.InnerText = newBook.Title;
            book.AppendChild(title);

            XmlNode author = doc.CreateElement("author");
           /// XmlAttribute salutation = doc.CreateAttribute("title");
            ///salutation.Value = "Lord";
            ////author.Attributes.Append(salutation);

            XmlNode first = doc.CreateElement("firstname");
            first.InnerText = newBook.FirstName;
            author.AppendChild(first);

            // if(!String.IsNullOrEmpty(newBook.MiddleName))
            if (newBook.MiddleName != null && newBook.MiddleName != "")
            {
                XmlNode middle = doc.CreateElement("middlename");
                middle.InnerText = newBook.MiddleName;
                author.AppendChild(middle);
            }

            XmlNode last = doc.CreateElement("lastname");
            last.InnerText = newBook.LastName;

           
            author.AppendChild(last);

            book.AppendChild(author);

            return book;
        }

        // GET: Books
        public ActionResult Index()
        {
            IList<Models.Book> bookList = new List<Models.Book>();
            ///load books.xml
            ///string path = Request.Path + "App_Data/books.xml";
            string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\books.xml"; 
                XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList books = doc.GetElementsByTagName("book");
                foreach (XmlElement p in books)
                {
                    Models.Book book = new Models.Book();
                    book.id = p.GetElementsByTagName("id")[0].InnerText;
                    book.Title = p.GetElementsByTagName("title")[0].InnerText;
                    book.FirstName = p.GetElementsByTagName("firstname")[0].InnerText;
                    if (p.GetElementsByTagName("middlename").Count != 0)
                    {
                        book.MiddleName = p.GetElementsByTagName("middlename")[0].InnerText;
                    }
                   
                   book.LastName = p.GetElementsByTagName("lastname")[0].InnerText;

                    bookList.Add(book);
;                }
            }

            return View(bookList);
        }
    }
}