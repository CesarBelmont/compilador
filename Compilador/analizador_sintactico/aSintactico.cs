using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Compilador
{

    class aSintactico
    {
        private int indice;
        private StringBuilder errores;
        private TreeNode nodoPrincipal;
        private List<String> tipoTokens;
        private Stack<String> stackParentesisAbre;
        private List<analizador_lexico.Token> listaTokens;
        private TreeView arbolSintactico;
        //StreamWriter Archivo = File.CreateText(@"../../Errores.txt");
        private Boolean abierto = false;
        private Boolean cerrado = false;
        public aSintactico(List<analizador_lexico.Token> listaTokens, TreeView treeSintactico, List<String> tokenTypeList)
        {
            this.listaTokens = listaTokens;
            this.arbolSintactico = treeSintactico;
            this.tipoTokens = tokenTypeList;
            indice = 0;
            errores = new StringBuilder();
            stackParentesisAbre = new Stack<string>();

        }

        public void analisisSintactico()
        {
            int start = 0;
            if (listaTokens.Count == 0)
            {
                nodoPrincipal = new TreeNode("Nada por aqui...");
                nodoPrincipal.Tag = 0;
                arbolSintactico.Nodes.Add(nodoPrincipal);
                return;
            }
            else if (isMain())
            {
                nodoPrincipal = new TreeNode(analizador_lexico.Estados.tokenMain);
                nodoPrincipal.Tag = 0;
                arbolSintactico.Nodes.Add(nodoPrincipal);
                start = 1;
            }
            else
            {
                nodoPrincipal = new TreeNode("Nada por aqui....");
                nodoPrincipal.Tag = 0;
                arbolSintactico.Nodes.Add(nodoPrincipal);
                analizador_lexico.Token token = new analizador_lexico.Token("No hay  " + analizador_lexico.Estados.tokenMain, new analizador_lexico.ubicacionToken(listaTokens[0].getLocation().getFila(), listaTokens[0].getLocation().getColumna()));
                agregarError(token);
                start = 0;
            }


            if (indice == listaTokens.Count)
            {

                analizador_lexico.Token token = new analizador_lexico.Token("Necesitas {}", new analizador_lexico.ubicacionToken(listaTokens[0].getLocation().getFila() + 1, 0));
                agregarError(token);

            }

            for (indice = start; indice < listaTokens.Count; indice++)
            {
                if (start == 0 || (indice == 1 && tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveA)))
                {
                    start = -1;
                    abierto = tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveA);
                    if (!abierto)
                    {
                        analizador_lexico.Token token = new analizador_lexico.Token("No hay" + analizador_lexico.Estados.tokenLlaveA, new analizador_lexico.ubicacionToken(listaTokens[0].getLocation().getFila(), listaTokens[0].getLocation().getColumna()));
                        agregarError(token);
                    }
                }
                else if (indice == (listaTokens.Count - 1))
                {
                    cerrado = tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveC);
                }
                else if (isDeclaracion(listaTokens[indice].getName()))
                {
                    if (!abierto)
                    {
                        analizador_lexico.Token token = new analizador_lexico.Token("No hay " + analizador_lexico.Estados.tokenLlaveA, new analizador_lexico.ubicacionToken(listaTokens[0].getLocation().getFila(), 0));
                        agregarError(token);
                        abierto = true;
                    }
                    declaraciones(nodoPrincipal);

                }
                else if (isSentencia(listaTokens[indice].getName()))
                {
                    sentencias(nodoPrincipal);
                }
                else if (isCin(listaTokens[indice].getName()))
                {
                    entradaCIN(nodoPrincipal);
                }
                else if (isCout(listaTokens[indice].getName()))
                {
                    salidaCOUT(nodoPrincipal);
                }
                else if (isIdentificador(tipoTokens[indice]))
                {
                    asignaciones(nodoPrincipal);
                }
                else
                {
                    try
                    {

                        if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenPuntoComa) && tokenActual(listaTokens[indice - 1].getName(), analizador_lexico.Estados.tokenParentesisC) &&
                            tokenActual(listaTokens[indice - 6].getName(), analizador_lexico.Estados.tokenUntil))
                        {

                        }
                        else
                        {

                            listaTokens[indice].setTokenDescription(indice);
                            TreeNode nodeError = new TreeNode("(Error): " + listaTokens[indice].getName());
                            nodeError.Tag = indice;
                            agregarNodo(nodoPrincipal, nodeError);
                            // agregarError(listaTokens[indice]);

                        }
                    }
                    catch (Exception e)
                    {
                        listaTokens[indice].setTokenDescription(indice);
                        TreeNode nodeError = new TreeNode("(Error): " + listaTokens[indice].getName());
                        nodeError.Tag = indice;
                        agregarNodo(nodoPrincipal, nodeError);
                        //agregarError(listaTokens[indice]);

                    }


                }
            }

            if (!cerrado)
            {

                try
                {

                    if (tokenActual(listaTokens[indice - 2].getName(), analizador_lexico.Estados.tokenLlaveC) && tokenActual(listaTokens[indice - 1].getName(), analizador_lexico.Estados.tokenLlaveC))
                    {

                    }
                    else if (!tokenActual(listaTokens[indice - 1].getName(), analizador_lexico.Estados.tokenLlaveC) && !tokenActual(listaTokens[indice - 2].getName(), analizador_lexico.Estados.tokenLlaveC))
                    {
                        analizador_lexico.Token token = new analizador_lexico.Token("Tu MAIN no esta bien cerrado, verifica tus LLAVES " + analizador_lexico.Estados.tokenLlaveC, new analizador_lexico.ubicacionToken(listaTokens[listaTokens.Count - 1].getLocation().getFila() + 1, 0));
                        agregarError(token);

                    }
                    else if (!tokenActual(listaTokens[indice - 1].getName(), analizador_lexico.Estados.tokenLlaveC) && tokenActual(listaTokens[indice - 2].getName(), analizador_lexico.Estados.tokenLlaveC))
                    {
                        analizador_lexico.Token token = new analizador_lexico.Token("Tu MAIN no esta bien cerrado, verifica tus LLAVES" + analizador_lexico.Estados.tokenLlaveC, new analizador_lexico.ubicacionToken(listaTokens[listaTokens.Count - 1].getLocation().getFila() + 1, 0));
                        agregarError(token);
                    }
                    else
                    {
                        analizador_lexico.Token token = new analizador_lexico.Token("Tu MAIN no esta bien cerrado, verifica tus LLAVES" + analizador_lexico.Estados.tokenLlaveC, new analizador_lexico.ubicacionToken(listaTokens[listaTokens.Count - 1].getLocation().getFila() + 1, 0));
                        agregarError(token);
                    }
                }
                catch (Exception e)
                {

                    if (tokenActual(listaTokens[listaTokens.Count - 1].getName(), analizador_lexico.Estados.tokenMain))
                    {

                    }

                    else if (!tokenActual(listaTokens[listaTokens.Count - 1].getName(), analizador_lexico.Estados.tokenLlaveC))
                    {
                        analizador_lexico.Token token = new analizador_lexico.Token("Falta una LLAVE QUE CIERRA } " + analizador_lexico.Estados.tokenLlaveC, new analizador_lexico.ubicacionToken(listaTokens.Count - 2, 0));
                        agregarError(token);
                    }
                }
            }

            // Archivo.Close();
        }

        private void agregarError(analizador_lexico.Token token)
        {
            errores.Append(token.getName() + " Ubicado en " + (token.getLocation().getFila() + 1) + '\n');
        }

        private void agregarNodo(TreeNode nodeRoot, TreeNode nodeChild)
        {
            if (nodeChild != null && nodeChild.Text != null && !nodeChild.Text.Equals(string.Empty))
            {
                nodeRoot.Nodes.Add(nodeChild);
            }
        }

        private TreeNode nodoActual()
        {
            listaTokens[indice].setTokenDescription(indice);
            TreeNode currentNode = new TreeNode(listaTokens[indice].getName());
            currentNode.Tag = indice;
            return currentNode;
        }

        private TreeNode Factor()
        {
            TreeNode nodeFactor = new TreeNode();
            if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisA))
            {
                indice++;
                nodeFactor = Expresion();
                if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisC))
                {
                    analizador_lexico.ubicacionToken location = new analizador_lexico.ubicacionToken(listaTokens[indice].getLocation().getFila() + 1, 0);
                    analizador_lexico.Token nodeError = new analizador_lexico.Token("Hace falta un PARENTESIS )", location);
                    agregarError(nodeError);
                }
                else
                {
                    indice++;
                }
            }

            else if (isIdentificador(tipoTokens[indice]) || isNumero(tipoTokens[indice]))
            {
                listaTokens[indice].setTokenDescription(indice);
                nodeFactor = new TreeNode(listaTokens[indice].getName());
                nodeFactor.Tag = indice;
                indice++;
                //detectar falta de ( en una expresion con paretensis
                if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisC))
                {
                    if (!tokenActual(listaTokens[indice - 4].getName(), analizador_lexico.Estados.tokenParentesisA))
                    {
                        //Console.WriteLine("Pos esta raro fijate esperaba un ( ---> " + listaTokens[indice - 4].getName());
                        analizador_lexico.ubicacionToken location = new analizador_lexico.ubicacionToken(listaTokens[indice].getLocation().getFila() + 1, 0);
                        analizador_lexico.Token nodeError = new analizador_lexico.Token("Falta un PARENTESIS ( ", location); //se debe restar 1 a la fila
                        agregarError(nodeError);
                    }
                    else if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisA))
                    {
                        Console.WriteLine("pos nada :V");
                    }
                }
                else
                {
                    //Console.WriteLine("Sigamos con la programacion habitual -> " + listaTokens[indice].getName());

                }
            }
            else
            {
                listaTokens[indice].setTokenDescription(indice);
                nodeFactor.Tag = indice;
                analizador_lexico.Token tokenError = new analizador_lexico.Token("Falta un IDENTIFICADOR o NUMERO ", listaTokens[indice].getLocation());
                agregarError(tokenError);
                indice--;
            }

            return nodeFactor;
        }

        private TreeNode Termino()
        {
            TreeNode nodeLeft = Factor();
            TreeNode nodeRight;
            TreeNode nodeRoot;
            if (isOperadorMayor(listaTokens[indice].getName()))
            {
                TreeNode nodeOperator = nodoActual();
                agregarNodo(nodeOperator, nodeLeft);
                indice++;
                nodeRight = Factor();
                agregarNodo(nodeOperator, nodeRight);
                while (isOperadorMayor(listaTokens[indice].getName()))
                {
                    nodeLeft = nodeOperator;
                    nodeRoot = nodoActual();
                    agregarNodo(nodeRoot, nodeLeft);
                    indice++;
                    nodeRight = Factor();
                    agregarNodo(nodeRoot, nodeRight);
                    nodeOperator = nodeRoot;
                }
                return nodeOperator;
            }
            else
            {
                return nodeLeft;
            }
        }

        private TreeNode ExpresionSimple()
        {
            TreeNode nodeLeft = Termino();
            TreeNode nodeRoot;
            TreeNode nodeRight;
            if (isSumaResta(listaTokens[indice].getName()))
            {
                listaTokens[indice].setTokenDescription(indice);
                TreeNode nodeOperator = new TreeNode(listaTokens[indice].getName());
                nodeOperator.Tag = indice;
                agregarNodo(nodeOperator, nodeLeft);
                indice++;
                nodeRight = Factor();
                if (isOperadorMayor(listaTokens[indice].getName()))
                {
                    indice--;
                    nodeRight = Expresion();
                }
                agregarNodo(nodeOperator, nodeRight);
                while ((isSumaResta(listaTokens[indice].getName())))
                {
                    nodeLeft = nodeOperator;
                    listaTokens[indice].setTokenDescription(indice);
                    nodeRoot = new TreeNode(listaTokens[indice].getName());
                    nodeRoot.Tag = indice;
                    agregarNodo(nodeRoot, nodeLeft);
                    indice++;
                    nodeRight = Termino();
                    agregarNodo(nodeRoot, nodeRight);
                    nodeOperator = nodeRoot;
                }
                return nodeOperator;
            }
            return nodeLeft;

        }

        private TreeNode Expresion()
        {
            TreeNode nodeTermino = ExpresionSimple();
            TreeNode nodeRight;
            if (isOperador(listaTokens[indice].getName()))
            {
                listaTokens[indice].setTokenDescription(indice);
                TreeNode nodeOperator = new TreeNode(listaTokens[indice].getName());
                nodeOperator.Tag = indice;
                agregarNodo(nodeOperator, nodeTermino);
                indice++;
                nodeRight = ExpresionSimple();
                agregarNodo(nodeOperator, nodeRight);
                return nodeOperator;
            }

            return nodeTermino;
        }

        private void sentencias(TreeNode nodeRoot)
        {

            if (listaTokens[indice].getName().Equals(analizador_lexico.Estados.tokenIf))
            {
                If(nodeRoot);
            }
            else if (listaTokens[indice].getName().Equals(analizador_lexico.Estados.tokenWhile))
            {
                While(nodeRoot);
            }
            else if (listaTokens[indice].getName().Equals(analizador_lexico.Estados.tokenDo))
            {
                repeat(nodeRoot);
            }
        }

        private void If(TreeNode nodeRoot)
        {
            listaTokens[indice].setTokenDescription(indice);
            TreeNode nodeIf = new TreeNode(listaTokens[indice].getName());
            nodeIf.Tag = indice;
            agregarNodo(nodeRoot, nodeIf);
            indice++;
            try
            {
                if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisA))
                {
                    indice++;
                    TreeNode nodeExpresion = Expresion();
                    agregarNodo(nodeIf, nodeExpresion);
                    if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisC))
                    {
                        listaTokens[indice].setTokenDescription(indice);
                        TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                        nodeError.Tag = indice;
                        agregarNodo(nodeIf, nodeError);
                        analizador_lexico.Token tokenError = new analizador_lexico.Token("Hace falta un PARENTESIS )", listaTokens[indice].getLocation());
                        agregarError(tokenError);
                        indice--;
                    }
                    indice++;
                    if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenThen))
                    {
                        listaTokens[indice].setTokenDescription(indice);
                        TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                        nodeError.Tag = indice;
                        agregarNodo(nodeIf, nodeError);
                        analizador_lexico.Token tokenError = new analizador_lexico.Token("Falta un THEN", listaTokens[indice].getLocation());
                        agregarError(tokenError);

                    }
                    indice--;
                }
                else
                {
                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                    nodeError.Tag = indice;
                    agregarNodo(nodeIf, nodeError);
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Hace falta un PARENTESIS (", listaTokens[indice].getLocation());
                    agregarError(tokenError);
                    indice--;
                }
                while (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenEnd))
                {
                    cuerpoIf(nodeIf);
                    indice++;
                }

                if (!tokenActual(listaTokens[indice - 1].getName(), analizador_lexico.Estados.tokenLlaveC) && tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenEnd))
                {
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Esperaba } antes de end", new analizador_lexico.ubicacionToken(listaTokens[indice].getLocation().getFila(), listaTokens[indice].getLocation().getColumna() - 1));
                    agregarError(tokenError);
                }


                if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenEnd))
                {
                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeEnd = new TreeNode(listaTokens[indice].getName());
                    nodeEnd.Tag = indice;
                    agregarNodo(nodeIf, nodeEnd);
                }
            }
            catch (Exception e)
            {
                analizador_lexico.ubicacionToken location = new analizador_lexico.ubicacionToken(listaTokens[listaTokens.Count - 1].getLocation().getFila() + 1, 0);
                analizador_lexico.Token tokenEnd = new analizador_lexico.Token("Falta End ", location);
                agregarError(tokenEnd);
            }
        }

        private void cuerpoIf(TreeNode nodeRoot)
        {
            try
            {
                indice++;
                while (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenEnd))
                {
                    if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenThen))
                    {
                        listaTokens[indice].setTokenDescription(indice);
                        TreeNode nodeThen = new TreeNode(listaTokens[indice].getName());
                        nodeThen.Tag = indice;
                        agregarNodo(nodeRoot, nodeThen);
                        Then(nodeThen);

                    }

                    else
                    if (isDeclaracion(listaTokens[indice].getName()))
                    {
                        declaraciones(nodeRoot);
                    }
                    else if (isSentencia(listaTokens[indice].getName()))
                    {
                        sentencias(nodeRoot);
                    }
                    else if (isCin(listaTokens[indice].getName()))
                    {
                        entradaCIN(nodeRoot);
                    }
                    else if (isCout(listaTokens[indice].getName()))
                    {
                        salidaCOUT(nodeRoot);
                    }
                    else if (isIdentificador(tipoTokens[indice]))
                    {
                        asignaciones(nodeRoot);
                    }
                    else if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenElse))
                    {
                        if (tokenActual(listaTokens[indice - 1].getName(), analizador_lexico.Estados.tokenLlaveC) && tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenElse))
                        {
                            listaTokens[indice].setTokenDescription(indice);
                            TreeNode nodeElse = new TreeNode(listaTokens[indice].getName());
                            nodeElse.Tag = indice;
                            agregarNodo(nodeRoot, nodeElse);
                            Else(nodeElse);
                        }
                        else
                        {

                            analizador_lexico.Token tokenError = new analizador_lexico.Token("Esperaba } antes de ELSE", new analizador_lexico.ubicacionToken(listaTokens[indice].getLocation().getFila(), listaTokens[indice].getLocation().getColumna() - 1));
                            agregarError(tokenError);

                        }

                    }
                    else
                    {

                    }
                    indice++;
                }
                indice--;
            }
            catch (Exception e)
            {
            }
        }

        private void Else(TreeNode nodeRoot)
        {
            try
            {
                indice++;
                if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveA) && tokenActual(listaTokens[indice - 1].getName(), analizador_lexico.Estados.tokenElse))
                {
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Esperaba { despues de else", new analizador_lexico.ubicacionToken(listaTokens[indice].getLocation().getFila() - 1, listaTokens[indice - 1].getLocation().getColumna() + 3));
                    agregarError(tokenError);

                }
                else
                {

                    indice++;

                    while (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenEnd))
                    {
                        if (isDeclaracion(listaTokens[indice].getName()))
                        {
                            declaraciones(nodeRoot);
                        }
                        else if (isSentencia(listaTokens[indice].getName()))
                        {
                            sentencias(nodeRoot);
                        }
                        else if (isCin(listaTokens[indice].getName()))
                        {
                            entradaCIN(nodeRoot);
                        }
                        else if (isCout(listaTokens[indice].getName()))
                        {
                            salidaCOUT(nodeRoot);
                        }
                        else if (isIdentificador(tipoTokens[indice]))
                        {
                            asignaciones(nodeRoot);
                        }
                        else
                        {
                            if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveC) && tokenActual(listaTokens[indice + 1].getName(), analizador_lexico.Estados.tokenEnd))
                            {
                            }
                            else
                            {

                                listaTokens[indice].setTokenDescription(indice);
                                TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                                nodeError.Tag = indice;
                                //agregarNodo(nodeRoot, nodeError);
                                agregarError(listaTokens[indice]);

                            }

                        }
                        indice++;
                    }
                }
                indice--;
            }
            catch (Exception e)
            {
            }
        }

        private void Then(TreeNode nodeRoot)
        {
            try
            {
                indice++;
                while (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenEnd) && !tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenElse))
                {
                    if (isDeclaracion(listaTokens[indice].getName()))
                    {
                        declaraciones(nodeRoot);
                    }
                    else if (isSentencia(listaTokens[indice].getName()))
                    {
                        sentencias(nodeRoot);
                    }
                    else if (isCin(listaTokens[indice].getName()))
                    {
                        entradaCIN(nodeRoot);
                    }
                    else if (isCout(listaTokens[indice].getName()))
                    {
                        salidaCOUT(nodeRoot);
                    }
                    else if (isIdentificador(tipoTokens[indice]))
                    {
                        asignaciones(nodeRoot);
                    }
                    else if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenEnd) && !tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenElse))
                    {

                        if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveC) && tokenActual(listaTokens[indice + 1].getName(), analizador_lexico.Estados.tokenEnd))
                        {
                        }
                        else if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveC) && tokenActual(listaTokens[indice + 1].getName(), analizador_lexico.Estados.tokenElse))
                        {

                        }
                        else
                        {

                            listaTokens[indice].setTokenDescription(indice);
                            TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                            nodeError.Tag = indice;
                            //agregarNodo(nodeRoot, nodeError);
                            //agregarError(listaTokens[indice]);

                        }

                    }
                    indice++;
                }
                indice--;
            }
            catch (Exception e)
            {
            }
        }

        private void While(TreeNode nodeRoot)
        {

            listaTokens[indice].setTokenDescription(indice);
            TreeNode nodeWhile = new TreeNode(listaTokens[indice].getName());
            nodeWhile.Tag = indice;
            agregarNodo(nodeRoot, nodeWhile);
            indice++;

            try
            {
                if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisA))
                {
                    indice++;
                    TreeNode nodeExpresion = Expresion();
                    agregarNodo(nodeWhile, nodeExpresion);
                    if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisC))
                    {
                        listaTokens[indice].setTokenDescription(indice);
                        TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                        nodeError.Tag = indice;
                        agregarNodo(nodeWhile, nodeError);
                        analizador_lexico.Token tokenError = new analizador_lexico.Token("Falta un PARETNTESIS )", listaTokens[indice].getLocation());
                        agregarError(tokenError);
                        indice--;
                    }
                }

                else
                {
                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                    nodeError.Tag = indice;
                    agregarNodo(nodeWhile, nodeError);
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Hace falta un PARENTESIS (", listaTokens[indice].getLocation());
                    agregarError(tokenError);
                    indice--;
                }

                indice++;
                if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveA))
                {
                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeError = new TreeNode(listaTokens[indice].getName());
                    nodeError.Tag = indice;
                    agregarNodo(nodeWhile, nodeError);
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Se esperaba una LLAVE { despues del WHILE", new analizador_lexico.ubicacionToken(listaTokens[indice].getLocation().getFila() - 1, listaTokens[indice - 1].getLocation().getColumna() + 1));
                    agregarError(tokenError);
                    indice--;
                }

                while (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveC))
                {
                    cuerpoWhile(nodeWhile);
                    indice++;

                }

                if (!tokenActual(listaTokens[indice + 1].getName(), analizador_lexico.Estados.tokenLlaveC))
                {
                    if (tokenActual(listaTokens[indice + 1].getName(), analizador_lexico.Estados.tokenPuntoComa))
                    {

                    }
                    else

                    {

                    }
                }
            }
            catch (Exception e)
            {
                if (!tokenActual(listaTokens[listaTokens.Count - 1].getName(), analizador_lexico.Estados.tokenLlaveC) && !tokenActual(listaTokens[listaTokens.Count - 2].getName(), analizador_lexico.Estados.tokenLlaveC))
                {
                    analizador_lexico.ubicacionToken location = new analizador_lexico.ubicacionToken(listaTokens[listaTokens.Count - 1].getLocation().getFila() + 1, 0);
                    analizador_lexico.Token tokenEnd = new analizador_lexico.Token("Se esperaba una LLAVE }", location);
                    agregarError(tokenEnd);
                }
                else
                {
                    analizador_lexico.ubicacionToken location = new analizador_lexico.ubicacionToken(listaTokens[listaTokens.Count - 1].getLocation().getFila() + 1, 0);
                    analizador_lexico.Token tokenEnd = new analizador_lexico.Token("Se esperaba una LLAVE }", location);
                    agregarError(tokenEnd);
                }

            }
        }

        private void cuerpoWhile(TreeNode nodeRoot)
        {
            try
            {
                indice++;
                while (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveC))
                {
                    if (isDeclaracion(listaTokens[indice].getName()))
                    {
                        declaraciones(nodeRoot);
                    }
                    else if (isSentencia(listaTokens[indice].getName()))
                    {
                        sentencias(nodeRoot);
                    }
                    else if (isCin(listaTokens[indice].getName()))
                    {
                        entradaCIN(nodeRoot);
                    }
                    else if (isCout(listaTokens[indice].getName()))
                    {
                        salidaCOUT(nodeRoot);
                    }
                    else if (isIdentificador(tipoTokens[indice]))
                    {
                        asignaciones(nodeRoot);
                    }
                    else
                    {
                        listaTokens[indice].setTokenDescription(indice);
                        TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                        nodeError.Tag = indice;
                        //agregarNodo(nodeRoot, nodeError);
                        // agregarError(listaTokens[indice]);
                    }
                    indice++;
                }
                indice--;
            }
            catch (Exception e)
            {
            }
        }

        private void repeat(TreeNode nodeRoot)
        {
            listaTokens[indice].setTokenDescription(indice);
            TreeNode nodeRepeat = new TreeNode(listaTokens[indice].getName());
            nodeRepeat.Tag = indice;
            agregarNodo(nodeRoot, nodeRepeat);
            indice++;

            try
            {

                if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveA))
                {

                    listaTokens[indice].setTokenDescription(indice);
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Esperaba { despues del repeat", new analizador_lexico.ubicacionToken(listaTokens[indice].getLocation().getFila() - 1, listaTokens[indice - 1].getLocation().getColumna() + 5));
                    agregarError(tokenError);

                }
                else
                {
                    indice++;
                }

                while (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenUntil))
                {

                    cuerpoRepeat(nodeRepeat);
                    indice++;


                }

                if (!tokenActual(listaTokens[indice - 1].getName(), analizador_lexico.Estados.tokenLlaveC) && tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenUntil))
                {
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Se necesita } antes de UNTIL", listaTokens[listaTokens.Count - 2].getLocation());
                    agregarError(tokenError);
                }

                if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenUntil))
                {
                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeUntil = new TreeNode(listaTokens[indice].getName());
                    nodeUntil.Tag = indice;
                    agregarNodo(nodeRepeat, nodeUntil);
                    indice++;
                    if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisA))
                    {
                        indice++;
                        TreeNode nodeExpresion = Expresion();
                        agregarNodo(nodeUntil, nodeExpresion);
                        if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisC))
                        {
                            listaTokens[indice].setTokenDescription(indice);
                            TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                            nodeError.Tag = indice;
                            agregarNodo(nodeUntil, nodeError);
                            analizador_lexico.Token tokenError = new analizador_lexico.Token("Falta un PARETNTESIS )", listaTokens[indice].getLocation());
                            agregarError(tokenError);
                            indice--;
                        }
                        if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenParentesisC) && !tokenActual(listaTokens[indice + 1].getName(), analizador_lexico.Estados.tokenPuntoComa))
                        {

                            analizador_lexico.Token tokenError = new analizador_lexico.Token("Se necesita un PUNTO Y COMA ;", listaTokens[indice].getLocation());
                            agregarError(tokenError);


                        }
                        else if (tokenActual(listaTokens[indice + 1].getName(), analizador_lexico.Estados.tokenPuntoComa))
                        {

                        }
                    }
                    else
                    {
                        listaTokens[indice].setTokenDescription(indice);
                        TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                        nodeError.Tag = indice;
                        agregarNodo(nodeUntil, nodeError);
                        analizador_lexico.Token tokenError = new analizador_lexico.Token("Hace falta un PARENTESIS (", listaTokens[indice].getLocation());
                        agregarError(tokenError);
                        indice--;
                    }

                    if (!tokenActual(listaTokens[indice - 1].getName(), analizador_lexico.Estados.tokenLlaveC) && tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenUntil))
                    {
                        analizador_lexico.Token tokenError = new analizador_lexico.Token("Se necesita } antes de UNTIL", listaTokens[listaTokens.Count - 1].getLocation());
                        agregarError(tokenError);
                    }



                }
                else
                {
                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                    nodeError.Tag = indice;
                    agregarNodo(nodeRepeat, nodeError);
                    //  agregarError(listaTokens[indice]);
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Falta un UNTIL", listaTokens[indice].getLocation());
                    agregarError(tokenError);
                    indice--;
                    return;
                }


            }
            catch (Exception e)
            {
                if (!tokenActual(listaTokens[listaTokens.Count - 1].getName(), analizador_lexico.Estados.tokenLlaveC) && !tokenActual(listaTokens[listaTokens.Count - 2].getName(), analizador_lexico.Estados.tokenLlaveC))
                {
                    analizador_lexico.ubicacionToken location = new analizador_lexico.ubicacionToken(listaTokens[listaTokens.Count - 1].getLocation().getFila() + 1, 0);
                    analizador_lexico.Token tokenEnd = new analizador_lexico.Token("Falta una LLAVE } al final del UNTIL", location);
                    agregarError(tokenEnd);
                }
                else
                {
                    analizador_lexico.ubicacionToken location = new analizador_lexico.ubicacionToken(listaTokens[listaTokens.Count - 1].getLocation().getFila() - 1, listaTokens[listaTokens.Count - 1].getLocation().getColumna() + 1);
                    analizador_lexico.Token tokenEnd = new analizador_lexico.Token("Falta un UNTIL despues de la LLAVE }", location);
                    agregarError(tokenEnd);
                }
            }


        }

        private void cuerpoRepeat(TreeNode nodeRoot)
        {
            if (isDeclaracion(listaTokens[indice].getName()))
            {
                declaraciones(nodeRoot);
            }
            else if (isSentencia(listaTokens[indice].getName()))
            {
                sentencias(nodeRoot);
            }
            else if (isCin(listaTokens[indice].getName()))
            {
                entradaCIN(nodeRoot);
            }
            else if (isCout(listaTokens[indice].getName()))
            {
                salidaCOUT(nodeRoot);
            }
            else if (isIdentificador(tipoTokens[indice]))
            {
                asignaciones(nodeRoot);
            }
            else
            {
                if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveC) && !tokenActual(listaTokens[indice + 1].getName(), analizador_lexico.Estados.tokenUntil))
                {
                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                    nodeError.Tag = indice;
                    //agregarNodo(nodeRoot, nodeError);
                    //agregarError(listaTokens[indice]);
                }
                else if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenLlaveC) && !tokenActual(listaTokens[indice + 1].getName(), analizador_lexico.Estados.tokenUntil))
                {

                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                    nodeError.Tag = indice;
                    //agregarNodo(nodeRoot, nodeError);
                    // agregarError(listaTokens[indice]);

                }

            }

        }

        private void entradaCIN(TreeNode nodeRoot)
        {
            try
            {
                listaTokens[indice].setTokenDescription(indice);
                TreeNode nodeCin = new TreeNode(listaTokens[indice].getName());
                nodeCin.Tag = indice;
                agregarNodo(nodeRoot, nodeCin);
                indice++;
                if (isIdentificador(tipoTokens[indice]))
                {
                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeId = new TreeNode(listaTokens[indice].getName());
                    nodeId.Tag = indice;
                    agregarNodo(nodeCin, nodeId);
                    indice++;
                    if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenPuntoComa))
                    {
                        //agregarError(listaTokens[indice]);
                        indice--;
                    }
                }
                else
                {
                    // agregarError(listaTokens[indice]);
                    if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenPuntoComa))
                    {
                        listaTokens[indice].setTokenDescription(indice);
                        TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                        nodeError.Tag = indice;
                        agregarNodo(nodeCin, nodeError);
                    }
                    indice--;
                }
            }
            catch (Exception e)
            {
            }
        }

        private void salidaCOUT(TreeNode nodeRoot)
        {
            listaTokens[indice].setTokenDescription(indice);
            TreeNode nodeCout = new TreeNode(listaTokens[indice].getName());
            nodeCout.Tag = indice;
            agregarNodo(nodeRoot, nodeCout);
            indice++;
            try
            {
                TreeNode nodeChild = Expresion();
                agregarNodo(nodeCout, nodeChild);
                if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenPuntoComa))
                {
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Se necesita un PUNTO Y COMA ;", listaTokens[indice].getLocation());
                    agregarError(tokenError);
                    indice--;
                }
            }
            catch (Exception e)
            {
            }
        }

        private void asignaciones(TreeNode nodeRoot)
        {
            listaTokens[indice].setTokenDescription(indice);
            TreeNode nodeId = new TreeNode(listaTokens[indice].getName());
            nodeId.Tag = indice;
            indice++;
            if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenAsignacion))
            {
                listaTokens[indice].setTokenDescription(indice);
                TreeNode nodeAsign = new TreeNode(listaTokens[indice].getName());
                nodeAsign.Tag = indice;
                agregarNodo(nodeRoot, nodeAsign);
                agregarNodo(nodeAsign, nodeId);
                indice++;

                TreeNode nodeExp = Expresion();
                agregarNodo(nodeAsign, nodeExp);
                if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenPuntoComa))
                {
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Se necesita un PUNTO Y COMA ;", listaTokens[indice].getLocation());
                    agregarError(tokenError);
                    indice--;
                }
            }
            else if (isDecremento(listaTokens[indice].getName()) || isIncremento(listaTokens[indice].getName()))
            {
                listaTokens[indice].setTokenDescription(indice);
                TreeNode nodeAsign = new TreeNode(analizador_lexico.Estados.tokenAsignacion);
                nodeAsign.Tag = indice;
                agregarNodo(nodeRoot, nodeAsign);
                agregarNodo(nodeAsign, nodeId);
                listaTokens[indice - 1].setTokenDescription(indice - 1);
                TreeNode nodeLeft = new TreeNode(listaTokens[indice - 1].getName());
                nodeLeft.Tag = indice - 1;
                listaTokens[indice].setTokenDescription(indice);
                TreeNode nodeOperador = new TreeNode(isDecremento(listaTokens[indice].getName()) ? analizador_lexico.Estados.tokenMenos : analizador_lexico.Estados.tokenMas);
                nodeOperador.Tag = indice;
                listaTokens[indice].setTokenDescription(indice);
                TreeNode nodeRight = new TreeNode("1");
                nodeRight.Tag = indice;
                agregarNodo(nodeAsign, nodeOperador);
                agregarNodo(nodeOperador, nodeLeft);
                agregarNodo(nodeOperador, nodeRight);
                indice++;
                if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenPuntoComa))
                {
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Se necesita un PUNTO Y COMA ;", listaTokens[indice].getLocation());
                    agregarError(tokenError);
                    indice--;
                }
            }
            else
            {
                //agregarError(listaTokens[indice]);
                listaTokens[indice].setTokenDescription(indice);
                TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                nodeError.Tag = indice;
                //agregarNodo(nodeRoot, nodeError);
            }

        }

        /*private void declaraciones(TreeNode nodeRoot)
        {
            try
            {
                listaTokens[indice].setTokenDescription(indice);
                TreeNode nodeType = new TreeNode(listaTokens[indice].getName());
                nodeType.Tag = indice;
                agregarNodo(nodeRoot, nodeType);
                indice++;
                Boolean isFirstId = false;
                while (!listaTokens[indice].getName().Equals(analizador_lexico.Estados.tokenPuntoComa))
                {
                    if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenComa))
                    {
                        if (!isFirstId)
                        {
                            analizador_lexico.Token tokenError = new analizador_lexico.Token("Falta un IDENTIFICADOR", listaTokens[indice].getLocation());
                            agregarError(tokenError);
                            listaTokens[indice].setTokenDescription(indice);
                            TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                            nodeError.Tag = indice;
                            agregarNodo(nodeType, nodeError);
                            isFirstId = true;
                        }
                        else
                        {
                            isFirstId = false;
                        }
                    }
                    else if (isIdentificador(tipoTokens[indice]))
                    {
                        listaTokens[indice].setTokenDescription(indice);
                        TreeNode nodeId = new TreeNode(listaTokens[indice].getName());
                        nodeId.Tag = indice;
                        agregarNodo(nodeType, nodeId);
                        isFirstId = true;
                    }
                    else
                    {
                        analizador_lexico.Token tokenError = new analizador_lexico.Token("Error ", listaTokens[indice].getLocation());
                        agregarError(tokenError);

                        listaTokens[indice].setTokenDescription(indice);
                       // TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                       // nodeError.Tag = indice;
                       // agregarNodo(nodeType, nodeError);
                        if (indice == listaTokens.Count - 1)
                        {
                          indice--;
                            return;
                        }
                       // indice--;
                        isFirstId = true;
                        break;
                    }
                    indice++;
                }
                if (!isFirstId)
                {
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Falta un IDENTIFICADOR", listaTokens[indice].getLocation());
                    agregarError(tokenError);
                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeError = new TreeNode("(Error)" + listaTokens[indice].getName());
                    nodeError.Tag = indice;
                    agregarNodo(nodeType, nodeError);
                }

                if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenPuntoComa))
                {

                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Se necesita un PUNTO Y COMA ;", listaTokens[indice].getLocation());
                    agregarError(tokenError);
                    indice--;
                }
            }
            catch (Exception e)
            {

                analizador_lexico.Token tokenPuntoComa = new analizador_lexico.Token("(Error)" + analizador_lexico.Estados.tokenPuntoComa, listaTokens[listaTokens.Count - 1].getLocation());
                agregarError(tokenPuntoComa);
            }

        }*/

        private void declaraciones(TreeNode nodeRoot)
        {
            try
            {
                
                listaTokens[indice].setTokenDescription(indice);
                TreeNode nodeType = new TreeNode(listaTokens[indice].getName());
                nodeType.Tag = indice;
                agregarNodo(nodeRoot, nodeType);
                indice++;
                Boolean isFirstId = false;
                while (!listaTokens[indice].getName().Equals(analizador_lexico.Estados.tokenPuntoComa))
                {
                    if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenComa))
                    {
                        if (!isFirstId)
                        {
                            analizador_lexico.Token tokenError = new analizador_lexico.Token("Se esperaba Identificador", listaTokens[indice].getLocation());
                            agregarError(tokenError);
                            listaTokens[indice].setTokenDescription(indice);
                            TreeNode nodeError = new TreeNode("Error: " + listaTokens[indice].getName());
                            nodeError.Tag = indice;
                            agregarNodo(nodeType, nodeError);
                            isFirstId = true;
                        }
                        else
                        {
                            isFirstId = false;
                        }
                    }
                    else if (isIdentificador(tipoTokens[indice]))
                    {
                        if (!tokenActual(listaTokens[indice].getName(), "suma"))
                        {
                            Console.WriteLine("Alto ahi soldado 1" + listaTokens[indice].getName());
                            listaTokens[indice].setTokenDescription(indice);
                            TreeNode nodeId = new TreeNode(listaTokens[indice].getName());
                            nodeId.Tag = indice;
                            agregarNodo(nodeType, nodeId);
                            isFirstId = true;
                        }

                    }
                    else
                    {

                        //  analizador_lexico.Token tokenError = new analizador_lexico.Token("Error ", listaTokens[indice].getLocation());
                        //  agregarError(tokenError);
                        if (indice == listaTokens.Count - 1)
                        {
                            indice--;
                            return;
                        }
                        //  indice++;
                        //   indice--;
                        isFirstId = true;
                        break;

                    }
                    indice++;
                }
                if (!isFirstId)
                {
                    analizador_lexico.Token tokenError = new analizador_lexico.Token("Se esperaba Identificador 0 ", listaTokens[indice].getLocation());
                    agregarError(tokenError);
                    listaTokens[indice].setTokenDescription(indice);
                    TreeNode nodeError = new TreeNode("Error: " + listaTokens[indice].getName());
                    nodeError.Tag = indice;
                    agregarNodo(nodeType, nodeError);
                }

                if (!tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenPuntoComa))
                {
                    if (tokenActual(listaTokens[indice].getName(), analizador_lexico.Estados.tokenAsignacion))
                    {
                        indice--;
                        indice--;
                        analizador_lexico.Token tokenError = new analizador_lexico.Token("Esperaba ; en identificador 1 ", listaTokens[indice].getLocation());
                        agregarError(tokenError);
                    }
                    else
                    {
                        analizador_lexico.Token tokenError = new analizador_lexico.Token("Esperaba ; en identificador 2 ", listaTokens[indice].getLocation());
                        agregarError(tokenError);
                        indice--;
                    }

                }
            }
            catch (Exception e)
            {

                analizador_lexico.Token tokenPuntoComa = new analizador_lexico.Token("Falta " + analizador_lexico.Estados.tokenPuntoComa, listaTokens[listaTokens.Count - 1].getLocation());
                agregarError(tokenPuntoComa);
                Console.Out.WriteLine(e.GetBaseException());
            }

        }

        private Boolean tokenActual(String currentToken, String token)
        {
            return currentToken.Equals(token);
        }

        private Boolean isMain()
        {
            return tokenActual(listaTokens[indice++].getName(), analizador_lexico.Estados.tokenMain);
        }

        public String Errores()
        {
            return errores.ToString();
        }

        private Boolean isDeclaracion(String token)
        {
            return token.Equals(analizador_lexico.Estados.tokenInt) || token.Equals(analizador_lexico.Estados.tokenBoolean) || token.Equals(analizador_lexico.Estados.tokenFloat);
        }

        private String nodoError(analizador_lexico.Token token)
        {
            return "(Error)" + token.getName();
        }

        private Boolean isSentencia(String token)
        {
            return token.Equals(analizador_lexico.Estados.tokenIf) || token.Equals(analizador_lexico.Estados.tokenWhile) || token.Equals(analizador_lexico.Estados.tokenDo);
        }

        private Boolean isOperador(String token)
        {
            return token.Equals(analizador_lexico.Estados.tokenMenor) || token.Equals(analizador_lexico.Estados.tokenMenorIgual) || token.Equals(analizador_lexico.Estados.tokenMayor) || token.Equals(analizador_lexico.Estados.tokenMayorIgual)
                || token.Equals(analizador_lexico.Estados.tokenIgualdad) || token.Equals(analizador_lexico.Estados.tokenDiferente);
        }

        private Boolean isNumero(String token)
        {
            return token.Equals(analizador_lexico.Estados.numeroFlotanteSS) || token.Equals(analizador_lexico.Estados.numeroFlotanteCS) || token.Equals(analizador_lexico.Estados.numeroEnteroSS)
                || token.Equals(analizador_lexico.Estados.tokenNumeroEnteroCS);
        }

        private Boolean isIdentificador(String token)
        {
            return token.Equals(analizador_lexico.Estados.identificador);
        }

        private Boolean isOperadorMayor(String token)
        {
            return token.Equals(analizador_lexico.Estados.tokenMultiplica) || token.Equals(analizador_lexico.Estados.tokenDivision) || token.Equals(analizador_lexico.Estados.modulo);
        }

        private Boolean isSumaResta(String token)
        {
            return token.Equals(analizador_lexico.Estados.tokenMas) || token.Equals(analizador_lexico.Estados.tokenMenos);
        }

        private Boolean isIncremento(String token)
        {
            return token.Equals(analizador_lexico.Estados.tokenIncremento);
        }

        private Boolean isDecremento(String token)
        {
            return token.Equals(analizador_lexico.Estados.tokenDecremento);
        }

        private Boolean isCin(String token)
        {
            return token.Equals(analizador_lexico.Estados.tokenCin);
        }

        private Boolean isCout(String token)
        {
            return token.Equals(analizador_lexico.Estados.tokenCout);
        }
    }
}
