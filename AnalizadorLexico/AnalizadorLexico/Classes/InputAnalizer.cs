using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalizadorLexico.Classes
{
    public class InputAnalizer
    {
        public List<string> Analize(List<string> input)
        {
            string operators = "|*+?^";
            string comparators = "|^";
            int count = 0; 
            List<string> result = new List<string>();

            for (int i = 0; i < input.Count; i++)
            {
                if(input.ElementAt(i).Length > 0)
                {
                    string stringSelectedCharacter1 = input.ElementAt(i);
                    char selectedCharacter1 = Convert.ToChar(stringSelectedCharacter1.Substring(0, 1));

                    if (i + 1 < input.Count)
                    {
                        if (input.ElementAt(i + 1).Length > 0)
                        {
                            string stringSelectedCharacter2 = input.ElementAt(i + 1);
                            char selectedCharacter2 = Convert.ToChar(stringSelectedCharacter2.Substring(0, 1));
                            result.Add(stringSelectedCharacter1);
                            //Validates if the character is not an operator or parentheses; if true adds a concat operator
                            if (!selectedCharacter1.Equals('(') && !selectedCharacter2.Equals(')') && !operators.Contains(selectedCharacter2)
                                && !comparators.Contains(selectedCharacter1))
                            {
                                result.Add(".");
                            }
                        }
                    }
                }
            }
            result.Add((input.ElementAt(input.Count - 1)));           

            //Checks if the last element in the expression is a character or operator; if true adds terminal operator
            /*
            if (!operators.Contains(lastElement) && !comparators.Contains(lastElement))
            {
                result.Append(".#");
            }
            else
            {
                result.Append("#");
            }
            */
            return result;
        }

        public List<CharDictionary> convertToChar(List<string> input)
        {
            List<CharDictionary> result = new List<CharDictionary>();

            for (int i = 0; i < input.Count; i++)
            {
                CharDictionary element = new CharDictionary();
                element.setKey(i);
                element.setValue(Convert.ToChar(input.ElementAt(i).Substring(0,1)));
                result.Add(element);
            }

            return result;
        }

        public List<TreeDictionary> convertToTreeDictionary (List<CharDictionary> finalResult, List<string> baseExreg)
        {
            List<TreeDictionary> result = new List<TreeDictionary>();

            for (int i = 0; i < finalResult.Count; i++)
            {
                TreeDictionary element = new TreeDictionary();
                element.setKey(finalResult.ElementAt(i).getKey());
                element.setValue(baseExreg.ElementAt(finalResult.ElementAt(i).getKey()));
                result.Add(element);
            }
            TreeDictionary elementNum = new TreeDictionary();
            elementNum.setKey(int.MaxValue);
            elementNum.setValue("#");
            TreeDictionary elementDot = new TreeDictionary();
            elementDot.setKey(int.MaxValue-1);
            elementDot.setValue(".");

            result.Add(elementNum);
            result.Add(elementDot);
            return result;
        }
    }
}