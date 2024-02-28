using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Markup;

namespace nfit
{
    /// <summary>
    /// Логика взаимодействия для statistika.xaml
    /// </summary>
    public partial class statistika : Window
    {
        Entities entities = new Entities();
        public SeriesCollection SeriesCollection { get; set; }

        public statistika()
        {
            InitializeComponent();
            pick1.Language = XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag);
            pick2.Language = XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag);

            SeriesCollection = new SeriesCollection();
            DataContext = this;
            
    }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime startDate = (DateTime)pick1.SelectedDate;
                DateTime endDate = (DateTime)pick2.SelectedDate;

                var query = from osmotr in entities.osmotry
                            where osmotr.data_osmotra >= startDate && osmotr.data_osmotra <= endDate
                            group osmotr by osmotr.data_osmotra into g
                            orderby g.Count() descending
                            select new { Date = g.Key, Count = g.Count() };

                var top10 = query.Take(10);

                SeriesCollection.Clear();

                if (query.Any())
                {

                    foreach (var result in top10)
                    {
                        SeriesCollection.Add(new PieSeries
                        {
                            Title = $"{result.Date:dd.MM.yyyy}",
                            Values = new ChartValues<int> { result.Count },
                            DataLabels = true,
                            LabelPoint = point => $" - {point.Y} посещение(я)"
                        });
                    }
                }
                else 
                {
                    MessageBox.Show("В выбранные даты не было осмотров.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Заполните начальную и конечную дату полностью!", "Ошибка!",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void vyhod_Click(object sender, RoutedEventArgs e)
        {
            var admin = new admin();
            this.Close();
            Hide();
            admin.ShowDialog();
        }
    }
}
