using System.Collections.Generic;
using System.Windows;
using Data;

namespace Aibim_Test_Vlasenko.S.A.Windows
{
    /// <summary>
    /// Interaction logic for DangerousContacts.xaml
    /// </summary>
    public partial class DangerousContactsWindow : Window
    {
        public DangerousContactsWindow()
        {
            InitializeComponent();

            this.Closing += DangerousContactsWindow_Closing1;
        }

        private void DangerousContactsWindow_Closing1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner = null;
        }

        public DangerousContactsWindow(List<Contact> dangerousContacts)
            : this()
        {
            DangerousContactsList.ItemsSource = dangerousContacts;
        }
    }
}
