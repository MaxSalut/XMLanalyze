// LabWork2/XML_Manager/Parser.cs

using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace XMLanalyze.XML_Manager
{
    /// <summary>
    /// Абстрактний базовий клас для парсерів XML
    /// </summary>
    public abstract class Parser : IXmlParser
    {
        protected List<Person> People;

        protected Parser()
        {
            People = new List<Person>();
        }

        /// <summary>
        /// Виконує пошук у списку `People` за вказаними фільтрами
        /// </summary>
        /// <param name="filters">Фільтри для пошуку</param>
        /// <returns>Список об'єктів `Person`, що відповідають критеріям</returns>
        public IList<Person> Find(Filters filters)
        {
            return People.Where(filters.ValidatePerson).ToList();
        }

        // Абстрактний метод завантаження, що реалізується в дочірніх класах
        public abstract bool Load(Stream inputStream, XmlReaderSettings settings);
    }
}
