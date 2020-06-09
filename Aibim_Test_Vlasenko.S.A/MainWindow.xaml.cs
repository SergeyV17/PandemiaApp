using Aibim_Test_Vlasenko.S.A.Windows;
using System.Windows;
using Data;
using ExportData;
using System;
using System.IO;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;

namespace Aibim_Test_Vlasenko.S.A
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Repository Repository { get; private set; } // Репозиторий данных

        private readonly string small_data_persons; //поля для путей
        private readonly string small_data_contacts;
        private readonly string infection_tree;
        private readonly string about;

        public MainWindow()
        {
            InitializeComponent();

            Repository = new Repository();

            small_data_persons = @"Data\JSON\small_data_persons.json";
            small_data_contacts = @"Data\JSON\small_data_contacts.json";
            infection_tree = @"InfectionTree\infection_tree.txt";
            about = @"Resources\About.txt";

            DataContext = Repository;
        }

        #region Menu

        /// <summary>
        /// Логика при нажатии кнопки LoadDataBtn
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        private void LoadDataBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Repository.DataIsLoaded)
            {
                // Загрузка данных в репозиторий
                bool success = Repository.LoadDataToRepository(small_data_persons, small_data_contacts);

                if (success)
                {
                    // Заполнение таблиц
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
        /// Логика при нажатии кнопки ExitBtn
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Логика при нажатии кнопки AboutBtn
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this,
                File.ReadAllText(about),
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        #endregion

        #region Command panel

        /// <summary>
        /// Логика при нажатии кнопки ShowAverageAgeByNameBtn
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        private void ShowAverageAgeByNameBtn_Click(object sender, RoutedEventArgs e)
        {
            var calculateAverageAgeWindow = new CalculateAverageAgeWindow { Owner = this };

            calculateAverageAgeWindow.ShowDialog();
        }

        /// <summary>
        /// Логика при нажатии кнопки ShowDangerousContactBtn
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        private void ShowDangerousContactsBtn_Click(object sender, RoutedEventArgs e)
        {
            var showDangerousContactsWindow = new ShowDangerousContactsWindow() { Owner = this };

            showDangerousContactsWindow.ShowDialog();
        }

        /// <summary>
        /// Логика при нажатии кнопки CreateInfectionTreeBtn
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        private void CreateInfectionTreeBtn_Click(object sender, RoutedEventArgs e)
        {
            InfectionTree.Visibility = Visibility.Hidden;
            LoadingPanel.Visibility = Visibility.Visible;

            var worker = new BackgroundWorker();

            worker.DoWork += (s, a) =>
            {
                // Создание дерева
                bool success = Repository.CreateInfectionTree();

                if (success)
                {
                    //Используем Dispatcher для выполнения метода в потоке UI
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            // Заполнение дерева
                            InfectionTree.ItemsSource = Repository.Tree.Children;
                        });
                }
                else
                {
                    MessageBox.Show(
                        this,
                        "Tree cannot create. Something goes wrong.",
                        Title,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            };

            worker.RunWorkerAsync();

            worker.RunWorkerCompleted += (s, a) =>
            {
                LoadingPanel.Visibility = Visibility.Hidden;
                InfectionTree.Visibility = Visibility.Visible;
            };
        }

        /// <summary>
        /// Логика при нажатии кнопки SaveInfectionTreeToTxtBtn
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        private void SaveInfectionTreeToTxtBtn_Click(object sender, RoutedEventArgs e)
        {
            bool success = SaveTree.SaveTreeToFile(infection_tree, Repository.Tree);

            if (success)
            {
                MessageBox.Show(
                    this,
                    @"The infection tree is saved to a file InfectionTree\infection_tree.txt",
                    Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            else
            {
                MessageBox.Show(
                    this,
                    "Tree cannot save. Something goes wrong",
                    Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        #endregion

        #region Virus settings

        /// <summary>
        /// Логика при нажатии кнопки AcceptVirusSettingsBtn
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        private void AcceptVirusSettingsAndRefreshTreeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Изменение параметров вируса и вызов метода построения дерева
                Virus.AcceptVirusSettings(VirusSafeTimeTxtBx.Text, FirstStageOfDiseaseTxtBx.Text, SecondtStageOfDiseaseTxtBx.Text, ImmunityTimeTxtBx.Text);
                CreateInfectionTreeBtn_Click(sender, e);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    this,
                    "Parsing error. You entered invalid characters on virus settings.",
                    Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        #endregion
    }
}
