using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.Classes
{
    class SetsValidations
    {
        public int line = 0;
        public string error = "";
        public bool Read(String[] file)
        {
            List<String> sets = new List<string>();
            List<String> valueSet = new List<string>();

            file = file.Where(c => c != null).ToArray();
            for (int z = 0; z < file.Count(); z++)
            {
                file[z] = file[z].Replace("\t", "");
            }

            error = file[file.Count() - 1].Replace("=", "");

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

            valueSet = sets.ToList();
            String[] sep = new string[2];
            for (int i = 0; i < sets.Count; i++)
            {
                int pos = sets[i].IndexOf("=");
                sep[0] = sets[i].Substring(0, pos);
                sep[1] = sets[i].Substring(pos + 1, (sets[i].Length - (pos + 1)));
                sets[i] = sep[0].Replace(" ", "");
            }

            for (int i = 0; i < valueSet.Count; i++)
            {
                String word = "";
                String[] split = new string[2];

                int pos = valueSet[i].IndexOf("=");
                split[0] = valueSet[i].Substring(0, pos);
                split[1] = valueSet[i].Substring(pos + 1, (valueSet[i].Length - (pos + 1)));

                for (int j = 0; j < split[1].Length; j++)
                {
                    String Word = split[1].Substring(j, 1);
                    word += Word;
                    String numbers = "";
                    if (split[1].ToUpper().Contains("CHR"))
                    {
                        j++;
                        split[1].ToUpper();
                        if (Word == "C")
                        {
                            if (split[1].Substring(j++, 1) == "H")
                            {
                                j--;
                                word += split[1].Substring(j, 1);
                                j++;
                                if (split[1].Substring(j++, 1) == "R")
                                {
                                    j--;
                                    word += split[1].Substring(j, 1);
                                    j++;
                                    if (split[1].Substring(j++, 1) == "(")
                                    {
                                        j--;
                                        word += split[1].Substring(j, 1);
                                        j++;
                                        while (split[1].Substring(j, 1) != ")")
                                        {
                                            numbers += split[1].Substring(j, 1);
                                            j++;
                                        }
                                        try
                                        {
                                            int pruba = int.Parse(numbers);
                                        }
                                        catch (Exception)
                                        {
                                            line = i + 2;
                                            return false;
                                        }
                                        word += numbers;
                                        word += split[1].Substring(j, 1);
                                        if (j + 2 < split[1].Length)
                                        {
                                            if ((split[1].Substring(j + 1, 1) == ".") && (split[1].Substring(j + 2, 1) == "."))
                                            {
                                                j++;
                                                word += split[1].Substring(j++, 1);
                                                word += split[1].Substring(j++, 1);
                                                j--;
                                            }
                                            else
                                            {
                                                j++;
                                                if (split[1].Substring(j++, 1) == "+")
                                                {
                                                    j--;
                                                    word += split[1].Substring(j++, 1);
                                                    j--;
                                                }
                                                else
                                                {
                                                    line = i + 2;
                                                    return false;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        line = i + 2;
                                        return false;
                                    }
                                }
                                else
                                {
                                    line = i + 2;
                                    return false;
                                }
                            }
                            else
                            {
                                line = i + 2;
                                return false;
                            }
                        }
                        else
                        {
                            line = i + 2;
                            return false;
                        }
                    }
                    else
                    {
                        if (Word == "'")
                        {
                            j += 1;
                            Word = split[1].Substring(j, 1);
                            while (Word != "'")
                            {
                                word += Word;
                                j += 1;
                                Word = split[1].Substring(j, 1);
                            }
                            word += Word;
                        }
                        else
                        {
                            if (Word == ".")
                            {
                                if (split[1].Substring(j++, 1) == ".")
                                {
                                    j--;
                                    word += split[1].Substring(j++, 1);
                                    j += 1;
                                    if (split[1].Substring(j++, 1) != "'")
                                    {
                                        line = i + 2;
                                        return false;
                                    }
                                    else
                                    {
                                        j -= 2;
                                    }
                                }
                                else
                                {
                                    line = i + 2;
                                    return false;
                                }
                            }
                            else
                            {
                                if (Word == "+")
                                {
                                    j++;
                                    if (split[1].Substring(j++, 1) != "'")
                                    {
                                        line = i + 2;
                                        return false;
                                    }
                                    j--;
                                    word += split[1].Substring(j, 1);
                                    j--;
                                }
                                else
                                {
                                    line = i + 2;
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}

