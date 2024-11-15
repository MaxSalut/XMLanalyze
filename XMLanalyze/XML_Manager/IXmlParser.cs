    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    namespace XMLanalyze.XML_Manager
    {
        /// <summary>
        /// Інтерфейс для парсерів XML, який задає методи для завантаження та пошуку даних
        /// </summary>
        public interface IXmlParser
        {
            /// <summary>
            /// Завантажує XML-документ з потоку даних
            /// </summary>
            /// <param name="inputStream">Вхідний потік XML</param>
            /// <param name="settings">Налаштування XmlReader</param>
            /// <returns>True, якщо завантаження пройшло успішно, інакше False</returns>
            bool Load(Stream inputStream, XmlReaderSettings settings);

            /// <summary>
            /// Здійснює пошук у завантаженому документі за заданими фільтрами
            /// </summary>
            /// <param name="filters">Фільтри для пошуку</param>
            /// <returns>Список об'єктів, що відповідають критеріям пошуку</returns>
            IList<Person> Find(Filters filters);
        }
    }


