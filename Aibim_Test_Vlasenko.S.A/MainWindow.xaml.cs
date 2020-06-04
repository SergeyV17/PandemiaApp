using Aibim_Test_Vlasenko.S.A.Windows;
using System.Windows;
using Data;

namespace Aibim_Test_Vlasenko.S.A
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Repository Repository { get; private set; } // Repository for data

        private readonly string big_data_persons; //path fields
        private readonly string big_data_contacts;

        public MainWindow()
        {
            InitializeComponent();

            Repository = new Repository();

            big_data_persons = @"Data\JSON\small_data_persons.json";
            big_data_contacts = @"Data\JSON\small_data_contacts.json";

            DataContext = Repository;
        }

        #region Menu

        /// <summary>
        /// Load data button handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">args</param>
        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            if (!Repository.IsCreated)
            {
                // Load data to repository
                bool success = Repository.LoadDataToRepository(big_data_persons, big_data_contacts);

                if (success)
                {
                    // Fill tables itemsources
                    PersonTable.ItemsSource = Repository.Persons;
                    ContactsTable.ItemsSource = Repository.Contacts;
                }
                else
                {
                    MessageBox.Show(
                        this,
                        "Data can't load. Check path folders.",
                        Title,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
            else
            {
                MessageBox.Show(
                    this,
                    "Data is already loaded",
                    Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        /// <summary>
        /// Exit button handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">args</param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(this,
                "Are you sure, you want to exit?",
                Title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                Close();
            else
                return;
        }

        #endregion

        #region Command panel

        /// <summary>
        /// Show average age by name button handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">args</param>
        private void ShowAverageAgeByName_Click(object sender, RoutedEventArgs e)
        {
            var calculateAverageAgeWindow = new CalculateAverageAgeWindow { Owner = this };

            calculateAverageAgeWindow.ShowDialog();
        }

        /// <summary>
        /// Show dangerous contacts button handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">args</param>
        private void ShowDangerousContacts_Click(object sender, RoutedEventArgs e)
        {
            var showDangerousContactsWindow = new ShowDangerousContactsWindow() { Owner = this };

            showDangerousContactsWindow.ShowDialog();
        }

        #endregion
    }
}
