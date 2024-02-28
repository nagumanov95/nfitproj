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
using System.Drawing;


namespace nfit
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entities entities = new Entities();
        private bool isEnterKeyPressed = false; // Флаг для проверки нажатия Enter
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (sender, e) =>
            {
                EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyDownEvent,
                    new KeyEventHandler(Window_PreviewKeyDown), true);
            };
            
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !isEnterKeyPressed)
            {
                if (!string.IsNullOrEmpty(tblogin.Text) && !string.IsNullOrEmpty(passbox.Password))
                {
                    isEnterKeyPressed = true;
                    Button_Reg_Click(null, null); // Вызываем метод для входа, если поля логина и пароля заполнены
                }
            }
        }


        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string m_aut = "Аутентификация";
            string m_errorincor = "Ошибка! Проверьте правильность данных!";
            if (!string.IsNullOrEmpty(tblogin.Text) && !string.IsNullOrEmpty(passbox.Password))
            {
                string login = tblogin.Text.Trim();
                string pass = passbox.Password.Trim();
                var user = entities.users.FirstOrDefault(u => u.login == login && u.password == pass);
                if (user != null)
                {
                    if (user.role == "0") // Здесь исправлено условие, чтобы проверить значение поля role
                    {
                        var admin = new admin();
                        this.Close();
                        Hide();
                        admin.ShowDialog();
                        
                    }
                    else if (user.role == "1") // Здесь исправлено условие. Если значение в поле role равно 1, то открывается окно 1
                    {
                        var vrachmenu = new vrachmenu();
                        this.Close();
                        Hide();
                        vrachmenu.ShowDialog();
                        
                    }
                    else if (user.role == "3") // Здесь исправлено условие. Если значение в поле role равно 3, то открывается окно 3
                    {
                        if (user.id_pacienta1 != null)
                        {
                            int userId = (int)user.id_pacienta1; // Получаем id пользователя
                            var polzovatelWindow = new polzovatel(userId); // Передаем userId в конструктор
                            this.Close();
                            Hide();
                            polzovatelWindow.ShowDialog();
                        }
                        else 
                        {
                            MessageBox.Show("Дождитесь пока вашу учетную запись подтвердят. \nДля боллее быстрого подтверждения обратитесь в больницу", "Предупреждение" , MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    //Close();
                }
                else
                {
                    MessageBox.Show(m_errorincor, m_aut, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            isEnterKeyPressed = false;
        }

        private void regi_GotFocus(object sender, RoutedEventArgs e)
        {
            regi.SelectionLength = 0;
            var registration = new registration();
            this.Close();
            Hide();
            registration.ShowDialog();
            
        }
    }
}
