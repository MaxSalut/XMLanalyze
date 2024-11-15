// LabWork2/Views/FindResultPage.xaml.cs

using Microsoft.Maui.Controls;
using System.Collections.Generic;
using XMLanalyze.XML_Manager;

namespace XMLanalyze.Views
{
    public partial class FindResultPage : ContentPage
    {
        public FindResultPage(IEnumerable<Person> people)
        {
            InitializeComponent();

            foreach (var person in people)
            {
                Console.WriteLine($"Displaying Person: {person.FullName}, Faculty: {person.Faculty}, Course: {person.Course}, Room: {person.Room}");
            }

            ResultsCollectionView.ItemsSource = people;
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Повернення на попередню сторінку
        }
    }
}
