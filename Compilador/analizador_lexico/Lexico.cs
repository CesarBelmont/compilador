using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador.analizador_lexico
{
    class Lexico
    {
        List<Token> lLexico;//Lista para guardar los tokens actuales
        List<String> clase;//Lista para retornar los tokens actuales
        private char[] letras;
        private char[] token;
        List<TokenLocation> location;
        int numErros;
        public Lexico()
        {
            token = new char[100];

        }

        public Boolean isSeparator(Char caracter) //verifica si es Tab, salto linea, espacio
        {
            return caracter == '\t' || caracter == '\n' || caracter == ' ';
        }
        private Boolean isSimbolos(Char caracter) //verifica si es...todo eso
        {
            return caracter == ':' || caracter == '=' || caracter == '>' || caracter == '<'
                || caracter == '!' || caracter == '+' || caracter == '-' || caracter == '*'
                || caracter == '%' || caracter == '/' || caracter == '(' || caracter == ')'
                || caracter == '{' || caracter == '}' || caracter == ',' || caracter == ';'
                || caracter == '.';
        }
        private Boolean isCharacterEspecial(Char caracter) //verifica caracteres especiales
        {
            return false;
        }
        private Boolean isCorchete(Char caracter) //verifica los corchetes
        {
            return caracter == '[' || caracter == ']';
        }
        private Boolean isOtherCase(Char caracter)
        {
            return caracter == '0';
        }

        public List<Token> detectarToken(String texto)
        {
            lLexico = new List<Token>();
            String tokenActual = "";
            letras = texto.ToCharArray();
            int fila = 1;
            int columna = 1;
            location = new List<TokenLocation>();
            TokenLocation Location;
            String Others = "^[A-Za-z][A-Za-z0-9]*$";
            for (int i = 0; i < letras.Length; i++)
            {
                columna++;
                if (!isSeparator(letras[i]) && !isSimbolos(letras[i]) && !isCorchete(letras[i]) && (letras[i] >= 48 && letras[i] <= 57 || (letras[i] >= 65 && letras[i] <= 90) || (letras[i] >= 97 && letras[i] <= 122)))
                //Verifica que la entrada sea una A-Z o a-z
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                    }
                }
                else //Si no es una letra verifica por el resto
                {
                    if (letras[i] == '\t')
                    {
                        columna += 4; //Si es un tabulado, la columna aumenta 4 espacios
                    }
                    if (letras[i] == '+' || letras[i] == '-')
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                                Location = new TokenLocation();
                                Location.setFila(fila);
                                Location.setColumna(columna - (tokenActual.Length - 1));
                                token.setLocation(Location);
                                location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if (i < (letras.Length - 1) && letras[i + 1] == '=')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if ((i < (letras.Length - 1)) && letras[i + 1] == '=')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna);
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if (i < (letras.Length - 1) && letras[i + 1] == '=')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if ((i < (letras.Length - 1)) && letras[i + 1] == '=')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        if ((i < (letras.Length - 1)) && letras[i + 1] == '=')
                        {
                            Token token = new Token();
                            tokenActual += letras[i];
                            tokenActual += letras[i + 1];
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                            continue;
                        }
                    }
                    else if (letras[i] == '*' || letras[i] == '%' || letras[i] == '('
                       || letras[i] == ')' || letras[i] == '{'
                       || letras[i] == '}' || letras[i] == ',' || letras[i] == ';')
                    {
                        if (tokenActual != "")
                        {
                            Token toke = new Token();
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            toke.setLocation(Location);
                            location.Add(Location);
                            toke.setName(tokenActual);
                            lLexico.Add(toke);
                            tokenActual = "";
                        }
                        Token token = new Token();
                        tokenActual += letras[i];
                        Location = new TokenLocation();
                        Location.setFila(fila);
                        Location.setColumna(columna - (tokenActual.Length - 1));
                        token.setLocation(Location);
                        location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length));
                            token.setLocation(Location);
                            location.Add(Location);
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
                        if (!isNumber(tokenActual) || (i < (letras.Length - 1) && (letras[i + 1] < 48 || letras[i + 1] > 57) || (i + 1) >= letras.Length))
                        {
                            Token token;
                            if (tokenActual != "")
                            {
                                token = new Token();
                                Location = new TokenLocation();
                                Location.setFila(fila);
                                Location.setColumna(columna - (tokenActual.Length - 1));
                                token.setLocation(Location);
                                location.Add(Location);
                                token.setName(tokenActual);
                                lLexico.Add(token);
                                tokenActual = "";
                            }
                            token = new Token();
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            columna--;
                            Location.setColumna(columna);
                            token.setLocation(Location);
                            location.Add(Location);
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
                                if (isNumber(letras[i].ToString()) || !isSeparator(letras[i]) && !isSimbolos(letras[i]))
                                {
                                    tokenActual += letras[i];
                                }
                                else
                                {
                                    i--;
                                    break;
                                }
                                i++;
                                columna++;
                            }
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
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
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        Token toke = new Token();
                        tokenActual += letras[i];
                        Location = new TokenLocation();
                        Location.setFila(fila);
                        Location.setColumna(columna);
                        toke.setLocation(Location);
                        location.Add(Location);
                        toke.setName(tokenActual);
                        lLexico.Add(toke);
                        tokenActual = "";
                        continue;
                    }
                    else if (!isSeparator(letras[i]))
                    {
                        if (tokenActual != "")
                        {
                            Token token = new Token();
                            Location = new TokenLocation();
                            Location.setFila(fila);
                            Location.setColumna(columna - (tokenActual.Length - 1));
                            token.setLocation(Location);
                            location.Add(Location);
                            token.setName(tokenActual);
                            lLexico.Add(token);
                            tokenActual = "";
                        }
                        tokenActual += letras[i];
                        Token toke = new Token();
                        Location = new TokenLocation();
                        Location.setFila(fila);
                        Location.setColumna(columna - (tokenActual.Length - 1));
                        toke.setLocation(Location);
                        location.Add(Location);
                        toke.setName(tokenActual);
                        lLexico.Add(toke);
                        tokenActual = "";
                    }
                    if (tokenActual != "")
                    {
                        Token token = new Token();
                        Location = new TokenLocation();
                        Location.setFila(fila);
                        Location.setColumna(columna - (tokenActual.Length - 1));
                        token.setLocation(Location);
                        location.Add(Location);
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
                Location = new TokenLocation();
                Location.setFila(fila);
                Location.setColumna(columna - (tokenActual.Length - 1));
                token.setLocation(Location);
                location.Add(Location);
                token.setName(tokenActual);
                lLexico.Add(token);
                tokenActual = "";
            }
            return lLexico;//se retorna la lista para escribirla en el espacio de Lexico
        }
        private Boolean isNumber(String token) //Verifica si es un numero
        {
            Boolean isnumber;
            try
            {
                int aux = Int32.Parse(token);
                isnumber = true;

            }
            catch (Exception e)
            {
                isnumber = false;
            }
            return isnumber;
        }
        public List<String> getDescription(List<Token> list)
        {
            numErros = 0;
            String validTokens = "";
            String patron = "^[A-Za-z_][A-Za-z0-9_]*$";
            String numerosSS = "^[0-9]+\\.[0-9]+$";
            String numeroIntSS = "^[0-9]+$";
            String signo = "^[\\+\\-][0-9]+$";
            String signoFloat = "^[\\+\\-][0-9]+\\.[0-9]+$";
            StreamWriter Archivo = File.CreateText(@"../../SalidaLexico/Lexico.txt");
            StreamWriter Errores = File.CreateText(@"../../SalidaLexico/Error.txt");
            clase = new List<String>();
            for (int i = 0; i < list.Count; i++)
            {
                String aux = list[i].getName();
                if (isReservada(aux))
                {
                    validTokens += Estados.RESERVADA_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens);
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.ASIGNACION_TOKEN)
                {//falta identificador y numero entero y numero flotante con o sin signo
                    validTokens += Estados.ASIGNACION_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.LLAVE_ABRE_TOKEN)//llave que abre
                {
                    validTokens += Estados.LLAVE_ABRE_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.LLAVE_CIERRA_TOKEN)//Llave que cierra
                {
                    validTokens += Estados.LLAVE_CIERRA_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.INCREMENTO_TOKEN)//Incremento
                {
                    validTokens += Estados.INCREMENTO_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.IGUALDAD_TOKEN)// Igualdad
                {
                    validTokens += Estados.IGUALDAD_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.PARENTESIS_ABRE_TOKEN)//Parentesis que abre
                {
                    validTokens += Estados.PARENTESIS_ABRE_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.PARENTESIS_CIERRA_TOKEN)//Parentesis que cierra
                {
                    validTokens += Estados.PARENTESIS_CIERRA_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.PUNTO_COMA_TOKEN)//Punto y coma
                {
                    validTokens += Estados.PUNTO_COMA_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.COMA_TOKEN)//Coma
                {
                    validTokens += Estados.COMA_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.DECREMENTO_TOKEN)//Decremento
                {
                    validTokens += Estados.DECREMENTO_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.DIFERENTE_TOKEN)//Diferente de
                {
                    validTokens += Estados.DIFERENTE_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.DIVISION_TOKEN)//DIVISION
                {
                    validTokens += Estados.DIVISION_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.MAS_TOKEN)//MAS
                {
                    validTokens += Estados.MAS_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.MAYOR_QUE_TOKEN)//MAYOR QUE
                {
                    validTokens += Estados.MAYOR_QUE_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.MENOR_QUE_TOKEN)//MENOR QUE
                {
                    validTokens += Estados.MENOR_QUE_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.MENOS_TOKEN)//MENOS
                {
                    validTokens += Estados.MENOS_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.MODULO_TOKEN)//MODULO
                {
                    validTokens += Estados.MODULO_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.MULTI_TOKEN)//MULTIPLICACION
                {
                    validTokens += Estados.MULTI_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.MAYOR_IGUAL_TOKEN)//Llave que cierra
                {
                    validTokens += Estados.MAYOR_IGUAL_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName() == Estados.MENOR_IGUAL_TOKEN)//Llave que cierra
                {
                    validTokens += Estados.MENOR_IGUAL_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName().Contains(Estados.COMENTARIO_SIMPLE_TOKEN))
                {
                    validTokens += Estados.COMENTARIO_SIMPLE_DESCRIPTION;
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (list[i].getName().Contains(Estados.COMENTARIO_MULTIPLE_TOKEN))
                {
                    validTokens += Estados.COMENTARIO_MULTIPLE_DESCRIPTION;
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (Regex.IsMatch(list[i].getName(), patron))
                {
                    validTokens += Estados.ID_DESCRIPTION;
                    clase.Add(validTokens);
                    Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    validTokens = "";
                    continue;
                }
                else if (Regex.IsMatch(list[i].getName(), numerosSS))
                {
                    validTokens += Estados.NUM_FLOAT_SSIGNO_DESCRIPTION;
                    clase.Add(validTokens);
                    if (list.Count != 0)
                    {
                        Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    }
                    validTokens = "";
                    continue;
                }
                else if (Regex.IsMatch(list[i].getName(), numeroIntSS))
                {
                    validTokens += Estados.NUM_INT_SSIGNO_DESCRIPTION;
                    clase.Add(validTokens);
                    if (list.Count != 0)
                    {
                        Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    }
                    validTokens = "";
                    continue;
                }
                else if (Regex.IsMatch(list[i].getName(), signo))
                {
                    validTokens += Estados.NUM_INT_CSIGNO_DESCRIPTION;
                    clase.Add(validTokens);
                    if (list.Count != 0)
                    {
                        Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    }
                    validTokens = "";
                    continue;
                }
                else if (Regex.IsMatch(list[i].getName(), signoFloat))
                {
                    validTokens += Estados.NUM_FLOAT_CSIGNO_DESCRIPTION;
                    clase.Add(validTokens);
                    if (list.Count != 0)
                    {
                        Archivo.WriteLine(list[i] + " " + validTokens + "\n");
                    }
                    validTokens = "";
                    continue;
                }
                else
                {
                    validTokens += Estados.ERROR_DESCRIPTION;
                    clase.Add(validTokens);
                    numErros++;
                    if (list.Count != 0)
                    {
                        Errores.WriteLine(list[i] + " " + validTokens + "\n");
                    }
                    validTokens = "";
                }
            }
            Archivo.Close();
            Errores.Close();
            return clase;
        }
        public int getNumErros()
        {
            return numErros;
        }
        public List<TokenLocation> getLocationList()
        {
            return location;
        }
        private Boolean isReservada(String token)
        {
            return token == Estados.MAIN_TOKEN || token == Estados.THEN_TOKEN || token == Estados.ELSE_TOKEN
                || token == Estados.DO_TOKEN || token == Estados.WHILE_TOKEN || token == Estados.REAL_TOKEN
                || token == Estados.END_TOKEN || token == Estados.FLOAT_TOKEN || token == Estados.INT_TOKEN
                || token == Estados.BOOLEAN_TOKEN || token == Estados.IF_TOKEN || token == Estados.COUT_TOKEN || token == Estados.CIN_TOKEN;
        }
    }
}
