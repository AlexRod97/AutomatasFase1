using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.Classes
{
    public class InfixToPosfix
    {
        //Returns the value from operator valuated, higher number means higher hierarchy
        public int Hierarchy(char character)
        {
            switch (character)
            {
                case '(':               
                    return 1;
               
                case '|':
                    return 2;

                case '.':
                    return 3;

                case '*':
                case '+':
                case '?':
                    return 4;

                case '^':
                    return 5; 
            }
            return 6;
        }

        public string ConvertToPosfix(string expression)
        {
            StringBuilder result = new StringBuilder();
            Stack<char> stack = new Stack<char>();

            for (int i = 0; i < expression.Length; i++)
            {
                char selectedCharacter = Convert.ToChar(expression.Substring(i, 1));

                /*
                 *Considers three cases where a parentheses is found and adds all inside while is found
                 * Next case pops all what is inside a parentheses 
                 * default case considers what is inside the stack and compares the hierarchy to pop  or push element
                 */
                switch (selectedCharacter)
                {
                    case'(':
                        stack.Push(selectedCharacter);
                    break;

                    case ')': 
                        while(!stack.Peek().Equals('('))
                        {
                            result.Append(stack.Pop());
                        }
                        stack.Pop();
                    break;

                    default:

                        while(stack.Count > 0)
                        {
                            char nextSelected = stack.Peek();
                            int nextHierarchy = Hierarchy(nextSelected);
                            int actualHierarchy = Hierarchy(selectedCharacter); 

                            if(nextHierarchy >= actualHierarchy)
                            {
                                result.Append(stack.Pop()); 
                            }
                            else
                            {
                                break;
                            }
                        }
                        stack.Push(selectedCharacter);
                    break;
                }
            }

            while (stack.Count > 0)
            {
                result.Append(stack.Pop());
            }

            return result.ToString(); 
        }
    }  
}
