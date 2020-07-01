using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador
{
    public enum ScrollBarType : uint
    {
        SbHorz = 0, SbVert = 1, SbCtl = 2, SbBoth = 3
    }
    public enum Message : uint
    {
        WM_VSCROLL = 0x0115
    }
    public enum ScrollBarCommands : uint
    {
        SB_THUMBPOSITION = 4
    }
    public partial class IDE : Form
    {
        aSintactico sintactico;
        analizador_lexico.Lexico lexico;
        List<analizador_lexico.Token> lLexico;
        List<String> clase;
        Lineas linea;
        [DllImport("User32.dll")]
        public extern static int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("User32.dll")]
        public extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        int cont;


        public IDE()
        {
            lexico = new analizador_lexico.Lexico();
            lLexico = new List<analizador_lexico.Token>();
            InitializeComponent();
            linea = new Lineas();
            cont = 0;
            contadorLineas.Text = "1";
            contadorLineas.Enabled = false;
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Clear(); //Elimina todo el contenido del editor de texto actual
            contadorLineas.Text = "1"; //Limpieza del label de numero de lineas
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog abrir = new OpenFileDialog(); //Abre un nuevo cuadro de dialogo
            abrir.Filter = "Archivos de texto (.txt)|*.txt";
            abrir.Title = "Abrir archivo";
            if (abrir.ShowDialog() == DialogResult.OK) //Si se decide abrir ese archivo
            {
                cont = 1;               
                contadorLineas.Text = "1";
                editorDeTexto.Clear();
                System.IO.StreamReader sr = new System.IO.StreamReader(abrir.FileName); //Lector de archivos
                System.IO.StreamReader sr2 = new System.IO.StreamReader(abrir.FileName);
                editorDeTexto.Text = sr.ReadToEnd(); //Escribe todo lo que tenga dicho archivo en el editor  
                cont = linea.getLienas(editorDeTexto.Text);
                StringBuilder auxiliar = new StringBuilder();
                for (int i = 0; i < cont; i++)
                {
                    auxiliar.AppendLine((String.Empty + (i + 1)));
                }
                contadorLineas.Text = auxiliar.ToString();
                sr.Close();
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog(); //Cuadro de dialogo
            guardar.Filter = "Archivos de texto (.txt)|*.txt";
            guardar.Title = "Guardar archivo";
            if (guardar.ShowDialog() == DialogResult.OK) //Si se decide guardar 
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(guardar.FileName); //Escritor de archivos
                sw.Write(editorDeTexto.Text); //Guarda todo lo que este en el editor en un archivo nuevo
                sw.Close();
            }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Lo mismo de guardar, basicamente
            SaveFileDialog guardar = new SaveFileDialog();
            guardar.Filter = "Archivos de texto (.txt)|*.txt";
            guardar.Title = "Guardar como...";
            if (guardar.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(guardar.FileName);
                sw.Write(editorDeTexto.Text);
                sw.Close();
            }
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Undo(); //ctrl+z
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Redo(); //ctrl+y
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Cut(); //ctrl+x
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Copy(); //ctrl+c
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Paste(); //ctrl+v
        }

        private void seleccionartodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.SelectAll(); //ctrl+a
        }

        private void editorDeTexto_VScroll(object sender, EventArgs e)
        {
            int nPos = GetScrollPos(editorDeTexto.Handle, (int)ScrollBarType.SbVert);
            nPos <<= 16;
            uint wParam = (uint)ScrollBarCommands.SB_THUMBPOSITION | (uint)nPos;
            SendMessage(contadorLineas.Handle, (int)Message.WM_VSCROLL, new IntPtr(wParam), new IntPtr(0));
        }

        private void editorDeTexto_SelectionChange()
        {
            int index = editorDeTexto.SelectionStart; //Saber el indice de la posicion del cursor
            int line = editorDeTexto.GetLineFromCharIndex(index); //Devuelve la linea del cursor
            int column = (editorDeTexto.SelectionStart - editorDeTexto.GetFirstCharIndexFromLine(line)); // Le resta al indice del cursor el indice del primer caracter de la linea
            line++;
            column++;
        }


        private void editorDeTexto_TextChanged(object sender, EventArgs e)
        {
            int index = editorDeTexto.SelectionStart;
            int line = editorDeTexto.GetLineFromCharIndex(index);
            int caracter = editorDeTexto.GetFirstCharIndexFromLine(line);
            int columna = index - caracter + 1;
            Lin.Text = (line + 1).ToString();
            Col.Text = columna.ToString();
            cont = linea.getLienas(editorDeTexto.Text);
            StringBuilder auxiliar = new StringBuilder();
            for (int i = 0; i < cont; i++)
            {
                auxiliar.AppendLine((String.Empty + (i + 1)));
            }
            contadorLineas.Text = auxiliar.ToString();
            startPrinting();
        }
    
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog(); //Cuadro de dialogo
            guardar.Filter = "Archivos de texto (.txt)|*.txt";
            guardar.Title = "Guardar archivo";
            if (guardar.ShowDialog() == DialogResult.OK) //Si se decide guardar 
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(guardar.FileName); //Escritor de archivos
                sw.Write(editorDeTexto.Text); //Guarda todo lo que este en el editor en un archivo nuevo
                sw.Close();
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            editorDeTexto.Clear(); //Elimina todo el contenido del editor de texto actual
            contadorLineas.Text = "1"; //Limpieza del label de numero de lineas
            OpenFileDialog abrir = new OpenFileDialog(); //Abre un nuevo cuadro de dialogo
            abrir.Filter = "Archivos de texto (.txt)|*.txt";
            abrir.Title = "Abrir archivo";
            if (abrir.ShowDialog() == DialogResult.OK) //Si se decide abrir ese archivo
            {
                int lineas = 0;
                System.IO.StreamReader sr = new System.IO.StreamReader(abrir.FileName); //Lector de archivos
                System.IO.StreamReader sr2 = new System.IO.StreamReader(abrir.FileName);
                editorDeTexto.Text = sr.ReadToEnd(); //Escribe todo lo que tenga dicho archivo en el editor
                while (sr2.ReadLine() != null)
                {
                    lineas++;
                    contadorLineas.Text += "\n" + lineas.ToString();
                }
                contadorLineas.Text = contadorLineas.Text.Remove(contadorLineas.Text.Length - 3);
                cont = lineas - 1;
                sr.Close();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            editorDeTexto.Clear(); //Elimina todo el contenido del editor de texto actual
            contadorLineas.Text = "1"; //Limpieza del label de numero de lineas
        }

        private void btnDeshacer_Click(object sender, EventArgs e)
        {
            editorDeTexto.Undo();
        }

        private void btnRehacer_Click(object sender, EventArgs e)
        {
            editorDeTexto.Redo();
        }

        private void editorDeTexto_Click(object sender, EventArgs e)
        {
            int index = editorDeTexto.SelectionStart;
            int line = editorDeTexto.GetLineFromCharIndex(index);
            int caracter = editorDeTexto.GetFirstCharIndexFromLine(line);
            int columna = index - caracter + 1;
            Lin.Text = (line + 1).ToString();
            Col.Text = columna.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lLexico = lexico.automata(editorDeTexto.Text);
            clase = lexico.getDescription(lLexico);
            errLexico();
            if (lexico.numeroErrores() == 0)
            {
                arbolSintactico.Nodes.Clear();
                sintactico = new aSintactico(lLexico, arbolSintactico, clase);
                sintactico.analisisSintactico();
                erroresSintacticos.Text = sintactico.Errores();
                if (sintactico.Errores().Length == 0)
                {
                    DialogResult noErrS = MessageBox.Show("Sin errores sintacticos", "Compilacion completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DialogResult siErrS = MessageBox.Show("Se han detectado errores sintacticos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                DialogResult noErr = MessageBox.Show("Sin errores lexicos", "Analisis Lexico Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult siErr = MessageBox.Show("Se han detectado errores lexicos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //DialogResult res = MessageBox.Show("No hace nada...aun =)", "Boton secreto que no hace nada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Salir del programa?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void errLexico()
        {

            StringBuilder tokens = new StringBuilder();
            StringBuilder errores = new StringBuilder();
            tokens.Append("Lexema\t\t\t\t" + "Tipo Token" + "\n\n");
            errores.Append("Lexema\t\t" + "Ubicacion" + "\n");
            if (lLexico.Count > 0)
            {
                for (int i = 0; i < lLexico.Count; i++)
                {
                    if (clase[i] != "Error")
                    {
                        tokens.Append(lLexico[i].getName() + "\t\t\t\t" + clase[i]);
                    }
                    else
                    {
                        errores.Append(lLexico[i].getName() + "\t\t" + "Fila: " + lLexico[i].getLocation().getFila() + "\t\t Columna: " + lLexico[i].getLocation().getColumna() + '\n');
                    }
                }
                salidaLexico.Text = tokens.ToString();
                erroresLexicos.Text = errores.ToString();

            }
        }

        public void startPrinting()
        {

            int selecionStart = editorDeTexto.SelectionStart;
            int selectionLenght = editorDeTexto.SelectionLength;
            Color colorOriginal = Color.Black;
            panelET.Focus();
            editorDeTexto.SelectionStart = 0;
            editorDeTexto.SelectionLength = editorDeTexto.Text.Length;
            editorDeTexto.SelectionColor = colorOriginal;
            printCode();
            editorDeTexto.SelectionStart = selecionStart;
            editorDeTexto.SelectionLength = selectionLenght;
            editorDeTexto.SelectionColor = colorOriginal;
            editorDeTexto.Focus();

        }

        private void printCode()
        {
            palabrasReservadas();
            simbolos();
            comentarios();
        }

        private void printBase(MatchCollection matches, Color color)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                editorDeTexto.SelectionStart = matches[i].Index;
                editorDeTexto.SelectionLength = matches[i].Length;
                editorDeTexto.SelectionColor = color;
            }
        }

        private void palabrasReservadas()
        {
            Regex regex = new Regex(@"(\+|\-|\*|/|<|>|=|;|,|\(|\)|{|})?(if|then|else|end|do|while|cout|cin|int|float|main|boolean|real)"
                + @"(\+|\-|\*|/|<|>|=|;|,|\(|\)|{|}|!)?");
            MatchCollection matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Red);

            regex = new Regex("^(if|then|else|end|do|while|cout|cin|int|until|float|main|boolean|real)");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Red);

            regex = new Regex(" (if|then|else|end|do|while|cout|cin|int|float|until|main|boolean|real) ");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Red);

            regex = new Regex(" (if|then|else|end|do|while|cout|cin|int|float|until|main|boolean|real)");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Red);

            regex = new Regex(" (if|then|else|end|do|while|cout|cin|int|float|until|main|boolean|real)\n");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Red);

            regex = new Regex("\t(if|then|else|end|do|while|cout|cin|int|float|until|main|boolean|real)");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Red);

            regex = new Regex("\t(if|then|else|end|do|while|cout|cin|int|float|until|main|boolean|real)\n");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Red);

            regex = new Regex("\n(if|then|else|end|do|while|cout|cin|int|float|until|main|boolean|real)");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Red);


            regex = new Regex(@"\(int|\(float");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Red);

            regex = new Regex("(if|then|else|end|do|while|cout|cin|int|float|until|main|boolean|real)([a-zA-Z]|[0-9])+");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Black);

            regex = new Regex("([a-zA-Z]|[0-9])+(if|then|else|end|do|while|until|cout|cin|int|float|main|boolean|real)");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Black);

        }

        private void simbolos()
        {
            Regex regex = new Regex(@"(\+|\-|\*|<|=|<=|>|>=|==|/|!=|;|,|\)|{|}|\+\+|\-\-)");
            MatchCollection matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Blue);

            regex = new Regex(@"\(");
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Blue);

        }

        private void comentarios()
        {
            Regex regex = new Regex("(//)(^\")*(.)*");
            MatchCollection matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Gray);
            regex = new Regex(@"/\*(?:(?!\*/).)*\*/", RegexOptions.Singleline);
            matches = regex.Matches(editorDeTexto.Text);
            printBase(matches, Color.Gray);
        }
    }
    public class Lineas
    {
        public Lineas()
        {

        }
        public int getLienas(String Texto)
        {

            Char[] letra = Texto.ToCharArray();
            int contador = 1;
            for (int i = 0; i < letra.Length; i++)
            {

                if (letra[i] == '\n')
                {
                    contador++;
                }
            }
            return contador;
        }
    }
}
