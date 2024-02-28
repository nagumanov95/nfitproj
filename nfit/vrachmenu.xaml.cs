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
    /// Логика взаимодействия для vrachmenu.xaml
    /// </summary>
    public partial class vrachmenu : Window
    {
        public vrachmenu()
        {
            InitializeComponent();
        }

        private void vyhod_Click(object sender, RoutedEventArgs e)
        {
            var MainWindow = new MainWindow();
            this.Close();
            Hide();
            MainWindow.ShowDialog();
        }

        private void osmotryokn_Click(object sender, RoutedEventArgs e)
        {
            var osnovnoe = new osnovnoe();
            this.Close();
            Hide();
            osnovnoe.ShowDialog();
        }

        private void predpisokn_Click(object sender, RoutedEventArgs e)
        {
            var predpisania = new predpisania();
            this.Close();
            Hide();
            predpisania.ShowDialog();
        }

        private void vrachokn_Click(object sender, RoutedEventArgs e)
        {
            var vrachiinfa = new vrachiinfa();
            this.Close();
            Hide();
            vrachiinfa.ShowDialog();
        }

        private void pacientokn_Click(object sender, RoutedEventArgs e)
        {
            var pacinfa = new pacinfa();
            this.Close();
            Hide();
            pacinfa.ShowDialog();
        }

        private void raspac_Click(object sender, RoutedEventArgs e)
        {
            var raspechpac = new raspechpac();
            this.Close();
            Hide();
            raspechpac.ShowDialog();
        }
    }
}
