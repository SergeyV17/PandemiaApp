using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Aibim_Test_Vlasenko.S.A.Windows
{
    /// <summary>
    ///Interaction logic for CalculateAverage.xaml
    /// </summary>
    public partial class CalculateAverageAgeWindow : Window
    {
        public CalculateAverageAgeWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Логика при нажатии кнопки ShowResultBtn
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        private void ShowResultBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = this.Owner as MainWindow;

            string firstName = FirstNameTxtBx.Text.Trim();

            string lastName = LastNameTxtBx.Text.Trim();

            if (ErrorProcessing(firstName) || ErrorProcessing(lastName))
            {
                MessageBox.Show(
                    "You entered ivalid characters",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                string soughtName = lastName + " " + firstName;

                // Поиск всех членов с одинаковыми фамилией и именем
                var namesakes = main.Repository.Persons.FindAll(p => p.Name == soughtName);

                // Если не найдено ни одного члена
                if (namesakes.Count == 0)
                {
                    MessageBox.Show(
                        "No found coinsidence",
                        "Notification",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    // Если найдено, расчитывается средний возраст
                    var average = Math.Round(namesakes.Average(p => p.Age));

                    MessageBox.Show(
                        string.Format("Average age for name {0} is: {1}", soughtName, average),
                        "Notification",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Метод обработки ошибок
        /// </summary>
        /// <param name="soughtName">искомое имя</param>
        /// <returns>ошибка ввода</returns>
        private bool ErrorProcessing(string initial)
        {
            // Валидация ввода
            if (initial.Any(e => !char.IsLetter(e)))
                return true;

            return false;
        }
    }
}
