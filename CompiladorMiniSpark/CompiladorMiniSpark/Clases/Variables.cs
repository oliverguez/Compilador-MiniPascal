using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorMiniSpark.Clases
{
    public class Variables
    {
        string lexema;
        string tipoVariable;
        Variables siguiente = null;

        public string Lexema
        {
            get
            {
                return lexema;
            }
            set
            {
                lexema = value;
            }
        }

        public string TipoVariable
        {
            get
            {
                return tipoVariable;
            }
            set
            {
                tipoVariable = value;
            }
        }

        public Variables Siguiente
        {
            get
            {
                return siguiente;
            }
            set
            {
                siguiente = value;
            }
        }


        public Variables(string lexema, string tipoVariable)
        {
            this.lexema = lexema;
            this.tipoVariable = tipoVariable;
        }


    }

   
}
