using CompiladorMiniSpark.Clases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CompiladorMiniSpark
{
    public class Program
    {
        static void Main()
        {
            Nodo p;
            Lexico lexico = new Lexico();
            
          

            if (!lexico.ErrorQueSeEncontro)
            {
                Console.WriteLine("Analis lexico terminado!" +
                    "");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Tipos de variables");
                Console.WriteLine("-----------------------------------");

                Sintactico sintactico = new Sintactico(lexico.Cabeza);

                if (!sintactico.ErrorEncontrado)
                {
                    Console.WriteLine("");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Analisis Sintactico terminado!");
                    Console.WriteLine("-----------------------------------");
                    Ensamblador ensamblador = new Ensamblador(lexico.Cabeza, sintactico.Cabeza_var);
                }
                else
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("!ERROR!");
                    
                }
            }
            
           


           

        }
    }
}
