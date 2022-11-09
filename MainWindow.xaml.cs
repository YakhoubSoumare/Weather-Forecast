using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<WeatherCast> weatherList = new List<WeatherCast>();
        WeatherCast weatherCast;
        string filNamn;

        public MainWindow()
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Now;
            filNamn = "WeatherCast.json";
        }

        private async void add_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = (DateTime)datePicker.SelectedDate;
            Int32.TryParse(temperatureTextBox.Text, out int temperatur);
            string summary = summaryCB.Text;
            weatherList.Add(new WeatherCast
            {
                _Date = date,
                _Temperature = temperatur,
                _Summary = summary
            }) ;
            viewListBox.Items.Clear();
            //Skapa en filström
            using FileStream createStream = File.Create(filNamn);
            // Serialisera datan asynkront
            await JsonSerializer.SerializeAsync(createStream, weatherList);
            // Skriv datan asynkront
            await createStream.DisposeAsync();
            viewListBox.Items.Add(weatherList.Last()._Date.ToString("yyyy-MM-dd")+" "+ weatherList.Last()._Temperature+" "+ weatherList.Last()._Summary);
            MessageBox.Show("Added!");
            AttributeClear();
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            viewListBox.Items.Clear();
            UpdateView();
        }

        private void UpdateView()
        {
            AttributeClear();
            if(filNamn != String.Empty)
            {
                viewListBox.Items.Add(File.ReadAllText(filNamn));
            }
        }

        private void AttributeClear()
        {
            datePicker.SelectedDate = DateTime.Now;
            temperatureTextBox.Text = String.Empty;
            summaryCB.Text = String.Empty;
        }
    }
}
