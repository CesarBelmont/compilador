using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador
{
    public partial class Form1 : Form
    {
        private int cont = 1; //Contador de lineas
        public Form1()
        {
            InitializeComponent();
            editorDeTexto.SelectAll();
            editorDeTexto.SelectionIndent += 25;
            editorDeTexto.SelectionRightIndent += 25;
            editorDeTexto.DeselectAll();
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
                int lineas = 1;
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

        private void editorDeTexto_TextChanged(object sender, EventArgs e)
        {
            //Este evento detecta cuando el editor tiene cambios (escribir, borrar, etc.)
            //Posicion de la linea
            int index = editorDeTexto.SelectionStart;
            int line = editorDeTexto.GetLineFromCharIndex(index);
            //Posicion de la columna
            int caracter = editorDeTexto.GetFirstCharIndexFromLine(line);
            int columna = index - caracter + 1;
            //Los pone en el label
            Lin.Text = (line + 1).ToString();
            Col.Text = columna.ToString();

            if (editorDeTexto.Lines.Length == 0)
            {
                //Limita el label de lineas a siempre mostrar 1, al igual que el indicador de la posicion
                Lin.Text = "1";
                contadorLineas.Text = "1";
            }
            if (columna == 0)
            {
                //Lo mismo que lo anterior, pero a las columnas
                Col.Text = "1";

            }
            if (editorDeTexto.Lines.Length < cont) //Compara el numero de lineas
            {
                if (editorDeTexto.Lines.Length == 0)
                {
                    cont = 1;
                }
                //La cantidad de caracteres a eliminar del label depende del numero de lineas
                if (editorDeTexto.Lines.Length > 0 && editorDeTexto.Lines.Length < 10)
                {
                    cont = editorDeTexto.Lines.Length;
                    contadorLineas.Text = contadorLineas.Text.Remove(contadorLineas.Text.Length - 2); //Elimina el ultimo numero de linea del label de lineas
                }
                if (editorDeTexto.Lines.Length >= 10 && editorDeTexto.Lines.Length < 100)
                {
                    contadorLineas.Text = contadorLineas.Text.Remove(contadorLineas.Text.Length - 3); //Elimina el ultimo numero de linea del label de lineas
                    cont = editorDeTexto.Lines.Length;
                }
                if (editorDeTexto.Lines.Length > 100 && editorDeTexto.Lines.Length < 1000)
                {
                    contadorLineas.Text = contadorLineas.Text.Remove(contadorLineas.Text.Length - 4); //Elimina el ultimo numero de linea del label de lineas
                    cont = editorDeTexto.Lines.Length;
                }
                if (editorDeTexto.Lines.Length > 1000 && editorDeTexto.Lines.Length < 10000)
                {
                    contadorLineas.Text = contadorLineas.Text.Remove(contadorLineas.Text.Length - 5); //Elimina el ultimo numero de linea del label de lineas
                    cont = editorDeTexto.Lines.Length;
                }
            }//if comparador de lineas
            if (panelET.VerticalScroll.Visible)
            {
                editorDeTexto.ScrollToCaret();
                panelET.VerticalScroll.Value = panelET.VerticalScroll.Maximum;
            }

        }

        private void editorDeTexto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) //Cada que el usuario usa enter
            {
                contadorLineas.Text += "\n" + editorDeTexto.Lines.Length.ToString(); //Agrega el numero de linea al label de las lineas
                cont++;
            }
            if (e.KeyChar == (char)37) //Izquierda
            {

            }
            if (e.KeyChar == (char)38) //Arriba
            {

            }
            if (e.KeyChar == (char)39) //Derecha
            {

            }
            if (e.KeyChar == (char)40) //Abajo
            {

            }
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
            OpenFileDialog abrir = new OpenFileDialog(); //Abre un nuevo cuadro de dialogo
            abrir.Filter = "Archivos de texto (.txt)|*.txt";
            abrir.Title = "Abrir archivo";
            if (abrir.ShowDialog() == DialogResult.OK) //Si se decide abrir ese archivo
            {
                int lineas = 1;
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

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("No hace nada...aun =)", "Boton secreto que no hace nada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
