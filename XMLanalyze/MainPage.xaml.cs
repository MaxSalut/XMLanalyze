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

            // Створення фільтрів на основі полів введення і чекбоксів
            var filters = new Filters
            {
                Name = NameCheckBox.IsChecked == true ? NameEntry.Text : "",
                Faculty = FacultyCheckBox.IsChecked == true ? FacultyEntry.Text : "",
                Course = CourseCheckBox.IsChecked == true ? CourseEntry.Text : "",
                Room = RoomCheckBox.IsChecked == true ? RoomEntry.Text : "",
                CheckInDate = CheckInCheckBox.IsChecked == true ? DateOnly.FromDateTime(CheckInEntry.Date) : (DateOnly?)null,
                CheckOutDate = CheckOutCheckBox.IsChecked == true ? DateOnly.FromDateTime(CheckOutEntry.Date) : (DateOnly?)null

            };

            // Виконання пошуку з використанням парсера
            IList<Person> results = _currentParser.Find(filters);

            // Перехід на сторінку з результатами, якщо знайдено хоча б один результат

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
            NameEntry.Text = FacultyEntry.Text = CourseEntry.Text = RoomEntry.Text = string.Empty; CheckInEntry.Date = DateTime.Now; CheckOutEntry.Date = DateTime.Now;
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
