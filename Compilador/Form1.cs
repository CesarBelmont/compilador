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
        public Form1()
        {
            InitializeComponent();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Clear();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.Filter = "Archivos de texto (.txt)|*.txt";
            abrir.Title = "Abrir archivo";
            if (abrir.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr= new System.IO.StreamReader(abrir.FileName);
                editorDeTexto.Text = sr.ReadToEnd();
                sr.Close();
            }

        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog();
            guardar.Filter = "Archivos de texto (.txt)|*.txt";
            guardar.Title = "Guardar archivo";
            if(guardar.ShowDialog()== DialogResult.OK)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(guardar.FileName);
                sw.Write(editorDeTexto.Text);
                sw.Close();
            }
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Undo();
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Redo();
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Cut();
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Copy();
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.Paste();
        }

        private void seleccionartodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorDeTexto.SelectAll();
        }
    }
}
