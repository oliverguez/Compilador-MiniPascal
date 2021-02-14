using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CompiladorMiniSpark.Clases;

namespace CompiladorMiniSpark
{
    public class Lexico
    {
        Nodo cabeza = null;
        Nodo p;
        int estado = 0;
        int token;
        int columna;
        int valorMatrizTransicion;
        int numeroDeRenglon = 1;
        int caracter;
        int posicionBuffer = 0;
        String lexema;
        private bool errorQueSeEncontro;

        public Nodo Cabeza
        {
            get
            {
                return cabeza;
            }

            set
            {
                cabeza = value;
            }



        }
        public bool ErrorQueSeEncontro

        {

            get
            {
                return errorQueSeEncontro;
            }

            set
            {
                errorQueSeEncontro = value;
            }

        }



        string rutaArchivo = @"C:\Users\olive\Desktop\Compilador\ArchivoLeer.txt";



        int[,] matriz = new int[,]
            {
                /*          l,          d,       +,      -,      *,      =,      .,      ,       ;,     :,      <,      >,      (,       ),      ",     eb,     tab,    nl      eol,    eof,    oc      */
           /*0*/    {       1,          2,      103,    104,    105,    110,    112,    113,    114,    7,      6,      5,      8,      116,    11,     0,      0,      0,      0,      0,      504 },
           /*1*/    {       1,          1,      100,    100,    100,    100,    100,    100,    100,    100,    100,    100,    100,    100,    100,    100,    100,    100,    100,    100,    100 },
           /*2*/    {       101,        2,      101,    101,    101,    101,    3,      101,    101,    101,    101,    101,    101,    101,    101,    101,    101,    101,    101,    101,    101 },
           /*3*/    {       500,        4,      500,    500,    500,    500,    500,    500,    500,    500,    500,    500,    500,    500,    500,    500,    500,    500,    500,    500,    500 },
           /*4*/    {       102,        4,      101,    102,    102,    102,    102,    102,    102,    102,    102,    102,    102,    102,    102,    102,    102,    102,    102,    102,    102 },
           /*5*/    {       107,        107,    107,    107,    107,    109,    107,    107,    107,    107,    107,    107,    107,    107,    107,    107,    107,    107,    107,    107,    107 },
           /*6*/    {       106,        106,    106,    106,    106,    108,    106,    106,    106,    106,    106,    111,    106,    106,    106,    106,    106,    106,    106,    106,    106 },
           /*7*/    {       117,        117,    117,    117,    117,    130,    117,    117,    117,    117,    117,    117,    117,    117,    117,    117,    117,    117,    117,    117,    117 },
           /*8*/    {       115,        115,    115,    115,    9,      115,    115,    115,    115,    115,    115,    115,    115,    115,    115,    115,    115,    115,    115,    115,    115 },
           /*9*/    {       9,          9,      9,      9,      10,     9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      502,    9   },
           /*10*/   {       9,          9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      0,      9,      9,      9,      9,      9,      9,      9   },
           /*11*/   {       11,         11,     11,     11,     11,     11,     11,     11,     11,     11,     11,     11,     11,     11,     128,    11,     11,     11,     503,    502,    11 }

        };

