using System;
using System.Windows;

namespace Aibim_Test_Vlasenko.S.A.Windows
{
    /// <summary>
    /// Interaction logic for ShowDangerousContactsWindow.xaml
    /// </summary>
    public partial class ShowDangerousContactsWindow : Window
    {
        public ShowDangerousContactsWindow()
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

            var start = TxtBoxStart.Text;
            var end = TxtBoxEnd.Text;

            DateTime intervalStart;
            DateTime intervalEnd;

            // Validation of input
            if (DateTime.TryParse(start, out intervalStart) && DateTime.TryParse(end, out intervalEnd))
            {
                // Find contacts that lasted more then 10 minutes in a given interval
                var dangerousContacts = main.Repository.Contacts.FindAll(c => 
                    c.From > intervalStart && 
                    c.To < intervalEnd && 
                    c.To - c.From > TimeSpan.FromMinutes(10));

                //if no contacts found in a given interval
                if (dangerousContacts.Count == 0)
                {
                    MessageBox.Show(
                        "No contacts found in a given interval",
                        "Notification",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    return;
                }

                //Initialize DangerousContactsWindow and put in parameters dangerousContacts list
                var dangerousContactsWindow = new DangerousContactsWindow(dangerousContacts) { Owner = main };

                dangerousContactsWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show(
                    "You entered ivalid datetime",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

    }
}
