using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Exception_Handling
{
    class Person
    {
        public static string PersonID { get; set; }
        public static string FName { get; set; }
        public static string LName { get; set; }
        public static string PhoneNumber { get; set; }

        public static List<string> people = new List<string>();

        private static string path = Application.StartupPath + "//People.txt";

        public Person(string id, string first, string last, string phone)
        {
            PersonID = id;
            FName = first;
            LName = last;
            PhoneNumber = phone;
        }


        public static void addPersonToList()
        {
            people.Add(PersonID + " " + FName + " " + LName + " " + PhoneNumber);
        }

        public static void addListToFile()
        {
            try
            {
                //Write to file.
                using (StreamWriter sw = File.AppendText(path))
                {
                    foreach (var item in people)
                    {
                        sw.WriteLine(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Method used to check text file for duplicate ID.
        public static string searchList()
        {
            List<string> lines = new List<string>();

            List<string> idCollection = new List<string>();

            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

                foreach (string s in lines)
                {
                    idCollection.Add(s);
                }
            }
            catch (Exception)
            {
                var myFile = File.Create(path);
                myFile.Close();
            }
            string idCollectionString = String.Join(",", idCollection);
            return idCollectionString;
        }

        public static void writeToXML()
        {
            if (!File.Exists("People.xml"))
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                using (XmlWriter writer = XmlWriter.Create("People.xml", xmlWriterSettings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("People");

                    writer.WriteStartElement("Person");

                    writer.WriteElementString("ID", PersonID);
                    writer.WriteElementString("FirstName", FName);
                    writer.WriteElementString("LastName", LName);
                    writer.WriteElementString("PhoneNumber", PhoneNumber);

                    writer.WriteEndElement();


                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load("People.xml");
                XElement root = xDocument.Element("People");
                IEnumerable<XElement> rows = root.Descendants("Person");
                XElement firstRow = rows.First();

                firstRow.AddBeforeSelf(
               new XElement("Person",
               new XElement("ID", PersonID),
               new XElement("FirstName", FName),
               new XElement("LastName", LName),
               new XElement("PhoneNumber", PhoneNumber)));

                xDocument.Save("People.xml");
            }
        }
    }
}