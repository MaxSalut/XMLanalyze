// LabWork2/Views/FindResultPage.xaml.cs

using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using XMLanalyze.XML_Manager;
using XMLanalyze.FindResultPageLogic;

namespace XMLanalyze.Views
{
    public partial class FindResultPage : ContentPage
    {
        private readonly IList<Person> _searchResults;

        public FindResultPage(IList<Person> searchResults)
        {
            InitializeComponent();
            _searchResults = searchResults;
            ResultsCollectionView.ItemsSource = _searchResults;
        }

        private async void OnTransformToHtmlClicked(object sender, EventArgs e)
        {
            await TransformToHtmlLogic.ExecuteAsync(_searchResults);
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Повернення на попередню сторінку
        }
    }
}
