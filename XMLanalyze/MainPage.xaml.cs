// LabWork2/MainPage.xaml.cs

using Microsoft.Maui.Controls;
using System;
using XMLanalyze.XML_Manager;
using System.Xml;
using System.Xml.Xsl;
using System.Reflection;


namespace XMLanalyze.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly string _filePath;
        private IXmlParser _currentParser;

        public MainPage(string filePath)
        {
            InitializeComponent();
            _filePath = filePath;
            ParserPicker.SelectedIndexChanged += OnParserPickerChanged;
            LoadCoursePickerData();
        }
        private void LoadCoursePickerData()
        {
            // Очищення попередніх значень
            CoursePicker.Items.Clear();

            try
            {
                var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
                using var stream = File.OpenRead(_filePath);
                using var reader = XmlReader.Create(stream, settings);

                var courses = new HashSet<string>();

                while (reader.Read())
                {
                    // Якщо це вузол Person
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Person")
                    {
                        if (reader.GetAttribute("Course") is string course)
                        {
                            courses.Add(course); // Унікальні значення курсу
                        }
                    }
                }

                foreach (var course in courses)
                {
                    CoursePicker.Items.Add(course);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час завантаження даних: {ex.Message}");
            }
        }
        private void OnParserPickerChanged(object sender, EventArgs e)
        {
            // Ініціалізація парсера на основі вибраного типу
            switch (ParserPicker.SelectedItem.ToString())
            {
                case "DOM":
                    _currentParser = new DomXmlParser();
                    break;
                case "LINQ to XML":
                    _currentParser = new LinqXmlParser();
                    break;
                case "SAX":
                    _currentParser = new SaxXmlParser();
                    break;
            }

            // Завантаження файлу з обраним парсером
            if (_currentParser != null)
            {
                var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
                using var stream = File.OpenRead(_filePath);
                if (_currentParser.Load(stream, settings))
                {
                    Console.WriteLine("Файл завантажено успішно.");
                }
                else
                {
                    Console.WriteLine("Помилка завантаження файлу.");
                }
            }

        }
        private async void OnFindClicked(object sender, EventArgs e)
        {
            if (_currentParser == null)
            {
                await DisplayAlert("Помилка", "Парсер не обрано.", "OK");
                return;
            }

            // Формування фільтрів
            var filters = new Filters();

            // Пошук по атрибутам
            if (NameCheckBox.IsChecked == true)
            {
                filters.AttributeName = "FullName";
                filters.AttributeValue = NameEntry.Text;
            }

            // Пошук по вузлам
            if (FacultyCheckBox.IsChecked == true)
            {
                filters.NodeName = "Faculty";
                filters.NodeValue = FacultyEntry.Text;
            }
            if (CourseCheckBox.IsChecked == true)
            {
                filters.NodeName = "Course";
                filters.NodeValue = CoursePicker.SelectedItem?.ToString();
            }
            if (RoomCheckBox.IsChecked == true)
            {
                filters.NodeName = "Room";
                filters.NodeValue = RoomEntry.Text;
            }

            if (CheckInCheckBox.IsChecked == true)
            {
                filters.NodeName = "CheckInDate";
                filters.NodeValue = CheckInEntry.Date.ToString("dd.MM.yyyy");
            }
            if (CheckOutCheckBox.IsChecked == true)
            {
                filters.NodeName = "CheckOutDate";
                filters.NodeValue = CheckOutEntry.Date.ToString("dd.MM.yyyy");
            }

            // Виконання пошуку
            IList<Person> results = _currentParser.Find(filters);

            if (results.Count > 0)
            {
                await Navigation.PushAsync(new FindResultPage(results));
            }
            else
            {
                await DisplayAlert("Результати пошуку", "Результатів не знайдено.", "OK");
            }
        }


        private void OnClearClicked(object sender, EventArgs e)
        {
            //тут має відбуватися очистка всіх критеріїв пошуку
            NameEntry.Text = FacultyEntry.Text  = RoomEntry.Text = string.Empty; CheckInEntry.Date = DateTime.Now; CheckOutEntry.Date = DateTime.Now; CoursePicker.SelectedIndex = -1;
        }
        private async void OnTransformToHtmlClicked(object sender, EventArgs e)
        {
            try
            {
                // Створення XSL-трансформатора
                var xslt = new XslCompiledTransform();

                // Отримуємо шаблон як вбудований ресурс
                using (Stream xslStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LabWork2.Resources.template.xsl"))
                {
                    if (xslStream == null)
                    {
                        await DisplayAlert("Помилка", "Шаблон template.xsl не знайдено як ресурс.", "OK");
                        return;
                    }
                    using (XmlReader reader = XmlReader.Create(xslStream))
                    {
                        xslt.Load(reader);
                    }
                }

                string projectPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string outputHtmlPath = Path.Combine(projectPath, "output.html");

                // Виконання трансформації
                using (var writer = new StreamWriter(outputHtmlPath))
                {
                    xslt.Transform(_filePath, null, writer);
                }

                await DisplayAlert("Успіх", $"HTML файл збережено в папці: {outputHtmlPath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Не вдалося виконати трансформацію: {ex.Message}", "OK");
            }
        }
    }
}