        string[][] palabrasReservadas = new string[][]
            {
                                /*                 1                 2         /*
                    /*1*/  new string [] {        "if"           , "200"       },
                    /*2*/  new string [] {        "then"         , "201"       },
                    /*3*/  new string [] {        "else"         , "202"       },
                    /*4*/  new string [] {        "of"           , "203"       },
                    /*5*/  new string [] {        "while"        , "204"       },
                    /*6*/  new string [] {        "do"           , "205"       },
                    /*7*/  new string [] {        "begin"        , "206"       },
                    /*8*/  new string [] {        "end"          , "207"       },
                    /*9*/  new string [] {        "read"         , "208"       },
                    /*10*/ new string [] {        "write"        , "209"       },
                    /*11*/ new string [] {        "var"          , "210"       },
                    /*12*/ new string [] {        "array"        , "211"       },
                    /*13*/ new string [] {        "procedure"    , "212"       },
                    /*14*/ new string [] {        "program"      , "213"       },
                    /*15*/ new string [] {        "true"         , "214"       },
                    /*16*/ new string [] {        "false"        , "215"       },
                    /*17*/ new string [] {        "and"          , "216"       },
                    /*18*/ new string [] {        "or"           , "217"       },
                    /*19*/ new string [] {        "not"          , "218"       },
                    /*20*/ new string [] {        "div"          , "219"       },
                    /*21*/ new string [] {        "integer"      , "220"       },
                    /*22*/ new string [] {        "real"         , "221"       },
                    /*23*/ new string [] {        "string"       , "222"       }
            };


        string[][] tablaDeTipos = new string[][]
            {




                                /*                 1                 2         /*
                    /*1*/  new string [] {        "if"           , "200"       },
                    /*2*/  new string [] {        "then"         , "201"       },
                    /*3*/  new string [] {        "else"         , "202"       },
                    /*4*/  new string [] {        "of"           , "203"       },
                    /*5*/  new string [] {        "while"        , "204"       },
                    /*6*/  new string [] {        "do"           , "205"       },
                    /*7*/  new string [] {        "begin"        , "206"       },
                    /*8*/  new string [] {        "end"          , "207"       },
                    /*9*/  new string [] {        "read"         , "208"       },
                    /*10*/ new string [] {        "write"        , "209"       },
                    /*11*/ new string [] {        "var"          , "210"       },
                    /*12*/ new string [] {        "array"        , "211"       },
                    /*13*/ new string [] {        "procedure"    , "212"       },
                    /*14*/ new string [] {        "program"      , "213"       },
                    /*15*/ new string [] {        "true"         , "214"       },
                    /*16*/ new string [] {        "false"        , "215"       },
                    /*17*/ new string [] {        "and"          , "216"       },
                    /*18*/ new string [] {        "or"           , "217"       },
                    /*19*/ new string [] {        "not"          , "218"       },
                    /*20*/ new string [] {        "div"          , "219"       },
                    /*21*/ new string [] {        "integer"      , "220"       },
                    /*22*/ new string [] {        "real"         , "221"       },
                    /*23*/ new string [] {        "string"       , "222"       }
            };



        string[][] tablaErrores = new string[][]
        {
                /*              1                      2            */ 
            /*1*/ new string [] {     "Se esperaba digito" ,       "500"       },
            /*3*/ new string [] {     "eof inesperado"     ,       "502"       },
            /*4*/ new string [] {     "eol inesperado"     ,       "503"       },
            /*5*/ new string [] {     " Simbolo no valido" ,       "504"       }




        };



