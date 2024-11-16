using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using XMLanalyze.XML_Manager;
namespace XMLanalyze.FindResultPageLogic
{
    public static class TransformToHtmlLogic
    {
        public static async Task ExecuteAsync(IList<Person> searchResults)
        {
            try
            {
                string tempXmlPath = Path.Combine(Path.GetTempPath(), "searchResults.xml");
                SaveResultsToXml(tempXmlPath, searchResults);

                var xslt = new XslCompiledTransform();

                using (Stream xslStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("XMLanalyze.Resources.template.xsl"))
                {
                    if (xslStream == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Помилка", "Шаблон template.xsl не знайдено як ресурс.", "OK");
                        return;
                    }
                    using (XmlReader reader = XmlReader.Create(xslStream))
                    {
                        xslt.Load(reader);
                    }
                }

                string projectPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string outputHtmlPath = Path.Combine(projectPath, "searchResults.html");

                using (var writer = new StreamWriter(outputHtmlPath))
                {
                    xslt.Transform(tempXmlPath, null, writer);
                }

                await Application.Current.MainPage.DisplayAlert("Успіх", $"HTML файл збережено в папці: {outputHtmlPath}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Не вдалося виконати трансформацію: {ex.Message}", "OK");
            }
        }

        private static void SaveResultsToXml(string filePath, IList<Person> searchResults)
        {
            var document = new XDocument(new XElement("Results"));

            foreach (var person in searchResults)
            {
                var personElement = new XElement("Person",
                    new XAttribute("FullName", person.FullName ?? "N/A"),
                    new XAttribute("Faculty", person.Faculty ?? "N/A"),
                    new XAttribute("Course", person.Course ?? "N/A"),
                    new XElement("Room", person.Room ?? "N/A"),
                    new XElement("Dates",
                        new XElement("CheckInDate", person.CheckInDate?.ToString("dd.MM.yyyy") ?? "N/A"),
                        new XElement("CheckOutDate", person.CheckOutDate?.ToString("dd.MM.yyyy") ?? "N/A")
                    )
                );

                document.Root.Add(personElement);
            }

            document.Save(filePath);
        }
    }
}
