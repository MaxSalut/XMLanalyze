// FileSelectionPage.xaml.cs
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
            // ������ ������ ��� ������ �����
            var result = await FilePicker.PickAsync(); // �������������� FilePicker ��� ������ �����
            if (result != null)
            {
                SelectedFilePath = result.FullPath; // �������� ���� �� ��������� �����
                FileStatusLabel.Text = $"�������� ����: {Path.GetFileName(SelectedFilePath)}";
                NextButton.IsEnabled = true; // ���������� ������� �� �������� �������
            }
        }

        private async void OnNextClicked(object sender, EventArgs e)
        {
            // ���������� �� MainPage � �������� ���� �� �����
            await Navigation.PushAsync(new MainPage(SelectedFilePath));
        }
    }
}
