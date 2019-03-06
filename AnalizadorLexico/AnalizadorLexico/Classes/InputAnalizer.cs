using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.Classes
{
    public class InputAnalizer
    {  
        public string Analize(string input)
        {
            string operators = "|*+?^";
            string comparators = "|^";
            StringBuilder result = new StringBuilder(); 

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
            if (!operators.Contains(lastElement) && !comparators.Contains(lastElement))
            {
                result.Append("#");
            }
            else
            {
                result.Append(".#");
            }
            return result.ToString(); 
        }
    }
}
