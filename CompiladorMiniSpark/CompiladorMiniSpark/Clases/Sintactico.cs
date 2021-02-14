using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiladorMiniSpark.Clases;


namespace CompiladorMiniSpark.Clases
{
    public class Sintactico
    {


        int if_level = 0;
        int while_level = 0;
        Nodo pWrite;
        Nodo pRead;
        private bool errorEncontrado;
        private Nodo cabeza;
        Variables cabeza_Var;
        List<Nodo> listaInfijo = new List<Nodo>();
        List<String> listaTipos = new List<String>();
        public static List<String> listaPolish = new List<String>();

        Stack<String> listaAxuliar = new Stack<String>();
        Stack<String> listaAuxiliarValidarTipos = new Stack<String>();

     

        public IDictionary<string, int> prioridades = new Dictionary<string, int>
        {
            {"(",  -1    },
            {")",  -1    },
            {":=",  0    },
            {"or",  1    },
            {"and", 2    },
            {"not", 3    },
            {"<",   4    },
            {">",   4    },
            {"<=",  4    },
            {">=",  4    },
            {"=",   4    },
            {"<>",  4    },
            {"+",   5    },
            {"-",   5    },
            {"*",   6    },
            {"div", 6    },
            {"s-",  7    },
            {"s+",  7    }
            
            
        };


        public IDictionary<string, int> tokenVariables = new Dictionary<string, int>
        {
            {"integer",  0 },
            {"real",  1    },
            {"string",  2  },
            {"bool", 3}
          


        };

        string[][] tablaErroresSemantico = new string[][]
       {
                /*              1                      2            */ 
            /*1*/ new string [] {   "Variable ya declarada ",       "530"       },
            /*3*/ new string [] {   "Variable no declarada ",       "531"       },
            /*4*/ new string [] {   "Incompatibilidad de tipos ",       "532"       },




       };

