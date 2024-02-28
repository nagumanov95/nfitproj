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
    /// Логика взаимодействия для admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        Entities entities = new Entities();
        public admin()
        {
            InitializeComponent();
            foreach (var users in entities.users)
                lb1.Items.Add(users);
            foreach (var pacienty in entities.pacienty)
                combo1.Items.Add(pacienty);
        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selected_user = lb1.SelectedItem as users;
                if (selected_user != null)
                {
                    tb1.Text = selected_user.login; // Поправил наименование свойства
                    tb2.Text = selected_user.ФИО; // Поправил наименование свойства
                    tb3.Text = selected_user.role; // Предполагаю, что role есть у объекта users
                    combo1.SelectedItem = entities.pacienty.FirstOrDefault(p => p.Id_pacienta == selected_user.id_pacienta1);
                }
            }
            catch
            {
                tb1.Text = "";
                tb2.Text = "";
                tb3.Text = "";
                combo1.SelectedIndex = -1;
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = lb1.SelectedItem as users;
                if (string.IsNullOrEmpty(tb1.Text) || string.IsNullOrEmpty(tb2.Text) || string.IsNullOrEmpty(tb3.Text))
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    user.login = tb1.Text;
                    user.ФИО = tb2.Text;
                    user.role = tb3.Text;
                    user.id_pacienta1 = (combo1.SelectedItem as pacienty)?.Id_pacienta; // Проверка на null перед присваиванием

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

        private void _new_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text = "";
            tb2.Text = "";
            tb3.Text = "";
            combo1.SelectedIndex = -1;
            lb1.SelectedIndex = -1;
            tb1.Focus();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var delete_user = lb1.SelectedItem as users;
            var rezult = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление",
                                    MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rezult == MessageBoxResult.No)
                return;

            if (delete_user != null)
            {
                entities.users.Remove(delete_user);

                entities.SaveChanges();
                tb1.Clear();
                tb2.Clear();
                tb3.Clear();
                combo1.SelectedIndex = -1;

                lb1.Items.Remove(delete_user);
                lb1.Items.Refresh();



                MessageBox.Show("Удаление прошло успешно");

            }
            else
            {
                MessageBox.Show("Нет удаляемых объектов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void vyhodak_Click(object sender, RoutedEventArgs e)
        {
            var MainWindow = new MainWindow();
            this.Close();
            Hide();
            MainWindow.ShowDialog();
        }

        private void stat_Click(object sender, RoutedEventArgs e)
        {
            var statistika = new statistika();
            this.Close();
            Hide();
            statistika.ShowDialog();
        }
    }
}
