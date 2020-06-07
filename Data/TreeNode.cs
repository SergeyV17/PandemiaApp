using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Data
{
    public partial class TreeNode
    {
        public bool IsSaved { get; private set; }
        public int MemberID { get; private set; }
        public DateTime InfectionDateTime { get; private set; }
        public TreeNode Parent { get; private set; }

        private static int[] NumbersOfInfected;

        private static int _maxNumberOfInfected;

        public static int MaxNumberOfInfected
        {
            get { return _maxNumberOfInfected; }
            set
            {
                _maxNumberOfInfected = value;
                OnMaxNumberOfInfectedChanged(EventArgs.Empty);
            }
        }

        public static string PandemiaPeakMonth {get; set;}

        public static event EventHandler MaxNumberOfInfectedChanged;

        private static void OnMaxNumberOfInfectedChanged(EventArgs e)
        {
            MaxNumberOfInfectedChanged?.Invoke(null, e);
        }

        public List<TreeNode> Children { get; private set; }

        static TreeNode()
        {
            MaxNumberOfInfectedChanged += (sender, e) => { return; };

            NumbersOfInfected = new int[12];
            _maxNumberOfInfected = 0;
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


        private void AddNode(TreeNode treeNode)
        {
            Children.Add(treeNode);
            treeNode.Parent = this;
        }

        public void CreateTree(TreeNode node, List<Contact> contacts, int count = 0)
        {
            if (count == 0)
            {
                var treeNode = new TreeNode(contacts[0].Member1_ID, contacts[0].From);
                this.Children.Add(treeNode);

                CreateTree(treeNode, contacts, ++count);
            }
            else
            {
                if (count < contacts.Count)
                {
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        // if infection datetime > current contact datetime then go forward
                        if (node.InfectionDateTime > contacts[i].From)
                        {
                            continue;
                        }
                        else
                        {
                            // if infected member == current contact member
                            if (node.MemberID == contacts[i].Member1_ID)
                            {
                                //if date of contact enters the stage when the disease is transmitted
                                if (contacts[i].From > node.InfectionDateTime + Virus._firstStageOfTheDisease &&
                                    contacts[i].From < node.InfectionDateTime + Virus._totalDiseaseTime)
                                {
                                    // if contact lasted longer than safe time
                                    if (contacts[i].To - contacts[i].From > Virus._safeTime)
                                    {
                                        var treeNode = new TreeNode(contacts[i].Member2_ID, contacts[i].From);
                                        node.AddNode(treeNode);

                                        CreateTree(treeNode, contacts, ++count);
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        //BFS
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