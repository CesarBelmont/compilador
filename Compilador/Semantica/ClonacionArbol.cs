using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador.Semantica
{
    class ClonacionArbol
    {
        public static void cloneNode(TreeNode parent, TreeNode child)
        {
            TreeNode newChild;
            foreach (TreeNode currentNode in child.Nodes)
            {
                newChild = new TreeNode(currentNode.Text);
                cloneNode(newChild, currentNode);
                if (!newChild.Text.Equals(string.Empty))
                {
                    parent.Nodes.Add(newChild);
                }
                else
                {
                    foreach (TreeNode aux in newChild.Nodes)
                    {
                        parent.Nodes.Add(aux);
                    }
                }
            }
        }
    }
}

