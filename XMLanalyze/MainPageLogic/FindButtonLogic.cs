using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XMLanalyze.XML_Manager;
using Microsoft.Maui.Controls;
using XMLanalyze.Views;

namespace XMLanalyze.MainPageLogic
{
    public static class FindButtonLogic
    {
        public static async Task ExecuteAsync(IXmlParser parser, Filters filters, INavigation navigation)
        {
            if (parser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", "Парсер не обрано.", "OK");
                return;
            }

            IList<Person> results = parser.Find(filters);

            if (results.Count > 0)
            {
                await navigation.PushAsync(new FindResultPage(results));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Результати пошуку", "Результатів не знайдено.", "OK");
            }
        }
    }
}
