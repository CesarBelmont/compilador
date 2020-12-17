using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador.Codigo_Intermedio
{
    class CodigoIntermedio
    {
        private struct sHashTable
        {
            public string strVarName;
            public string strType;
            public string strValor;
            public int intHashCode;
        }
        private sHashTable[,] vHashTable;
        TreeView tvSemantico;
        TreeView tvSintactico;
        int intSiseRows = 211, intSiseCols = 5, intHashCode, intColColision;
        int intIf, intWhile, intRepeat;
        Stack<int> stackIf, stackWhile, stackRepeat;
        Stack<string> stackExpresion, stackBreakWhile, stackBreakDo;
        Semantica.aSemantico vSema;
        List<string> list_TvSemantico;
        List<string> list_TvSintactico;
        StringBuilder sbEtiquetas;
        int treePosition;
        string IF = "if", REPEAT = "do", END = "end", CIN = "cin", INT = "int", THEN = "then", ELSE = "else", COUT = "cout", FLOAT = "float", MAIN = "main",
            BREAK = "break", UNTIL = "until", WHILE = "while", BOOLEAN = "boolean", NULL = "0", IGUAL = ":=", MAYOR_Q = ">", MENOR_Q = "<", MAYOR_IQ = ">=",
            MENOR_IQ = "<=", IGUAL_I = "==", DIFERENTE = "!=", RESULTADO = "&";
        char charType;
        string strType, strIgual, strID;
        // if-repeat-end-cin-int-then-else-cout-float-main-break-until-while-boolean
        #region "Terminado"
        public CodigoIntermedio(TreeView tvSemantico, TreeView tvSintactico)
        {
            this.tvSemantico = new TreeView();
            this.tvSintactico = new TreeView();
            this.vSema = new Semantica.aSemantico(null, null);
            this.vHashTable = new sHashTable[intSiseRows, intSiseCols];
            mVoidCloneTree(tvSemantico, this.tvSemantico);
            mVoidCloneTree(tvSintactico, this.tvSintactico);
            mVoidFillLists();
            mVoidComienzaEtiquetado();
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
        private void mVoidFillLists()
        {
            list_TvSemantico = new List<string>();
            TreeNodeCollection tnColection = this.tvSemantico.Nodes;
            treePosition = 0;
            foreach (TreeNode tnNext in tnColection)
            {
                tnNext.Tag = treePosition;
                list_TvSemantico.Add(tnNext.Text);
                mVoidFillList_Semantico(tnNext);
            }
            list_TvSintactico = new List<string>();
            tnColection = this.tvSintactico.Nodes;
            treePosition = 0;
            foreach (TreeNode tnNext in tnColection)
            {
                tnNext.Tag = treePosition;
                list_TvSintactico.Add(tnNext.Text);
                mVoidFillList_Sintactico(tnNext);
            }
        }
        private void mVoidFillList_Semantico(TreeNode tnParent)
        {
            foreach (TreeNode tnNext in tnParent.Nodes)
            {
                treePosition++;
                tnNext.Tag = treePosition;
                list_TvSemantico.Add(tnNext.Text);
                mVoidFillList_Semantico(tnNext);
            }
        }
        private void mVoidFillList_Sintactico(TreeNode tnParent)
        {
            foreach (TreeNode tnNext in tnParent.Nodes)
            {
                treePosition++;
                tnNext.Tag = treePosition;
                list_TvSintactico.Add(tnNext.Text);
                mVoidFillList_Sintactico(tnNext);
            }
        }
        #endregion

        #region "Métodos para recorrer Árbol y listas al mismo tiempo! =)"
        private void mVoidComienzaEtiquetado()
        {
            sbEtiquetas = new StringBuilder();
            strType = string.Empty;
            TreeNodeCollection tnCol = this.tvSintactico.Nodes;
            this.intIf = 0;
            this.intWhile = 0;
            this.intRepeat = 0;
            this.stackIf = new Stack<int>();
            this.stackWhile = new Stack<int>();
            this.stackRepeat = new Stack<int>();
            this.stackExpresion = new Stack<string>();
            this.stackBreakWhile = new Stack<string>();
            this.stackBreakDo = new Stack<string>();
            foreach (TreeNode tnRaiz in tnCol)
            {
                foreach (TreeNode tnNext in tnRaiz.Nodes)
                {
                    if (isReservadaTipo(tnNext.Text))                   // DECLARACIONES
                    {
                        charType = getTypeChar(this.list_TvSemantico[(int)tnNext.Tag]);
                        strType = this.list_TvSintactico[(int)tnNext.Tag];
                        mVoidRecorreIDs(tnNext);    // Recorrer todos los hijos del "charType";
                    }
                    else
                    {
                        if (tnNext.Text.Equals(this.IGUAL))             // ASIGNACIÓN
                        {
                            mVoidAsignaEnCiclo(tnNext, true);
                            mVoidSetAsignaCiclo();
                        }
                        else
                        {
                            if (tnNext.Text.Equals(this.IF))
                            {
                                mVoidIfElse(tnNext);
                            }
                            else
                            {
                                if (tnNext.Text.Equals(this.REPEAT))
                                {
                                    this.stackRepeat.Push(this.intRepeat++);
                                    mVoidSetDo(CodigoIntermedioEtiqueta.strRepeat, this.stackRepeat.Peek().ToString());
                                    mVoidRepeat(tnNext);
                                }
                                else
                                {
                                    if (tnNext.Text.Equals(this.WHILE))
                                    {
                                        mVoidWhile(tnNext);
                                        mVoidSetWhileEnd(CodigoIntermedioEtiqueta.strWhileReturn, this.stackWhile.Peek().ToString(), CodigoIntermedioEtiqueta.strJump);
                                        mVoidSetWhileOut(CodigoIntermedioEtiqueta.strWhileOut, this.stackWhile.Peek().ToString());
                                        int intBreak = this.stackWhile.Pop();
                                        if (this.stackBreakWhile.Count > 0)
                                        {
                                            while (this.stackBreakWhile.Peek().IndexOf(intBreak.ToString()) != -1)
                                            {
                                                mVoidSetBreakOut(this.stackBreakWhile.Pop());
                                                if (this.stackBreakWhile.Count == 0)
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (tnNext.Text.Equals(this.BREAK))
                                        {
                                            mVoidSetBreak(CodigoIntermedioEtiqueta.strBreak, "$");
                                        }
                                        else
                                        {
                                            if (tnNext.Text.Equals(this.CIN))
                                                mVoidSetCin(tnNext);
                                            else
                                            {
                                                if (tnNext.Text.Equals(this.COUT))
                                                    mVoidGetCout(tnNext);
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
        private void mVoidRecorreIDs(TreeNode tnParent)
        {
            foreach (TreeNode tnNext in tnParent.Nodes)
            {
                mVoidSetValueId(this.list_TvSintactico[(int)tnNext.Tag], this.NULL, charType);
                intHashCode = mIntHashCode(this.list_TvSintactico[(int)tnNext.Tag]);
                vHashTable[intHashCode, 0].intHashCode = intHashCode;
                vHashTable[intHashCode, 0].strType = strType;
                vHashTable[intHashCode, 0].strValor = this.NULL;
                vHashTable[intHashCode, 0].strVarName = this.list_TvSintactico[(int)tnNext.Tag];
                mVoidRecorreIDs(tnNext);
            }
        }
        private void mVoidRecorreAsigna(TreeNode tnParent)
        {
            foreach (TreeNode tnNext in tnParent.Nodes)
            {
                mVoidRecorreAsigna(tnNext);
            }
        }
        private bool isReservadaTipo(string strTipo)
        {
            if (strTipo.Equals(this.INT) || strTipo.Equals(this.FLOAT) || strTipo.Equals(this.BOOLEAN))
                return true;
            return false;
        }
        private bool isComparador(string strOperador)
        {
            if (strOperador.Equals(this.MAYOR_IQ) || strOperador.Equals(this.MAYOR_Q) || strOperador.Equals(this.MENOR_IQ) || strOperador.Equals(this.MENOR_Q) ||
                strOperador.Equals(this.IGUAL_I) || strOperador.Equals(this.DIFERENTE))
                return true;
            return false;
        }
        private bool isOperador(string strCadena)
        {
            if (strCadena.Equals("+") || strCadena.Equals("-") || strCadena.Equals("*") || strCadena.Equals("/"))
                return true;
            return false;
        }
        private string getBoolean(string strExpresion)
        {
            strExpresion = strExpresion.ToLower();
            string strTrue = "true";
            string strFalse = "false";
            if (strExpresion.IndexOf(strTrue) != -1)
                return strTrue;
            return strFalse;
        }
        private void mVoidAsigna(TreeNode tnParent)
        {
            this.strIgual = this.list_TvSemantico[(int)tnParent.Tag];
            this.strIgual = getValue(this.strIgual);
            this.strID = tnParent.FirstNode.Text;
            this.intHashCode = mIntHashCode(this.strID);
            this.intColColision = mIntColColision(this.strID);
            this.charType = getTypeChar(vHashTable[this.intHashCode, this.intColColision].strType);
            mVoidSetValueId(this.strID, this.strIgual, this.charType);
            foreach (TreeNode tnHijo in tnParent.LastNode.Nodes)
            {
                mVoidRecorreAsigna(tnHijo);
            }
        }
        private void mVoidAsignaEnCiclo(TreeNode tnParent, bool flag)
        {
            if (flag)
            {
                this.strID = tnParent.FirstNode.Text;                                                       // ID
                this.intHashCode = mIntHashCode(this.strID);
                this.intColColision = mIntColColision(this.strID);
                this.charType = getTypeChar(vHashTable[this.intHashCode, this.intColColision].strType);     // Tipo de dato
                mVoidSetValueId(this.strID, this.RESULTADO, this.charType);
            }
            foreach (TreeNode tnNext in tnParent.Nodes)
            {
                stackExpresion.Push(tnNext.Text);
                mVoidAsignaEnCiclo(tnNext, false);
            }
        }
        private void mVoidIfElse(TreeNode tnIfElse)
        {
            foreach (TreeNode tnNext in tnIfElse.Nodes)
            {
                if (isComparador(tnNext.Text))          // Parte de la comparación
                {
                    int currentPosition = (int)tnNext.Tag;
                    this.sbEtiquetas.Append("LD " + this.list_TvSintactico[currentPosition + 1]);
                    this.sbEtiquetas.AppendLine("");
                    this.sbEtiquetas.Append("LDC " + this.list_TvSintactico[currentPosition + 2]);
                    this.sbEtiquetas.AppendLine("");
                    switch (this.list_TvSintactico[currentPosition])
                    {
                        case "==":
                            this.sbEtiquetas.Append("JEQ");
                            break;
                        case "!=":
                            this.sbEtiquetas.Append("JNE");
                            break;
                        case "<":
                            this.sbEtiquetas.Append("JLT");
                            break;
                        case ">":
                            this.sbEtiquetas.Append("JGT");
                            break;
                        case "<=":
                            this.sbEtiquetas.Append("JLE");
                            break;
                        case ">=":
                            this.sbEtiquetas.Append("JGE");
                            break;

                    }
                    this.sbEtiquetas.AppendLine("");
                    mVoidSetBoolean(CodigoIntermedioEtiqueta.strIf, this.intIf++, getBoolean(this.list_TvSemantico[currentPosition]) + " " +
                        this.list_TvSintactico[currentPosition + 1] + this.list_TvSintactico[currentPosition] +  this.list_TvSintactico[currentPosition + 2]);
                    //sbEtiquetas.Append(this.list_TvSintactico[currentPosition] + "\r"); // == <= >= < >
                    this.stackIf.Push(this.intIf - 1);
                    this.sbEtiquetas.Append(",\n");

                }
                else
                {
                    if (tnNext.Text.Equals(this.IF))                    // If Anidado
                        mVoidIfElse(tnNext);
                    else
                    {
                        if (tnNext.Text.Equals(this.REPEAT))                // Do Anidado
                        {
                            this.stackRepeat.Push(this.intRepeat++);
                            mVoidSetDo(CodigoIntermedioEtiqueta.strRepeat, this.stackRepeat.Peek().ToString());
                            mVoidRepeat(tnNext);
                        }
                        else
                        {
                            if (tnNext.Text.Equals(this.WHILE))         // While Anidado
                            {
                                mVoidWhile(tnNext);
                                mVoidSetWhileEnd(CodigoIntermedioEtiqueta.strWhileReturn, this.stackWhile.Peek().ToString(), CodigoIntermedioEtiqueta.strJump);
                                mVoidSetWhileOut(CodigoIntermedioEtiqueta.strWhileOut, this.stackWhile.Peek().ToString());
                                int intBreak = this.stackWhile.Pop();
                                if (this.stackBreakWhile.Count > 0)
                                {
                                    while (this.stackBreakWhile.Peek().IndexOf(intBreak.ToString()) != -1)
                                    {
                                        mVoidSetBreakOut(this.stackBreakWhile.Pop());
                                        if (this.stackBreakWhile.Count == 0)
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (tnNext.Text.Equals(this.IGUAL))     // Asignación Anidada
                                {
                                    mVoidAsignaEnCiclo(tnNext, true);
                                    mVoidSetAsignaCiclo();
                                }
                                else
                                {
                                    if (tnNext.Text.Equals(this.BREAK))
                                        mVoidSetBreak(CodigoIntermedioEtiqueta.strBreak, "$");
                                    else
                                    {
                                        if (tnNext.Text.Equals(this.ELSE))
                                        {
                                            mVoidSetBreak(CodigoIntermedioEtiqueta.strElse, this.stackIf.Peek().ToString());
                                            mVoidIfElse(tnNext);
                                        }
                                        else
                                        {
                                            if (tnNext.Text.Equals(this.END))
                                                mVoidSetBreak(CodigoIntermedioEtiqueta.strEnd, this.stackIf.Pop().ToString());
                                            else
                                            {
                                                if (tnNext.Text.Equals(this.CIN))
                                                {
                                                    mVoidSetCin(tnNext);
                                                }
                                                else
                                                {
                                                    if (tnNext.Text.Equals(this.COUT))
                                                    {
                                                        mVoidGetCout(tnNext);
                                                    }
                                                    else if (tnNext.Text.Equals(this.THEN))
                                                    {
                                                        mVoidIfElse(tnNext);
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
            }
        }
        private void mVoidRepeat(TreeNode tnParent)
        {
            foreach (TreeNode tnNext in tnParent.Nodes)
            {
                if (isComparador(tnNext.Text))
                {
                    int currentPosition = (int)tnNext.Tag;
                    String comparationResult = getBoolean(this.list_TvSemantico[currentPosition]);
                    String labelResult = comparationResult + " " + this.list_TvSintactico[currentPosition + 1] + " " + this.list_TvSintactico[currentPosition]
                   + " " + this.list_TvSintactico[currentPosition + 2];
                    mVoidSetBoolean(CodigoIntermedioEtiqueta.strUntil, this.stackRepeat.Peek(), labelResult);
                    int intBreak = this.stackRepeat.Pop();
                    if (this.stackBreakDo.Count > 0)
                    {
                        while (this.stackBreakDo.Peek().IndexOf(intBreak.ToString()) != -1)
                        {
                            mVoidSetBreakOut(this.stackBreakDo.Pop());
                            if (this.stackBreakDo.Count == 0)
                                break;
                        }
                    }
                }
                else
                {
                    if (tnNext.Text.Equals(this.IF))                    // If Anidado
                        mVoidIfElse(tnNext);
                    else
                    {
                        if (tnNext.Text.Equals(this.REPEAT))                // Do Anidado
                        {
                            this.stackRepeat.Push(this.intRepeat++);
                            mVoidSetDo(CodigoIntermedioEtiqueta.strRepeat, this.stackRepeat.Peek().ToString());
                            mVoidRepeat(tnNext);
                        }
                        else
                        {
                            if (tnNext.Text.Equals(this.WHILE))         // While Anidado
                            {
                                mVoidWhile(tnNext);
                                mVoidSetWhileEnd(CodigoIntermedioEtiqueta.strWhileReturn, this.stackWhile.Peek().ToString(), CodigoIntermedioEtiqueta.strJump);
                                mVoidSetWhileOut(CodigoIntermedioEtiqueta.strWhileOut, this.stackWhile.Peek().ToString());
                                int intBreak = this.stackWhile.Pop();
                                if (this.stackBreakWhile.Count > 0)
                                {
                                    while (this.stackBreakWhile.Peek().IndexOf(intBreak.ToString()) != -1)
                                    {
                                        mVoidSetBreakOut(this.stackBreakWhile.Pop());
                                        if (this.stackBreakWhile.Count == 0)
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (tnNext.Text.Equals(this.IGUAL))     // Asignación Anidada
                                {
                                    mVoidAsignaEnCiclo(tnNext, true);
                                    mVoidSetAsignaCiclo();
                                }
                                else
                                {
                                    if (tnNext.Text.Equals(this.BREAK))
                                    {
                                        this.stackBreakDo.Push(CodigoIntermedioEtiqueta.strBreak + this.stackRepeat.Peek().ToString() + " " +
                                            CodigoIntermedioEtiqueta.strRepeat.Substring(0, CodigoIntermedioEtiqueta.strRepeat.IndexOf("_")));
                                        mVoidSetBreak(CodigoIntermedioEtiqueta.strBreak, this.stackRepeat.Peek().ToString() + " " +
                                            CodigoIntermedioEtiqueta.strRepeat.Substring(0, CodigoIntermedioEtiqueta.strRepeat.IndexOf("_")));
                                    }
                                    else
                                    {
                                        if (tnNext.Text.Equals(this.UNTIL))
                                            mVoidUntil(tnNext);
                                        else
                                        {
                                            if (tnNext.Text.Equals(this.CIN))
                                            {
                                                mVoidSetCin(tnNext);
                                            }
                                            else
                                            {
                                                if (tnNext.Text.Equals(this.COUT))
                                                {
                                                    mVoidGetCout(tnNext);
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
        }

        private void mVoidUntil(TreeNode treeNode)
        {
            String until = treeNode.Text;
            foreach (TreeNode currentNode in treeNode.Nodes)
            {
                if (isComparador(currentNode.Text))
                {
                    int currentPosition = (int)currentNode.Tag;
                    String comparationResult = getBoolean(this.list_TvSemantico[currentPosition]);
                    String labelResult = comparationResult + " " + this.list_TvSintactico[currentPosition + 1] + " " + this.list_TvSintactico[currentPosition]
                    + " " + this.list_TvSintactico[currentPosition + 2];
                    mVoidSetBoolean(CodigoIntermedioEtiqueta.strUntil, this.intWhile++, labelResult);
                    this.stackWhile.Push(this.intWhile - 1);
                    this.sbEtiquetas.Append(",\n");

                }
            }
        }

        private void mVoidWhile(TreeNode tnParent)
        {
            foreach (TreeNode tnNext in tnParent.Nodes)
            {
                if (isComparador(tnNext.Text))
                {


                    int currentPosition = (int)tnNext.Tag;
                    String comparationResult = getBoolean(this.list_TvSemantico[currentPosition]);
                    String labelResult = comparationResult + " " + this.list_TvSintactico[currentPosition + 1] + " " + this.list_TvSintactico[currentPosition]
                     + " " + this.list_TvSintactico[currentPosition + 2];
                    mVoidSetBoolean(CodigoIntermedioEtiqueta.strWhile, this.intWhile++, labelResult);             
                    this.stackWhile.Push(this.intWhile - 1);
                    this.sbEtiquetas.Append(",\n");
                }
                else
                {
                    if (tnNext.Text.Equals(this.IF))                    // If Anidado
                        mVoidIfElse(tnNext);
                    else
                    {
                        if (tnNext.Text.Equals(this.REPEAT))                // Do Anidado
                        {
                            this.stackRepeat.Push(this.intRepeat++);
                            mVoidSetDo(CodigoIntermedioEtiqueta.strRepeat, this.stackRepeat.Peek().ToString());
                            mVoidRepeat(tnNext);
                        }
                        else
                        {
                            if (tnNext.Text.Equals(this.WHILE))         // While Anidado
                            {
                                mVoidWhile(tnNext);
                                mVoidSetWhileEnd(CodigoIntermedioEtiqueta.strWhileReturn, this.stackWhile.Peek().ToString(), CodigoIntermedioEtiqueta.strJump);
                                mVoidSetWhileOut(CodigoIntermedioEtiqueta.strWhileOut, this.stackWhile.Peek().ToString());
                                int intBreak = this.stackWhile.Pop();
                                if (this.stackBreakWhile.Count > 0)
                                {
                                    while (this.stackBreakWhile.Peek().IndexOf(intBreak.ToString()) != -1)
                                    {
                                        mVoidSetBreakOut(this.stackBreakWhile.Pop());
                                        if (this.stackBreakWhile.Count == 0)
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (tnNext.Text.Equals(this.IGUAL))     // Asignación Anidada
                                {
                                    mVoidAsignaEnCiclo(tnNext, true);
                                    mVoidSetAsignaCiclo();
                                }
                                else
                                {
                                    if (tnNext.Text.Equals(this.BREAK))
                                    {
                                        this.stackBreakWhile.Push(CodigoIntermedioEtiqueta.strBreak + this.stackWhile.Peek().ToString() + " " +
                                            CodigoIntermedioEtiqueta.strWhile.Substring(0, CodigoIntermedioEtiqueta.strWhile.IndexOf("_")));
                                        mVoidSetBreak(CodigoIntermedioEtiqueta.strBreak, this.stackWhile.Peek().ToString() + " " +
                                            CodigoIntermedioEtiqueta.strWhile.Substring(0, CodigoIntermedioEtiqueta.strWhile.IndexOf("_")));
                                    }
                                    else
                                    {
                                        if (tnNext.Text.Equals(this.CIN))
                                        {
                                            mVoidSetCin(tnNext);
                                        }
                                        else
                                        {
                                            if (tnNext.Text.Equals(this.COUT))
                                            {
                                                mVoidGetCout(tnNext);
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
        private void mVoidSetAsignaCiclo()
        {
            Stack<string> stackRes = new Stack<string>();
            string strIzq = string.Empty, strDer = string.Empty;
            string strOp;
            while (this.stackExpresion.Count > 1)
            {
                if (isOperador(this.stackExpresion.Peek()))
                {
                    if (stackRes.Count > 0)
                        strIzq = stackRes.Pop();
                    if (stackRes.Count > 0)
                        strDer = stackRes.Pop();
                    strOp = this.stackExpresion.Pop();
                    if (strIzq != null)
                    {
                        if (isNumber(strIzq))
                            mVoidGetValueConst(charType, strIzq);
                        else
                            mVoidGetValueId(charType, strIzq);
                    }
                    if (strDer != null)
                    {
                        if (isNumber(strDer))
                            mVoidGetValueConst(charType, strDer);
                        else
                            mVoidGetValueId(charType, strDer);
                    }
                    strIzq = null;
                    strDer = null;
                    mVoidSetOperator(charType, Char.Parse(strOp));
                    stackRes.Push("%");
                }
                else
                    stackRes.Push(this.stackExpresion.Pop());
            }
            while (this.stackExpresion.Count != 0)
                this.stackExpresion.Pop();
            if (stackRes.Count > 0)
            {
                strIzq = stackRes.Pop();
                if (isNumber(strIzq))
                    mVoidGetValueConst(charType, strIzq);
                else
                    mVoidGetValueId(charType, strIzq);
            }
            this.sbEtiquetas.Append(CodigoIntermedioEtiqueta.strFin);
            this.sbEtiquetas.AppendLine();
        }
        public bool isNumber(string strCadena)
        {
            char[] charCadena = strCadena.ToCharArray();
            for (int i = 0; i < charCadena.Length; i++)
            {
                if (Char.IsLetter(charCadena[i]))
                    return false;
            }
            return true;
        }
        #endregion // Termina "Métodos para recorrer Árbol y listas al mismo tiempo! =)"

        private char getTypeChar(string strType)
        {
            if (strType.Equals(this.INT))
                return 'i';
            else
            {
                if (strType.Equals(this.FLOAT))
                    return 'r';
                else
                {
                    if (strType.Equals(this.BOOLEAN))
                        return 'b';
                    else
                        return 'e';
                }
            }
        }
        private void mVoidSetValueId(string id, string strValue, char charType)
        {
            switch (charType)
            {
                case 'b':
                    sbEtiquetas.Append(CodigoIntermedioEtiqueta.strSetId_Boolean + " " + strValue + " " + id);
                    break;
                case 'i':
                    sbEtiquetas.Append(CodigoIntermedioEtiqueta.strSetId_Int + " " + strValue + " " + id);
                    break;
                case 'r':
                    sbEtiquetas.Append(CodigoIntermedioEtiqueta.strSetId_Float + " " + strValue + " " + id);
                    break;

            }
            sbEtiquetas.AppendLine();
        }
        //La intocable
        private void mVoidSetBoolean(string strLabel, int pos, string res)
        {
            sbEtiquetas.Append(strLabel + pos + " " + res ); //res = true|false Operacion
            sbEtiquetas.AppendLine();
        }
        private void mVoidSetBreak(string strLabel, string pos)
        {
            sbEtiquetas.Append(strLabel + pos);
            sbEtiquetas.AppendLine();
        }
        private void mVoidSetBreakOut(string strLabel)
        {
            sbEtiquetas.Append(strLabel);
            sbEtiquetas.AppendLine();
        }
        private void mVoidSetWhileEnd(string strLabel, string pos, string jump)
        {
            sbEtiquetas.Append(strLabel + pos + " " + jump);
            sbEtiquetas.AppendLine();
        }
        private void mVoidSetWhileOut(string strLabel, string pos)
        {
            sbEtiquetas.Append(strLabel + pos);
            sbEtiquetas.AppendLine();
        }
        private void mVoidSetDo(string strLabel, string pos)
        {
            sbEtiquetas.Append(strLabel + pos);
            sbEtiquetas.AppendLine();
        }
        private void mVoidSetUntil(string strLabel, string pos)
        {
            sbEtiquetas.Append(strLabel + pos);
            sbEtiquetas.AppendLine();
        }
        private void mVoidGetValueId(char charType, string strId)
        {
            switch (charType)
            {
                case 'b':
                    this.sbEtiquetas.Append(CodigoIntermedioEtiqueta.strGetId_Boolean + " " + strId);
                    break;
                case 'i':
                    this.sbEtiquetas.Append(CodigoIntermedioEtiqueta.strGetId_Int + " " + strId);
                    break;
                case 'r':
                    this.sbEtiquetas.Append(CodigoIntermedioEtiqueta.strGetId_Float + " " + strId);
                    break;

            }
            this.sbEtiquetas.AppendLine();
        }
        private void mVoidGetValueConst(char charType, string strVal)
        {
            switch (charType)
            {
                case 'b':
                    this.sbEtiquetas.Append(CodigoIntermedioEtiqueta.strGetConst_Boolean + " " + strVal);
                    break;
                case 'i':
                    this.sbEtiquetas.Append(CodigoIntermedioEtiqueta.strGetConst_Int + " " + strVal);
                    break;
                case 'r':
                    this.sbEtiquetas.Append(CodigoIntermedioEtiqueta.strGetConst_Float + " " + strVal);
                    break;

            }
            this.sbEtiquetas.AppendLine();
        }
        private void mVoidSetOperator(char charType, char charOperador)
        {
            string strOperador = string.Empty;
            switch (charOperador)
            {
                case '+':
                    strOperador = CodigoIntermedioEtiqueta.strSuma;
                    break;
                case '-':
                    strOperador = CodigoIntermedioEtiqueta.strResta;
                    break;
                case '*':
                    strOperador = CodigoIntermedioEtiqueta.strMult;
                    break;
                case '/':
                    strOperador = CodigoIntermedioEtiqueta.strDivision;
                    break;
            }
            switch (charType)
            {
                case 'b':
                    this.sbEtiquetas.Append(strOperador + CodigoIntermedioEtiqueta.strBoolean);
                    break;
                case 'i':
                    this.sbEtiquetas.Append(strOperador + CodigoIntermedioEtiqueta.strInt);
                    break;
                case 'r':
                    this.sbEtiquetas.Append(strOperador + CodigoIntermedioEtiqueta.strFloat);
                    break;

            }
            this.sbEtiquetas.AppendLine();
        }
        private void mVoidSetCin(TreeNode tnParent)
        {
            this.strID = tnParent.FirstNode.Text;
            this.intHashCode = mIntHashCode(this.strID);
            this.intColColision = mIntColColision(this.strID);
            this.charType = getTypeChar(vHashTable[this.intHashCode, this.intColColision].strType);
            this.sbEtiquetas.Append(CodigoIntermedioEtiqueta.strCin + getType() + " " + this.strID);
            this.sbEtiquetas.AppendLine();
        }
        private void mVoidGetCout(TreeNode tnParent)
        {
            this.strID = tnParent.FirstNode.Text;
            this.intHashCode = mIntHashCode(this.strID);
            this.intColColision = mIntColColision(this.strID);
            this.charType = getTypeChar(vHashTable[this.intHashCode, this.intColColision].strType);
            this.sbEtiquetas.Append(CodigoIntermedioEtiqueta.strCout + getType() + " " + this.strID);
            this.sbEtiquetas.AppendLine();
        }
        public StringBuilder getSBCodigo()
        {
            return this.sbEtiquetas;
        }
        private int mIntHashCode(string id)
        {
            char[] charID = id.ToCharArray();
            int intHashCode = 0;
            for (int i = 0; i < charID.Length; i++)
                intHashCode = ((intHashCode << 4) + charID[i]) % intSiseRows;
            return intHashCode;
        }
        private int mIntColColision(string id)
        {
            int intHashCode = mIntHashCode(id);
            for (int i = 0; i < intSiseCols; i++)
            {
                if (vHashTable[intHashCode, i].strVarName.Equals(id))
                    return i;
            }
            return -1;
        }
        private string getValue(string strIgual)
        {
            try
            {
                string strAux;
                strAux = strIgual.Substring(strIgual.IndexOf("["));
                strAux = strAux.Substring(strAux.IndexOf("=") + 2);
                strAux = strAux.Substring(0, strAux.IndexOf(" "));
                strAux = strAux.Trim();
                return strAux;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        private string getType()
        {
            switch (charType)
            {
                case 'b':
                    return CodigoIntermedioEtiqueta.strBoolean;
                case 'i':
                    return CodigoIntermedioEtiqueta.strInt;
                case 'r':
                    return CodigoIntermedioEtiqueta.strFloat;
            }
            return string.Empty;
        }
    }
}
