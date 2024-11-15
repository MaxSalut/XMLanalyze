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
                var document = XDocument.Load(inputStream);

                if (document.Root == null)
                    return false;

                _people.AddRange(document.Descendants("Person").Select(personNode =>
                {
                    var person = new Person
                    {
                        FullName = personNode.Attribute("FullName")?.Value ?? "",
                        Room = personNode.Element("Room")?.Value ?? "",
                        CheckInDate = DateOnly.TryParse(personNode.Element("Dates")?.Element("CheckInDate")?.Value, out var checkIn) ? checkIn : (DateOnly?)null,
                        CheckOutDate = DateOnly.TryParse(personNode.Element("Dates")?.Element("CheckOutDate")?.Value, out var checkOut) ? checkOut : (DateOnly?)null,
                        Attributes = personNode.Attributes().ToDictionary(a => a.Name.ToString(), a => a.Value)
                    };

                    var faculty = personNode.Element("Faculty")?.Value;
                    var course = personNode.Element("Course")?.Value;

                    if (!string.IsNullOrEmpty(faculty))
                        person.Attributes["Faculty"] = faculty;

                    if (!string.IsNullOrEmpty(course))
                        person.Attributes["Course"] = course;

                    return person;
                }));

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading XML: {ex.Message}");
                return false;
            }
        }



        public IList<Person> Find(Filters filters)
        {
            return _people.FindAll(person => filters.ValidatePerson(person));
        }
    }
}
