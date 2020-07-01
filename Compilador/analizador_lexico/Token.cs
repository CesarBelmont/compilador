using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.analizador_lexico
{
    class Token
    {
        private String name; //Nombre del token
        private ubicacionToken Location; //Su ubicacion
        private int tokenDescriptor; //Su descripcion

        public Token(String name, ubicacionToken Location)
        {
            this.name = name;
            this.Location = Location;
        }

        public Token()
        {

        }

        public void setName(String name)
        {
            this.name = name;
        }

        public String getName()
        {
            return this.name;
        }

        public void setLocation(ubicacionToken Location)
        {
            this.Location = Location;
        }

        public ubicacionToken getLocation()
        {
            return this.Location;
        }

        public int getTokenDescriptor()
        {
            return tokenDescriptor;
        }

        public void setTokenDescription(int tokenDescriptor)
        {
            this.tokenDescriptor = tokenDescriptor;
        }
    }
}
