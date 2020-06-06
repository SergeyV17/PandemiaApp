using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Data
{
    enum Monthes
    {
        February = 2,
        Marсh,
        April,
        May
    }
    
    public class TreeNode
    {
        public bool IsSaved { get; private set; }
        public int MemberID {get; private set;}
        public DateTime InfectionDateTime { get; private set; }
        public TreeNode Parent { get; private set; }

        public int NumberOfInfected { get; private set; }

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

        public static int CurrentMonth { get; private set; }

        public static event EventHandler MaxNumberOfInfectedChanged;

        private static void OnMaxNumberOfInfectedChanged(EventArgs e)
        {
            MaxNumberOfInfectedChanged?.Invoke(null, e);
        }

        public List<TreeNode> Children { get; private set; }

        static TreeNode()
        {
            MaxNumberOfInfectedChanged += (sender, e) => { return; };

            _maxNumberOfInfected = 0;
            CurrentMonth = 0;
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

        public void CalculateInfectedPersonsOnPeak(TreeNode tree)
        {
            foreach (var node in tree.Children)
            {
                if (node.Parent == null)
                {
                    CurrentMonth = node.InfectionDateTime.Month;

                    NumberOfInfected++;
                }
                else
                {
                    if (CurrentMonth != node.InfectionDateTime.Month)
                    {
                        MaxNumberOfInfected = NumberOfInfected;

                        NumberOfInfected = 0;
                    }

                    NumberOfInfected++;
                }
            }
        }

        public bool SaveTreeToFile(string path, TreeNode tree, string trim = "")
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