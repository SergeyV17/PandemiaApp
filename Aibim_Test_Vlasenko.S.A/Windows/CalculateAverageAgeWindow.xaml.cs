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
        /// Show result button handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">args</param>
        private void ShowResult_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = this.Owner as MainWindow;

            string firstName = TxtBoxFirstName.Text.Trim();

            string lastName = TxtBoxLastName.Text.Trim();

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

                // Find all persons with identical names
                var namesakes = main.Repository.Persons.FindAll(p => p.Name == soughtName);

                //if not found
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
                    // if found calculate average
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
        /// Error processing method
        /// </summary>
        /// <param name="soughtName">sought name</param>
        /// <returns>error sign</returns>
        private bool ErrorProcessing(string initial)
        {
            // Validation of input
            if (initial.Any(e => !char.IsLetter(e)))
                return true;

            return false;
        }
    }
}
