using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace XMLanalyze.XML_Manager
{
    public class SaxXmlParser : IXmlParser
    {
        private readonly List<Person> _people;
        private Person _currentPerson;
        private string _currentElement;

        public SaxXmlParser()
        {
            _people = new List<Person>();
        }


        public IList<Person> Find(Filters filters)
        {
            return _people.FindAll(person => filters.ValidatePerson(person));
        }

        private void SetPersonData(string elementName, string value)
        {
            switch (elementName)
            {
                case "Faculty":
                    _currentPerson.Attributes["Faculty"] = value;
                    break;
                case "Course":
                    _currentPerson.Attributes["Course"] = value;
                    break;
                case "Room":
                    _currentPerson.Room = value;
                    break;
                case "CheckInDate":
                    _currentPerson.CheckInDate = DateOnly.TryParse(value, out var checkIn) ? checkIn : (DateOnly?)null;
                    break;
                case "CheckOutDate":
                    _currentPerson.CheckOutDate = DateOnly.TryParse(value, out var checkOut) ? checkOut : (DateOnly?)null;
                    break;
            }
        }

        public bool Load(Stream inputStream, XmlReaderSettings settings)
        {
            _people.Clear();

            try
            {
                using var reader = XmlReader.Create(inputStream, settings);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "Person")
                            {
                                _currentPerson = new Person();
                                if (reader.HasAttributes)
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        _currentPerson.Attributes[reader.Name] = reader.Value;
                                        if (reader.Name == "FullName")
                                        {
                                            _currentPerson.FullName = reader.Value;
                                        }
                                    }
                                    reader.MoveToElement();
                                }
                            }
                            else if (_currentPerson != null)
                            {
                                _currentElement = reader.Name;
                            }
                            break;

                        case XmlNodeType.Text:
                            if (_currentPerson != null && _currentElement != null)
                            {
                                SetPersonData(_currentElement, reader.Value);
                            }
                            break;

                        case XmlNodeType.EndElement:
                            if (reader.Name == "Person" && _currentPerson != null)
                            {
                                _people.Add(_currentPerson);
                                _currentPerson = null;
                            }
                            _currentElement = null;
                            break;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading XML: {ex.Message}");
                return false;
            }
        }

    }
}
