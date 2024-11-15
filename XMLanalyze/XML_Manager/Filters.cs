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

        // Пошук по датам
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }

        public Filters()
        {
            AttributeName = string.Empty;
            AttributeValue = string.Empty;
            NodeName = string.Empty;
            NodeValue = string.Empty;
            CheckInDate = null;
            CheckOutDate = null;
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

            // Перевірка по датам
            bool checkInMatches = !CheckInDate.HasValue || person.CheckInDate == CheckInDate;
            bool checkOutMatches = !CheckOutDate.HasValue || person.CheckOutDate == CheckOutDate;

            // Повертаємо результат
            return attributeMatches && nodeMatches && checkInMatches && checkOutMatches;
        }
    }
}
