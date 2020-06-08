using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    static class TreeSearch
    {
        /// <summary>
        /// Поиск заражённого члена
        /// </summary>
        /// <param name="tree">корень дерева</param>
        /// <param name="match">условие сравнения</param>
        /// <returns>искомый член</returns>
        public static TreeNode SearchInfectedMember(TreeNode tree, Predicate<TreeNode> match)
        {
            if (tree == null)
                return null;

            // Реализация обхода по ширине (Breadth first search)
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
    }
}
