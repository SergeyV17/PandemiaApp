using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Data;

namespace ExportData
{
    /// <summary>
    /// Класс сохранения дерева в файл
    /// </summary>
    public static class SaveTree
    {
        public static bool IsSaved { get; private set; }

        /// <summary>
        /// Метод сохранения дерева в файл
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="tree">корень дерева</param>
        /// <returns>признак успешного сохранения</returns>
        public static bool SaveTreeToFile(string path, TreeNode tree)
        {
            IsSaved = false;

            try
            {
                File.Delete(path);
                Print(path, tree);

                return IsSaved = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return IsSaved = false;
            }
        }

        /// <summary>
        /// Метод печати дерева в файл
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="tree">корень дерева</param>
        /// <param name="trim">отступ</param>
        private static void Print(string path, TreeNode tree, string trim = "")
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
