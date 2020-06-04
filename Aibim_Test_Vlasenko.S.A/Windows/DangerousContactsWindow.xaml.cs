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
        }

        public DangerousContactsWindow(List<Contact> dangerousContacts)
            : this()
        {
            DangerousContactsList.ItemsSource = dangerousContacts;
        }
    }
}
