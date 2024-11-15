using System;

namespace XMLanalyze.XML_Manager
{
    public class Filters
    {
        // Пошук по атрибутам
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }

        // Пошук по вузлам
        public string NodeName { get; set; }
        public string NodeValue { get; set; }

        // Діапазони дат
        public DateOnly? CheckInStartDate { get; set; }
        public DateOnly? CheckInEndDate { get; set; }
        public DateOnly? CheckOutStartDate { get; set; }
        public DateOnly? CheckOutEndDate { get; set; }

        public Filters()
        {
            AttributeName = string.Empty;
            AttributeValue = string.Empty;
            NodeName = string.Empty;
            NodeValue = string.Empty;
            CheckInStartDate = null;
            CheckInEndDate = null;
            CheckOutStartDate = null;
            CheckOutEndDate = null;
        }

        /// <summary>
        /// Перевіряє, чи відповідає об'єкт Person заданим фільтрам.
        /// </summary>
        /// <param name="person">Об'єкт Person для перевірки.</param>
        /// <returns>True, якщо об'єкт відповідає фільтрам; інакше False.</returns>
        public bool ValidatePerson(Person person)
        {
            // Перевірка по атрибутам
            bool attributeMatches = string.IsNullOrEmpty(AttributeName) ||
                                    (person.Attributes.ContainsKey(AttributeName) &&
                                     person.Attributes[AttributeName].Contains(AttributeValue, StringComparison.OrdinalIgnoreCase));

            // Перевірка по вузлам
            bool nodeMatches = string.IsNullOrEmpty(NodeName) ||
                               (person.GetType().GetProperty(NodeName)?.GetValue(person)?.ToString()
                                   ?.Contains(NodeValue, StringComparison.OrdinalIgnoreCase) == true);

            // Перевірка по діапазонах дат
            bool checkInMatches = (!CheckInStartDate.HasValue || (person.CheckInDate >= CheckInStartDate)) &&
                                  (!CheckInEndDate.HasValue || (person.CheckInDate <= CheckInEndDate));

            bool checkOutMatches = (!CheckOutStartDate.HasValue || (person.CheckOutDate >= CheckOutStartDate)) &&
                                   (!CheckOutEndDate.HasValue || (person.CheckOutDate <= CheckOutEndDate));

            return attributeMatches && nodeMatches && checkInMatches && checkOutMatches;
        }
    }
}
