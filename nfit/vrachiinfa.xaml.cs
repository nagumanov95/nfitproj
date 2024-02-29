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
using Microsoft.Win32;
using System.Threading;

namespace nfit
{
    /// <summary>
    /// Логика взаимодействия для vrachiinfa.xaml
    /// </summary>
    public partial class vrachiinfa : Window
    {
        Entities entities = new Entities();
        public vrachiinfa()
        {
            InitializeComponent();
            foreach (var vrachi in entities.vrachi)
                lb1.Items.Add(vrachi);
        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                var selected_vrachi = lb1.SelectedItem as vrachi;
                if (selected_vrachi != null)
                {
                    tb1.Text = selected_vrachi.fio1;
                    tb2.Text = selected_vrachi.staj.ToString();
                    tb3.Text = selected_vrachi.specializaciya;
                    tb4.Text = selected_vrachi.nomer_tela;


                }

                string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
                if (selected_vrachi != null)
                {

                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.UriSource = new Uri(projectDirectory + "\\Image\\"
                    + selected_vrachi.photo);

                    myBitmapImage.DecodePixelWidth = 400;
                    myBitmapImage.EndInit();
                    im.Source = myBitmapImage;

                }

                else

                {
                    // Сброс значений полей, если выбранный объект null
                    tb1.Text = "";
                    tb2.Text = "";
                    tb3.Text = "";
                    tb4.Text = "";
                    im.Source = null;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, например, логирование и вывод сообщения пользователю
                //tb1.Text = "";
                //tb2.Text = "";
                //tb3.Text = "";
                //tb4.Text = "";
                im.Source = null;
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var vrachi = lb1.SelectedItem as vrachi;
                if (tb1.Text == "" || tb2.Text == "" || tb3.Text == "" || tb4.Text == "" || im.Source == null)
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (vrachi == null)
                    {
                        vrachi = new vrachi();
                        entities.vrachi.Add(vrachi);
                        lb1.Items.Add(vrachi);
                    }
                    vrachi.fio1 = tb1.Text;
                    vrachi.staj = int.Parse(tb2.Text);
                    vrachi.specializaciya = tb3.Text;

                    string fullFileName = im.Source.ToString();
                    fullFileName = fullFileName.Replace(@"file:///", "");
                    FileInfo fileInfo = new FileInfo(fullFileName);
                    vrachi.photo = fileInfo.Name;

                    // Добавляем проверку длины строки tb4 перед сохранением
                    if (tb4.Text.Length <= 11)
                    {
                        vrachi.nomer_tela = tb4.Text;
                        entities.SaveChanges();
                        lb1.Items.Refresh();
                        MessageBox.Show("Запись успешно сохранена");
                    }
                    else
                    {
                        MessageBox.Show("Длина номера телефона не должна превышать 11 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        if (!entities.vrachi.Local.Contains(vrachi))
                        {
                            entities.vrachi.Remove(vrachi);
                            lb1.Items.Remove(vrachi);
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
            var delete_vrachi = lb1.SelectedItem as vrachi;
            try
            {
                var exist_ = (from structure in entities.osmotry where structure.Id_vracha == delete_vrachi.Id_vracha select structure).First();
                MessageBox.Show("Запись удалить нельзя!\nСуществуют осмотры у этого врача!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                
                var rezult = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezult == MessageBoxResult.No)
                    return;

                if (delete_vrachi != null)
                {
                    entities.vrachi.Remove(delete_vrachi);

                    entities.SaveChanges();
                    tb1.Clear();
                    tb2.Clear();
                    tb3.Clear();
                    tb4.Clear();
                    im.Source = null;


                    lb1.Items.Remove(delete_vrachi);
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
            tb4.Text = "";
            im.Source = null;
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

        private void ph_Click(object sender, RoutedEventArgs e)
        {
            //string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            //string destinationPath = "";

            //OpenFileDialog dlg = new OpenFileDialog();
            //dlg.InitialDirectory = IOPath.Combine(projectDirectory, "image");

            //if (dlg.ShowDialog() == true && !string.IsNullOrWhiteSpace(dlg.FileName))
            //{
            //    using (var sourceStream = File.Open(dlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        destinationPath = IOPath.Combine(dlg.InitialDirectory, dlg.SafeFileName);

            //        // Закрываем файл после чтения и перед копированием
            //        sourceStream.Close();
            //    }

            //    // После закрытия файла можно безопасно скопировать его
            //    File.Copy(dlg.FileName, destinationPath, true);

            //    lb1.Items.Refresh();



            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = projectDirectory + "\\image\\";

            if (dlg.ShowDialog() == true && !string.IsNullOrWhiteSpace(dlg.FileName))
            {
                try
                {
                    im.Source = new BitmapImage(new Uri(dlg.FileName));
                    File.Copy(dlg.FileName, dlg.InitialDirectory + dlg.SafeFileName);
                    lb1.Items.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Введите корректное значение: " + ex.Message);
                }
            }
        }
    }
}
