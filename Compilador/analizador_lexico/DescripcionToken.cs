using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.analizador_lexico
{
    class DescripcionToken //Descripcion del token, arroja errores en caso de ser de ese tipo
    {
        public enum TokenType
        {
            VALIDO,
            ERROR
        }
        private TokenType tokenType;
        private String description;
        public TokenType gettokenType()
        {
            return tokenType;
        }
        public void settokenType(TokenType tokenType)
        {
            this.tokenType = tokenType;
        }
        public String getdescription()
        {
            return description;
        }
        public void setdescription(String description)
        {
            this.description = description;
        }
    }
}
