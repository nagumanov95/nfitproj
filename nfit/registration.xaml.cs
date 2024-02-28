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
    /// Логика взаимодействия для registration.xaml
    /// </summary>
    public partial class registration : Window
    {
        public registration()
        {
            InitializeComponent();
            // Создаем ImageBrush с изображением, которое вы хотите установить на кнопку
            //ImageBrush imageBrush = new ImageBrush();
            //imageBrush.ImageSource = new BitmapImage(new Uri("Resourses/nazad.png", UriKind.Relative));

            //// Устанавливаем ImageBrush как фон кнопки
            //nazad.Background = imageBrush;
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string m_reg = "Регистрация";
            string m_error = "Ошибка! Пользователь с таким логином уже существует!";
            string login1 = tblogin.Text;
            string password1 = passbox.Password;
            int role = 3;

            if (string.IsNullOrEmpty(tblogin.Text) || string.IsNullOrEmpty(passbox.Password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var entities = new Entities())
            {
                bool userExists = entities.users.Any(u => u.login == login1);

                if (userExists)
                {
                    MessageBox.Show(m_error, m_reg, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var newUser = new users
                    {
                        login = login1,
                        password = password1,
                        role = Convert.ToString(role)
                    };

                    var fio = new fio();
                    if (fio.ShowDialog() == true)
                    {
                        string fio1 = fio.fiotb.Text;

                        if (!string.IsNullOrEmpty(fio1))
                        {
                            newUser.ФИО = fio1;
                            entities.users.Add(newUser);
                            entities.SaveChanges();

                            MessageBox.Show("Регистрация прошла успешно!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                            var MainWindow = new MainWindow();
                            this.Close();
                            Hide();
                            MainWindow.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Для продолжения необходимо ввести ФИО!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            var MainWindow = new MainWindow();
            this.Close();
            Hide();
            MainWindow.ShowDialog();
        }
    }
}