        string[,,,] tablaSistemaTipos = new string[1, 16, 4, 4]
                    {
                        {
                           
                           /*+*/ {
                                { "integer", "real", "string", "error"},
                                { "real", "real", "string", "error"},
                                { "string", "string", "string", "error"},
                                { "error", "error", "error","error" } 
                            
                            },

                           {
                           /*-*/     { "integer", "real", "error", "error"},
                                { "real", "real", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error","error" } 
                           },
                           {
                           /***/     { "integer", "real", "error", "error"},
                                { "real", "real", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error","error" }
                           },
                           {
                           /*DIV*/     { "real", "real", "error", "error"},
                                { "real", "real", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error","error" } 
                           },
                           {
                           /*:=*/     { "ok", "ok", "error", "error"},
                                { "ok", "ok", "error", "error"},
                                { "error", "error", "ok", "error"},
                                { "error", "error", "error","ok" }
                            },
                           {
                            /*>*/    { "bool", "bool", "error", "error"},
                                { "bool", "bool", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error","error" } 
                            },
                           {
                           /*<*/     { "bool", "bool", "error", "error"},
                                { "bool", "bool", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error","error" } 
                           },
                           {
                           /*>=*/     { "bool", "bool", "error", "error"},
                                { "bool", "bool", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error","error" } 
                            },
                           {
                           /*<=*/     { "bool", "bool", "error", "error"},
                                { "bool", "bool", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error","error" }
                            },
                           {
                            /*=*/    { "bool", "bool", "error", "error"},
                                { "bool", "bool", "error", "error"},
                                { "error", "error", "bool", "error"},
                                { "error", "error", "error","bool" } 
                           },
                           {
                            /*<>*/    { "bool", "bool", "error", "error"},
                                { "bool", "bool", "error", "error"},
                                { "error", "error", "bool", "error"},
                                { "error", "error", "error","bool" } 
                           },
                           {
                            /*And*/    { "error", "error", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error", "bool" }
                           },
                           {
                           /*or*/     { "error", "error", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error", "bool" } 
                           },
                           {
                           /*not*/     { "bool", "error", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error","error" } 
                           },
                           {
                            /*s+*/    { "integer", "real", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error","error" }

                            },
                           {
                            /*s-*/    { "integer", "real", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error", "error"},
                                { "error", "error", "error","error" }
                            }


                        }
                    };



                




        public Sintactico(Nodo cabeza)
        {
            this.cabeza = cabeza;
            Program(cabeza);
        }

        

        

        public Variables Cabeza_var
        {
            get
            {
                return cabeza_Var;
            }

            set
            {
                cabeza_Var = value;
            }

        }


        public bool ErrorEncontrado

        {

            get
            {
                return errorEncontrado;
            }

            set
            {
                errorEncontrado = value;
            }

        }






        public Nodo Program(Nodo p)
        {
            if (p == null || p.Token != 213)
            {
                Console.WriteLine("Error!: Se esperaba 'Program' en el renlgon" + p.Renglon); ;
                errorEncontrado = true;
            }
            p = p.Siguiente;
            if (p == null || p.Token != 100)
            {
                Console.WriteLine("Error!: Se esperaba 'Identificador' en el renglon " + p.Renglon);
                errorEncontrado = true;
            }
            p = p.Siguiente;
            if (p == null || p.Token != 114)
            {
                Console.WriteLine("Error!: Se esperaba ';' en el renglon " + p.Renglon);
                errorEncontrado = true;

            }
            p = p.Siguiente;
            p = Block(p);
            if (p == null || p.Token != 112)
            {
                Console.WriteLine("Error!: Se esperaba '.' en el renglon " + p.Renglon);
                errorEncontrado = true;
            }
           
            Console.WriteLine("");
            Console.WriteLine("Lista de polish");

            foreach (string item in listaPolish)
            {
                Console.Write("|" + item + "|" + " ");

            }
            return p;
        }

        public Nodo Block(Nodo p)
        {

            p = Variable_declaration_part(p);
            p = Statement_part(p);

            return p;
        }


        public Nodo Variable_declaration_part(Nodo p)
        {
            if (p == null || p.Token != 210)
            {
                Console.WriteLine("Error!: Se esperaba Var en el renglon " + p.Renglon);
            }
            p = p.Siguiente;
            p = Variable_declaration(p);
            while (p != null && p.Token != 206)
            {
                p = Variable_declaration(p);
            }
            return p;
        }

        public Nodo Variable_declaration(Nodo p)
        {
            List<string> identificadores = new List<string>();
            Variables v = cabeza_Var;

            if (p == null || p.Token != 100)
            {
                Console.WriteLine("Error!: Se esperaba 'Identificador' en el renglon " + p.Renglon);
                errorEncontrado = true;

            }
            identificadores.Add(p.Lexema.ToString());
            p = p.Siguiente;

            while (p.Token == 113)
            {
                p = p.Siguiente;
                if (p == null && p.Token != 100)
                {
                    Console.WriteLine("Error!: Se esperaba 'Identificador' en el renglon " + p.Renglon);
                    errorEncontrado = true;
                }

                identificadores.Add(p.Lexema.ToString());
                p = p.Siguiente;
            }
            if (p == null || p.Token != 117)
            {
                Console.WriteLine("Error!: Se esperaba ':' en el renglon " + p.Renglon);
                errorEncontrado = true;
            }
            p = p.Siguiente;

            if (p == null || (p.Token != 220 && p.Token != 221 && p.Token != 222))
            {
                Console.WriteLine("Error!: Se esperaba 'real, string o integer' en el renglon " + p.Renglon);
                errorEncontrado = true;
            }

            foreach (String id in identificadores)
            {
                bool existe = false;
                v = cabeza_Var;

                while (v != null)
                {
                    if (id == v.Lexema)
                    {
                        Console.WriteLine("Error! Variable ya declarada, error en el renglon " + p.Renglon);
                        errorEncontrado = true;
                        existe = true;
                        break;
                    }
                    if (v.Siguiente != null)
                    {
                        v = v.Siguiente;
                    }
                    else
                    {
                        break;
                    }
                }
                if (!existe)
                {
                    Variables prueba = new Variables(id, p.Lexema);
                    if (cabeza_Var == null)
                    {
                        cabeza_Var = prueba;
                        v = cabeza_Var;
                    }
                    else
                    {
                        v.Siguiente = prueba;
                        v = prueba;
                    }
                    Console.WriteLine(v.Lexema + " ------- >" + v.TipoVariable);
                }
            }




            p = p.Siguiente;
            if (p == null || p.Token != 114)
            {
                Console.WriteLine("Error!: Se esperaba ';' en el renglon " + p.Renglon);
                errorEncontrado = true;
            }
            p = p.Siguiente;

            return p;


        }

        public Nodo Statements(Nodo p)
        {


            if (p.Token == 100)
            {
                return Assignment_statement(p);

            }
            if (p.Token == 206)
            {

                return Statement_part(p);
            }
            if (p.Token == 208)
            {

                return Read_statement(p);
            }
            if (p.Token == 209)
            {

                return Write_statement(p);
            }
            if (p.Token == 200)
            {

                return If_statement(p);
            }
            if (p.Token == 204)
            {

                return While_statement(p);
            }

            return p;

        }

        public Nodo While_statement(Nodo p)
        {
            if (p == null || p.Token != 204)
            {
                Console.WriteLine("Error! se esperaba While en el renglon  " + p.Renglon);
            }
            p = p.Siguiente;


            int level = while_level;
            while_level += 1;
            listaPolish.Add("D" + level + ":");
            p = Expression(p);
            InfijoPostfijo(listaInfijo, p);
          
            ValidarTipo(p);

            listaInfijo.Clear();
            listaPolish.Add("BRF-C" + level);
            if (p == null || p.Token != 205)
            {
                Console.WriteLine("Error! se esperaba Do en el renglon  " + p.Renglon);
                errorEncontrado = true;
            }
            p = p.Siguiente;
            p = Statements(p);

            listaPolish.Add("BRI-D" + level);
            listaPolish.Add("C" + level+ ":");

            return p;
        }

        public Nodo If_statement(Nodo p)
        {
            if (p == null || p.Token != 200)
            {
                Console.WriteLine("Error!: Se esperaba 'if' en el renglon " + p.Renglon);
                errorEncontrado = true;
            }
            p = p.Siguiente;
            p = Expression(p);

            

            InfijoPostfijo(listaInfijo, p);
            int level = if_level;
            if_level += 1;
            listaPolish.Add("BRF-A" + level);
            ValidarTipo(p);
            listaInfijo.Clear();

           if (p == null || p.Token != 201)
           {
               Console.WriteLine("Error!: Se esperaba 'then' en el renglon " + p.Renglon);
               errorEncontrado = true;

           }
           p = p.Siguiente;
           p = Statements(p);

            
            listaPolish.Add("BRI-B" + level);
            listaPolish.Add("A" + level + ":");

            if (p != null && p.Token == 202)
           {
               p = p.Siguiente;
               p = Statements(p);
           }
            listaPolish.Add("B" + level + ":");

            return p;
            
        }

       public Nodo Statement_part(Nodo p)
       {


           if (p == null || p.Token != 206)
           {
               Console.WriteLine("Error! se esperaba Begin en el renglon " + p.Renglon);
               errorEncontrado = true;

           }
           p = p.Siguiente;
           p = Statements(p);
           while (p != null && p.Token == 114)
           {
               p = p.Siguiente;
               p = Statements(p);
           }

           if (p == null || p.Token != 207)
           {
               Console.WriteLine("Error!: Se esperaba 'end' en el renglon " + p.Renglon);
           }
            

            p = p.Siguiente;

           

            return p;
       }


       public Nodo Write_statement(Nodo p)
       {
           if (p == null || p.Token != 209)
           {

               Console.WriteLine("Error!: Se esperaba 'Write' en el renglon " + p.Renglon);
               errorEncontrado = true;

           }
            pWrite = p;
            listaInfijo.Add(pWrite);

            p = p.Siguiente;

           if (p == null || p.Token != 115)
           {
               Console.WriteLine("Error!: Se esperaba  '(' en el renglon " + p.Renglon);
               errorEncontrado = true;

           }
           p = p.Siguiente;
           p = Expression(p);

           InfijoPostfijo(listaInfijo, p);
           

           ValidarTipo(p);

           listaInfijo.Clear();
           while (p != null && p.Token == 113)
           {
                
                p = p.Siguiente;
                listaInfijo.Add(pWrite);   
                p = Expression(p);
                InfijoPostfijo(listaInfijo, p);

                ValidarTipo(p);

                listaInfijo.Clear();

            }

            if (p == null || p.Token != 116)
            {
                Console.WriteLine("Error!: Se esperaba  ')' en el renglon " + p.Renglon);
                errorEncontrado = true;

            }
            p = p.Siguiente;
            return p;
        }

        public Nodo Read_statement(Nodo p)
        {

            if (p == null || p.Token != 208)
            {
                Console.WriteLine("Error!: Se esperaba 'read' en el renglon " + p.Renglon);
                errorEncontrado = true;

            }
            pRead = p;
            listaInfijo.Add(pRead);
            p = p.Siguiente;
            if (p == null || p.Token != 115)
            {
                Console.WriteLine("Error!: Se esperaba '(' en el renglon " + p.Renglon);
                errorEncontrado = true;
            }
            p = p.Siguiente;
            
            if (p == null || p.Token != 100)
            {
                Console.WriteLine("Error!: Se esperaba 'identificador' en el renglon " + p.Renglon);
                errorEncontrado = true;

            }

            VerficarVariables(p);
            listaInfijo.Add(p);
            InfijoPostfijo(listaInfijo, p);
            p = p.Siguiente;
            listaInfijo.Clear();
            while (p != null && p.Token == 113)
            {
                p = p.Siguiente;
                listaInfijo.Add(pRead);
                if (p == null || p.Token != 100)
                {

                    Console.WriteLine("Error!: Se esperaba 'identificador' en el renglon " + p.Renglon);
                    errorEncontrado = true;

                }
                listaInfijo.Add(p);
                InfijoPostfijo(listaInfijo, p);
                listaInfijo.Clear();
                p = p.Siguiente;
            }

            if (p == null || p.Token != 116)
            {
                Console.WriteLine("Error!: Se esperaba ')' en el renglon " + p.Renglon);
                errorEncontrado = true;
            }
            p = p.Siguiente;
            return p;

        }

        public Nodo Assignment_statement(Nodo p)
        {


            if (p == null || p.Token != 100)
            {
                Console.WriteLine("Error!: Se esperaba Identificador  en el renglon  '" + p.Renglon);
                errorEncontrado = true;

            }
            VerficarVariables(p);

            listaInfijo.Add(p);

            p = p.Siguiente;
            if (p == null || p.Token != 130)
            {
                Console.WriteLine("Error!: Se esperaba ':=' en el renglon " + p.Renglon);
                errorEncontrado = true;

            }
            listaInfijo.Add(p);
            p = p.Siguiente;
            p = Expression(p);
            InfijoPostfijo(listaInfijo, p);
            
           

            
           
            
            ValidarTipo(p);

            listaInfijo.Clear();


            return p;
        }

        public Nodo Expression(Nodo p)
        {
            p = Simple_expression(p);
            if ((p != null) && (p.Token >= 106 && p.Token <= 111))
            {
                listaInfijo.Add(p);
                p = p.Siguiente;
                p = Simple_expression(p);
            }
            return p;
        }

        public Nodo Simple_expression(Nodo p)
        {
            if ((p != null) && (p.Token == 103 || p.Token == 104))
            {
                listaInfijo.Add(p);
                p.Lexema = "s" + p.Lexema;
                p = p.Siguiente;
            }
            p = Term(p);
            while (p != null && p.Token == 103 || p.Token == 104 || p.Token == 217)
            {
                listaInfijo.Add(p);
                p = p.Siguiente;
                p = Term(p);
            }
            return p;
        }

        public Nodo Term(Nodo p)
        {
            p = Factor(p);
            while (p != null && p.Token == 105 || p.Token == 219 || p.Token == 216)
            {

                listaInfijo.Add(p);
                p = p.Siguiente;
                p = Factor(p);
            }
            return p;
        }

        public Nodo Factor(Nodo p)
        {
            if ((p != null) && (p.Token == 100 || p.Token == 101 || p.Token == 102 || p.Token == 128))
            {

                if (p.Token == 100)
                {
                    VerficarVariables(p);
                }

                listaInfijo.Add(p);
                p = p.Siguiente;
                return p;
            }
            if (p != null && p.Token == 218)
            {
                listaInfijo.Add(p);
                p = p.Siguiente;
                p = Factor(p);
            }
            if (p != null && p.Token == 115)
            {
                listaInfijo.Add(p);
                p = p.Siguiente;
                p = Expression(p);
                if (p == null || p.Token != 116)
                {
                    Console.WriteLine("Error!: Se esperaba ')' en el renglon " + p.Renglon);
                    errorEncontrado = true;

                }
                listaInfijo.Add(p);
                p = p.Siguiente;
                return p;


            }
            return p;

        }
        public void InfijoPostfijo(IEnumerable<Nodo> listaInfijo, Nodo p)
        {


            foreach (Nodo nodo in listaInfijo)
            {
                CompararOperadores(nodo);
            }

            foreach (String nodo in listaAxuliar)
            {
               listaTipos.Add(nodo);
               listaPolish.Add(nodo);
            }
            listaAxuliar.Clear();    
         
        }

        public void CompararOperadores(Nodo p)
        
        {
            if (p.Token == 100 || p.Token == 101 || p.Token == 102 || p.Token == 128)
            {
                
                switch (p.Token)
                {


                    
                    case 100:
                        Variables v = cabeza_Var;
                        while (v != null)
                        {
                            if (v.Lexema == p.Lexema)
                            {
                                listaPolish.Add(v.Lexema);
                                listaTipos.Add(v.TipoVariable);
                                break;
                            }

                            v = v.Siguiente;

                        }
                        break;
                    case 101:
                        listaTipos.Add("integer");
                        listaPolish.Add(p.Lexema);
                        break;
                    case 102:
                        listaTipos.Add("real");
                        listaPolish.Add(p.Lexema);

                        break;
                    case 128:
                        listaTipos.Add("string");
                        listaPolish.Add(p.Lexema);

                        break;
                   
                }
              

            }
            else
            {
                if (listaAxuliar.Count > 0)
                {

                    var prioridadOp1 = prioridades[p.Lexema];
                    var prioridadOp2 = prioridades[listaAxuliar.Peek()];

                    if (p.Token == 116)
                    {
                        var variableNodo = listaAxuliar.Pop();
                        while (variableNodo != "(")
                        {
                            listaTipos.Add(variableNodo);
                            listaPolish.Add(variableNodo);

                            variableNodo = listaAxuliar.Pop();
                        }

                    }
                    else if (p.Token == 115 || prioridadOp1 > prioridadOp2  )
                    {
                        listaAxuliar.Push(p.Lexema);
                    }
                    else
                    {
                        listaPolish.Add(listaAxuliar.Peek());
                        listaTipos.Add(listaAxuliar.Pop());
                        
                        CompararOperadores(p);
                    }



                }
                else
                {
                    listaAxuliar.Push(p.Lexema);

                }
            }


        } 

      


        public void VerficarVariables(Nodo p )
        {
            Variables v = cabeza_Var;
            bool encontrado = false;
            while (v != null)
            {
                if (v.Lexema == p.Lexema)
                {
                    encontrado = true;
                    break;
                }

                v = v.Siguiente;
            }
            if (!encontrado)
            {
                Console.WriteLine("Error, variable " + p.Lexema + " nunca fue declarada, !ERROR! en el renglon " + p.Renglon);
            }



        }
        

        public string ValidarTipo(Nodo p) 
        {

            string result = "";
            foreach (String item in listaTipos)
            {

                
                if (item == "integer" || item == "real"|| item == "string")
                {
                    listaAuxiliarValidarTipos.Push(item);
                   
                }
                else
                {
                    String op1 = listaAuxiliarValidarTipos.Pop();
                    String op2 = listaAuxiliarValidarTipos.Pop();
                    switch (item)
                    {
                        case "+":
                            result = tablaSistemaTipos[0, 0, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "-":
                            result = tablaSistemaTipos[0, 1, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "*":
                            result = tablaSistemaTipos[0, 2, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "div":
                            result = tablaSistemaTipos[0, 3, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case ":=":
                            result = tablaSistemaTipos[0, 4, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case ">":
                            result = tablaSistemaTipos[0, 5, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "<":
                            result = tablaSistemaTipos[0, 6, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case ">=":
                            result = tablaSistemaTipos[0, 7, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "<=":
                            result = tablaSistemaTipos[0, 8, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "=":
                            result = tablaSistemaTipos[0, 9, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "<>":
                            result = tablaSistemaTipos[0, 10, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "and":
                            result = tablaSistemaTipos[0, 11, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "or":
                            result = tablaSistemaTipos[0, 12, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "not":
                            result = tablaSistemaTipos[0, 13, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "s+":
                            result = tablaSistemaTipos[0, 14, tokenVariables[op1], tokenVariables[op2]];
                            break;
                        case "s-":
                            result = tablaSistemaTipos[0, 15, tokenVariables[op1], tokenVariables[op2]];
                            break;
                    }
                    if (result == "error")
                    {
                        return "ERROR! El error encontrado es: Incompatibilidad de tipos, el operador "+ item + " es incompatible con los tipos " + op1 + " y " + op2 + ". Error en la linea " + p.Renglon;
                    }

                    listaAuxiliarValidarTipos.Push(result);
                   
                    
                                    }

                
            }
            return "";






        }
        
    }
}

