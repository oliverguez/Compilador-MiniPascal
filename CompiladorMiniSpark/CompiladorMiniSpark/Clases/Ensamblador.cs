using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiladorMiniSpark.Clases;

namespace CompiladorMiniSpark.Clases
{
    public class Ensamblador
    {
        private Nodo cabeza;
        string variable_assembly = "INCLUDE macros.mac\nDOSSEG\n.MODEL SMALL\nSTACK 100H\n.DATA\n\t\t\tMAXLEN DB 254\n\t\t\tLEN DB 0\n\t\t\tMSG   DB 254 DUP(?)\n\t\t\tMSG_DD   DD MSG\n\t\t\tBUFFER		DB 8 DUP('$')\n\t\t\tCADENA_NUM		DB 10 DUP('$')\n\t\t\tBUFFERTEMP	DB 8 DUP('$')\n\t\t\tBLANCO	DB '#'\n\t\t\tBLANCOS	DB '$'\n\t\t\tMENOS	DB '-$'\n\t\t\tCOUNT	DW 0\n\t\t\tNEGATIVO	DB 0\n\t\t\tBUF	DW 10\n\t\t\tLISTAPAR	LABEL BYTE\n\t\t\tLONGMAX	DB 254\n\t\t\tTRUE	DW 1\n\t\t\tFALSE DW 0\n\t\t\tINTRODUCIDOS	DB 254 DUP ('$')\n\t\t\tMULT10	DW 1\n\t\t\ts_true	DB 'true$'\n\t\t\ts_false DB 'false$'\n";
        string program_assembly = ".CODE\n.386\nBEGIN:\n\t\t\tMOV     AX, @DATA\n\t\t\tMOV     DS, AX\nCALL COMPI\n\t\t\tMOV     AX, 4C00H\n\t\t\tINT 	21H\nCOMPI PROC\n";
        Variables cabeza_Var;
        int contadorVarTemp = 0;
        string result;
        string temp_var;
        String op1, op2;

        Stack<String> listaAuxiliar = new Stack<String>();


        public Ensamblador(Nodo cabeza, Variables cabezaVar)
        {

            this.cabeza = cabeza;
            this.cabeza_Var = cabezaVar;
            convertirEnsamblador(cabeza, cabezaVar);

        }


        public IDictionary<string, string> simbolos_macros = new Dictionary<string, string>
        {
            {"+", "SUMAR"},
            {"-", "RESTA"           },  
            {"*", "MULTI"           },
            {"div","DIVIDE"         },
            {"<",   "I_MENOR"       },
            {">",   "I_MAYOR"       },
            {"<=",  "I_MENORIGUAL"  },
            {">=",  "I_MAYORIGUAL"  },
            {"=",   "I_IGUAL"       },
            {"<>",  "I_DIFERENTES"  },
            {"s-","SIGNOMENOS"      },
            {"and", "I_AND"         },
            {"or",  "I_OR"          },
            {"not", "I_NOT"         },
            {"write", "WRITE"         },
            {"read", "READ"         },

        };

        //public IDictionary<string, int> tokenVariables = new Dictionary<string, int>
        //{
        //    {"integer",  0 },
        //    {"real",  1    },
        //    {"string",  2  },
        //    {"bool", 3}

        //};

     


        public void convertirEnsamblador(Nodo p, Variables v) {

           
            while (v != null)
            {
                variable_assembly += "\t\t\t" + v.Lexema + "  DW 0\n";
                if (v.Siguiente != null)
                {
                    v = v.Siguiente;
                }
                else
                {
                    break;
                }
            }

            foreach (string item in Sintactico.listaPolish)
            {


                switch (item)
                {
                    case "+":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "-":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString(); ///t0
                        contadorVarTemp += 1;  /// 1
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "*":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "div":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case ":=":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        program_assembly += "\tI_ASIGNAR " + op2 + ", " + op1 + "\n";

                        break;
                    case ">":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "<":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case ">=":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "<=":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "=":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "<>":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "and":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "or":
                        op1 = listaAuxiliar.Pop();
                        op2 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op2 + "," + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "not":
                        op1 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] + " " + op1 + "," + temp_var + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "s+":   
                        break;
                    case "s-":
                        op1 = listaAuxiliar.Pop();
                        temp_var = "t" + contadorVarTemp.ToString();
                        contadorVarTemp += 1;
                        variable_assembly += "\t\t\t" + temp_var + "  DW 0\n";
                        program_assembly += "\t" + simbolos_macros[item] +" "+ op1 + "," + temp_var  + "\n";
                        listaAuxiliar.Push(temp_var);
                        break;
                    case "read":
                        op1 = listaAuxiliar.Pop();
                        program_assembly += "READ\n" + "ASCTODEC " + op1 + "," + "MSG\n";
                        break;
                    case "write":
                        op1 = listaAuxiliar.Pop();
                        program_assembly += "\tITOA BUFFER, " + op1 + "\n";
                        program_assembly += "\tWRITE BUFFERTEMP\n";
                        break;
                    default:
                        if (item.Contains(":"))
                        {
                            program_assembly += item + "\n";
                        }
                        else if (item.Contains("BRF"))
                        { 
                            op1 = listaAuxiliar.Pop();
                            program_assembly += "\tJF " + op1 + "," + item.Split('-')[1] + "\n";
                        }
                        else if (item.Contains("BRI"))
                        {
                            program_assembly += "\tJMP "+ item.Split('-')[1]+"\n";
                        }
                        else
                        {
                            listaAuxiliar.Push(item);
                        }
                        break;
                }


            }

            program_assembly += "\t\tret\nCOMPI ENDP\nEND BEGIN";
            string complete_program = variable_assembly + program_assembly;

            System.IO.File.WriteAllText(@"C:\Users\olive\source\repos\CompiladorMiniSpark\CompiladorMiniSpark\compi.asm", complete_program);


        }

        

      
        
    }
}
