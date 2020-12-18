using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Codigo_Intermedio
{
    class VirtualMachine
    {
        #region "Bloque de Variables"
        List<string> list_Codigo;
        private struct sHashTable
        {
            public string strVarName;
            public string strType;
            public string strValor;
            public int intHashCode;
        }
        private sHashTable[,] vHashTable;
        int intSiseRows = 211, intSiseCols = 5;
        int intHashCode, intCollCode;
        string SPACE = " ";
        int i;
        string izq, der, res;
        static public string value;
        StringBuilder sbResultados;
        Stack<string> stackIf;
        Stack<string> stackWhileS;
        Stack<int> stackWhileI;
        Stack<string> stackDoS;
        Stack<int> stackDoI;
        private DataTable dtTable;
        //cRegistros vRegistros;
        #endregion // Termina "Bloque de Variables"
        #region "Bloque de Registros"
        string[] Registros = { "$reg0", "$reg1", "$reg2", "$reg3", "$reg4", "$reg5", "$reg6", "$reg7", "$reg8", "$reg9" };
        bool[] boolReg = new bool[10];
        #endregion // Termina "Bloque de Registros"
        #region "Métodos de inicialización de datos"
        public VirtualMachine(StringBuilder sbCodigo)
        {
            list_Codigo = new List<string>();
            char[] charAux = sbCodigo.ToString().ToCharArray();
            string strAux = string.Empty;
            for (int i = 0; i < charAux.Length; i++)
            {
                if (charAux[i] != '\n')
                {
                    if (charAux[i] != '\r')
                        strAux += charAux[i].ToString();
                }
                else
                {
                    list_Codigo.Add(strAux);
                    strAux = string.Empty;
                }
            }
            mVoidMakeTable();
        }
        private void mVoidInitRegistros()
        {
            for (int j = 0; j < boolReg.Length; j++)
                vHashTable[mIntHashCode(Registros[j]), mIntColColision(Registros[j])].strVarName = Registros[j];
        }
        private void mVoidMakeTable()
        {
            dtTable = new DataTable();
            dtTable.Columns.Add("Variable", typeof(string));
            dtTable.Columns.Add("Tipo", typeof(string));
            dtTable.Columns.Add("Valor", typeof(string));
            dtTable.Columns.Add("Hash Code", typeof(string));
        }
        public void Compilar()
        {
            vHashTable = new sHashTable[intSiseRows, intSiseCols];
            mVoidInitRegistros();
            sbResultados = new StringBuilder();
            stackIf = new Stack<string>();
            stackWhileS = new Stack<string>();
            stackWhileI = new Stack<int>();
            stackDoS = new Stack<string>();
            stackDoI = new Stack<int>();
            for (i = 0; i < list_Codigo.Count; i++)
            {
                if (isSetId(list_Codigo[i]))                // Para las declaraciones y Asignaciones. // en si, una declaracion, es una asignación de un "null".
                    mSetId();
                else
                {
                    if (isCin(list_Codigo[i]))          // Para introducir valores por teclado
                        mSetCin();
                    else
                    {
                        if (isCout(list_Codigo[i]))     // para imprimir resultados en pantalla
                            mSetCout();
                        else
                        {
                            if (isBreak(list_Codigo[i]))    // Para saltar etiquetas en ciclos
                                mSetBreak();
                            else
                            {
                                if (isIf(list_Codigo[i]))   // Para el inicio de un if
                                    mSetIf();
                                else
                                {
                                    if (isWhile(list_Codigo[i]))    // para el inicio de un while
                                        mSetWhile();
                                    else
                                    {
                                        if (isRepeat(list_Codigo[i]))   // para el inicio de un do-until
                                            mSetDo();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            mVoidLlenaTabla();
        }
        private void mVoidLlenaTabla()
        {
            for (int i = 0; i < intSiseRows; i++)
            {
                for (int j = 0; j < intSiseCols; j++)
                {
                    if (vHashTable[i, j].strVarName != null)
                        dtTable.Rows.Add(vHashTable[i, j].strVarName, vHashTable[i, j].strType, vHashTable[i, j].strValor, vHashTable[i, j].intHashCode);
                }
            }
        }
        public StringBuilder getResultados()
        {
            return sbResultados;
        }
        public DataTable getRegistros()
        {
            return dtTable;
        }
        #endregion // Termina "Métodos de inicialización de datos"
        #region "Métodos booleanos para tomar desición de proceso"
        private bool isSetId(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strSetId_Boolean) != -1 || strLabel.IndexOf(CodigoIntermedioEtiqueta.strSetId_Int) != -1 ||
                strLabel.IndexOf(CodigoIntermedioEtiqueta.strSetId_Float) != -1)
                return true;
            return false;
        }
        private bool isGetId(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strGetId_Boolean) != -1 || strLabel.IndexOf(CodigoIntermedioEtiqueta.strGetId_Int) != -1 ||
                strLabel.IndexOf(CodigoIntermedioEtiqueta.strGetId_Float) != -1)
                return true;
            return false;
        }
        private bool isCin(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strCin) != -1)
                return true;
            return false;
        }
        private bool isCout(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strCout) != -1)
                return true;
            return false;
        }
        private bool isBreak(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strBreak) != -1)
                return true;
            return false;
        }
        private bool isIf(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strIf) != -1)
                return true;
            return false;
        }
        private bool isElse(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strElse) != -1)
                return true;
            return false;
        }
        private bool isEnd(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strEnd) != -1)
                return true;
            return false;
        }
        private bool isWhile(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strWhile) != -1)
                return true;
            return false;
        }
        private bool isWhileReturn(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strWhileReturn) != -1)
                return true;
            return false;
        }
        private bool isWhileOut(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strWhileOut) != -1)
                return true;
            return false;
        }
        private bool isRepeat(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strRepeat) != -1)
                return true;
            return false;
        }
        private bool isUntil(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strUntil) != -1)
                return true;
            return false;
        }
        private bool isConstante(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strGetConst_Boolean) != -1 || strLabel.IndexOf(CodigoIntermedioEtiqueta.strGetConst_Int) != -1 ||
                strLabel.IndexOf(CodigoIntermedioEtiqueta.strGetConst_Float) != -1)
                return true;
            return false;
        }
        private bool isOperador(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strSuma) != -1 || strLabel.IndexOf(CodigoIntermedioEtiqueta.strResta) != -1 ||
                strLabel.IndexOf(CodigoIntermedioEtiqueta.strMult) != -1 || strLabel.IndexOf(CodigoIntermedioEtiqueta.strDivision) != -1)
                return true;
            return false;
        }

        private bool isSuma(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strSuma) != -1)
                return true;
            return false;
        }
        private bool isResta(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strResta) != -1)
                return true;
            return false;
        }
        private bool isMulti(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strMult) != -1)
                return true;
            return false;
        }
        private bool isDivicion(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strDivision) != -1)
                return true;
            return false;
        }
        private bool isBoolean(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strBoolean) != -1)
                return true;
            return false;
        }
        private bool isInt(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strInt) != -1)
                return true;
            return false;
        }
        private bool isReal(string strLabel)
        {
            if (strLabel.IndexOf(CodigoIntermedioEtiqueta.strFloat) != -1)
                return true;
            return false;
        }

        private bool isMayorQue(string strLabel)
        {
            if (strLabel.IndexOf(">") != -1 && strLabel.IndexOf(">=") == -1)
                return true;
            return false;
        }
        private bool isMayorIgual(string strLabel)
        {
            if (strLabel.IndexOf(">=") != -1)
                return true;
            return false;
        }
        private bool isMenorQue(string strLabel)
        {
            if (strLabel.IndexOf("<") != -1 && strLabel.IndexOf("<=") == -1)
                return true;
            return false;
        }
        private bool isMenorIgual(string strLabel)
        {
            if (strLabel.IndexOf("<=") != -1)
                return true;
            return false;
        }
        private bool isIgualIgual(string strLabel)
        {
            if (strLabel.IndexOf("==") != -1)
                return true;
            return false;
        }
        private bool isDiferente(string strLabel)
        {
            if (strLabel.IndexOf("!=") != -1)
                return true;
            return false;
        }

        private bool isTrueBreak(string izq, string op, string der)
        {

            if (isMayorQue(op))
            {
                if (float.Parse(izq) > float.Parse(der)) //calvo
                  
                    return true;
            }
            else
            {
                if (isMayorIgual(op))
                {
                    if (float.Parse(izq) >= float.Parse(der))
                    return true;

                }
                else
                {
                    if (isMenorQue(op))
                    {
                        if (float.Parse(izq) < float.Parse(der))
                            return true;
                    }
                    else
                    {
                        if (isMenorIgual(op))
                        {
                            if (float.Parse(izq) <= float.Parse(der))
                                return true;
                        }
                        else
                        {
                            if (isIgualIgual(op))
                            {
                                if (float.Parse(izq) == float.Parse(der))
                                    return true;
                            }
                            else
                            {
                                if (isDiferente(op))
                                {
                                    if (float.Parse(izq) != float.Parse(der))
                                         return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        private bool isNumber(string strCadena)
        {
            char[] charCadena = strCadena.ToCharArray();
            for (int i = 0; i < charCadena.Length; i++)
            {
                if (Char.IsLetter(charCadena[i]))
                    return false;
            }
            return true;
        }
        #endregion // Termina "Métodos booleanos para tomar desición de proceso"
        #region "Métodos para realizar proceso de compilación"
        private void mSetId()
        {
            string id = strGetId(list_Codigo[i]);
            intHashCode = mIntHashCode(id);
            intCollCode = mIntColColision(id);
            string value = strGetValue(list_Codigo[i]);
            if (value.IndexOf("&") != -1)
                value = mGetValue(list_Codigo[i]);
            if (vHashTable[intHashCode, intCollCode].strVarName == null)        // Se carga el registro para la variable por primera vez
            {
                vHashTable[intHashCode, intCollCode].strVarName = id;
                vHashTable[intHashCode, intCollCode].intHashCode = intHashCode;
                vHashTable[intHashCode, intCollCode].strValor = value;
                vHashTable[intHashCode, intCollCode].strType = strGetType(list_Codigo[i]);
            }
            else                                                                // Se actualiza el valor de la variable
                vHashTable[intHashCode, intCollCode].strValor = value;
        }
        private string mGetValue(string strLabel)
        {
            izq = null;
            der = null;
            res = null;
            string type = strGetType(list_Codigo[i]);
            i++;
            for (int j = 0; j < boolReg.Length; j++)
                boolReg[j] = false;
            do
            {
                if (isConstante(list_Codigo[i]))
                {
                    if (izq == null)
                    {
                        izq = strGetId(list_Codigo[i]);
                        if (izq.Equals("%"))                // Obtener valor del registro N
                        {
                            for (int j = boolReg.Length - 1; j >= 0; j--)
                            {
                                if (boolReg[j])
                                {
                                    izq = vHashTable[mIntHashCode(Registros[j]), mIntColColision(Registros[j])].strValor;
                                    boolReg[j] = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (der == null)
                        {
                            der = strGetId(list_Codigo[i]);
                            if (der.Equals("%"))          // Obtener valor del registro N
                            {
                                for (int j = boolReg.Length - 1; j >= 0; j--)
                                {
                                    if (boolReg[j])
                                    {
                                        der = vHashTable[mIntHashCode(Registros[j]), mIntColColision(Registros[j])].strValor;
                                        boolReg[j] = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (isGetId(list_Codigo[i]))
                    {
                        if (izq == null)
                        {
                            izq = strGetId(list_Codigo[i]);
                            izq = vHashTable[mIntHashCode(izq), mIntColColision(izq)].strValor;
                        }
                        else
                        {
                            if (der == null)
                            {
                                der = strGetId(list_Codigo[i]);
                                der = vHashTable[mIntHashCode(der), mIntColColision(der)].strValor;
                            }
                        }
                    }
                    else
                    {
                        if (isOperador(list_Codigo[i]))
                        {
                            int hashCode = 0;
                            int CollCode = 0;
                            for (int j = 0; j < boolReg.Length; j++)            // Se posiciona en un registro disponible.
                            {
                                if (!boolReg[j])
                                {
                                    hashCode = mIntHashCode(Registros[j]);
                                    CollCode = mIntColColision(Registros[j]);
                                    boolReg[j] = true;
                                    break;
                                }
                            }
                            vHashTable[hashCode, CollCode].strValor = mGetOpera(izq, list_Codigo[i], der, type);
                            izq = null;
                            der = null;
                        }
                    }
                }
                i++;
            } while (!list_Codigo[i].Equals(CodigoIntermedioEtiqueta.strFin));
            for (int j = boolReg.Length - 1; j >= 0; j--)
            {
                if (boolReg[j])
                {
                    res = vHashTable[mIntHashCode(Registros[j]), mIntColColision(Registros[j])].strValor;
                    break;
                }
            }
            if (res == null)
                res = izq;
            return res;
        }
        private string mGetOpera(string izq, string ope, string der, string type)
        {
            string res = null;
            if (isSuma(ope))
            {
                if (type.Equals(CodigoIntermedioEtiqueta.strInt))  // Es Int
                    res = (Int32.Parse(izq) + Int32.Parse(der)).ToString();
                else                                        // Es Real
                    res = (float.Parse(izq) + float.Parse(der)).ToString();
            }
            else
            {
                if (isResta(ope))
                {
                    if (type.Equals(CodigoIntermedioEtiqueta.strInt))  // Es Int
                        res = (Int32.Parse(izq) - Int32.Parse(der)).ToString();
                    else                                        // Es Real
                        res = (float.Parse(izq) - float.Parse(der)).ToString();
                }
                else
                {
                    if (isMulti(ope))
                    {
                        if (type.Equals(CodigoIntermedioEtiqueta.strInt))  // Es Int
                            res = (Int32.Parse(izq) * Int32.Parse(der)).ToString();
                        else                                        // Es Real
                            res = (float.Parse(izq) * float.Parse(der)).ToString();
                    }
                    else
                    {
                        if (isDivicion(ope))
                        {
                            if (type.Equals(CodigoIntermedioEtiqueta.strInt))  // Es Int
                                res = (Int32.Parse(izq) / Int32.Parse(der)).ToString();
                            else                                        // Es Real
                                res = (float.Parse(izq) / float.Parse(der)).ToString();
                        }
                    }
                }
            }
            return res;
        }
        private void mSetCin()
        {
            string id = strGetId(list_Codigo[i]);
            intHashCode = mIntHashCode(id);
            intCollCode = mIntColColision(id);
            VirtualMachine.value = "0";
            char type;
            if (vHashTable[intHashCode, intCollCode].strType.Equals(CodigoIntermedioEtiqueta.strInt))
                type = 'i';
            else
                type = 'r';
            new CinInput(id, type).ShowDialog();
            vHashTable[intHashCode, intCollCode].strValor = VirtualMachine.value;


        }
        private void mSetCout()
        {
            string id = strGetId(list_Codigo[i]);
            intHashCode = mIntHashCode(id);
            intCollCode = mIntColColision(id);
            // Int id = val
            sbResultados.Append(vHashTable[intHashCode, intCollCode].strType + " " + id + " = " + vHashTable[intHashCode, intCollCode].strValor);
            sbResultados.AppendLine();
        }
        private void mSetBreak()
        {
            if (list_Codigo[i].IndexOf("$") != -1)
                return;
            string nextLabel = list_Codigo[i];
            i++;
            do
            {
                i++;
            } while (!list_Codigo[i].Equals(nextLabel));
        }
        private void mSetIf()
        {
            bool isTrue;
            string strFin;
            /*if (list_Codigo[i].IndexOf("true") != -1)
                isTrue = true;
            else
                isTrue = false;
            */
            string aux = strGetId(list_Codigo[i]);
            string izq = strGetIzquierda(aux);
            string ope = strGetCompara(aux);
            string der = strGetDerecha(aux);
            if (!isNumber(izq))
                izq = vHashTable[mIntHashCode(izq), mIntColColision(izq)].strValor;
            if (!isNumber(der))
                der = vHashTable[mIntHashCode(der), mIntColColision(der)].strValor;
            if (isTrueBreak(izq, ope, der))
                isTrue = true;
            else
                isTrue = false;


            strFin = list_Codigo[i].Substring(list_Codigo[i].IndexOf("_") + 1);
            strFin = strFin.Substring(0, strFin.IndexOf(" "));
            stackIf.Push(strFin);
            if (isTrue)
            {
                do
                {
                    try
                    {
                        i++;
                        if (isSetId(list_Codigo[i]))                // Para las declaraciones y Asignaciones. // en si, una declaracion, es una asignación de un "null".
                            mSetId();
                        else
                        {
                            if (isCin(list_Codigo[i]))          // Para introducir valores por teclado
                                mSetCin();
                            else
                            {
                                if (isCout(list_Codigo[i]))     // para imprimir resultados en pantalla
                                    mSetCout();
                                else
                                {
                                    if (isBreak(list_Codigo[i]))    // Para saltar etiquetas en ciclos
                                        mSetBreak();
                                    else
                                    {
                                        if (isIf(list_Codigo[i]))   // Para el inicio de un if
                                            mSetIf();
                                        else
                                        {
                                            if (isElse(list_Codigo[i])) // para los else's
                                            {
                                                if (list_Codigo[i].IndexOf(stackIf.Peek()) != -1)
                                                    break;
                                            }
                                            else
                                            {
                                                if (isWhile(list_Codigo[i]))    // para el inicio de un while
                                                    mSetWhile();
                                                else
                                                {
                                                    if (isRepeat(list_Codigo[i]))   // para el inicio de un do-until
                                                        mSetDo();
                                                    else if (isEnd(list_Codigo[i])) // para los else's
                                                        if (list_Codigo[i].IndexOf(stackIf.Peek()) != -1)
                                                        {
                                                            i--;
                                                            break;
                                                        }

                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                } while (true);
                do
                {
                    try
                    {
                        i++;
                        if (isEnd(list_Codigo[i]))
                        {
                            if (list_Codigo[i].IndexOf(stackIf.Peek()) != -1)
                            {
                                stackIf.Pop();
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                } while (true);
            }
            else
            {
                do
                {
                    i++;
                    if (isElse(list_Codigo[i]) && list_Codigo[i].IndexOf(stackIf.Peek()) != -1
                        ) // para los else's
                    {
                        stackIf.Pop();
                        break;
                    }
                    else if (isEnd(list_Codigo[i]) && list_Codigo[i].IndexOf(stackIf.Peek()) != -1)
                    {
                        stackIf.Pop();
                        i--;
                        break;
                    }
                } while (true);
                do
                {
                    i++;
                    if (isSetId(list_Codigo[i]))                // Para las declaraciones y Asignaciones. // en si, una declaracion, es una asignación de un "null".
                        mSetId();
                    else
                    {
                        if (isCin(list_Codigo[i]))          // Para introducir valores por teclado
                            mSetCin();
                        else
                        {
                            if (isCout(list_Codigo[i]))     // para imprimir resultados en pantalla
                                mSetCout();
                            else
                            {
                                if (isBreak(list_Codigo[i]))    // Para saltar etiquetas en ciclos
                                    mSetBreak();
                                else
                                {
                                    if (isIf(list_Codigo[i]))   // Para el inicio de un if
                                        mSetIf();
                                    else
                                    {
                                        if (isEnd(list_Codigo[i])) // para los else's
                                            break;
                                        else
                                        {
                                            if (isWhile(list_Codigo[i]))    // para el inicio de un while
                                            {

                                            }
                                            else
                                            {
                                                if (isWhileReturn(list_Codigo[i]))  // para la etiqueta de retorno al inico del while
                                                {

                                                }
                                                else
                                                {
                                                    if (isWhileOut(list_Codigo[i])) // para si no se cumple la condición del if, saltar a esta etiqueta
                                                    {

                                                    }
                                                    else
                                                    {
                                                        if (isRepeat(list_Codigo[i]))   // para el inicio de un do-until
                                                        {

                                                        }
                                                        else
                                                        {
                                                            if (isUntil(list_Codigo[i]))
                                                            {

                                                            }
                                                            else
                                                            {
                                                                if (isConstante(list_Codigo[i]))
                                                                {

                                                                }
                                                                else
                                                                {
                                                                    if (isOperador(list_Codigo[i]))
                                                                    {

                                                                    }
                                                                    else
                                                                    {
                                                                        // Seguramente es un error
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
                    }
                } while (true);
            }
        }
        private void mSetWhile()
        {
            bool isTrue;
            string strFin;
            if (list_Codigo[i].IndexOf("true") != -1)
                isTrue = true;
            else
                isTrue = false;
            strFin = list_Codigo[i].Substring(list_Codigo[i].IndexOf("_") + 1);
            strFin = strFin.Substring(0, strFin.IndexOf(" "));
            stackWhileS.Push(strFin);
            if (isTrue)
            {
                stackWhileI.Push(i);
                do
                {
                    i++;
                    if (isSetId(list_Codigo[i]))                // Para las declaraciones y Asignaciones. // en si, una declaracion, es una asignación de un "null".
                        mSetId();
                    else
                    {
                        if (isCin(list_Codigo[i]))          // Para introducir valores por teclado
                            mSetCin();
                        else
                        {
                            if (isCout(list_Codigo[i]))     // para imprimir resultados en pantalla
                                mSetCout();
                            else
                            {
                                if (isBreak(list_Codigo[i]))    // Para saltar etiquetas en ciclos
                                {
                                    mSetBreak();
                                    break;
                                }
                                else
                                {
                                    if (isIf(list_Codigo[i]))   // Para el inicio de un if
                                        mSetIf();
                                    else
                                    {
                                        if (isWhile(list_Codigo[i]))    // para el inicio de un while
                                            mSetWhile();
                                        else
                                        {
                                            if (isWhileReturn(list_Codigo[i]))  // para la etiqueta de retorno al inico del while
                                            {
                                                string aux = strGetId(list_Codigo[stackWhileI.Peek()]);
                                                string izq = strGetIzquierda(aux).Trim();
                                                string ope = strGetCompara(aux).Trim();
                                                string der = strGetDerecha(aux).Trim();
                                                if (!isNumber(izq))
                                                    izq = vHashTable[mIntHashCode(izq), mIntColColision(izq)].strValor;
                                                if (!isNumber(der))
                                                    der = vHashTable[mIntHashCode(der), mIntColColision(der)].strValor;
                                                if (isTrueBreak(izq, ope, der))
                                                    i = stackWhileI.Peek();
                                                else
                                                {
                                                    stackWhileI.Pop();
                                                    stackWhileS.Pop();
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (isRepeat(list_Codigo[i]))   // para el inicio de un do-until
                                                    mSetDo();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                } while (true);
            }
            else
            {
                do
                {
                    i++;
                    if (isWhileOut(list_Codigo[i]) && list_Codigo[i].IndexOf(stackWhileS.Peek()) != -1)
                    {
                        stackWhileS.Pop();
                        break;
                    }
                } while (true);
            }
        }
        private void mSetDo()
        {
            stackDoI.Push(i);
            string strInicio;
            strInicio = list_Codigo[i].Substring(list_Codigo[i].IndexOf("_") + 1);
            stackDoS.Push(strInicio);
            do
            {
                i++;
                if (isSetId(list_Codigo[i]))                // Para las declaraciones y Asignaciones. // en si, una declaracion, es una asignación de un "null".
                    mSetId();
                else
                {
                    if (isCin(list_Codigo[i]))          // Para introducir valores por teclado
                        mSetCin();
                    else
                    {
                        if (isCout(list_Codigo[i]))     // para imprimir resultados en pantalla
                            mSetCout();
                        else
                        {
                            if (isBreak(list_Codigo[i]))    // Para saltar etiquetas en ciclos
                            {
                                mSetBreak();
                                if (list_Codigo[stackDoI.Peek()].IndexOf(stackDoS.Peek()) != -1)
                                {
                                    stackDoS.Pop();
                                    stackDoI.Pop();
                                }
                                break;
                            }
                            else
                            {
                                if (isIf(list_Codigo[i]))   // Para el inicio de un if
                                    mSetIf();
                                else
                                {
                                    if (isWhile(list_Codigo[i]))    // para el inicio de un while
                                        mSetWhile();
                                    else
                                    {
                                        if (isRepeat(list_Codigo[i]))   // para el inicio de un do-until
                                            mSetDo();
                                        else
                                        {
                                            if (isUntil(list_Codigo[i]))
                                            {
                                                string aux = strGetId(list_Codigo[i]);
                                                string izq = strGetIzquierda(aux).Trim();
                                                string ope = strGetCompara(aux).Trim();
                                                string der = strGetDerecha(aux).Trim();
                                                if (!isNumber(izq))
                                                    izq = vHashTable[mIntHashCode(izq), mIntColColision(izq)].strValor;
                                                if (!isNumber(der))
                                                    der = vHashTable[mIntHashCode(der), mIntColColision(der)].strValor;
                                                if (isTrueBreak(izq, ope, der))
                                                    i = stackDoI.Peek();
                                                else
                                                {
                                                    stackDoI.Pop();
                                                    stackDoS.Pop();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } while (true);
        }
        #endregion // Termina "Métodos para realizar proceso de compilación"
        #region "Métodos extra para realiza proceso"

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
                if (vHashTable[intHashCode, i].strVarName == null || vHashTable[intHashCode, i].strVarName.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion // Termina "Métodos extra para realiza proceso"
        #region "Métodos para manejar Strings"
        private string strGetId(string strLabel)
        {
            strLabel = strLabel.Remove(0, strLabel.IndexOf(this.SPACE) + 1);
            try
            {
                strLabel = strLabel.Remove(0, strLabel.IndexOf(this.SPACE) + 1);
            }
            catch (Exception) { }
            return strLabel;
        }
        private string strGetValue(string strLabel)
        {
            strLabel = strLabel.Remove(0, strLabel.IndexOf(this.SPACE) + 1);
            strLabel = strLabel.Remove(strLabel.IndexOf(this.SPACE));
            return strLabel;
        }
        private string strGetType(string strLabel)
        {
            if (isBoolean(strLabel))
                return CodigoIntermedioEtiqueta.strBoolean;
            if (isInt(strLabel))
                return CodigoIntermedioEtiqueta.strInt;
            if (isReal(strLabel))
                return CodigoIntermedioEtiqueta.strFloat;

            return string.Empty;
        }
        private string strGetIzquierda(string strLabel)
        {
            string strOperador = strGetCompara(strLabel);
            return strLabel.Substring(0, strLabel.IndexOf(strOperador)).Trim();
        }
        private string strGetCompara(string strLabel)
        {
            if (isMayorQue(strLabel))
                return ">";
            else
            {
                if (isMayorIgual(strLabel))
                    return ">=";
                else
                {
                    if (isMenorQue(strLabel))
                        return "<";
                    else
                    {
                        if (isMenorIgual(strLabel))
                            return "<=";
                        else
                        {
                            if (isIgualIgual(strLabel))
                                return "==";
                            else
                            {
                                if (isDiferente(strLabel))
                                    return "!=";
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
        private string strGetDerecha(string strLabel)
        {
            string strOperador = strGetCompara(strLabel);
            return strLabel.Substring(strLabel.IndexOf(strOperador) + strOperador.Length).Trim();
        }
        #endregion // Termina "Métodos para manejar Strings"
    }
}

