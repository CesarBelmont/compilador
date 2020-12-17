using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador.Codigo_Intermedio
{
    class CodigoIntermedioAux
    {
        TreeView tvSemantico;
        TreeView tvSintactico;
        Semantica.aSemantico vSema;
        StringBuilder sbEtiquetas;
        public CodigoIntermedioAux(TreeView tvSemantico, TreeView tvSintactico)
        {
            this.tvSemantico = new TreeView();
            this.tvSintactico = new TreeView();
            vSema = new Semantica.aSemantico(null, null);
            mVoidCloneTree(tvSemantico, this.tvSemantico);
            mVoidCloneTree(tvSintactico, this.tvSintactico);
            createLabels();
        }
        private void mVoidCloneTree(TreeView tvSemantico, TreeView tvThis)
        {
            TreeNodeCollection tnColection = tvSemantico.Nodes;
            TreeNode tnRaiz;
            foreach (TreeNode tnNext in tnColection)
            {
                tnRaiz = new TreeNode(tnNext.Text);
                vSema.cloneTreeViewSintactico(tnRaiz, tnNext);
                tvThis.Nodes.Add(tnRaiz);
            }
        }
        private void createLabels()
        {
            sbEtiquetas = new StringBuilder();
            foreach (TreeNode currentNode in tvSemantico.Nodes)
            {
                readTree(currentNode);
            }
        }
        public StringBuilder getSBCodigo()
        {
            return this.sbEtiquetas;
        }
        private void readTree(TreeNode treeNode)
        {
            foreach (TreeNode currentNode in treeNode.Nodes)
            {
                if (isTokenType(currentNode))
                {

                }
            }
        }

        private Boolean isTokenType(TreeNode treeNode)
        {
            return treeNode.Text.Equals(analizador_lexico.Estados.tokenInt) || treeNode.Text.Equals(analizador_lexico.Estados.tokenFloat) || treeNode.Text.Equals(analizador_lexico.Estados.tokenBoolean);
        }

        private void declareLabel()
        {

        }
    }
}
