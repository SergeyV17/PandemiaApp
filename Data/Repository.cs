using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using ImportData;
using System.Linq;

namespace Data
{
    /// <summary>
    /// Репозиторий
    /// </summary>
    public class Repository: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _dataIsLoaded;

        public bool DataIsLoaded
        {
            get { return _dataIsLoaded; }
            set
            {
                _dataIsLoaded = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DataIsLoaded"));
                }
            }
        }

        private bool _treeIsCreated;

        public bool TreeIsCreated
        {
            get { return _treeIsCreated; }
            set
            {
                _treeIsCreated = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TreeIsCreated"));
                }
            }
        }

        public List<Person> Persons { get; private set; }
        public List<Contact> Contacts { get; private set; }

        public TreeNode Tree { get; private set; }

        public Repository()
        {
            Persons = new List<Person>();
            Contacts = new List<Contact>();
        }

        /// <summary>
        /// Метод загрузки данных из Json
        /// </summary>
        /// <param name="personPath">путь к json содержащий список персон</param>
        /// <param name="contactsPath">путь к json содержащий список контактов </param>
        /// <returns>DataIsLoaded</returns>
        public bool LoadDataToRepository(string personPath, string contactsPath)
        {
            try
            {
                //Загрузка данных из Json
                Persons = Deserialize.LoadJson<Person>(personPath).OrderBy(e => e.ID).ToList(); // Сортировка по ID (для удобства)
                Contacts = Deserialize.LoadJson<Contact>(contactsPath).OrderBy(e => e.From).ToList(); // Сортировка по дате (для удобства)

                return DataIsLoaded = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return DataIsLoaded = false;
            }
        }

        /// <summary>
        /// Метод создания дерева
        /// </summary>
        /// <returns>TreeIsCreated</returns>
        public bool CreateInfectionTree()
        {
            TreeIsCreated = false;

            try
            {
                Tree = new TreeNode();
                Tree.CreateTree(Tree, Contacts, new DateTime(2020, 02, 01));
                Tree.CalculateInfectedMemberAndManHoursLost(Tree);

                return TreeIsCreated = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return TreeIsCreated = false;
            }
        }
    }
}
