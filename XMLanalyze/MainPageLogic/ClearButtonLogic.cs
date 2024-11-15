using Microsoft.Maui.Controls;
using System;

namespace XMLanalyze.MainPageLogic
{
    public static class ClearButtonLogic
    {
        public static void Execute(Entry nameEntry, Entry facultyEntry, Entry roomEntry, DatePicker checkInEntry, DatePicker checkOutEntry, Picker coursePicker)
        {
            nameEntry.Text = string.Empty;
            facultyEntry.Text = string.Empty;
            roomEntry.Text = string.Empty;
            checkInEntry.Date = DateTime.Now;
            checkOutEntry.Date = DateTime.Now;
            coursePicker.SelectedIndex = -1;
        }
    }
}
