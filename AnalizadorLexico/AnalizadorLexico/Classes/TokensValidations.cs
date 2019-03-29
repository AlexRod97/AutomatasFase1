using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalizadorLexico.Classes
{
    class TokensValidations
    {
        public List<String> result = new List<string>();
        List<String> sets = new List<string>();
        List<String> exreg = new List<string>();
        public int line = 0;
        public Boolean Read(String[] archivo)
        {
            List<String> tokens = new List<string>();
            int cont = 0;
            while (archivo[cont].ToUpper() != "TOKENS")
            {
                cont++;
            }
            cont++;
            while (archivo[cont].ToUpper().Contains("TOKEN"))
            {
                tokens.Add(archivo[cont].Replace("\t", ""));
                cont++;
            }            

            for (int i = 0; i < tokens.Count; i++)
            {
                List<String> split = new List<string>();
                int pos = tokens[i].IndexOf("=");
                split.Add(tokens[i].Substring(0, pos));
                split.Add(tokens[i].Substring(pos + 1, (tokens[i].Length - (pos + 1))));
                split[1].Replace(" ", "");

                if (i != 0)
                {
                    exreg.Add("|");
                }

                for (int j = 0; j < split[1].Length; j++)
                {
                    String word = "";
                    String letter = split[1].Substring(j, 1);
                    word += letter;

                    if (letter == "'")
                    {
                        j += 1;
                        letter = split[1].Substring(j, 1);
                        while (letter != "'")
                        {
                            word += letter;
                            j += 1;
                            letter = split[1].Substring(j, 1);
                        }
                        word += letter;
                        exreg.Add(word);
                    }
                    else
                    {
                        if (Symbol(letter))
                        {
                            if (j + 1 < split[1].Length)
                            {

                                if (!Symbol(split[1].Substring(j + 1, 1)))
                                {
                                    if (letter == "+" || letter == "?" || letter == "*")
                                    {
                                        exreg.Add(letter);
                                    }
                                    else
                                    {
                                        if (exreg.Count != 0)
                                        {
                                            if (j + 1 < split[1].Length)
                                            {
                                                if (Symbol(split[1].Substring(j + 1, 1)))
                                                {
                                                    line = i + 2;
                                                    return false;
                                                }
                                                else
                                                {
                                                    exreg.Add(letter);

                                                }
                                            }
                                        }
                                        else
                                        {
                                            line = i + 2;
                                            return false;
                                        }
                                    }
                                }
                                else
                                {
                                    line = i;
                                    return false;
                                }
                            }
                            else
                            {
                                exreg.Add(letter);
                            }
                        }
                        else
                        {
                            if (letter != " ")
                            {
                                if (letter == "|" || letter == "(" || letter == ")")
                                {
                                    exreg.Add(letter);
                                }
                                else
                                {
                                    j++;
                                    while (!Symbol(letter) || !Symbol(split[1].Substring(j, 1)) 
                                        || split[1].Substring(j, 1) != "(" || split[1].Substring(j, 1) != "|")
                                    {
                                        letter = split[1].Substring(j++, 1);
                                        word += letter;
                                        if (sets.Contains(word))
                                        {
                                            exreg.Add(word);
                                            j--;
                                            word = "";
                                            break;
                                        }
                                        else
                                        {
                                            if (split[1].Substring(j, 1) == "(" || split[1].Substring(j, 1) == "|")
                                            {
                                                line = i + 2;
                                                return false;
                                            }
                                        }
                                        if (Symbol(letter))
                                        {
                                            exreg.Add(letter);

                                            j -= 2;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Add(exreg);
            return true;
        }

        public void Add(List<String> expression)
        {

            for (int i = 0; i < expression.Count; i++)
            {
                expression[i] = expression[i].Replace("'", "");
                result.Add(expression[i]);
                /*if (i + 1 < expression.Count)
                {
                    
                    if (expression[i + 1] != "(" && expression[i + 1] != ")" && expression[i + 1] != "|" && expression[i + 1] != "*" && expression[i+1] != "+" && expression[i+1] != "?"
                        && expression[i] != "(" && expression[i] != ")" && expression[i] != "|")
                    {
                        result.Add(".");
                    }
                }*/
            }
        }


        public Boolean Symbol(String letra)
        {
            if (letra != "*")
            {
                if (letra != "?")
                {
                    if (letra != "+")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public Boolean Exists(String input)
        {
            for (int i = 0; i < sets.Count; i++)
            {
                if (sets[i] == input)
                {
                    return true;
                }
            }
            return false;
        }


        public void Get(String[] file)
        {

            for (int i = 0; i < file.Count(); i++)
            {
                file[i] = file[i].ToUpper().Replace("\t", "");
            }

            int cont = 0;
            while (file[cont].ToUpper().Replace(" ", "") != "TOKENS")
            {
                if (file[cont].ToUpper().Replace(" ", "") != "SETS")
                {
                    file[cont] = file[cont].Replace("\t", "");
                    if (file[cont].Replace(" ", "") != "")
                    {
                        sets.Add(file[cont].Replace(" ", ""));
                    }
                }
                cont++;
            }

            for (int i = 0; i < sets.Count; i++)
            {
                int posicion = sets[i].IndexOf("=");
                sets[i] = sets[i].Substring(0, posicion).Replace(" ", "");
            }
        }
    }
}
