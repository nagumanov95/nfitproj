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
    /// Логика взаимодействия для pacinfa.xaml
    /// </summary>
    public partial class pacinfa : Window
    {
        Entities entities = new Entities();
        public pacinfa()
        {
            InitializeComponent();
            foreach (var pacienty in entities.pacienty)
                lb1.Items.Add(pacienty);
            datepic.Language = XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag);
        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var selected_pacienty = lb1.SelectedItem as pacienty;
                if (selected_pacienty != null)
                {
                    tb1.Text = selected_pacienty.fio;
                    tb2.Text = selected_pacienty.pol;
                    tb3.Text = selected_pacienty.nomer_tel;
                    datepic.SelectedDate = selected_pacienty.birthday;


                }

                else

                {
                    // Сброс значений полей, если выбранный объект null
                    tb1.Text = "";
                    tb2.Text = "";
                    tb3.Text = "";
                    datepic.SelectedDate = null;
                }
            }
            catch (Exception ex)
            {
              
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var pacienty = lb1.SelectedItem as pacienty;
                if (tb1.Text == "" || tb2.Text == "" || tb3.Text == "" || datepic.SelectedDate == null)
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (pacienty == null)
                    {
                        pacienty = new pacienty();
                        entities.pacienty.Add(pacienty);
                        lb1.Items.Add(pacienty);
                    }
                    pacienty.fio = tb1.Text;
                    pacienty.pol = tb2.Text;
                    pacienty.birthday = datepic.SelectedDate.Value;

                    // Добавляем проверку длины строки tb4 перед сохранением
                    if (tb3.Text.Length <= 11)
                    {
                        pacienty.nomer_tel = tb3.Text;
                        entities.SaveChanges();
                        lb1.Items.Refresh();
                        MessageBox.Show("Запись успешно сохранена");
                    }
                    else
                    {
                        MessageBox.Show("Длина номера телефона не должна превышать 11 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        if (!entities.pacienty.Local.Contains(pacienty))
                        {
                            entities.pacienty.Remove(pacienty);
                            lb1.Items.Remove(pacienty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Введите корректное значение: " + ex.Message);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var delete_pacienty = lb1.SelectedItem as pacienty;
            try
            {
                var exist_ = (from structure in entities.osmotry where structure.Id_pacienta == delete_pacienty.Id_pacienta select structure).First();
                MessageBox.Show("Запись удалить нельзя!\nСуществуют осмотры у этого пациента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {

                var rezult = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezult == MessageBoxResult.No)
                    return;

                if (delete_pacienty != null)
                {
                    entities.pacienty.Remove(delete_pacienty);

                    entities.SaveChanges();
                    tb1.Clear();
                    tb2.Clear();
                    tb3.Clear();
                    datepic.SelectedDate = null;



                    lb1.Items.Remove(delete_pacienty);
                    lb1.Items.Refresh();



                    MessageBox.Show("Удаление прошло успешно");

                }
                else
                {
                    MessageBox.Show("Нет удаляемых объектов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void _new_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text = "";
            tb2.Text = "";
            tb3.Text = "";
            datepic.SelectedDate = null;
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
