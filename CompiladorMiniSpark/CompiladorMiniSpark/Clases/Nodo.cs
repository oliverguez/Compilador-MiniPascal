using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorMiniSpark
{
    public class Nodo
    {
        string lexema;
        int token;
        int renglon;
         Nodo siguiente = null;

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
        public int Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
            }
        }
        public int Renglon
        {
            get
            {
                return renglon;
            }
            set
            {
                renglon = value;
            }
        }
        public Nodo Siguiente
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
        public Nodo(string lexema, int token, int renglon)
        {
            this.lexema = lexema;
            this.token = token;
            this.renglon = renglon;
        }
    }
}
