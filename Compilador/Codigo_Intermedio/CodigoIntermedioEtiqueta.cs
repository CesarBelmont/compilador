using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Codigo_Intermedio
{
    class CodigoIntermedioEtiqueta
    {
        // Labels para indicar que se guardaran valores en las variables, desde una declaración (se asigna un NULL), hasta una declaración (se asigna el valor.
        static public string strSetId_Boolean = "setId_Boolean";
        static public string strSetId_Int = "setId_Int";
        static public string strSetId_Float = "setId_Float";
        // Labels para cargar los valores que tiene la variable
        static public string strGetId_Boolean = "getId_Boolean";
        static public string strGetId_Int = "getId_Int";
        static public string strGetId_Float = "getId_Float";
        static public string strGetConst_Int = "getConst_Int";
        static public string strGetConst_Float = "getConst_Float";
        static public string strGetConst_Boolean = "getConst_Boolean";
        // Labels para IF END | IF ELSE END
        static public string strIf = "IF_";
        static public string strElse = "ELSE_";
        static public string strEnd = "END_";
        // Labels para WHILE { }
        static public string strWhile = "WHILE_";
        static public string strWhileReturn = "WHILERETURN_";
        static public string strWhileOut = "WHILEOUT_";
        // Labels para REPEAT {} UNTIL EXPRESION
        static public string strRepeat = "DO_";
        static public string strUntil = "UNTIL_";
        // Label para BREAK
        static public string strBreak = "BREAK_";
        static public string strJump = "JUMP";
        // Label para Operadores
        static public string strSuma = "sum_";
        static public string strResta = "res_";
        static public string strMult = "mul_";
        static public string strDivision = "div_";
        static public string strModulo = "mod_";
        // Label para tipos de dato
        static public string strInt = "Int";
        static public string strFloat = "Float";
        static public string strBoolean = "Boolean";
        // Label para E/S de datos
        static public string strCin = "READ_";
        static public string strCout = "PRINT_";
        // Label para fin de operaciones
        static public string strFin = "ST";
    }
}
