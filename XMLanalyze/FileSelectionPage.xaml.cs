
using Microsoft.Maui.Controls;
using System;
using System.IO;

namespace XMLanalyze.Views
{
    public partial class FileSelectionPage : ContentPage
    {
        public string SelectedFilePath { get; private set; }

        public FileSelectionPage()
        {
            InitializeComponent();
        }

        private async void OnAddFileClicked(object sender, EventArgs e)
        {
           
            var result = await FilePicker.PickAsync();

            if (result != null)
            {
                
                if (Path.GetExtension(result.FullPath).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    SelectedFilePath = result.FullPath; 
                    FileStatusLabel.Text = $"Вибраний файл: {Path.GetFileName(SelectedFilePath)}";
                    NextButton.IsEnabled = true; 
                }
                else
                {
                    
                    await DisplayAlert("Помилка", "Будь ласка, оберіть файл у форматі XML.", "OK");
                }
            }
        }


        private async void OnNextClicked(object sender, EventArgs e)
        {
            // Переходимо до MainPage і передаємо шлях до файлу
            await Navigation.PushAsync(new MainPage(SelectedFilePath));
        }
    }
}
