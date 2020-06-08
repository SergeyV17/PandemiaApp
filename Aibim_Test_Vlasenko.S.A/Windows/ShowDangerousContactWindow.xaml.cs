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
        /// Логика при нажатии кнопки ShowResultBtn
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        private void ShowResultBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = this.Owner as MainWindow;

            var start = StartTxtBx.Text;
            var end = EndTxtBx.Text;

            DateTime intervalStart;
            DateTime intervalEnd;

            // Валидация входных параметров
            if (DateTime.TryParse(start, out intervalStart) && DateTime.TryParse(end, out intervalEnd))
            {
                // Поиск связей которые были в контакте более 10 минут
                var dangerousContacts = main.Repository.Contacts.FindAll(c => 
                    c.From > intervalStart && 
                    c.To < intervalEnd && 
                    c.To - c.From > TimeSpan.FromMinutes(10));

                // Если связи не найдены
                if (dangerousContacts.Count == 0)
                {
                    MessageBox.Show(
                        "No contacts found in a given interval",
                        "Notification",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    return;
                }

                // Инициализация DangerousContactsWindow и передача в качестве параметров список найденных контактов
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
