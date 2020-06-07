using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public partial class TreeNode
    {
        private static readonly object _locker;

        public bool IsSaved { get; private set; }
        public int MemberID { get; private set; }
        public DateTime InfectionDateTime { get; private set; }
        public DateTime ImmunityDateTime { get; private set; }
        public TreeNode Parent { get; private set; }

        private static int[] NumbersOfInfected;
        public static int MaxNumberOfInfected { get; private set; }
        public static string PandemiaPeakMonth {get; set;}

        public List<TreeNode> Children { get; private set; }

        static TreeNode()
        {
            NumbersOfInfected = new int[12];
            MaxNumberOfInfected = 0;

            _locker = new object();
        }

        public TreeNode()
        {
            Children = new List<TreeNode>();
        }

        public TreeNode(int MemberID, DateTime InfectionDateTime)
        {
            this.MemberID = MemberID;
            this.InfectionDateTime = InfectionDateTime;

            Children = new List<TreeNode>();
        }

        public TreeNode(int MemberID, DateTime InfectionDateTime, DateTime ImmunityDateTime)
        {
            this.MemberID = MemberID;
            this.InfectionDateTime = InfectionDateTime;
            this.ImmunityDateTime = ImmunityDateTime;

            Children = new List<TreeNode>();
        }


        private void AddNode(TreeNode treeNode)
        {
            Children.Add(treeNode);
            treeNode.Parent = this;
        }

        public void CreateTree(TreeNode node, List<Contact> contacts, DateTime initialDateTime)
        {   
            //Перегруз
            CreateTree(node, contacts, initialDateTime, new DateTime(2020, 02, 01));
        }

        public void CreateTree(TreeNode node, List<Contact> contacts, DateTime initialDateTime, DateTime start)
        {
            if (start == initialDateTime)
            {
                // Контактирующий заражается спустя завершения безопасного времени после контакта с заражённым
                var infectionDateTime = contacts[0].From.Add(Virus._safeTime);

                //Иммунитет второго контактирующего начинает действовать после завершения болезни
                var immunityDateTime = infectionDateTime + Virus._totalDiseaseTime;

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
                                if (contacts[i].From > node.InfectionDateTime + Virus._firstStageOfTheDisease &&
                                    contacts[i].From < node.InfectionDateTime + Virus._totalDiseaseTime)
                                {
                                    int infectedMemberID = node.MemberID == contacts[i].Member1_ID ? contacts[i].Member2_ID : contacts[i].Member1_ID;

                                    // Если контакт длился больше безопасного времени
                                    if (contacts[i].To - contacts[i].From > Virus._safeTime)
                                    {
                                        // Проверка был ли данный член уже заражён в максимально близкое к текущему заражению времени
                                        var infectedMember = SearchInfectedPerson(this, e => e.MemberID == infectedMemberID &&
                                        e.InfectionDateTime <= node.InfectionDateTime);

                                        // Если данный член не был до этого заражён, опускаем проверку на иммунитет
                                        if (infectedMember == null)
                                        {
                                            //Второй контактирующий заражается спустя завершения безопасного времени после контакта с заражённым
                                            DateTime infectionDateTime = contacts[i].From.Add(Virus._safeTime);

                                            //Иммунитет второго контактирующего начинает действовать после завершения болезни
                                            DateTime immunityDateTime = infectionDateTime + Virus._totalDiseaseTime;

                                            var treeNode = new TreeNode(infectedMemberID, infectionDateTime, immunityDateTime);
                                            node.AddNode(treeNode);

                                            CreateTree(treeNode, contacts, initialDateTime, start.AddDays(1));
                                        }
                                        else
                                        {
                                            //Проверка имеет ли данный член иммунитет
                                            if (contacts[i].From > infectedMember.ImmunityDateTime + Virus._immunityTime)
                                            {
                                                //Второй контактирующий заражается спустя завершения безопасного времени после контакта с заражённым
                                                DateTime infectionDateTime = contacts[i].From.Add(Virus._safeTime);

                                                //Иммунитет второго контактирующего начинает действовать после завершения болезни
                                                DateTime immunityDateTime = infectionDateTime + Virus._totalDiseaseTime;

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


        //BFS
        private TreeNode SearchInfectedPerson(TreeNode tree, Predicate<TreeNode> match)
        {
            var queue = new Queue<TreeNode>();

            queue.Enqueue(tree);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                if (match(node))
                    return node;

                foreach (var child in node.Children)
                {
                    queue.Enqueue(child);
                }
            }

            return null;
        }

        //BFT
        public void CalculateInfectedPersonsOnPeak(TreeNode tree)
        {
            if (tree == null)
                return;

            var queue = new Queue<TreeNode>();
            queue.Enqueue(tree);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

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

            MaxNumberOfInfected = NumbersOfInfected.Max();
            PandemiaPeakMonth = ((Monthes)Array.IndexOf(NumbersOfInfected, MaxNumberOfInfected)).ToString();
        }

        public bool SaveTreeToFile(string path, TreeNode tree)
        {
            try
            {
                File.Delete(path);
                File.WriteAllText(path, "");

                Print(path, tree);

                return IsSaved = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return IsSaved = false;
            }
        }

        private void Print(string path, TreeNode tree, string trim = "")
        {
            foreach (var node in tree.Children)
            {
                string line = string.Format("{0} ID: {1} Infection time: {2:dd.MM.yyyy HH:mm:ss}", trim, node.MemberID, node.InfectionDateTime);
                
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(line);
                }

                Print(path, node, trim + "     ");
            }
        }
    }
}