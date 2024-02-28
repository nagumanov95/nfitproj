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

namespace nfit
{
    /// <summary>
    /// Логика взаимодействия для predpisania.xaml
    /// </summary>
    public partial class predpisania : Window
    {
        Entities entities = new Entities();
        public predpisania()
        {
            InitializeComponent();
            foreach (var predpisanya in entities.predpisanya)
                lb1.Items.Add(predpisanya);
            foreach (var osmotry in entities.osmotry)
                combo1.Items.Add(osmotry);
        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try 
            { 
            var selected_predpisanya = lb1.SelectedItem as predpisanya;
            if (selected_predpisanya != null)
            {
                tb1.Text = selected_predpisanya.naznachenie;
                tb2.Text = selected_predpisanya.dozirovka;
                tb3.Text = selected_predpisanya.srok_priema;

                combo1.SelectedItem = entities.osmotry.FirstOrDefault(p => p.Id_osmotra == selected_predpisanya.id_osmotra);
            }
            else

            {
                // Сброс значений полей, если выбранный объект null
                tb1.Text = "";
                tb2.Text = "";
                tb3.Text = "";
                combo1.SelectedIndex = -1;
            }
        }
            catch (Exception ex)
            {
                // Обработка ошибок, например, логирование и вывод сообщения пользователю
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var predpisanya = lb1.SelectedItem as predpisanya;
                if (tb1.Text == "" || tb2.Text == "" || tb3.Text == "" || combo1.SelectedIndex == -1)
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (predpisanya == null)
                    {
                        predpisanya = new predpisanya();
                        entities.predpisanya.Add(predpisanya);
                        lb1.Items.Add(predpisanya);
                    }
                    predpisanya.naznachenie = tb1.Text;
                    predpisanya.dozirovka = tb2.Text;
                    predpisanya.srok_priema = tb3.Text;
                    predpisanya.id_osmotra = (combo1.SelectedItem as osmotry).Id_osmotra;

                    entities.SaveChanges();
                    lb1.Items.Refresh();
                    MessageBox.Show("Запись успешно сохранена");

                }

            }
            catch
            {
                MessageBox.Show("Введите корректное значение!");
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

            var delete_predpisanya = lb1.SelectedItem as predpisanya;
            var rezult = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление",
                                    MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rezult == MessageBoxResult.No)
                return;

            if (delete_predpisanya != null)
            {
                entities.predpisanya.Remove(delete_predpisanya);

                entities.SaveChanges();
                tb1.Clear();
                tb2.Clear();
                tb3.Clear();
                combo1.SelectedIndex = -1;

                lb1.Items.Remove(delete_predpisanya);
                lb1.Items.Refresh();



                MessageBox.Show("Удаление прошло успешно");

            }
            else
            {
                MessageBox.Show("Нет удаляемых объектов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void _new_Click(object sender, RoutedEventArgs e)
        {

            tb1.Text = "";
            tb2.Text = "";
            tb3.Text = "";
            combo1.SelectedIndex = -1;
            lb1.SelectedIndex = -1;
            tb1.Focus();
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
