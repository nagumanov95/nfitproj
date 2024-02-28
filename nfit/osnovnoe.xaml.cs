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
using System.Windows.Shapes;
using nfit;
using System.Windows.Markup;

namespace nfit
{
    /// <summary>
    /// Логика взаимодействия для osnovnoe.xaml
    /// </summary>
    public partial class osnovnoe : Window
    {
        Entities entities = new Entities();
        public osnovnoe()
        {
            InitializeComponent();
            foreach (var osmotry in entities.osmotry)
                lb1.Items.Add(osmotry);
            foreach (var pacienty in entities.pacienty)
                combo1.Items.Add(pacienty);
            foreach (var vrachi in entities.vrachi)
                combo2.Items.Add(vrachi);
            datepick.Language = XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag);

            //List<OsmotryDisplayItem> displayItems = new List<OsmotryDisplayItem>();
            //foreach (var osmotry in entities.osmotry)
            //{
            //    displayItems.Add(new OsmotryDisplayItem(osmotry.pacienty, osmotry.data_osmotra));
            //}

            //lb1.ItemsSource = displayItems;
        }

    


        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selected_osmotr = lb1.SelectedItem as osmotry;
                if (selected_osmotr != null)
                {
                    tb1.Text = selected_osmotr.rezult_osmotra;
                    tb2.Text = selected_osmotr.rec_vracha;
                    tb3.Text = selected_osmotr.diagnoz;
                    datepick.SelectedDate = selected_osmotr.data_osmotra;

                    combo1.SelectedItem = entities.pacienty.FirstOrDefault(p => p.Id_pacienta == selected_osmotr.Id_pacienta);
                    combo2.SelectedItem = entities.vrachi.FirstOrDefault(v => v.Id_vracha == selected_osmotr.Id_vracha);
                }
                else

                {
                    // Сброс значений полей, если выбранный объект null
                    tb1.Text = "";
                    tb2.Text = "";
                    tb3.Text = "";
                    datepick.SelectedDate = null;
                    combo1.SelectedIndex = -1;
                    combo2.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, например, логирование и вывод сообщения пользователю
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
            int rowCount = lb1.Items.Count; // Получаем количество строк в ListBox
            kolvo.Text = "Количество строк - " + rowCount.ToString(); // Выводим количество строк в TextBox
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var osmotr = lb1.SelectedItem as osmotry;
                if (tb1.Text == "" || tb2.Text == "" || tb3.Text == "" || combo1.SelectedIndex == -1 || combo2.SelectedIndex == -1 || datepick.SelectedDate == null)
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (osmotr == null)
                    {
                        osmotr = new osmotry();
                        entities.osmotry.Add(osmotr);
                        lb1.Items.Add(osmotr);
                    }
                    osmotr.rezult_osmotra = tb1.Text;
                    osmotr.rec_vracha = tb2.Text;
                    osmotr.diagnoz = tb3.Text;
                    osmotr.data_osmotra = datepick.SelectedDate.Value;
                    osmotr.Id_pacienta = (combo1.SelectedItem as pacienty).Id_pacienta;
                    osmotr.Id_vracha = (combo2.SelectedItem as vrachi).Id_vracha;

                    entities.SaveChanges();
                    lb1.Items.Refresh();
                    MessageBox.Show("Запись успешно сохранена");

                }

            }
            catch
            {
                MessageBox.Show("Введите корректное значение!");
            }
            int rowCount = lb1.Items.Count; // Получаем количество строк в ListBox
            kolvo.Text = rowCount.ToString(); // Выводим количество строк в TextBox
        
    }

        private void _new_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text = "";
            tb2.Text = "";
            tb3.Text = "";
            combo1.SelectedIndex = -1;
            combo2.SelectedIndex = -1;
            datepick.SelectedDate = null;
            lb1.SelectedIndex = -1;
            tb1.Focus();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var delete_osmotry = lb1.SelectedItem as osmotry;
            try
            {
                var exist_ = (from structure in entities.predpisanya where structure.id_osmotra == delete_osmotry.Id_osmotra select structure).First();
                MessageBox.Show("Запись удалить нельзя!\nСуществуют предписания у этого осмотра!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {

                
                var rezult = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezult == MessageBoxResult.No)
                    return;

                if (delete_osmotry != null)
                {
                    entities.osmotry.Remove(delete_osmotry);

                    entities.SaveChanges();
                    tb1.Clear();
                    tb2.Clear();
                    tb3.Clear();
                    combo1.SelectedIndex = -1;
                    combo2.SelectedIndex = -1;
                    datepick.SelectedDate = null;

                    lb1.Items.Remove(delete_osmotry);
                    lb1.Items.Refresh();



                    MessageBox.Show("Удаление прошло успешно");

                }
                else
                {
                    MessageBox.Show("Нет удаляемых объектов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void vyhod_Click(object sender, RoutedEventArgs e)
        {
            var vrachmenu = new vrachmenu();
            this.Close();
            Hide();
            vrachmenu.ShowDialog();
        }
    }
}
