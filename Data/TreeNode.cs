using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Data
{
    public class TreeNode
    {
        public int MemberID {get; private set;}
        public DateTime InfectionDateTime { get; private set; }
        public TreeNode Parent { get; set; }

        public List<TreeNode> TreeNodes;

        public TreeNode()
        {
            TreeNodes = new List<TreeNode>();
        }

        public TreeNode(int MemberID, DateTime InfectionDateTime)
        {
            this.MemberID = MemberID;
            this.InfectionDateTime = InfectionDateTime;

            TreeNodes = new List<TreeNode>();
        }

        private void AddNode(TreeNode treeNode)
        {
            TreeNodes.Add(treeNode);
            treeNode.Parent = this;
        }

        public void CreateTree(TreeNode node, List<Contact> contacts, int count = 0)
        {
            if (count == 0)
            {
                var treeNode = new TreeNode(contacts[0].Member1_ID, contacts[0].From);
                this.TreeNodes.Add(treeNode);

                CreateTree(treeNode, contacts, ++count);
            }
            else
            {
                if (count < contacts.Count)
                {
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        if (contacts[i].Member1_ID == node.MemberID)
                        {
                            if (contacts[i].From >= node.InfectionDateTime + Virus._firstStageOfTheDisease)
                            {
                                if (contacts[i].To - contacts[i].From > Virus._infectedTime)
                                {
                                    var treeNode = new TreeNode(contacts[i].Member2_ID, contacts[i].From);
                                    node.AddNode(treeNode);

                                    CreateTree(treeNode, contacts, ++count);

                                    //treeNode = new TreeNode(contacts[i].Member2_ID, contacts[i].From);
                                    //node.AddNode(treeNode);

                                    //CreateTree(treeNode, contacts, ++count);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void PrintTree(string path, TreeNode tree, List<Person> persons, string trim = "")
        {
            File.WriteAllText(path, "");

            Print(path, tree, persons);
        }

        private void Print(string path, TreeNode tree, List<Person> persons, string trim = "")
        {
            foreach (var node in tree.TreeNodes)
            {
                string name = persons.Find(e => e.ID == node.MemberID).Name;

                string line = string.Format("{0} Name: {1} ID: {2}", trim, name, node.MemberID);

                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(line);
                }

                Print(path, node, persons, trim + "     ");
            }
        }
    }
}