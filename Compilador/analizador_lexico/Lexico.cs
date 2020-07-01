using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Compilador.analizador_lexico
{
    class Lexico
    {
        List<Token> lLexico;//Lista para guardar los tokens actuales
        List<String> tokensValidos;//Lista para retornar los tokens actuales
        private char[] letras;
        private char[] token;
        List<ubicacionToken> Ubicacion;
        int numErros;
        public Lexico()
        {
            token = new char[100];

        }
        public List<Token> automata(String t)
        {
            lLexico = new List<Token>();
            String tokenActual = "";
            letras = t.ToCharArray();
            int fila = 1;
            int columna = 1;
            Ubicacion = new List<ubicacionToken>();
            ubicacionToken ubica;
            String Others = "^[A-Za-z][A-Za-z0-9]*$";
            for (int i = 0; i < letras.Length; i++)
            {
                columna++;
                if (!separador(letras[i]) && !simbolos(letras[i]) && !corchete(letras[i]) && (letras[i] >= 48 && letras[i] <= 57 || (letras[i] >= 65 && letras[i] <= 90) || (letras[i] >= 97 && letras[i] <= 122)))
                {
                    tokenActual += letras[i];
                    if (Regex.IsMatch(tokenActual, Others))
                    {
                        continue;
                    }
                    if (letras[i] >= 48 && letras[i] <= 57)
                    {
                        if (i < (letras.Length - 1) && ((letras[i + 1] >= 65 && letras[i + 1] <= 90) || (letras[i + 1] >= 97 && letras[i + 1] <= 122)))
                        {
                            Token token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                    }
                }
                else
                {
                    if (letras[i] == '\t')
                    {
                        columna += 4;
                    }
                    if (letras[i] == '+' || letras[i] == '-')
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if (i < (letras.Length - 1) && (letras[i + 1] >= 48 && letras[i + 1] <= 57))
                        {
                            tokenActual += letras[i];
                            if ((i - 1) != -1 && ((letras[i - 1] >= 48 && letras[i - 1] <= 57) || (letras[i - 1] >= 65 && letras[i - 1] <= 90) || (letras[i - 1] >= 97 && letras[i - 1] <= 122)))
                            {
                                Token token = new Token();
                                ubica = new ubicacionToken();
                                ubica.setFila(fila);
                                ubica.setColumna(columna - (tokenActual.Length - 1));
                                token.setLocation(ubica);
                                Ubicacion.Add(ubica);
                                token.setName(tokenActual);
                                lLexico.Add(token);
                                tokenActual = "";
                            }
                            continue;
                        }
                        else if ((i < (letras.Length - 1)) && letras[i] == '+' && letras[i + 1] == '+')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            i++;
                            continue;
                        }
                        else if ((i < (letras.Length - 1)) && letras[i] == '-' && letras[i + 1] == '-')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            i++;
                            continue;
                        }
                        else
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            continue;
                        }

                    }
                    else if (letras[i] == ':' && i < (letras.Length))
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if (i < (letras.Length - 1) && letras[i + 1] == '=')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            columna++;
                            i++;
                            continue;
                        }
                        else
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            continue;
                        }
                    }
                    else if (letras[i] == '=')
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if ((i < (letras.Length - 1)) && letras[i + 1] == '=')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            columna++;
                            i++;
                            continue;
                        }
                        else
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna);
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            continue;
                        }
                    }
                    else if (letras[i] == '>')
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if (i < (letras.Length - 1) && letras[i + 1] == '=')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            i++;
                            columna++;
                            continue;
                        }
                        else
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            continue;
                        }
                    }
                    else if (letras[i] == '<')
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if ((i < (letras.Length - 1)) && letras[i + 1] == '=')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            i++;
                            columna++;
                            continue;
                        }
                        else
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            continue;
                        }
                    }
                    else if (letras[i] == '!')
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if ((i < (letras.Length - 1)) && letras[i + 1] == '=')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            i++;
                            columna++;
                            continue;
                        }
                        else
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            continue;
                        }
                    }
                    else if (letras[i] == '*' || letras[i] == '%' || letras[i] == '(' || letras[i] == ')' || letras[i] == '{' || letras[i] == '}' || letras[i] == ',' || letras[i] == ';')
                    {
                        if (tokenActual != "")
                        {
                            Token toke = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            toke.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            toke.setName(tokenActual);
                            lLexico.Add(toke);
                            tokenActual = "";
                        }
                        Token token = new Token();
                        tokenActual += letras[i];
                        ubica = new ubicacionToken();
                        ubica.setFila(fila);
                        ubica.setColumna(columna - (tokenActual.Length - 1));
                        token.setLocation(ubica);
                        Ubicacion.Add(ubica);
                        token.setName(tokenActual);
                        lLexico.Add(token);
                        tokenActual = "";
                        continue;
                    }
                    else if (letras[i] == '/')
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if (i < (letras.Length - 1) && letras[i + 1] == '/')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[++i];
                            i++;
                            while ((i < letras.Length) && letras[i] != '\n')
                            {
                                tokenActual += letras[i];
                                i++;
                            }
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            tokenActual = "";
                            continue;
                        }
                        else if (i < (letras.Length - 1) && letras[i + 1] == '*')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[++i];
                            i++;
                            columna += 2;
                            while ((i < letras.Length))
                            {
                                columna++;
                                if (letras[i] == '\n')
                                {
                                    fila++;
                                    columna = 0;
                                }
                                if (letras[i] == '*' && (i < letras.Length - 1) && letras[i + 1] == '/')
                                {
                                    tokenActual += letras[i];
                                    tokenActual += letras[i + 1];
                                    i++;
                                    columna++;
                                    break;
                                }
                                else
                                {
                                    tokenActual += letras[i];
                                }
                                i++;
                            }
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            tokenActual = "";
                            continue;
                        }
                        else
                        {
                            tokenActual += letras[i];
                        }

                    }
                    else if (letras[i] == '.')
                    {

                        if (!isNumber(tokenActual))
                        {
                            Token token;
                            if (tokenActual != "")
                            {
                                token = new Token();
                                ubica = new ubicacionToken();
                                ubica.setFila(fila);
                                ubica.setColumna(columna - (tokenActual.Length - 1));
                                token.setLocation(ubica);
                                Ubicacion.Add(ubica);
                                token.setName(tokenActual);
                                lLexico.Add(token);

                                tokenActual = "";

                            }
                            token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            columna--;
                            ubica.setColumna(columna);
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            tokenActual += letras[i];
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            continue;

                        }
                        else
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            i++;
                            columna++;
                            while ((i < letras.Length))
                            {

                                if (isNumber(letras[i].ToString()) || !separador(letras[i]) && !simbolos(letras[i]))
                                {

                                    if (!isNumber(letras[i].ToString()))
                                    {
                                        i--;
                                        break;
                                    }
                                    else
                                    {
                                        tokenActual += letras[i];
                                    }
                                }
                                else
                                {
                                    i--;
                                    break;
                                }
                                i++;
                                columna++;
                            }
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            continue;
                        }

                    }
                    else if (letras[i] == '[' || letras[i] == ']')
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        Token toke = new Token();
                        tokenActual += letras[i];
                        ubica = new ubicacionToken();
                        ubica.setFila(fila);
                        ubica.setColumna(columna);
                        toke.setLocation(ubica);
                        Ubicacion.Add(ubica);
                        toke.setName(tokenActual);
                        lLexico.Add(toke);
                        tokenActual = "";
                        continue;
                    }
                    else if (!separador(letras[i]))
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            ubica = new ubicacionToken();
                            ubica.setFila(fila);
                            ubica.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(ubica);
                            Ubicacion.Add(ubica);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        tokenActual += letras[i];
                        Token toke = new Token();
                        ubica = new ubicacionToken();
                        ubica.setFila(fila);
                        ubica.setColumna(columna - (tokenActual.Length - 1));
                        toke.setLocation(ubica);
                        Ubicacion.Add(ubica);
                        toke.setName(tokenActual);
                        lLexico.Add(toke);
                        tokenActual = "";
                    }
                    if (tokenActual != "")
                    {
                        Token token = new Token();
                        ubica = new ubicacionToken();
                        ubica.setFila(fila);
                        ubica.setColumna(columna - (tokenActual.Length - 1));
                        token.setLocation(ubica);
                        Ubicacion.Add(ubica);
                        token.setName(tokenActual);
                        lLexico.Add(token);
                        tokenActual = "";
                    }
                }
                if (i < letras.Length && letras[i] == '\n')
                {
                    fila++;
                    columna = 0;
                }

            }

            if (tokenActual != "")
            {
                Token token = new Token();
                ubica = new ubicacionToken();
                ubica.setFila(fila);
                ubica.setColumna(columna - (tokenActual.Length - 1));
                token.setLocation(ubica);
                Ubicacion.Add(ubica);
                token.setName(tokenActual);
                lLexico.Add(token);
                tokenActual = "";
            }
            return lLexico;
        }

        public Boolean separador(Char caracter) //verifica si es Tab, salto linea, espacio
        {
            return caracter == '\t' || caracter == '\n' || caracter == ' ';
        }
        private Boolean simbolos(Char caracter) //verifica si es...todo eso
        {
            return caracter == ':' || caracter == '=' || caracter == '>' || caracter == '<'
                || caracter == '!' || caracter == '+' || caracter == '-' || caracter == '*'
                || caracter == '%' || caracter == '/' || caracter == '(' || caracter == ')'
                || caracter == '{' || caracter == '}' || caracter == ',' || caracter == ';'
                || caracter == '.';
        }
        private Boolean caracterEspecial(Char caracter) //verifica caracteres especiales
        {
            return false;
        }
        private Boolean corchete(Char caracter) //verifica los corchetes
        {
            return caracter == '[' || caracter == ']';
        }
        private Boolean otroCaracter(Char caracter)
        {
            return caracter == '0';
        }

        private Boolean isNumber(String token) //Verifica si es un numero
        {
            Boolean isnumber;
            try
            {
                int aux = Int32.Parse(token);
                isnumber = true;

            }
            catch (Exception)
            {
                isnumber = false;
            }
            return isnumber;
        }

        public List<String> getDescription(List<Token> list)
        {
            numErros = 0;
            String tkns = "";
            //Expresiones regulares 
            String identificador = "^[A-Za-z_][A-Za-z0-9_]*$"; //Identificadores
            String floatNS = "^[0-9]+\\.[0-9]+$"; //Flotantes sin signo
            String intNS = "^[0-9]+$"; //Enteros sin signo
            String intCS = "^[\\+\\-][0-9]+$"; //enteros con signo
            String floatCS = "^[\\+\\-][0-9]+\\.[0-9]+$"; //flotantes con signo

            //Crea un archivo para insertarlo en el RichTextBox de errores y salida
            StreamWriter Tokens = File.CreateText(@"../../SalidaLexico/Lexico.txt");
            StreamWriter Errores = File.CreateText(@"../../SalidaLexico/Error.txt");

            tokensValidos = new List<String>();

            for (int i = 0; i < list.Count; i++)
            {
                String aux = list[i].getName();
                if (isReservada(aux))
                {
                    tkns += Estados.palabraRes;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns);
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenAsignacion)
                {
                    tkns += Estados.asignacion;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenLlaveA)
                {
                    tkns += Estados.llaveAbre;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenLlaveC)
                {
                    tkns += Estados.llaveCierra;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenIncremento)
                {
                    tkns += Estados.incremento;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenIgualdad)
                {
                    tkns += Estados.igualdad;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenParentesisA)
                {
                    tkns += Estados.parentesisAbre;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenParentesisC)
                {
                    tkns += Estados.parentesisCierra;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenPuntoComa)
                {
                    tkns += Estados.puntoComa;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenComa)
                {
                    tkns += Estados.coma;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenDecremento)
                {
                    tkns += Estados.decremento;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenDiferente)
                {
                    tkns += Estados.diferente;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenDivision)
                {
                    tkns += Estados.division;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenMas)
                {
                    tkns += Estados.mas;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenMayor)
                {
                    tkns += Estados.mayorQue;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenMenor)
                {
                    tkns += Estados.menorQue;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenMenos)
                {
                    tkns += Estados.menos;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenModulo)
                {
                    tkns += Estados.modulo;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenMultiplica)
                {
                    tkns += Estados.multiplicacion;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenMayorIgual)
                {
                    tkns += Estados.mayorIgual;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName() == Estados.tokenMenorIgual)
                {
                    tkns += Estados.menotIgual;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName().Contains(Estados.tokenComentarioS))
                {
                    tkns += Estados.comentarioSimple;
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (list[i].getName().Contains(Estados.tokenComentarioM))
                {
                    tkns += Estados.comentarioMultiple;
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (Regex.IsMatch(list[i].getName(), identificador))
                {
                    tkns += Estados.identificador;
                    tokensValidos.Add(tkns);
                    Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    tkns = "";
                    continue;
                }
                else if (Regex.IsMatch(list[i].getName(), floatNS))
                {
                    tkns += Estados.numeroFlotanteSS;
                    tokensValidos.Add(tkns);
                    if (list.Count != 0)
                    {
                        Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    }
                    tkns = "";
                    continue;
                }
                else if (Regex.IsMatch(list[i].getName(), intNS))
                {
                    tkns += Estados.numeroEnteroSS;
                    tokensValidos.Add(tkns);
                    if (list.Count != 0)
                    {
                        Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    }
                    tkns = "";
                    continue;
                }
                else if (Regex.IsMatch(list[i].getName(), intCS))
                {
                    tkns += Estados.numeroEnteroCS;
                    tokensValidos.Add(tkns);
                    if (list.Count != 0)
                    {
                        Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    }
                    tkns = "";
                    continue;
                }
                else if (Regex.IsMatch(list[i].getName(), floatCS))
                {
                    tkns += Estados.numeroFlotanteCS;
                    tokensValidos.Add(tkns);
                    if (list.Count != 0)
                    {
                        Tokens.WriteLine(list[i] + " " + tkns + "\n");
                    }
                    tkns = "";
                    continue;
                }
                else
                {
                    tkns += Estados.error;
                    tokensValidos.Add(tkns);
                    numErros++;
                    if (list.Count != 0)
                    {
                        Errores.WriteLine(list[i] + " " + tkns + "\n");
                    }
                    tkns = "";
                }
            }
            Tokens.Close();
            Errores.Close();
            return tokensValidos;
        }
        public int numeroErrores()
        {
            return numErros;
        }
        public List<ubicacionToken> listaUbicaciones()
        {
            return Ubicacion;
        }
        private Boolean isReservada(String token)
        {
            return token == Estados.tokenMain || token == Estados.tokenThen || token == Estados.tokenElse
                || token == Estados.tokenDo || token == Estados.tokenWhile || token == Estados.tokenReal || token == Estados.tokenUntil
                || token == Estados.tokenEnd || token == Estados.tokenFloat || token == Estados.tokenInt
                || token == Estados.tokenBoolean || token == Estados.tokenIf || token == Estados.tokenCout || token == Estados.tokenCin;
        }
    }
}
