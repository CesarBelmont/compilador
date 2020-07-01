using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.analizador_lexico
{
    class ubicacionToken //Localizar en donde se encuentra el token
    {
        private int fila;
        private int columna;

        public ubicacionToken(int fila, int columna)
        {
            this.fila = fila;
            this.columna = columna;
        }
        public ubicacionToken()
        {

        }
        public void setFila(int fila)
        {
            this.fila = fila;
        }
        public int getFila()
        {
            return this.fila;
        }
        public void setColumna(int columna)
        {
            this.columna = columna;
        }
        public int getColumna()
        {
            return this.columna;
        }
        public override string ToString()
        {
            return fila + "," + columna;
        }
    }
}
