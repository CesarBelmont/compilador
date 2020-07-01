using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.analizador_lexico
{
    class Estados //Todos los "estados" del automata
    {
        //Todos los tokens posibles
        public static String tokenIf = "if";
        public static String tokenThen = "then";
        public static String tokenElse = "else";
        public static String tokenDo = "do";     
        public static String tokenWhile = "while";
        public static String tokenUntil = "until";
        public static String tokenEnd = "end";
        public static String tokenMain = "main";
        public static String tokenCin = "cin";
        public static String tokenCout = "cout";
        public static String tokenFloat = "float";
        public static String tokenReal = "real";
        public static String tokenInt = "int";
        public static String tokenBoolean = "boolean";
        public static String tokenAsignacion = ":=";
        public static String tokenIgualdad = "==";
        public static String tokenMayor = ">";
        public static String tokenIdentificador = "";
        public static String tokenMenor = "<";
        public static String tokenDiferente = "!=";
        public static String tokenMayorIgual = ">=";
        public static String tokenMas = "+";
        public static String tokenMenos = "-";
        public static String tokenMultiplica = "*";
        public static String tokenModulo = "%";
        public static String tokenMenorIgual = "<=";
        public static String tokenIncremento = "++";
        public static String tokenDecremento = "--";
        public static String tokenDivision = "/";
        public static String tokenParentesisA = "(";
        public static String tokenParentesisC = ")";
        public static String tokenLlaveA = "{";
        public static String tokenLlaveC = "}";
        public static String tokenComa = ",";
        public static String tokenPuntoComa = ";";
        public static String tokenComentarioS = "//";
        public static String tokenComentarioM = "/*";
        public static String tokenNumeroEnteroS = "";
        public static String tokenNumeroFloatS = "";
        public static String tokenNumeroEnteroCS = "";
        //Descripciones a mostrar en el analizador lexico
        public static String numeroEnteroCS = "Numero entero con signo \n";
        public static String palabraRes = "Palabra reservada \n";
        public static String identificador = "Identificador \n";
        public static String numeroEnteroSS = "Numero entero sin signo\n";
        public static String numeroFlotanteSS = "Numero flotante sin signo\n";
        public static String numeroFlotanteCS = "Numero flotante con signo\n";       
        public static String asignacion = "Asignacion\n";
        public static String igualdad = "Igualdad\n";
        public static String mayorQue = "Mayor que\n";
        public static String menorQue = "Menor que\n";      
        public static String diferente = "Diferente\n";       
        public static String mayorIgual = "Mayor o igual\n";
        public static String menotIgual = "Menor o igual\n";       
        public static String mas = "Signo mas\n";
        public static String menos = "Signo menos\n";        
        public static String multiplicacion = "Signo multiplicacion\n";
        public static String modulo = "Modulo\n";
        public static String incremento = "Incremento\n";
        public static String decremento = "Decremento\n";
        public static String division = "Division\n";   
        public static String parentesisAbre = "Parentesis que abre\n";
        public static String parentesisCierra = "Parentesis de cierre\n";
        public static String llaveAbre = "Llave que abre\n";
        public static String llaveCierra = "Llave de cierre\n";
        public static String coma = "Coma\n";
        public static String puntoComa = "Punto y coma\n";
        public static String comentarioSimple = "Comentario de una linea\n";
        public static String comentarioMultiple = "Comentario Multiple \n";
        public static String error = "Error Lexico \n";
    }
}
