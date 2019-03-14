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
        public static int Hierarchy(char character)
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

                case '#':
                    return 6; 
            }
            return 7;
        }

        public Dictionary<int, string> ConvertToPosfix(Dictionary<int, string> expression)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            Stack<Dictionary<int, char>> stack = new Stack<Dictionary<int, char>>();

            for (int i = 0; i < expression.Count; i++)
            {
                char selectedCharacter = Convert.ToChar(expression.ElementAt(i).Value);
                int charCount = expression.ElementAt(i).Key;
                Dictionary<int, char> newElement, temp;
                char comparator = ' '; 

                /*
                 *Considers three cases where a parentheses is found and adds all inside while is found
                 * Next case pops all what is inside a parentheses 
                 * default case considers what is inside the stack and compares the hierarchy to pop  or push element
                 */
                switch (selectedCharacter)
                {
                    case'(':
                        newElement = new Dictionary<int, char>();
                        newElement.Add(charCount, selectedCharacter); 
                        stack.Push(newElement);
                    break;

                    case ')': 
                        while(!comparator.Equals('('))
                        {
                            newElement = stack.Pop();
                            result.Add(newElement.Keys.ElementAt(0), newElement.Values.ElementAt(0).ToString());
                            temp = stack.Peek();
                            comparator = temp.ElementAt(0).Value;
                        }
                        stack.Pop();
                    break;

                    default:

                        while(stack.Count > 0)
                        {
                            newElement = stack.Peek();
                            char nextSelected = newElement.Values.ElementAt(0);
                            int nextHierarchy = Hierarchy(nextSelected);
                            int actualHierarchy = Hierarchy(selectedCharacter); 

                            if(nextHierarchy >= actualHierarchy)
                            {
                                newElement = stack.Pop();
                                result.Add(newElement.Keys.ElementAt(0), newElement.Values.ElementAt(0).ToString());
                            }
                            else
                            {
                                break;
                            }
                        }
                        newElement = new Dictionary<int, char>();
                        newElement.Add(charCount, selectedCharacter);
                        stack.Push(newElement);
                        break;
                }
            }

            ////////////////////
            while (stack.Count > 0)
            {
                Dictionary<int, char> newElement;
                newElement = stack.Pop();
                result.Add(newElement.Keys.ElementAt(0), newElement.Values.ElementAt(0).ToString());
            }

            return result; 
        }
    }  
}
