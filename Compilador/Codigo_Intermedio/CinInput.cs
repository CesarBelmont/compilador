using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador.Codigo_Intermedio
{
    public partial class CinInput : Form
    {
        private string id;
        private char type;
        private Boolean flagMenos;
        private Boolean flagDecimal;
        public CinInput()
        {
            InitializeComponent();
        }

        public CinInput(string id, char type)
        {
            this.id = id;
            this.type = type;
            InitializeComponent();
            Titulo.Text = "Pon el valor " + (type == 'i' ? "int" : "float") + " de " + id + ":";
            flagDecimal = false;
            flagMenos = false;
            buttonOk.Enabled = false;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams paramse = base.CreateParams;
                paramse.ClassStyle |= 0x200;
                return paramse;
            }
        }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            VirtualMachine.value = value.Text;
            this.Close();
        }

        private void value_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool flagBase = false;
            if (e.KeyChar != (char)Keys.Back)
            {
                switch (this.type)
                {
                    case 'i':
                        if (mBoolKeys(e.KeyChar))
                        {
                            if (e.KeyChar == '.' || e.KeyChar == ',')
                                e.Handled = true;
                            if (flagBase)
                                e.Handled = true;
                        }
                        else
                            e.Handled = true;
                        if (flagBase && mBoolKeys(e.KeyChar))
                            e.Handled = true;
                        break;
                    case 'r':
                        if (mBoolKeys(e.KeyChar))
                        {
                            if (this.flagDecimal)
                            {
                                if (e.KeyChar == '.' || e.KeyChar == ',')
                                    e.Handled = true;
                            }
                            if (flagBase)
                                e.Handled = true;
                        }
                        else
                            e.Handled = true;
                        if (flagBase && mBoolKeys(e.KeyChar))
                            e.Handled = true;
                        break;
                }
                if (this.flagMenos)
                {
                    if (e.KeyChar == '-')
                        e.Handled = true;
                }
            }
        }
        private bool mBoolKeys(char e)
        {
            if (e == (char)Keys.Back || e == (char)Keys.Delete || e == (char)Keys.Left || e == (char)Keys.Right || Char.IsNumber(e) || e == '-')
                return true;
            return false;
        }

        private void value_TextChanged(object sender, EventArgs e)
        {
            switch (type)
            {
                case 'i':
                    if (value.Text.IndexOf("-") != -1)
                        this.flagMenos = true;
                    else
                        this.flagMenos = false;
                    break;
                case 'r':
                    if (value.Text.IndexOf(".") != -1)
                        this.flagDecimal = true;
                    else
                        this.flagDecimal = false;
                    if (value.Text.Length > 0)
                    {
                        this.buttonOk.Enabled = true;
                        if (value.Text.IndexOf(".") != -1 && value.Text.IndexOf(".") < value.Text.Length - 1)
                        {

                            this.buttonOk.Enabled = true;
                        }
                        else
                        {

                            this.buttonOk.Enabled = false;
                        }
                        if (value.Text.IndexOf(".") == -1 && value.Text.IndexOf("-") == -1)
                        {

                            this.buttonOk.Enabled = true;
                        }
                    }
                    else
                    {

                        this.buttonOk.Enabled = false;
                    }
                    break;
            }
            if (value.Text.IndexOf("-") != -1)
                this.flagMenos = true;
            else
                this.flagMenos = false;
            if (this.value.Text.IndexOf("-") != -1)
            {
                if (this.value.Text.Length == 1)
                {
                    this.buttonOk.Enabled = false;
                }
                else
                {
                    if (this.value.Text.IndexOf("-") > 0)
                    {
                        this.buttonOk.Enabled = false;
                    }
                    else
                    {
                        this.buttonOk.Enabled = true;
                    }
                }
            }
            if (this.value.Text.IndexOf(".") == this.value.Text.Length - 1)
            {
                this.buttonOk.Enabled = false;
            }
            if (this.value.Text.Length > 0 && this.value.Text.IndexOf(".") == -1 && this.value.Text.IndexOf("-") == -1)
            {
                this.buttonOk.Enabled = true;
            }
        }
    }
}
