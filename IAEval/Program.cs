using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace IAEval
{
    class Program
    {
        #region Zavatra Tsy Ilaina
        public static string GetVariable(int i, int nvariable)
        {
            string a = Convert.ToString(i, 2);
            a = new string('0', nvariable - a.Length) + a;
            return a;
        }
        public static void Main1()
        {
            DataTable dt = new DataTable();

            Console.Write("Entrer le nombre de variable: ");
            int nvariable = Convert.ToInt32(Console.ReadLine());
            List<string> variables = new List<string>();
            for (int i = 0; i < nvariable; i++)
            {
                variables.Add(Console.ReadLine());
            };

            Console.Write("Entrer le nombre d'hypothese: ");
            int nHypothese = Convert.ToInt32(Console.ReadLine());
            List<string> hypothese = new List<string>();
            for (int i = 0; i < nHypothese; i++)
            {
                hypothese.Add("(" + Console.ReadLine() + ")");
            }

            List<string> tables = new List<string>();
            for (int k = 0; k < (int)Math.Pow(2, nvariable); k++)
            {
                string value = GetVariable(k, nvariable);
                tables.Add(value);
                hypothese.ForEach(h =>
                {
                    string expression = null;
                    for (int i = 0; i < nvariable; i++)
                    {
                        expression = h.Replace(variables[i], (value[i] == '1').ToString());
                    }
                });
            }

            Console.ReadLine();
        }

        public static string RemoveWhiteSpace(string a)
        {
            string b="";
            foreach(var c in a)
            {
                if (c.Equals(' ')) continue;
                b += c;
            };
            return b;
        }
        public static List<string> enleveParenthese(string s)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[()]");
            var tmp = regex.Split(s);
            return tmp.ToList();
        }

        public static string ReplaceImplication(string a)
        {
            string ret = a;
            var index = a.IndexOf("->");
            string variable = ret[index-1].ToString();
            if (variable.Equals(")"))
            {
                var tmp = ret.Substring(0, index);
                var lohany = tmp.LastIndexOf("(");
                variable = ret.Substring(lohany-1, index);
            }
            ret = ret.Replace(variable+"->", $"not {variable} or ");
            return ret;
        }
        #endregion

        public static void Main(string[] args)
        {

            DataTable dt = new DataTable();
            int nvariable,nHypothese;
            string conclusion;
            try
            {
                #region Aide
                Console.WriteLine("=====================");
                Console.WriteLine("Ou (p ou q): p or q");
                Console.WriteLine("Et (p et q): p and q");
                Console.WriteLine("Negation (non p ): not p");
                Console.WriteLine("Implique (p->q): not p or q");
                Console.WriteLine("Exemple l'hypothese ( ((p-> non q) et q ) -> q) sera écrit:\n ( not ( (not p or not q) and q) or q)");
                Console.WriteLine("=====================\n");
                #endregion
            
                #region MakaVariable
                Console.Write("Entrer le nombre de variable (chiffre): ");
                nvariable = Convert.ToInt32(Console.ReadLine());

                List<string> variables = new List<string>();
                for (int i = 0; i < nvariable; i++)
                {
                    Console.Write("V"+i+": ");
                    variables.Add(Console.ReadLine());
                };
                #endregion

                #region MakaHypothese
                Console.Write("Entrer le nombre d'hypothese (chiffre): ");
                nHypothese = Convert.ToInt32(Console.ReadLine());
                List<string> hypothese = new List<string>();

                string hypotheseAll = "";

                for (int i = 0; i < nHypothese; i++)
                {
                    Console.Write("H" + i + ": ");
                    var tmp = Console.ReadLine();
                    if (i>0)
                    {
                        hypotheseAll = hypotheseAll + " and " + "(" + tmp +")";
                    }
                    else
                    {
                        hypotheseAll = "(" + tmp + ")";
                    }
                    hypothese.Add("("+tmp+")");
                }
                #endregion

                #region Conclusion
                Console.Write("---------------\n.:  ");
                conclusion = Console.ReadLine();
                #endregion

                #region MtesteExpression de manova variable tsirairay
                var estVraie = true;
                for (int k = 0; k < (int)Math.Pow(2, nvariable); k++)
                {
                    string value = GetVariable(k, nvariable);
                    string expression = hypotheseAll;
                    string tmpConclusion = conclusion;
                    for (int i = 0; i < nvariable; i++)
                    {
                        expression = expression.Replace(variables[i], (value[i] == '1').ToString());
                        tmpConclusion = tmpConclusion.Replace(variables[i], (value[i] == '1').ToString());
                    }
                    bool compile = (bool)dt.Compute(tmpConclusion,"");
                    bool boolExpression = (bool)dt.Compute(expression, "");
                    if (boolExpression)
                    {
                        if (!compile)
                        {
                            Console.WriteLine("L'argumentation est fausse pour:");
                            for(int i = 0; i < nvariable; i++)
                            {
                                Console.WriteLine("     -"+variables[i]+":  "+ (value[i] == '1').ToString());
                            }
                            estVraie = false;
                            break;
                        }
                    }

                }
                if (estVraie)
                {
                    Console.WriteLine("== L'argumentation est vraie!!!");
                }
                #endregion
            }
            catch(Exception e)
            {

                Console.WriteLine("\n======[Erreur]======");
                Console.WriteLine("[Message] "+e.Message);
                Console.WriteLine("[Source] " + e.Source);
                Console.WriteLine("[Callstack] " + e.StackTrace);
                Console.WriteLine("=====================\n");
            };
            Console.ReadKey();
        }
    }
}
