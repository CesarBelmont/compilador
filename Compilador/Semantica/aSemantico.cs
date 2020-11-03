using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador.Semantica
{
    class aSemantico
    {
        public const String NULL = "Null";
        public const String ERROR = "Error";
        private TreeView treeSemantico;
        private TreeView treeViewSintactico;
        private TreeNode nodeSem;
        private DataTable dataTable;
        private List<analizador_lexico.Token> tokensSem;
        private List<String> tokenDescription;
        private HashTableObject[,] vHashTable;
        private HashTableObject vSubHashTable;
        private StringBuilder sbErrors;
        private int location;
        private static int intSizeRows = 211;
        private static int intSizeCols = 4;
        public const int TokenErrorTipoAsignacion = 1;
        public const int TokenErrorTipoDeclaracion = 2;
        public const int TokenErrorNoInt = 3;
        public const int TokenErrorTiposdeDatosDif = 4;
        public const int TokenErrorYaDeclarado = 5;
        public const int TokenErrorNulo = 6;
        private struct HashTableObject
        {
            public int hashCode;
            public String idName;
            public String value;
            public String type;
            public String lineNumber;
            public int location;
        }
        private struct TreeNodeObject
        {
            public String label;
            public String type;
            public String value;
            public int tokenDescriptor;
        }
        private int hashCode;
        private int column;
        private int auxColumn;
        private int intCountCompare;
        private int currentPosition;
        private int intCountAsign;
        private int intCountType;
        private TreeNodeObject[] vTreeNodeToken;
        private TreeNodeObject[] vTreeNodeCin;
        private TreeNodeObject[] vTreeNodeCout;
        private List<TreeNode> listTreeNodeToken;
        private List<int> listPositionToken;
        private List<TreeNode> listTreeNodeComparation;
        private List<int> listPositionComparation;
        private List<TreeNode> listTreeNodeCin;
        private List<int> listPositionCin;
        private List<TreeNode> listTreeNodeCout;
        private List<int> listPositionCout;
        private int HASH_COLUMN_ZERO = 0;
        private Stack<String> stackOperation;
        private Stack<String> stackExpression;
        private Stack<String> stackResults;
        private Stack<int> stackPositions;
        private List<String> listComparations;

        #region "Constructor"
        public aSemantico(List<analizador_lexico.Token> tokens, List<String> clase)
        {

            this.tokensSem = tokens;
            this.tokenDescription = clase;
            sbErrors = new StringBuilder();
            location = 0;
            vHashTable = new HashTableObject[intSizeRows, intSizeCols];
        }
        #endregion

        #region "Metodos Terminados"
        private int getHashCode(string id)
        {
            char[] charID = id.ToCharArray();
            int intHashCode = 0;
            for (int i = 0; i < charID.Length; i++)
                intHashCode = ((intHashCode << 4) + charID[i]) % intSizeRows;
            return intHashCode;
        }
        private int getCollisionColumn(string id)
        {
            int intHashCode = getHashCode(id);
            for (int i = 0; i < intSizeCols; i++)
            {
                if (vHashTable[intHashCode, i].idName == null || vHashTable[intHashCode, i].idName.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        private bool isVarType(TreeNode node)
        {
            return node.Text.Equals(analizador_lexico.Estados.tokenInt) || node.Text.Equals(analizador_lexico.Estados.tokenFloat) || node.Text.Equals(analizador_lexico.Estados.tokenBoolean);
        }
        private bool isAsignType(TreeNode node)
        {
            if (node.Text.Equals(analizador_lexico.Estados.tokenAsignacion))
            {
                intCountCompare++;
                return true;
            }
            return false;
        }
        private bool isComparationType(TreeNode node)
        {
            return node.Text.Equals(analizador_lexico.Estados.tokenMenor) || node.Text.Equals(analizador_lexico.Estados.tokenMenorIgual) || node.Text.Equals(analizador_lexico.Estados.tokenMayorIgual)
                || node.Text.Equals(analizador_lexico.Estados.tokenMayor) || node.Text.Equals(analizador_lexico.Estados.tokenIgualdad) || node.Text.Equals(analizador_lexico.Estados.tokenDiferente);
        }
        private bool isCoutType(TreeNode node)
        {
            return node.Text.Equals(analizador_lexico.Estados.tokenCout);
        }
        private bool isCinType(TreeNode node)
        {
            return node.Text.Equals(analizador_lexico.Estados.tokenCin);
        }
        private void fillList(TreeNode currentNode)
        {
            foreach (TreeNode node in currentNode.Nodes)
            {
                location++;
                if (isVarType(node) || isAsignType(node))
                {
                    currentPosition++;
                    listTreeNodeToken.Add(node);
                    listPositionToken.Add(currentPosition);
                }
                else if (isComparationType(node))
                {
                    currentPosition++;
                    listTreeNodeComparation.Add(node);
                    listPositionComparation.Add(currentPosition);
                }
                else if (isCoutType(node))
                {
                    currentPosition++;
                    listTreeNodeCout.Add(node);
                    listPositionCout.Add(currentPosition);
                }
                else if (isCinType(node))
                {
                    currentPosition++;
                    listTreeNodeCin.Add(node);
                    listPositionCin.Add(currentPosition);
                }
                else
                {
                    fillList(node);
                }
            }
        }
        private void readTree()
        {
            listTreeNodeToken = new List<TreeNode>();
            listPositionToken = new List<int>();
            listTreeNodeComparation = new List<TreeNode>();
            listPositionComparation = new List<int>();
            listTreeNodeCin = new List<TreeNode>();
            listPositionCin = new List<int>();
            listTreeNodeCout = new List<TreeNode>();
            listPositionCout = new List<int>();
            TreeNodeCollection collectionNodes = treeViewSintactico.Nodes;
            foreach (TreeNode currentNode in collectionNodes)
            {

                fillList(currentNode);
            }
            String[] tokensToRemove = new String[tokenDescription.Count];
            for (int i = 0; i < tokenDescription.Count; i++)
            {
                if (isTokenToRemove(tokenDescription[i]))
                {
                    if (!tokenDescription[i].Equals(analizador_lexico.Estados.numeroEnteroSS) && !tokenDescription[i].Equals(analizador_lexico.Estados.numeroEnteroCS)
                        && !tokenDescription[i].Equals(analizador_lexico.Estados.numeroFlotanteCS) && !tokenDescription[i].Equals(analizador_lexico.Estados.numeroFlotanteSS)
                        && !tokensSem[i].getName().Equals(analizador_lexico.Estados.tokenInt) && !tokensSem[i].getName().Equals(analizador_lexico.Estados.tokenFloat) && !tokensSem[i].getName().Equals(analizador_lexico.Estados.tokenBoolean))
                    {
                        tokensToRemove[i] = tokenDescription[i];
                    }

                }
            }
            List<analizador_lexico.Token> listTokenAux = new List<analizador_lexico.Token>();
            List<String> listDescriptionAux = new List<string>();
            for (int i = 0; i < tokensToRemove.Length; i++)
            {
                if (tokensToRemove[i] == null)
                {
                    listTokenAux.Add(tokensSem[i]);
                    listDescriptionAux.Add(tokenDescription[i]);
                }

            }
            tokensSem = listTokenAux;
            tokenDescription = listDescriptionAux;
        }
        private bool isTokenToRemove(String tokenDescription)
        {
            return !tokenDescription.Equals(analizador_lexico.Estados.identificador) && !tokenDescription.Equals(analizador_lexico.Estados.asignacion) && !tokenDescription.Equals(analizador_lexico.Estados.igualdad)
                  && !tokenDescription.Equals(analizador_lexico.Estados.diferente) && !tokenDescription.Equals(analizador_lexico.Estados.menorQue) && !tokenDescription.Equals(analizador_lexico.Estados.menorIgual)
                  && !tokenDescription.Equals(analizador_lexico.Estados.mayorQue) && !tokenDescription.Equals(analizador_lexico.Estados.mayorIgual) && !tokenDescription.Equals(analizador_lexico.Estados.tokenInt)
                  && !tokenDescription.Equals(analizador_lexico.Estados.tokenFloat) && !tokenDescription.Equals(analizador_lexico.Estados.tokenFloat) && !tokenDescription.Equals(analizador_lexico.Estados.mas) && !tokenDescription.Equals(analizador_lexico.Estados.menos)
                  && !tokenDescription.Equals(analizador_lexico.Estados.multiplicacion) && !tokenDescription.Equals(analizador_lexico.Estados.division) && !tokenDescription.Equals(analizador_lexico.Estados.puntoComa);
        }
        private bool isId(String token)
        {
            return token.Equals(analizador_lexico.Estados.identificador);
        }
        private string getError(int tokenErrorType, analizador_lexico.Token token)
        {
            switch (tokenErrorType)
            {
                case TokenErrorYaDeclarado:
                    return token.getName().ToUpper() + " Esto ya existe";
                case TokenErrorTipoAsignacion:
                    return "Esos datos son diferentes, no se puede eso";
                case TokenErrorTiposdeDatosDif:
                    return "Los datos deben ser iguales en comparaciones";
                case TokenErrorNoInt:
                    return "Se debe inicializar " + token.getName().ToUpper();
                case TokenErrorNulo:
                    return "Como puede existir algo Nulo?";
                case TokenErrorTipoDeclaracion:
                    return "Primero se debe declarar " + token.getName().ToUpper();
            }
            return String.Empty;
        }
        private void setErrors(analizador_lexico.Token token, int tokenErrorType, int from, String tokenDescription)
        {
            if (!isId(tokenDescription))
            {
                return;
            }
            switch (from)
            {
                case 0:
                    sbErrors.Append("Error: " + getError(tokenErrorType, token));
                    sbErrors.Append(" [Fila: " + token.getLocation().getFila() + ", columna: " + token.getLocation().getColumna() + " ]");
                    sbErrors.AppendLine();
                    break;
                case 1:
                    sbErrors.Append("Error: " + getError(tokenErrorType, token));
                    sbErrors.AppendLine();
                    break;
            }
        }
        private void fillHashTable()
        {
            location = 0;
            int row;
            Boolean flag = true;
            for (int i = 0; i < tokensSem.Count; i++)
            {
                row = i;
                if (isId(tokenDescription[i]))
                {
                    hashCode = getHashCode(tokensSem[i].getName());
                    if (vHashTable[hashCode, HASH_COLUMN_ZERO].idName == null)
                    {
                        HashTableObject newId = new HashTableObject();
                        newId.hashCode = hashCode;
                        newId.idName = tokensSem[i].getName();

                        try
                        {
                            newId.type = getTypeByRC(i, HASH_COLUMN_ZERO);
                            if (newId.type.Equals(analizador_lexico.Estados.tokenInt))
                            {
                                newId.value = "0";
                            }
                            else if (newId.type.Equals(analizador_lexico.Estados.tokenFloat))
                            {
                                newId.value = "0.00";
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            newId.type = null;
                            //TODO AGREGAR ERROR DE DECLARACION
                            setErrors(tokensSem[i], TokenErrorTipoDeclaracion, 0, tokenDescription[i]);
                        }
                        newId.lineNumber = tokensSem[i].getLocation().ToString();
                        newId.location = location;

                        vHashTable[hashCode, HASH_COLUMN_ZERO] = newId;
                    }
                    else if (vHashTable[hashCode, HASH_COLUMN_ZERO].idName.Equals(tokensSem[i].getName()))
                    {
                        vHashTable[hashCode, HASH_COLUMN_ZERO].lineNumber += "- " + tokensSem[i].getLocation().ToString();
                        if (vHashTable[hashCode, HASH_COLUMN_ZERO].type == null)
                        {
                            //TODO AGREGAR ERROR DE DECLARACION
                            while (row >= 0 && !tokensSem[row].getName().Equals(analizador_lexico.Estados.tokenInt) && !tokensSem[row].getName().Equals(analizador_lexico.Estados.tokenFloat) && !tokensSem[row].getName().Equals(analizador_lexico.Estados.tokenBoolean))
                            {
                                if (tokensSem[row].getName().Equals(analizador_lexico.Estados.tokenPuntoComa))
                                {
                                    setErrors(tokensSem[i], TokenErrorTipoDeclaracion, 0, tokenDescription[i]);
                                    flag = false;
                                }
                                row--;
                            }
                            if (flag)
                            {
                                vHashTable[hashCode, HASH_COLUMN_ZERO].type = tokensSem[i].getName();
                            }
                            //setErrors(tokensSem[i], TOKEN_ERROR_TYPE_DECLARATION, 0, tokenDescription[i]);

                        }
                        else
                        {
                            int c = i;
                            Boolean isPuntoComa = false;
                            while (!tokensSem[c].getName().Equals(analizador_lexico.Estados.tokenPuntoComa))
                            {
                                if (tokensSem[c].getName().Equals(analizador_lexico.Estados.tokenInt) || tokensSem[c].getName().Equals(analizador_lexico.Estados.tokenFloat) || tokensSem[c].getName().Equals(analizador_lexico.Estados.tokenBoolean))
                                {
                                    isPuntoComa = true;
                                    break;
                                }
                                c--;
                            }
                            if (isPuntoComa)
                            {
                                setErrors(tokensSem[row], TokenErrorYaDeclarado, 0, tokenDescription[i]);
                            }
                        }

                    }
                    else
                    {
                        //TODO RESOLVER COLICIONES
                        fixCollitions(hashCode, tokensSem[i], i);
                    }
                }
            }
        }
        private void fixCollitions(int hashCode, analizador_lexico.Token token, int position)
        {
            for (int i = 1; i < intSizeCols; i++)
            {
                try
                {
                    if (vHashTable[hashCode, i].idName == null)
                    {

                        vSubHashTable = new HashTableObject();
                        vSubHashTable.hashCode = hashCode;
                        vSubHashTable.idName = token.getName();
                        try
                        {
                            vSubHashTable.type = getTypeByRC(position, i - 1);
                            if (vSubHashTable.type.Equals(analizador_lexico.Estados.tokenInt))
                            {
                                vSubHashTable.value = "0";
                            }
                            else if (vSubHashTable.type.Equals(analizador_lexico.Estados.tokenFloat))
                            {
                                vSubHashTable.value = "0.00";
                            }
                        }
                        catch (Exception e)
                        {
                            vSubHashTable.type = null;
                            //TODO AGREGAR ERROR DE DECLARACION
                            setErrors(tokensSem[position], TokenErrorTipoDeclaracion, 0, tokenDescription[position]);
                        }
                        vSubHashTable.lineNumber = token.getLocation().ToString();
                        vSubHashTable.location = location;
                        location++;
                        vHashTable[hashCode, i] = vSubHashTable;
                        break;
                    }
                    else if (vHashTable[hashCode, i].idName.Equals(token.getName()))
                    {
                        vHashTable[hashCode, i].lineNumber = "- " + token.getLocation().ToString();
                        if (vHashTable[hashCode, i].type == null)
                        {
                            setErrors(token, TokenErrorNoInt, 0, tokenDescription[position]);
                            break;
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
        private void readId(TreeNode node)
        {
            foreach (TreeNode currentNode in node.Nodes)
            {
                intCountType++;
                readId(currentNode);
            }
        }
        private void readAsign(TreeNode node)
        {
            foreach (TreeNode currentNode in node.Nodes)
            {
                intCountAsign++;
                readAsign(currentNode);
            }
        }
        private void fillDeclaration(TreeNode node)
        {
            foreach (TreeNode currentNode in node.Nodes)
            {
                currentPosition++;
                vTreeNodeToken[currentPosition].label = currentNode.Text;
                vTreeNodeToken[currentPosition].tokenDescriptor = (int)currentNode.Tag;
                fillDeclaration(currentNode);
            }
        }
        private void fillAsign(TreeNode node, String type)
        {
            foreach (TreeNode currentNode in node.Nodes)
            {
                currentPosition++;
                vTreeNodeToken[currentPosition].label = currentNode.Text;
                vTreeNodeToken[currentPosition].tokenDescriptor = (int)currentNode.Tag;
                if (!isOperator(currentNode.Text) && !isNumber(currentNode.Text))
                {
                    vTreeNodeToken[currentPosition].type = type;
                }
                fillAsign(currentNode, type);
            }
        }
        public Boolean isOperator(String token)
        {
            return token.Equals(analizador_lexico.Estados.tokenMas) || token.Equals(analizador_lexico.Estados.tokenMenos) || token.Equals(analizador_lexico.Estados.tokenMultiplica) || token.Equals(analizador_lexico.Estados.tokenDivision);
        }
        public Boolean isNumber(String token)
        {
            char[] tokenChar = token.ToCharArray();
            for (int i = 0; i < tokenChar.Length; i++)
            {
                if (Char.IsLetter(tokenChar[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public void fillTreeNodeToken()
        {
            currentPosition = 0;
            String type = String.Empty;
            String id = String.Empty;
            int globalPosition = 1;
            int comparePosition = 0;
            int tokenPosition = 0;
            int coutPosition = 0;
            Boolean flagComparation = false;
            Boolean flagToken = false;
            Boolean flagCout = false;
            while (!flagComparation || !flagToken || !flagCout)
            {
                if (listPositionCout.Count > 0)
                {
                    flagCout = true;
                }
                else
                {
                    flagCout = true;
                }
                if (listPositionComparation.Count > 0)
                {
                    if (globalPosition == listPositionComparation[comparePosition])
                    {
                        comparePosition++;
                        if (listPositionComparation.Count == comparePosition)
                        {
                            flagComparation = true;
                            comparePosition--;
                        }
                    }
                }
                else
                {
                    flagComparation = true;
                }
                if (listTreeNodeToken.Count > 0)
                {
                    if (!isComparationType(listTreeNodeToken.ElementAt(tokenPosition)))
                    {
                        if (globalPosition == listPositionToken[tokenPosition])
                        {
                            if (isVarType(listTreeNodeToken[tokenPosition]))
                            {
                                vTreeNodeToken[currentPosition].label = listTreeNodeToken[tokenPosition].Text;
                                fillDeclaration(listTreeNodeToken[tokenPosition]);
                                currentPosition++;
                            }
                            else if (isAsignType(listTreeNodeToken[tokenPosition]))
                            {
                                id = listTreeNodeToken[tokenPosition].FirstNode.Text;
                                type = vHashTable[getHashCode(id), getCollisionColumn(id)].type;
                                vTreeNodeToken[currentPosition].label = listTreeNodeToken[tokenPosition].Text;
                                fillAsign(listTreeNodeToken[tokenPosition], type);
                                currentPosition++;
                            }
                            tokenPosition++;
                            if (listTreeNodeToken.Count == tokenPosition)
                            {
                                flagToken = true;
                                tokenPosition--;
                            }
                        }
                    }
                }
                else
                {
                    flagToken = true;
                }
                globalPosition++;
            }

        }
        private void setUpTreeAnnotation()
        {
            intCountCompare = 0;
            intCountAsign = 0;
            intCountType = 0;
            int currentPosition = 1;
            int comparePosition = 0;
            int tokenPosition = 0;
            int coutPosition = 0;
            int cinPosition = 0;
            while (true)
            {
                if (listPositionComparation.Count > 0 && currentPosition == listPositionComparation[comparePosition])
                {
                    treeSemantico.Nodes.Add(listTreeNodeComparation.ElementAt(comparePosition));
                    comparePosition++;
                    if (listPositionComparation.Count == comparePosition)
                    {
                        comparePosition--;
                    }
                }
                if (listTreeNodeToken.Count > 0)
                {
                    if (!isComparationType(listTreeNodeToken.ElementAt(tokenPosition)))
                    {
                        if (currentPosition == listPositionToken[tokenPosition])
                        {
                            if (isVarType(listTreeNodeToken.ElementAt(tokenPosition)))
                            {
                                intCountType++;
                                readId(listTreeNodeToken.ElementAt(tokenPosition));
                            }
                            else if (analizador_lexico.Estados.tokenAsignacion.Equals(listTreeNodeToken.ElementAt(tokenPosition).Text))
                            {
                                intCountCompare++;
                                intCountAsign++;
                                readAsign(listTreeNodeToken.ElementAt(tokenPosition));
                            }
                            treeSemantico.Nodes.Add(listTreeNodeToken.ElementAt(tokenPosition));
                            tokenPosition++;
                            if (listPositionToken.Count == tokenPosition)
                            {
                                tokenPosition--;
                            }
                        }
                    }
                    if (listPositionCout.Count > 0)
                    {
                        if (currentPosition == listPositionCout[coutPosition])
                        {
                            treeSemantico.Nodes.Add(listTreeNodeCout.ElementAt(coutPosition++));
                            if (listPositionCout.Count == coutPosition)
                            {
                                coutPosition--;
                            }
                        }
                    }
                    if (listPositionCin.Count > 0)
                    {
                        if (currentPosition == listPositionCin.Count)
                        {
                            treeSemantico.Nodes.Add(listTreeNodeCin.ElementAt(cinPosition++));
                            if (listPositionCin.Count == cinPosition)
                            {
                                cinPosition--;
                            }
                        }
                    }
                }
                currentPosition++;
                if (currentPosition >= (listTreeNodeComparation.Count + listTreeNodeToken.Count + listTreeNodeCout.Count + listTreeNodeCin.Count + 1))
                {
                    break;
                }
            }
            int total = intCountAsign + intCountType;
            vTreeNodeToken = new TreeNodeObject[total];
            fillTreeNodeToken();
            addAnnotation();
        }
        private void fillAsign(TreeNode node)
        {
            String id = String.Empty;
            int newHashCode = -1;
            int col = 0;
            foreach (TreeNode currentNode in node.Nodes)
            {
                currentPosition++;
                if (id.Equals(String.Empty) && !isOperator(currentNode.Text) && !isNumber(currentNode.Text))
                {
                    id = currentNode.Text;
                    newHashCode = getHashCode(id);
                    col = getCollisionColumn(id);
                }
                if (newHashCode != -1 && !isNumber(currentNode.Text) && vHashTable[newHashCode, col].type == null)
                {
                    setErrors(tokensSem[currentPosition], TokenErrorTipoDeclaracion, 0, tokenDescription[currentPosition]);
                }
                if (isOperator(currentNode.Text) || !isNumber(currentNode.Text))
                {
                    stackPositions.Push(currentPosition);
                    stackExpression.Push(currentNode.Text);

                }
                else if (isNumber(currentNode.Text))
                {
                    stackExpression.Push(currentNode.Text);
                }
                fillAsign(currentNode);
            }
        }
        private void clearStacks()
        {
            stackOperation.Clear();
            stackResults.Clear();
            stackExpression.Clear();
            stackPositions.Clear();
        }
        private Boolean isFloat(String token)
        {
            return token.IndexOf('.') != -1;
        }
        private void operateStruct(String id)
        {
            String push = String.Empty;//El simbolo
            String auxLeft = String.Empty;//Id-# left
            String auxRight = String.Empty;//Id-# right
            String type = String.Empty;//Tipo de dato del ID
            String result = String.Empty;//Resultado de las operaciones
            hashCode = getHashCode(id);
            column = getCollisionColumn(id);
            type = vHashTable[hashCode, column].type;
            //clearStacks();
            if (type == null)
            {
                clearStacks();
                return;
            }
            if (type.Equals(analizador_lexico.Estados.tokenBoolean))
            {
                while (stackPositions.Count != 1)
                {
                    stackPositions.Pop();
                }
                int position = stackPositions.Pop();
                setErrors(tokensSem[position], TokenErrorTipoAsignacion, 0, tokenDescription[position]);
                clearStacks();
                return;
            }
            if (stackExpression.Count == 2)
            {
                String aux;
                int auxPosition;
                result = stackExpression.Pop();
                if (!isNumber(result))
                {
                    aux = result;
                    result = vHashTable[getHashCode(result), getCollisionColumn(result)].value;
                }
                if (result == null)
                {
                    auxPosition = stackPositions.Pop();
                    vTreeNodeToken[auxPosition].value = "0";
                    //int tokenSemPosition = getTokenSemanticoPosition(vTreeNodeToken[auxPosition].tokenDescriptor);
                    int tokenSemPosition = getTokenSemanticoPosition(vTreeNodeToken[auxPosition].tokenDescriptor);
                    setErrors(tokensSem[tokenSemPosition], TokenErrorNoInt, 0, tokenDescription[tokenSemPosition]);
                    auxPosition = stackPositions.Pop();
                    vTreeNodeToken[auxPosition].value = "0";
                    vTreeNodeToken[auxPosition - 1].value = "0";
                }
                else
                {
                    auxPosition = stackPositions.Pop();
                    if (type.Equals(analizador_lexico.Estados.tokenInt) && isInt(result))
                    {
                        vTreeNodeToken[auxPosition].value = result;
                        vHashTable[hashCode, column].value = result;
                        int vPosition = stackPositions.Pop();
                        vTreeNodeToken[vPosition].value = result;
                        vTreeNodeToken[vPosition].type = type;
                    }
                    else if (type.Equals(analizador_lexico.Estados.tokenInt))
                    {
                        vTreeNodeToken[auxPosition].value = ERROR;
                        vHashTable[hashCode, column].value = ERROR;
                        int vPosition = stackPositions.Pop();
                        vTreeNodeToken[vPosition].value = ERROR;
                        vTreeNodeToken[vPosition].type = type;
                        int tokenSemPosition = getTokenSemanticoPosition(vTreeNodeToken[auxPosition].tokenDescriptor);
                        setErrors(tokensSem[tokenSemPosition], TokenErrorTipoAsignacion, 0, tokenDescription[tokenSemPosition]);
                    }
                    else if (!isFloat(result))
                    {
                        if (!result.Equals(ERROR) && type.Equals(analizador_lexico.Estados.tokenFloat) && !isFloat(result))
                        {
                            result = result + ".00";
                        }
                        vTreeNodeToken[auxPosition].value = result;
                        vHashTable[hashCode, column].value = result;
                        int vPosition = stackPositions.Pop();
                        vTreeNodeToken[vPosition].value = result;
                        vTreeNodeToken[vPosition].type = type;
                    }
                    else
                    {
                        vTreeNodeToken[auxPosition].value = result;
                        vHashTable[hashCode, column].value = result;
                        int vPosition = stackPositions.Pop();
                        vTreeNodeToken[vPosition].value = result;
                        vTreeNodeToken[vPosition].type = type;
                    }
                    stackExpression.Pop();
                }
                clearStacks();
            }
            else
            {
                String idLeft;
                String idRight;
                int auxPosition = -1;
                while (stackExpression.Count != 1)
                {
                    idLeft = null;
                    idRight = null;
                    auxPosition = -1;
                    push = stackExpression.Pop();
                    if (!isOperator(push))
                    {
                        stackResults.Push(push);
                    }
                    else
                    {
                        //stackOperation.Push(push);
                        auxLeft = stackResults.Pop();
                        auxRight = stackResults.Pop();
                        if (!(auxLeft.Equals(NULL) || auxRight.Equals(NULL)))
                        {
                            if (auxLeft != null && !isNumber(auxLeft))
                            {
                                idLeft = auxLeft;
                                auxLeft = vHashTable[getHashCode(auxLeft), getCollisionColumn(auxLeft)].value;

                            }
                            if (auxRight != null && !isNumber(auxRight))
                            {
                                idRight = auxRight;
                                auxRight = vHashTable[getHashCode(auxRight), getCollisionColumn(auxRight)].value;

                            }
                            if (auxLeft == null || auxLeft.Equals(NULL) || auxLeft.Equals(ERROR))
                            {
                                if (stackOperation.Count > 0)
                                {
                                    if (vTreeNodeToken[Int32.Parse(stackOperation.Peek())].label.Equals(idLeft))
                                    {
                                        auxPosition = Int32.Parse(stackOperation.Pop());
                                    }
                                    else
                                    {
                                        auxPosition = stackPositions.Pop();
                                    }
                                }
                                else
                                {
                                    auxPosition = stackPositions.Pop();
                                }
                                vTreeNodeToken[auxPosition].value = "0";
                                int aux2Position = auxPosition;
                                /*while (!tokensSem[auxPosition].getName().Equals(idLeft))
                                {
                                    auxPosition++;
                                }*/
                                //int tokenSemPosition = getTokenSemanticoPosition(vTreeNodeToken[auxPosition].tokenDescriptor);
                                int tokenSemPosition = getTokenSemanticoPosition(vTreeNodeToken[auxPosition].tokenDescriptor);//aqui le puse esto
                                setErrors(tokensSem[tokenSemPosition], TokenErrorNoInt, 0, tokenDescription[tokenSemPosition]);
                                auxPosition = aux2Position;
                                if (stackPositions.Count != 0)
                                {
                                    auxPosition = stackPositions.Pop();

                                }
                                vTreeNodeToken[auxPosition].value = "0"; //LE QUITE EL ERROR
                                stackResults.Push("0");
                            }
                            if (auxRight == null || auxRight.Equals(NULL) || auxRight.Equals(ERROR))
                            {
                                if (stackOperation.Count > 0)
                                {
                                    if (vTreeNodeToken[Int32.Parse(stackOperation.Peek())].label.Equals(idRight))
                                    {
                                        auxPosition = Int32.Parse(stackOperation.Pop());
                                    }
                                    else
                                    {
                                        auxPosition = stackPositions.Pop();
                                    }
                                }
                                else
                                {
                                    auxPosition = stackPositions.Pop();
                                }
                                vTreeNodeToken[auxPosition].value = "0";
                                int aux2Position = auxPosition;
                                /*while (!vTreeNodeToken[auxPosition].label.Equals(idRight))
                                {
                                    auxPosition++;
                                }*/
                                //int tokenSemPosition = getTokenSemanticoPosition(vTreeNodeToken[auxPosition].tokenDescriptor);
                                int tokenSemPosition = getTokenSemanticoPosition(vTreeNodeToken[auxPosition].tokenDescriptor);
                                setErrors(tokensSem[tokenSemPosition], TokenErrorNoInt, 0, tokenDescription[tokenSemPosition]);
                                auxPosition = aux2Position;
                                if (stackPositions.Count != 0)
                                {
                                    auxPosition = stackPositions.Pop();
                                }
                                vTreeNodeToken[auxPosition].value = "0";
                                stackResults.Push("0");
                            }
                            if (auxRight != null && auxLeft != null && !auxRight.Equals(NULL) && !auxLeft.Equals(NULL) && !auxRight.Equals(ERROR) && !auxLeft.Equals(ERROR))
                            {
                                // int tokenSemPosition = getTokenSemanticoPosition(vTreeNodeToken[auxPosition].tokenDescriptor);
                                result = resultOperation(auxLeft, Char.Parse(push), auxRight, type, stackPositions.Peek());
                                stackResults.Push(result);
                                if (idLeft != null)
                                {
                                    if (vTreeNodeToken[stackPositions.Peek()].label != null && vTreeNodeToken[stackPositions.Peek()].label.Equals(idLeft) || stackOperation.Count == 0)
                                    {
                                        auxPosition = stackPositions.Pop();
                                        vTreeNodeToken[auxPosition].value = vHashTable[getHashCode(vTreeNodeToken[auxPosition].label), getCollisionColumn(vTreeNodeToken[auxPosition].label)].value;
                                    }
                                    else
                                    {

                                        auxPosition = Int32.Parse(stackOperation.Pop());
                                        vTreeNodeToken[auxPosition].value = vHashTable[getHashCode(vTreeNodeToken[auxPosition].label), getCollisionColumn(vTreeNodeToken[auxPosition].label)].value;
                                    }
                                }
                                if (idRight != null)
                                {
                                    if (vTreeNodeToken[stackPositions.Peek()].label != null && vTreeNodeToken[stackPositions.Peek()].label.Equals(idRight) || stackOperation.Count == 0)
                                    {
                                        auxPosition = stackPositions.Pop();
                                        vTreeNodeToken[auxPosition].value = vHashTable[getHashCode(vTreeNodeToken[auxPosition].label), getCollisionColumn(vTreeNodeToken[auxPosition].label)].value;
                                    }
                                    else
                                    {
                                        auxPosition = Int32.Parse(stackOperation.Pop());
                                        vTreeNodeToken[auxPosition].value = vHashTable[getHashCode(vTreeNodeToken[auxPosition].label), getCollisionColumn(vTreeNodeToken[auxPosition].label)].value;
                                    }
                                }
                                auxPosition = stackPositions.Pop();
                                if (push.Equals(vTreeNodeToken[auxPosition].label))
                                {
                                    if (result == null)
                                    {
                                        vTreeNodeToken[auxPosition].value = "0";
                                    }
                                    else
                                    {
                                        vTreeNodeToken[auxPosition].value = result;
                                    }
                                    if (stackPositions.Count == 1 && stackOperation.Count != 0)
                                    {
                                        auxPosition = Int32.Parse(stackOperation.Pop());
                                        vTreeNodeToken[auxPosition].value = vHashTable[getHashCode(vTreeNodeToken[auxPosition].label), getCollisionColumn(vTreeNodeToken[auxPosition].label)].value;
                                    }
                                }
                                else
                                {
                                    do
                                    {
                                        stackOperation.Push(auxPosition.ToString());
                                        auxPosition = stackPositions.Pop();
                                    } while (!push.Equals(vTreeNodeToken[auxPosition].label));
                                    if (result == null)
                                    {
                                        vTreeNodeToken[auxPosition].value = "0";
                                    }
                                    else
                                    {
                                        vTreeNodeToken[auxPosition].value = result;
                                    }
                                }
                            }
                        }
                    }
                }
                if (stackPositions.Count > 0)
                {
                    auxPosition = stackPositions.Pop();
                }
                if (stackResults.Count != 0)
                {
                    while (stackResults.Count != 0)
                    {
                        result = stackResults.Pop();
                    }
                    if (!result.Equals(ERROR) && type.Equals(analizador_lexico.Estados.tokenFloat) && !isFloat(result))
                    {
                        result = result + ".00";
                    }
                }
                else
                {
                    result = NULL;
                }
                if (auxPosition != -1)
                {
                    vTreeNodeToken[auxPosition].type = type;
                    vTreeNodeToken[auxPosition].value = result;
                    vHashTable[hashCode, column].value = result;
                }
                if (stackPositions.Count > 0)
                {
                    auxPosition = stackPositions.Pop();
                    vTreeNodeToken[auxPosition].type = type;
                    vTreeNodeToken[auxPosition].value = result;
                }

                clearStacks();
            }
        }
        private String resultOperation(String auxLeft, Char op, String auxRight, String type, int position)
        {
            Char charType;
            if (type.Equals(analizador_lexico.Estados.tokenInt))
            {
                charType = 'i';
            }
            else
            {
                charType = 'f';
            }
            String stringResult = null;
            if ((isFloat(auxLeft) || isFloat(auxRight)) && charType == 'i')
            {
                setErrors(tokensSem[position], TokenErrorTipoAsignacion, 0, tokenDescription[position]);
                return "0";
            }
            switch (op)
            {
                case '+':
                    switch (charType)
                    {
                        case 'i':
                            stringResult = (Int32.Parse(auxLeft) + Int32.Parse(auxRight)).ToString();
                            break;
                        case 'f':
                            float left = float.Parse(auxLeft, CultureInfo.InvariantCulture);
                            float right = float.Parse(auxRight, CultureInfo.InvariantCulture);
                            stringResult = (left + right).ToString();
                            stringResult = stringResult.Replace(",", ".");
                            String qiubo = "hola";
                            qiubo = qiubo.Replace(",", ".");
                            break;
                    }
                    break;
                case '-':
                    switch (charType)
                    {
                        case 'i':
                            stringResult = (Int32.Parse(auxLeft) - Int32.Parse(auxRight)).ToString();
                            break;
                        case 'f':
                            float left = float.Parse(auxLeft, CultureInfo.InvariantCulture);
                            float right = float.Parse(auxRight, CultureInfo.InvariantCulture);
                            stringResult = (left - right).ToString();
                            stringResult = stringResult.Replace(",", ".");
                            break;
                    }
                    break;
                case '*':
                    switch (charType)
                    {
                        case 'i':
                            stringResult = (Int32.Parse(auxLeft) * Int32.Parse(auxRight)).ToString();
                            break;
                        case 'f':
                            float left = float.Parse(auxLeft, CultureInfo.InvariantCulture);
                            float right = float.Parse(auxRight, CultureInfo.InvariantCulture);
                            stringResult = (left * right).ToString();
                            stringResult = stringResult.Replace(",", ".");
                            break;
                    }
                    break;
                case '/':
                    switch (charType)
                    {
                        case 'i':
                            if (auxRight.Equals("0"))
                            {
                                return "0";
                            }
                            stringResult = (Int32.Parse(auxLeft) / Int32.Parse(auxRight)).ToString();
                            break;
                        case 'f':
                            Boolean flagLeft = isInt(auxLeft);
                            Boolean flagRight = isInt(auxRight);
                            if (flagLeft && flagRight)
                            {
                                if (Int32.Parse(auxRight) == 0)
                                {
                                    return "0";
                                }
                                stringResult = (Int32.Parse(auxLeft) / Int32.Parse(auxRight)).ToString() + ".00";
                            }
                            else
                            {
                                float left = float.Parse(auxLeft, CultureInfo.InvariantCulture);
                                float right = float.Parse(auxRight, CultureInfo.InvariantCulture);
                                if (right == 0)
                                {
                                    return "0";
                                }
                                stringResult = (left / right).ToString();
                                stringResult = stringResult.Replace(",", ".");
                            }
                            break;
                    }
                    break;
            }
            return stringResult;
        }
        private void fillStruct(String label, String value, String type)
        {
            vTreeNodeToken[currentPosition].label = label;
            vTreeNodeToken[currentPosition].value = value;
            vTreeNodeToken[currentPosition].type = type;
        }
        private void fillDeclaration(TreeNode node, String type)
        {
            foreach (TreeNode currentNode in node.Nodes)
            {
                currentPosition++;
                fillStruct(currentNode.Text, String.Empty, type);
                fillDeclaration(currentNode, type);
            }
        }


        public TreeView getTreeViewSemanticNew()
        {
            return treeViewSintactico;
        }
        public StringBuilder getErrors()
        {
            return sbErrors;
        }
        private string getTypeByRC(int row, int column)
        {
            while (row >= 0 && !tokensSem[row].getName().Equals(analizador_lexico.Estados.tokenInt) && !tokensSem[row].getName().Equals(analizador_lexico.Estados.tokenFloat) && !tokensSem[row].getName().Equals(analizador_lexico.Estados.tokenBoolean))
            {
                if (tokensSem[row].getName().Equals(analizador_lexico.Estados.tokenPuntoComa))
                {
                    throw new IndexOutOfRangeException();
                }
                row--;
            }
            if (row < 0)
            {
                throw new IndexOutOfRangeException();
            }
            return tokensSem[row].getName();
        }

        public void setTreeViewSintactico(TreeView sintactico)
        {
            TreeNodeCollection allNodes = sintactico.Nodes;
            TreeNode child;
            treeViewSintactico = new TreeView();
            foreach (TreeNode currentNode in allNodes)
            {
                child = new TreeNode(currentNode.Text);
                child.Tag = currentNode.Tag;
                cloneTreeViewSintactico(child, currentNode);
                treeViewSintactico.Nodes.Add(child);
            }
        }

        public void cloneTreeViewSintactico(TreeNode parent, TreeNode child)
        {
            TreeNode newChild;
            foreach (TreeNode currentNode in child.Nodes)
            {
                newChild = new TreeNode(currentNode.Text);
                newChild.Tag = currentNode.Tag;
                cloneTreeViewSintactico(newChild, currentNode);
                if (!newChild.Text.Equals(String.Empty))
                {
                    parent.Nodes.Add(newChild);
                }
                else
                {
                    foreach (TreeNode auxiliar in newChild.Nodes)
                    {
                        parent.Nodes.Add(auxiliar);
                    }
                }
            }
        }
        private void addAnnotation()
        {
            stackOperation = new Stack<string>();
            stackExpression = new Stack<string>();
            stackPositions = new Stack<int>();
            stackResults = new Stack<string>();
            String type = String.Empty;
            String left = String.Empty;
            String right = String.Empty;
            listComparations = new List<string>();
            currentPosition = 0;
            for (int i = 0; i < listTreeNodeToken.Count; i++)
            {
                if (isVarType(listTreeNodeToken[i]))
                {
                    fillStruct(listTreeNodeToken[i].Text, string.Empty, string.Empty);
                    fillDeclaration(listTreeNodeToken[i], listTreeNodeToken[i].Text);
                    currentPosition++;
                }
                else if (isAsignType(listTreeNodeToken[i]))
                {
                    String id = listTreeNodeToken[i].FirstNode.Text;
                    stackPositions.Push(currentPosition);
                    fillAsign(listTreeNodeToken[i]);
                    try
                    {
                        operateStruct(id);
                    }
                    catch (Exception e)
                    {
                    }
                    currentPosition++;
                }
            }
            refreshTree();
            refreshCompareTree();
        }
        public void startSemantic()
        {
            dataTable = new DataTable();//hashCode, nombre de variable, tipo, valor, numero de linea, registro 
            //dataTable.Columns.Add("HashCode", typeof(int));
            dataTable.Columns.Add("Nombre", typeof(string));
            dataTable.Columns.Add("Tipo", typeof(string));
            dataTable.Columns.Add("Valor", typeof(string));
            dataTable.Columns.Add("Numero de Linea", typeof(string));
            hashCode = 0;
            column = 0;
            auxColumn = 0;
            currentPosition = 0;
            treeSemantico = new TreeView();
            readTree();
            fillHashTable();
            setUpTreeAnnotation();
        }
        private void fillStruct(TreeNode node)
        {
            foreach (TreeNode currentNode in node.Nodes)
            {
                currentPosition++;
                if (!isNumber(currentNode.Text))
                {
                    vTreeNodeToken[currentPosition].type = (vHashTable[getHashCode(currentNode.Text), getCollisionColumn(currentNode.Text)].type != null)
                        ? vHashTable[getHashCode(currentNode.Text), getCollisionColumn(currentNode.Text)].type
                        : "ERROR type";
                    if (vTreeNodeToken[currentPosition].value != null)
                    {
                        vTreeNodeToken[currentPosition].label += "(" + vTreeNodeToken[currentPosition].value + ") ";
                    }
                    else
                    {
                        vTreeNodeToken[currentPosition].label += "(" + NULL + ")" ;
                    }
                }
                else if (isOperator(currentNode.Text))
                {
                    if (vTreeNodeToken[currentPosition].value != null)
                    {
                        vTreeNodeToken[currentPosition].label += " ( " + vTreeNodeToken[currentPosition].value + " )";
                    }
                    else
                    {
                        vTreeNodeToken[currentPosition].label += " { " + NULL + " }";
                    }

                }
                fillStruct(currentNode);
            }
        }
        private void refreshTree()
        {
            currentPosition = -1;
            for (int i = 0; i < listTreeNodeToken.Count; i++)
            {
                currentPosition++;
                if (isVarType(listTreeNodeToken[i]))
                {
                    fillDeclaration(listTreeNodeToken[i], false);

                }
                else if (isAsignType(listTreeNodeToken[i]))
                {
                    fillStruct(listTreeNodeToken[i]);
                }
            }
            currentPosition = -1;
            for (int i = 0; i < listTreeNodeToken.Count; i++)
            {
                currentPosition++;
                if (isVarType(listTreeNodeToken[i]))
                {
                    fillDeclaration(listTreeNodeToken[i], true);
                }
                else if (isAsignType(listTreeNodeToken[i]))
                {
                    if (vTreeNodeToken[currentPosition].value != null)
                    {
                        listTreeNodeToken[i].Text = vTreeNodeToken[currentPosition].label +  " (" + vTreeNodeToken[currentPosition].value + ")";
                    }
                    else
                    {
                        listTreeNodeToken[i].Text = vTreeNodeToken[currentPosition].label + " (" + listTreeNodeToken[i].FirstNode.Text +  NULL + ")";
                    }
                    try
                    {
                        fillAsignTree(listTreeNodeToken[i]);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }

        }
        private void fillAsignTree(TreeNode node)
        {
            foreach (TreeNode currentNode in node.Nodes)
            {
                currentPosition++;
                currentNode.Text = vTreeNodeToken[currentPosition].label;
                fillAsignTree(currentNode);
            }
        }
        private void fillDeclaration(TreeNode node, Boolean flag)
        {
            foreach (TreeNode currentNode in node.Nodes)
            {
                currentPosition++;
                if (flag)
                {
                    currentNode.Text = vTreeNodeToken[currentPosition].label;
                }
                fillDeclaration(currentNode, flag);
            }
        }


        private void refreshCompareTree()
        {
            int globalPosition = 1;
            int comparePosition = 0;
            int tokenPosition = 0;
            int coutPosition = 0;
            int cinPosition = 0;
            bool flag = true;
            vTreeNodeCout = new TreeNodeObject[listPositionCout.Count * 2];
            vTreeNodeCin = new TreeNodeObject[listPositionCin.Count * 2];
            while (true)
            {
                if (listPositionComparation.Count > 0)
                {
                    if (globalPosition == listPositionComparation[comparePosition])
                    {
                        if (flag)
                        {
                            flag = false;
                        }
                        else
                        {
                            tokenPosition--;
                        }
                        fillComparation(listTreeNodeComparation[comparePosition], tokenPosition);
                        comparePosition++;
                        if (listPositionComparation.Count == comparePosition)
                        {
                            comparePosition--;
                        }
                        if (!flag)
                        {
                            tokenPosition++;
                        }
                    }
                }
                if (listTreeNodeToken.Count > 0)
                {
                    if (listPositionToken.Count == tokenPosition)
                    {
                        tokenPosition--;
                    }
                    if (!isComparationType(listTreeNodeToken.ElementAt(tokenPosition)))
                    {
                        if (globalPosition == listPositionToken[tokenPosition])
                        {
                            tokenPosition++;
                            if (listPositionToken.Count == tokenPosition)
                            {
                                tokenPosition--;
                            }
                        }
                    }
                }
                if (listPositionCout.Count > 0)
                {
                    if (globalPosition == listPositionCout[coutPosition])
                    {
                        coutPosition++;
                        if (listPositionCout.Count == coutPosition)
                        {
                            coutPosition--;
                        }
                    }
                }
                if (listPositionCin.Count > 0)
                {
                    if (globalPosition == listPositionCin[cinPosition])
                    {
                        cinPosition++;
                        if (listPositionCin.Count == cinPosition)
                        {
                            cinPosition--;
                        }
                    }
                }
                globalPosition++;
                if (globalPosition == listTreeNodeComparation.Count + listTreeNodeToken.Count + listTreeNodeCout.Count + listTreeNodeCin.Count + 1)
                {
                    break;
                }
            }
        }
        private void fillComparation(TreeNode node, int tokenPosition)
        {
            string auxLeft = node.FirstNode.Text;
            string auxOp = node.Text;
            string auxRight = node.LastNode.Text;
            string typeLeft = null;
            string typeRight = null;
            bool flag;
            if (!isNumber(auxLeft))
            {
                typeLeft = vHashTable[getHashCode(auxLeft), getCollisionColumn(auxLeft)].type;
                auxLeft = vHashTable[getHashCode(auxLeft), getCollisionColumn(auxLeft)].value;
                node.FirstNode.Text += " { " + auxLeft + " }";
            }
            if (!isNumber(auxRight))
            {
                typeRight = vHashTable[getHashCode(auxRight), getCollisionColumn(auxRight)].type;
                auxRight = vHashTable[getHashCode(auxRight), getCollisionColumn(auxRight)].value;
                node.LastNode.Text += " { " + auxRight + " }";
            }
            if (auxLeft == null || auxRight == null)
            {
                setErrors(tokensSem[0], TokenErrorNulo, 1, tokenDescription[0]);
                return;
            }
            if (typeLeft == null && typeRight == null)
            {
                if (isInt(auxLeft))
                {
                    typeLeft = analizador_lexico.Estados.tokenInt;
                }
                else
                {
                    typeLeft = analizador_lexico.Estados.tokenFloat;
                }
                if (isInt(auxRight))
                {
                    typeRight = analizador_lexico.Estados.tokenInt;
                }
                else
                {
                    typeRight = analizador_lexico.Estados.tokenFloat;
                }
                if (typeLeft.Equals(typeRight))
                {
                    if (isNumber(auxLeft) && isNumber(auxRight))
                    {
                        flag = isSameDataType(auxLeft, auxOp, auxRight, typeLeft);
                        //node.Text += " { " + flag + ", " + (flag ? "1" : "0") + " }";
                    }
                    else
                    {
                        //node.Text += " { false, 0 }";
                        setErrors(tokensSem[0], TokenErrorNulo, 1, tokenDescription[0]);
                    }
                }
            }
            else
            {
                if (typeLeft == null)
                {
                    if (isInt(auxLeft))
                    {
                        typeLeft = analizador_lexico.Estados.tokenInt;
                    }
                    else
                    {
                        typeLeft = analizador_lexico.Estados.tokenFloat;
                    }
                }
                if (typeRight == null)
                {
                    if (isInt(auxRight))
                    {
                        typeRight = analizador_lexico.Estados.tokenInt;
                    }
                    else
                    {
                        typeRight = analizador_lexico.Estados.tokenFloat;
                    }
                }
                if (typeLeft.Equals(typeRight))
                {
                    if (isNumber(auxLeft) && isNumber(auxRight))
                    {
                        flag = isSameDataType(auxLeft, auxOp, auxRight, typeLeft);
                        //node.Text += " { " + flag + ", " + (flag ? "1" : "0") + " }";
                    }
                    else
                    {
                        //node.Text += " { false, 0 }";
                        setErrors(tokensSem[0], TokenErrorNulo, 1, tokenDescription[0]);
                    }
                }
            }
        }


        private Boolean isInt(String token)
        {
            return token.IndexOf(".") == -1;
        }
        private bool isSameDataType(string auxLeft, string auxOp, string auxRight, string type)
        {
            if (type.Equals(analizador_lexico.Estados.tokenInt))
            {
                int leftValue = Int32.Parse(auxLeft);
                int rightValue = Int32.Parse(auxRight);
                return isComparingIntegers(leftValue, auxOp, rightValue);
            }
            else
            {
                float leftValue = float.Parse(auxLeft);
                float rightValue = float.Parse(auxRight);
                return isComparingFloat(leftValue, auxOp, rightValue);
            }
        }
        #endregion

        private bool isComparingIntegers(int auxLeft, string auxOp, int auxRight)
        {
            if (auxOp.Equals(analizador_lexico.Estados.tokenMenor))
            {
                return auxLeft < auxRight;
            }
            else if (auxOp.Equals(analizador_lexico.Estados.tokenMenorIgual))
            {
                return auxLeft <= auxRight;
            }
            else if (auxOp.Equals(analizador_lexico.Estados.tokenMayor))
            {
                return auxLeft > auxRight;
            }
            else if (auxOp.Equals(analizador_lexico.Estados.tokenMayorIgual))
            {
                return auxLeft >= auxRight;
            }
            else if (auxOp.Equals(analizador_lexico.Estados.tokenIgualdad))
            {
                return auxLeft == auxRight;
            }
            else if (auxOp.Equals(analizador_lexico.Estados.tokenDiferente))
            {
                return auxLeft != auxRight;
            }
            return false;
        }
        private bool isComparingFloat(float auxLeft, string auxOp, float auxRight)
        {
            if (auxOp.Equals(analizador_lexico.Estados.tokenMenor))
            {
                return auxLeft < auxRight;
            }
            else if (auxOp.Equals(analizador_lexico.Estados.menorIgual))
            {
                return auxLeft <= auxRight;
            }
            else if (auxOp.Equals(analizador_lexico.Estados.tokenMayor))
            {
                return auxLeft > auxRight;
            }
            else if (auxOp.Equals(analizador_lexico.Estados.tokenMayorIgual))
            {
                return auxLeft >= auxRight;
            }
            else if (auxOp.Equals(analizador_lexico.Estados.tokenIgualdad))
            {
                return auxLeft == auxRight;
            }
            else if (auxOp.Equals(analizador_lexico.Estados.tokenDiferente))
            {
                return auxLeft != auxRight;
            }
            return false;
        }

        public DataTable getDataTable()
        {
            for (int i = 0; i < intSizeRows; i++)
            {
                for (int j = 0; j < intSizeCols; j++)
                {
                    if (vHashTable[i, j].idName != null)
                    {
                        dataTable.Rows.Add(
                            //vHashTable[i, j].hashCode,
                            vHashTable[i, j].idName,
                            vHashTable[i, j].type,
                            vHashTable[i, j].value,
                            vHashTable[i, j].lineNumber
                            );

                    }
                }
            }
            return dataTable;
        }

        private int getTokenSemanticoPosition(int node)
        {
            int position = 0;
            for (int i = 0; i < tokensSem.Count; i++)
            {
                if (tokensSem[i].getTokenDescriptor() == node)
                {
                    position = i;
                    break;
                }
            }
            return position;
        }
    }
}

