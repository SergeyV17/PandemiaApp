using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class TreeNode
    {
        public int MemberID {get; private set;}
        public DateTime InfectionTime { get; private set; }
        public TreeNode Parent { get; private set; }

        public List<TreeNode> TreeNodes;

        public TreeNode(int MemberId, DateTime InfectionTime)
        {
            this.MemberID = MemberID;
            this.InfectionTime = InfectionTime;

            TreeNodes = new List<TreeNode>();
        }

        private void AddNode(TreeNode treeNode)
        {
            TreeNodes.Add(treeNode);
            treeNode.Parent = this;
        }

        //private void CreateTree(TreeNode node, List<Contact> contacts, int count = 0)
        //{
        //    while (count != contacts.Count)
	       // {
        //        if (count == 0)
	       //     {
		      //      this.MemberID = 213127;
        //            this.InfectionTime = new DateTime(2020,01,01,0,0,0);
	       //     }
        //        else
	       //     {
        //            for (int i = 0; i < contacts.Count; i++)
			     //   {
			     //       if (true)
	       //             {
		                    
	       //             }
			     //   }
	       //     }
        //        var virus = new Virus();
	         
        //        var fromDateTime = Convert.ToDateTime(contacts[count].From);
        //        var toDateTime = Convert.ToDateTime(contacts[count].To);

        //        if (toDateTime - fromDateTime > virus._infectedTime)
        //        {
        //            var treeNode = new TreeNode(contacts[count].Member1_ID, Convert.ToDateTime(contacts[count].From));
        //            node.AddNode(treeNode);

        //            treeNode = new TreeNode(contacts[count].Member2_ID, Convert.ToDateTime(contacts[count].To));
        //            node.AddNode(treeNode);
        //        }

        //        CreateTree
	       // }
        //}

        //private void CreateTree( TreeNode tree, List<Contact> contacts)
        //{
        //    var virus = new Virus();

        //    for (int i = 0; i < contacts.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            var fromDateTime = Convert.ToDateTime(contacts[i].From);
        //            var toDateTime = Convert.ToDateTime(contacts[i].To);

        //            if (toDateTime - fromDateTime > virus._infectedTime)
        //            {
        //                var treeNode = new TreeNode(contacts[i].Member1_ID, Convert.ToDateTime(contacts[i].From));
        //                tree.AddNode(treeNode);

        //                treeNode = new TreeNode(contacts[i].Member2_ID, Convert.ToDateTime(contacts[i].To));
        //                tree.AddNode(treeNode);
        //            }
        //        }
        //        else
        //        {
        //            if (MemberID contacts[i].)
        //            {
                        
        //            }
        //        }
        //    }
        //}
    }
}
