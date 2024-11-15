// LabWork2/XML_Manager/DomXmlParser.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace XMLanalyze.XML_Manager
{
    public class DomXmlParser : IXmlParser
    {
        private readonly List<Person> _people;

        public DomXmlParser()
        {
            _people = new List<Person>();
        }

        public bool Load(Stream inputStream, XmlReaderSettings settings)
        {
            _people.Clear();

            var document = new XmlDocument();
            try
            {
                using var reader = XmlReader.Create(inputStream, settings);
                document.Load(reader);

                if (document.DocumentElement == null)
                    return false;

                foreach (XmlNode personNode in document.DocumentElement.SelectNodes("Person"))
                {
                    var person = new Person
                    {
                        Name = new Person.FullName
                        {
                            FirstName = personNode.SelectSingleNode("Name/FirstName")?.InnerText ?? "",
                            LastName = personNode.SelectSingleNode("Name/LastName")?.InnerText ?? ""
                        },
                        Faculty = personNode.SelectSingleNode("Faculty")?.InnerText ?? "",
                        Course = personNode.SelectSingleNode("Course")?.InnerText ?? "",
                        Room = personNode.SelectSingleNode("Room")?.InnerText ?? ""
                    };

                    // Парсинг дат
                    DateOnly.TryParse(personNode.SelectSingleNode("CheckInDate")?.InnerText, out DateOnly checkInDate);
                    DateOnly.TryParse(personNode.SelectSingleNode("CheckOutDate")?.InnerText, out DateOnly checkOutDate);
                    person.CheckInDate = checkInDate != default ? checkInDate : (DateOnly?)null;
                    person.CheckOutDate = checkOutDate != default ? checkOutDate : (DateOnly?)null;

                    _people.Add(person);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IList<Person> Find(Filters filters)
        {
            return _people.FindAll(person => filters.ValidatePerson(person));
        }
    }
}
