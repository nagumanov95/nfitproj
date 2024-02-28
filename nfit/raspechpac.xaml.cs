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
using Microsoft.Office.Interop.Word;

namespace nfit
{
    using Word = Microsoft.Office.Interop.Word;
    /// <summary>
    /// Логика взаимодействия для raspechpac.xaml
    /// </summary>
    public partial class raspechpac : System.Windows.Window
    {
        Entities entities = new Entities();
        public raspechpac()
        {
            InitializeComponent();
            foreach (var pacienty in entities.pacienty)
                lb1.Items.Add(pacienty);

        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var selected_pacienty = lb1.SelectedItem as pacienty;
                if (selected_pacienty != null)
                {
                    tb1.Text = selected_pacienty.fio;
                    tb3.Text = selected_pacienty.nomer_tel;

                    combo1.Items.Clear();

                    int selectedPacientId = selected_pacienty.Id_pacienta;

                    // Фильтрация дат для выбранного пациента
                    var datesForSelectedPacient = entities.osmotry.Where(o => o.Id_pacienta == selectedPacientId).Select(o => o.data_osmotra);

                    foreach (var date in datesForSelectedPacient)
                    {
                        string formattedDate = date.ToString("dd.MM.yyyy");
                        combo1.Items.Add(formattedDate);
                    }
                }

                else

                {
                    // Сброс значений полей, если выбранный объект null
                    tb1.Text = "";
                    tb3.Text = "";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void vyhod_Click(object sender, RoutedEventArgs e)
        {
            var vrachmenu = new vrachmenu();
            this.Close();
            Hide();
            vrachmenu.ShowDialog();
        }

        private void pechat_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Получаем выбранного пациента
                var selectedPacient = lb1.SelectedItem as pacienty;

                if (selectedPacient != null || combo1.SelectedItem != null)
                {
                    if (combo1.SelectedItem != null)
                    {
                        string selectedDate = combo1.SelectedItem.ToString();
                        DateTime selectedDateParsed = DateTime.Parse(selectedDate);


                        // Создаем новый экземпляр приложения Word
                        Word.Application wordApp = new Word.Application();
                        wordApp.Visible = true;

                        // Создаем новый документ
                        Document doc = wordApp.Documents.Add();

                        // Устанавливаем ориентацию документа на альбомную
                        doc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;

                        // Устанавливаем шрифт и размер текста
                        doc.Content.Font.Name = "Calibri";
                        doc.Content.Font.Size = 14;

                        var osmotryData = entities.osmotry.FirstOrDefault(o => o.Id_pacienta == selectedPacient.Id_pacienta && o.data_osmotra == selectedDateParsed);
                        // Получаем предписания для данного осмотра
                        var predpisanieData = entities.predpisanya.Where(p => p.id_osmotra == osmotryData.Id_osmotra).ToList();

                        var vrach = entities.vrachi.FirstOrDefault(v => v.Id_vracha == osmotryData.Id_vracha);
                        string predpisanieInfo = "";


                        if (osmotryData == null)
                        {
                            osmotryData = new osmotry { rezult_osmotra = "нет информации", rec_vracha = "нет информации", diagnoz = "нет информации" };
                        }

                        if (predpisanieData == null || predpisanieData.Count == 0)
                        {
                            predpisanieInfo = "нет информации";
                        }
                        else
                        {
                            foreach (var predpisanie in predpisanieData)
                            {
                                if (string.IsNullOrEmpty(predpisanie.naznachenie))
                                {
                                    predpisanie.naznachenie = "нет информации";
                                }
                                if (string.IsNullOrEmpty(predpisanie.dozirovka))
                                {
                                    predpisanie.dozirovka = "нет информации";
                                }
                                if (string.IsNullOrEmpty(predpisanie.srok_priema))
                                {
                                    predpisanie.srok_priema = "нет информации";
                                }

                                predpisanieInfo += $"Назначаемые препараты: {predpisanie.naznachenie}\n" +
                                                   $"Дозировка: {predpisanie.dozirovka}\n" +
                                                   $"Срок приема: {predpisanie.srok_priema}\n\n";
                            }
                        }



                        // Заполняем документ данными из базы данных
                        if (osmotryData != null)
                        {


                            doc.Content.Text = $"Информация о пациенте:\n" +
                                              $"ФИО: {selectedPacient.fio}\n" +
                                              $"Номер телефона: {selectedPacient.nomer_tel}\n" +
                                              $"Дата рождения: { selectedPacient.birthday.ToShortDateString()}\n" +
                                              $"Лечащий врач: {vrach.fio1}\n" +
                                              $"Дата осмотра: {osmotryData.data_osmotra.ToShortDateString()}\n" +
                                              $"Результат осмотра: {osmotryData.rezult_osmotra}\n" +
                                              $"Рекомендации врача: {osmotryData.rec_vracha}\n" +
                                              $"Диагноз: {osmotryData.diagnoz}\n" +
                                              $"Предписания:\n{predpisanieInfo}";

                        }

                        else
                        {
                            MessageBox.Show("Для выбранного пациента и даты отсутствуют данные осмотра.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        // Сохраняем документ
                        string filePath = @"Путь_к_файлу.docx"; // Укажите путь к файлу
                        doc.SaveAs(filePath);

                        // Освобождаем ресурсы
                        doc = null;
                        wordApp = null;
                    }
                    else
                    {
                        MessageBox.Show("выберите дату осмотра");
                    }
                }






                else
                {
                    MessageBox.Show("Выберите пациента из списка.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }



            }

            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }
    }
}
