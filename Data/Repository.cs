using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using ImportData;
using System.Linq;

namespace Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public class Repository: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isCreated;

        public bool IsCreated
        {
            get { return _isCreated; }
            set
            {
                _isCreated = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsCreated"));
            }
        }

        public List<Person> Persons { get; private set; }
        public List<Contact> Contacts { get; private set; }

        public TreeNode Tree { get; private set; }

        public Repository()
        {
            Persons = new List<Person>();
            Contacts = new List<Contact>();
            Tree = new TreeNode();
        }

        /// <summary>
        /// Load data method
        /// </summary>
        /// <param name="personPath">path to file with persons</param>
        /// <param name="contactsPath">path to file with contacts</param>
        /// <returns>IsCreated</returns>
        public bool LoadDataToRepository(string personPath, string contactsPath)
        {
            try
            {
                //load data
                Persons = Deserialize.LoadJson<Person>(personPath).OrderBy(e => e.ID).ToList(); // sort by ID for convenience
                Contacts = Deserialize.LoadJson<Contact>(contactsPath).OrderBy(e => e.From).ToList(); // sort by date for convenience

                Tree.CreateTree(Tree, Contacts);

                return IsCreated = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return IsCreated = false;
            }
        }
    }
}
