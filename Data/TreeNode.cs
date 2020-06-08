using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Data
{
    /// <summary>
    /// Класс узла дерева
    /// </summary>
    public partial class TreeNode
    {
        // Эвент срабатывающий при изменении значения статических свойств
        public static event PropertyChangedEventHandler StaticPropertyChanged;
        public static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (StaticPropertyChanged != null)
            {
                StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int MemberID { get; private set; }
        public DateTime InfectionDateTime { get; private set; }
        public DateTime ImmunityDateTime { get; private set; }
        public TreeNode Parent { get; private set; }

        private int[] NumbersOfInfected;

        private static int _maxNumberOfInfected;

        public static int MaxNumberOfInfected
        {
            get { return _maxNumberOfInfected; }
            set
            {
                _maxNumberOfInfected = value;
                NotifyStaticPropertyChanged("MaxNumberOfInfected");
            }
        }

        private static string _pandemiaPeakMonth;

        public static string PandemiaPeakMonth
        {
            get { return _pandemiaPeakMonth; }
            set
            {
                _pandemiaPeakMonth = value;
                 NotifyStaticPropertyChanged("PandemiaPeakMonth");
            }
        }

        private static int _manHoursLosted;

        public static int ManHoursLost
        {
            get { return _manHoursLosted; }
            set
            {
                _manHoursLosted = value;
                NotifyStaticPropertyChanged("ManHoursLost");
            }
        }

        public List<TreeNode> Children { get; private set; }

        public TreeNode()
        {
            Children = new List<TreeNode>();
            NumbersOfInfected = new int[12];
        }

        /// <summary>
        /// Конструктор узла
        /// </summary>
        /// <param name="MemberID">ID члена</param>
        /// <param name="InfectionDateTime">Дата заражения</param>
        public TreeNode(int MemberID, DateTime InfectionDateTime) : 
            this()
        {
            this.MemberID = MemberID;
            this.InfectionDateTime = InfectionDateTime;
        }

        /// <summary>
        /// Конструктор узла
        /// </summary>
        /// <param name="MemberID">ID члена</param>
        /// <param name="InfectionDateTime">дата заражения</param>
        /// <param name="ImmunityDateTime">дата начала действия иммунитета</param>
        public TreeNode(int MemberID, DateTime InfectionDateTime, DateTime ImmunityDateTime) :
            this(MemberID,InfectionDateTime)
        {
            this.ImmunityDateTime = ImmunityDateTime;
        }

        /// <summary>
        /// Метод добавления нового узла
        /// </summary>
        /// <param name="treeNode">новый узел дерева</param>
        private void AddNode(TreeNode treeNode)
        {
            Children.Add(treeNode);
            treeNode.Parent = this;
        }

        /// <summary>
        /// Метод создания дерева
        /// </summary>
        /// <param name="node">первоначальный узел</param>
        /// <param name="contacts">список контактов</param>
        /// <param name="initialDateTime">исходная дата (необходима для создания первого заражённого)</param>
        public void CreateTree(TreeNode node, List<Contact> contacts, DateTime initialDateTime)
        {   
            // Метод перегружен для инициализации необязательного параметра DateTime start
            CreateTree(node, contacts, initialDateTime, new DateTime(2020, 02, 01));
        }

        /// <summary>
        /// Метод создания дерева
        /// </summary>
        /// <param name="node">узел дерева</param>
        /// <param name="contacts">список контактов</param>
        /// <param name="initialDateTime">исходная дата(необходима для создания первого заражённого)</param>
        /// <param name="start">исходная дата</param>
        public void CreateTree(TreeNode node, List<Contact> contacts, DateTime initialDateTime, DateTime start)
        {
            if (start == initialDateTime)
            {
                // Контактирующий заражается спустя завершения безопасного времени после контакта с заражённым
                var infectionDateTime = contacts[0].From.Add(Virus.SafeTime);

                //Иммунитет второго контактирующего начинает действовать после завершения болезни
                var immunityDateTime = infectionDateTime + Virus.TotalDiseaseTime;

                var treeNode = new TreeNode(contacts[0].Member1_ID, infectionDateTime, immunityDateTime);
                this.Children.Add(treeNode);

                CreateTree(treeNode, contacts, initialDateTime, initialDateTime.Add(TimeSpan.FromMilliseconds(1)));
            }
            else
            {
                int daysCount = DateTime.Now.Subtract(start).Days;

                if (daysCount > 0)
                {
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        // Если дата контакта меньше текущей даты отсчёта то переходим к следующей дате
                        if (contacts[i].From < start)
                        {
                            continue;
                        }
                        else
                        {
                            // Если ID заражённого == ID контактирующего члена
                            if (node.MemberID == contacts[i].Member1_ID || node.MemberID == contacts[i].Member2_ID)
                            {
                                //Если дата контакта в пределах даты заражения (т.е. член имеет способность к заражению в указанном интервале)
                                if (contacts[i].From > node.InfectionDateTime + Virus.FirstStageOfTheDisease &&
                                    contacts[i].From < node.InfectionDateTime + Virus.TotalDiseaseTime)
                                {
                                    int infectedMemberID = node.MemberID == contacts[i].Member1_ID ? contacts[i].Member2_ID : contacts[i].Member1_ID;

                                    // Если контакт длился больше безопасного времени
                                    if (contacts[i].To - contacts[i].From > Virus.SafeTime)
                                    {
                                        // Проверка был ли данный член уже заражён в максимально близкое к текущему заражению времени
                                        var infectedMember = TreeSearch.SearchInfectedMember(this, e => e.MemberID == infectedMemberID &&
                                        e.InfectionDateTime <= node.InfectionDateTime);

                                        // Если данный член не был до этого заражён, опускаем проверку на иммунитет
                                        if (infectedMember == null)
                                        {
                                            //Второй контактирующий заражается спустя завершения безопасного времени после контакта с заражённым
                                            DateTime infectionDateTime = contacts[i].From.Add(Virus.SafeTime);

                                            //Иммунитет второго контактирующего начинает действовать после завершения болезни
                                            DateTime immunityDateTime = infectionDateTime + Virus.TotalDiseaseTime;

                                            var treeNode = new TreeNode(infectedMemberID, infectionDateTime, immunityDateTime);
                                            node.AddNode(treeNode);

                                            CreateTree(treeNode, contacts, initialDateTime, start.AddDays(1));
                                        }
                                        else
                                        {
                                            //Проверка имеет ли данный член иммунитет
                                            if (contacts[i].From > infectedMember.ImmunityDateTime + Virus.ImmunityTime)
                                            {
                                                //Второй контактирующий заражается спустя завершения безопасного времени после контакта с заражённым
                                                DateTime infectionDateTime = contacts[i].From.Add(Virus.SafeTime);

                                                //Иммунитет второго контактирующего начинает действовать после завершения болезни
                                                DateTime immunityDateTime = infectionDateTime + Virus.TotalDiseaseTime;

                                                var treeNode = new TreeNode(infectedMemberID, infectionDateTime, immunityDateTime);
                                                node.AddNode(treeNode);

                                                CreateTree(treeNode, contacts, initialDateTime, start.AddDays(1));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Метод подсчёта заражённых членов и потери экономики в человеко-часах
        /// </summary>
        /// <param name="tree">корень дерева</param>
        public void CalculateInfectedMemberAndManHoursLost(TreeNode tree)
        {
            if (tree == null)
                return;

            //Реализация обхода по ширине (Breadth first traversing)
            var queue = new Queue<TreeNode>();
            queue.Enqueue(tree);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                if (node.ImmunityDateTime != DateTime.MinValue)
                {
                    // Объединил логику нахождения заражённых с подсчётом человека часов в пользу производительности
                    ManHoursLost += (node.ImmunityDateTime - node.InfectionDateTime).Days * 8;
                }

                switch ((Monthes)node.InfectionDateTime.Month)
                {
                    case Monthes.January:
                        NumbersOfInfected[(int)Monthes.January]++;
                        break;
                    case Monthes.February:
                        NumbersOfInfected[(int)Monthes.February]++;
                        break;
                    case Monthes.March:
                        NumbersOfInfected[(int)Monthes.March]++;
                        break;
                    case Monthes.April:
                        NumbersOfInfected[(int)Monthes.April]++;
                        break;
                    case Monthes.May:
                        NumbersOfInfected[(int)Monthes.May]++;
                        break;
                    case Monthes.June:
                        NumbersOfInfected[(int)Monthes.June]++;
                        break;
                    case Monthes.July:
                        NumbersOfInfected[(int)Monthes.July]++;
                        break;
                    case Monthes.August:
                        NumbersOfInfected[(int)Monthes.August]++;
                        break;
                    case Monthes.September:
                        NumbersOfInfected[(int)Monthes.September]++;
                        break;
                    case Monthes.November:
                        NumbersOfInfected[(int)Monthes.November]++;
                        break;
                    case Monthes.December:
                        NumbersOfInfected[(int)Monthes.December]++;
                        break;
                    default:
                        Debug.WriteLine("Incorrect number");
                        break;
                }

                foreach (var child in node.Children)
                {
                    queue.Enqueue(child);
                }
            }

            // Нахождение максимального количество заражённых исходя из статистики по месяцам
            MaxNumberOfInfected = NumbersOfInfected.Max();

            //Нахождения пикового месяца пандемии по количеству заражённых
            PandemiaPeakMonth = ((Monthes)Array.IndexOf(NumbersOfInfected, MaxNumberOfInfected)).ToString();
        }
    }
}