// LabWork2/XML_Manager/LinqXmlParser.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace XMLanalyze.XML_Manager
{
    public class LinqXmlParser : IXmlParser
    {
        private readonly List<Person> _people;

        public LinqXmlParser()
        {
            _people = new List<Person>();
        }

        public bool Load(Stream inputStream, XmlReaderSettings settings)
        {
            _people.Clear();

            try
            {
                using var reader = XmlReader.Create(inputStream, settings);
                var document = XDocument.Load(reader);

                if (document.Root == null)
                    return false;

                var result = from personNode in document.Descendants("Person")
                             select new Person
                             {
                                 Name = new Person.FullName
                                 {
                                     FirstName = personNode.Element("Name")?.Element("FirstName")?.Value ?? "",
                                     LastName = personNode.Element("Name")?.Element("LastName")?.Value ?? ""
                                 },
                                 Faculty = personNode.Element("Faculty")?.Value ?? "",
                                 Course = personNode.Element("Course")?.Value ?? "",
                                 Room = personNode.Element("Room")?.Value ?? "",
                                 CheckInDate = DateOnly.TryParse(personNode.Element("CheckInDate")?.Value, out DateOnly checkIn) ? checkIn : (DateOnly?)null,
                                 CheckOutDate = DateOnly.TryParse(personNode.Element("CheckOutDate")?.Value, out DateOnly checkOut) ? checkOut : (DateOnly?)null
                             };

                _people.AddRange(result);
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
