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
using System.IO;

namespace nfit
{
    /// <summary>
    /// Логика взаимодействия для polzovatel.xaml
    /// </summary>
    public partial class polzovatel : Window
    {
        Entities entities = new Entities();
        public polzovatel(int userId)
        {
            InitializeComponent();

            var userOsmotry = entities.osmotry.Where(o => o.pacienty.Id_pacienta == userId).ToList();

            // Перебираем осмотры для текущего пользователя
            foreach (var osmotry in userOsmotry)
            {
                string formattedDate = osmotry.data_osmotra.ToString("dd.MM.yyyy");
                lb1.Items.Add(osmotry); // Добавляем информацию об осмотре в ListBox
            }
        }

       

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var selected_osmotry = lb1.SelectedItem as osmotry;
                if (selected_osmotry != null)
                {
                    tb4.Text = selected_osmotry.rezult_osmotra;
                    tb6.Text = selected_osmotry.rec_vracha;
                    tb7.Text = selected_osmotry.diagnoz;
                    datepic2.SelectedDate = selected_osmotry.data_osmotra;
                    // Получаем информацию о пациенте, связанную с выбранным осмотром
                    var pacient = entities.pacienty.FirstOrDefault(p => p.Id_pacienta == selected_osmotry.Id_pacienta);
                    if (pacient != null)
                    {
                        tb1.Text = pacient.fio;
                        tb2.Text = pacient.nomer_tel;
                        datepic1.SelectedDate = pacient.birthday;
                    }
                    // Получаем информацию о враче, связанную с выбранным осмотром
                    string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
                    var vrach = entities.vrachi.FirstOrDefault(v => v.Id_vracha == selected_osmotry.Id_vracha);
                    if (vrach != null)
                    {
                        tb3.Text = vrach.nomer_tela;
                        tb5.Text = vrach.fio1; // Выводим ФИО врача
                        BitmapImage myBitmapImage = new BitmapImage();
                        myBitmapImage.BeginInit();
                        myBitmapImage.UriSource = new Uri(projectDirectory + "\\Image\\"
                        + vrach.photo);

                        myBitmapImage.DecodePixelWidth = 400;
                        myBitmapImage.EndInit();
                        im.Source = myBitmapImage;
                    }
                    var predp = entities.predpisanya.FirstOrDefault(pr => pr.id_osmotra == selected_osmotry.Id_osmotra);
                    if (predp != null)
                    {
                        tb8.Text = predp.naznachenie;
                        tb9.Text = predp.dozirovka;
                        tb10.Text = predp.srok_priema;
                    }
                    else
                    {
                        tb8.Text = "Информация пуста";
                        tb9.Text = "Информация пуста";
                        tb10.Text = "Информация пуста";
                    }

                    



                }

                else

                {
                    // Сброс значений полей, если выбранный объект null
                    tb1.Text = "";
                    tb2.Text = "";
                    tb3.Text = "";
                    tb4.Text = "";
                    tb5.Text = "";
                    tb6.Text = "";
                    tb7.Text = "";
                    tb8.Text = "";
                    tb9.Text = "";
                    tb10.Text = "";
                    datepic1.SelectedDate = null;
                    datepic2.SelectedDate = null;
                    im.Source = null;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void vyhod_Click(object sender, RoutedEventArgs e)
        {
            var MainWindow = new MainWindow();
            this.Close();
            Hide();
            MainWindow.ShowDialog();
        }
    }
}
