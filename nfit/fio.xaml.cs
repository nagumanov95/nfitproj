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
    /// Логика взаимодействия для fio.xaml
    /// </summary>
    public partial class fio : Window
    {
        Entities entities = new Entities();
        public fio()
        {
            InitializeComponent();
            //Closing += Window_Closing;
        }
        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    e.Cancel = true; // Отменяем закрытие окна
        //                     // Здесь вы можете добавить логику, которая определяет, когда окно должно быть закрыто
        //}
        private void registr_Click(object sender, RoutedEventArgs e)
        {
            string input = fiotb.Text;

            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input.Trim()))
            {
                MessageBox.Show("Заполните поле!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {


                DialogResult = true; // Установка результата окна как успешное (true)
                Close();

            }

        }
        
    }
}
