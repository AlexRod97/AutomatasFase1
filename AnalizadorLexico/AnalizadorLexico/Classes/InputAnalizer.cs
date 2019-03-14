using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.Classes
{
    public class InputAnalizer
    {
        public Dictionary<int, string> Analize(string input)
        {
            string operators = "|*+?^";
            string comparators = "|^#";
            StringBuilder result = new StringBuilder();
            List<TreeDictionary> regularExpressionMap = new List<TreeDictionary>();
            Dictionary<int, string> regularExpressionResult = new Dictionary<int, string>();

            int charCount = 0;

            input += "#";

            for (int i = 0; i < input.Length; i++)
            {
                char selectedCharacter1 =  Convert.ToChar(input.Substring(i,1)); 

                if( i + 1 < input.Length)
                {
                    char selectedCharacter2 = Convert.ToChar(input.Substring(i + 1, 1));

                    result.Append(selectedCharacter1);

                    //Validates if the character is not an operator or parentheses; if true adds a concat operator
                    if(!selectedCharacter1.Equals('(') && !selectedCharacter2.Equals(')') && !operators.Contains(selectedCharacter2) 
                        && !comparators.Contains(selectedCharacter1))
                    {
                        result.Append(".");
                    }                   
                }               
            }
            char lastElement = Convert.ToChar(input.Substring(input.Length - 1, 1));
            result.Append(lastElement);

            //Checks if the last element in the expression is a character or operator; if true adds terminal operator
            //if (!operators.Contains(lastElement) && !comparators.Contains(lastElement))
            //{
            //    result.Append(".#");
            //}
            //else
            //{
            //    result.Append(".#.");
            //}
            string expression = result.ToString(); 

            for (int i = 0; i < expression.Length; i++)
            {
                TreeDictionary tree = new TreeDictionary();
                tree.setKey(charCount++);
                tree.setValue(expression.Substring(i, 1));
                regularExpressionMap.Add(tree);
            }

            for (int i = 0; i < expression.Length; i++)
            {
                string valueActual = regularExpressionMap.ElementAt(i).getValue();
                int keyActual = regularExpressionMap.ElementAt(i).getKey();
                
                if(i -1 > 0)
                {
                    string valuePrev = regularExpressionMap.ElementAt(i - 1).getValue();
                    int keyPrev = regularExpressionMap.ElementAt(i - 1).getKey();
                    bool letter = Char.IsLetterOrDigit(Convert.ToChar(valuePrev));

                    if ((valueActual.Equals("*") && letter) || (valueActual.Equals("+") && letter) || (valueActual.Equals("?") && letter) )
                    {
                        regularExpressionMap.ElementAt(i).setKey(keyPrev);
                        regularExpressionMap.ElementAt(i - 1).setKey(keyActual);
                    }

                    if(i + 2 == regularExpressionMap.Count)
                    {
                        regularExpressionMap.ElementAt(i).setKey(int.MaxValue - 1);
                    }

                    if(valueActual.Equals("#"))
                    {
                        regularExpressionMap.ElementAt(i).setKey(int.MaxValue);
                    }
                }
            }

            for (int i = 0; i < regularExpressionMap.Count; i++)
            {
                regularExpressionResult.Add(regularExpressionMap.ElementAt(i).getKey(), regularExpressionMap.ElementAt(i).getValue());
            }

            return regularExpressionResult;
        }
    }
}