        public Lexico()
        {
            StreamReader sr = new StreamReader(rutaArchivo);
            try
            {
                using (sr)
                {

                    while (caracter != -1)
                    {
                        caracter = sr.Read();
                        posicionBuffer++;
                        sr.DiscardBufferedData();
                        sr.BaseStream.Position = posicionBuffer;

                        if (Char.IsLetter((char)caracter))
                        {
                            columna = 0;
                        }
                        else if (Char.IsDigit((char)caracter))
                        {
                            columna = 1;
                        }
                        else
                        {
                            switch (caracter)
                            {
                                case '+':
                                    columna = 2;
                                    break;
                                case '-':
                                    columna = 3;
                                    break;
                                case '*':
                                    columna = 4;
                                    break;
                                case '=':
                                    columna = 5;
                                    break;
                                case '.':
                                    columna = 6;
                                    break;
                                case ',':
                                    columna = 7;
                                    break;
                                case ';':
                                    columna = 8;
                                    break;
                                case ':':
                                    columna = 9;
                                    break;
                                case '<':
                                    columna = 10;
                                    break;
                                case '>':
                                    columna = 11;
                                    break;
                                case '(':
                                    columna = 12;
                                    break;
                                case ')':
                                    columna = 13;
                                    break;
                                case '"':
                                    columna = 14;
                                    break;
                                case ' ':
                                    columna = 15; //eb
                                    break;
                                case (char)9:
                                    columna = 16; //tab
                                    break;
                                case (char)10:
                                    {
                                        columna = 17;
                                        numeroDeRenglon = numeroDeRenglon + 1;
                                    }
                                    break;
                                case (char)13:
                                    columna = 18; //eol
                                    break;
                                case -1:
                                    columna = 19; //eof
                                    break;
                                default:
                                    columna = 20;
                                    break;
                            }
                        }

                        valorMatrizTransicion = matriz[estado, columna];

                        if (valorMatrizTransicion < 100)
                        {
                            estado = valorMatrizTransicion;

                            if (estado == 0)
                            {
                                lexema = "";
                            }
                            else
                            {
                                lexema += (char)caracter;
                            }

                        }
                        else if (valorMatrizTransicion >= 100 && valorMatrizTransicion < 500)
                        {
                            if (valorMatrizTransicion == 100)
                            {
                                ValidarReservadas();
                            }
                           
                            if (valorMatrizTransicion == 100 || valorMatrizTransicion == 101 || valorMatrizTransicion == 102 || valorMatrizTransicion == 106 || valorMatrizTransicion == 107 ||
                                valorMatrizTransicion == 115 || valorMatrizTransicion >= 200)
                            {
                                posicionBuffer--;
                                sr.BaseStream.Position = posicionBuffer;

                            }
                            else
                            {
                                lexema = lexema + (char)caracter;
                            }

                            NodoInsertar();
                            estado = 0;
                            lexema = "";
                            


                        }
                        else
                        {
                            Console.WriteLine("!ERROR!");
                            MensajeDeError();
                            break;
                        }
                    }
                    ImprimirNodos();
                } 
            }
            catch (Exception )
            {

                throw ;

            } 
            

        }

        private void ImprimirNodos()
        {
            if (errorQueSeEncontro == false)
            {
                p = cabeza;
                Console.WriteLine("---------------------------------------------------------------------------------------");
                Console.WriteLine("|Lexema                          |       Token                   |       Renglon     |   ");
                Console.WriteLine("---------------------------------------------------------------------------------------");

                while (p != null)
                {
                    Console.WriteLine(String.Format("| {0,-30} | {1,-30} | {2,8}        |", p.Lexema, p.Token, p.Renglon));

                    p = p.Siguiente;

                }

            }
            else
            {

                Console.WriteLine("!ERROR!");
            }
           
        }

        public void MensajeDeError()
        {
            if ( valorMatrizTransicion >= 500)
            {
                foreach (string[] erroresMatriz in tablaErrores)
                {
                    
                        if (valorMatrizTransicion == int.Parse(erroresMatriz[1]))
                        {
                            Console.WriteLine($@"El error encontrado es: {erroresMatriz[0]} error {valorMatrizTransicion} en el renglon {numeroDeRenglon}" );
                        }
                   
                    errorQueSeEncontro = true;

                }

            }
        }

        private void ValidarReservadas()
        {
            foreach (string[] palReservada in palabrasReservadas)
            {
                
                    if (lexema.Equals(palReservada[0]))
                    {
                        valorMatrizTransicion = int.Parse(palReservada[1]);
                    }
                
               
            }
        }

        private void NodoInsertar()
        {
            Nodo nodo = new Nodo(lexema, valorMatrizTransicion, numeroDeRenglon);
            {
                if (cabeza == null)
                {
                    cabeza = nodo;
                    p = cabeza;
                }
                else
                {
                    p.Siguiente = nodo;
                    p = nodo;
                }
            }
        }

      

    }

        

        
    }


    