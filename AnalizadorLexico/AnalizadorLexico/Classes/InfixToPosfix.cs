using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalizadorLexico.Classes;

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

    public List<CharDictionary> ConvertToPosfix(List<CharDictionary> expression)
    {
        List<CharDictionary> result = new List<CharDictionary>();
        Stack<CharDictionary> stack = new Stack<CharDictionary>();

        for (int i = 0; i < expression.Count; i++)
        {
            CharDictionary selectedCharacter = new CharDictionary();
            selectedCharacter.setKey(expression.ElementAt(i).getKey());
            selectedCharacter.setValue(expression.ElementAt(i).getValue());
            //char selectedCharacter = Convert.ToChar(expression.Substring(i, 1));

            /*
             *Considers three cases where a parentheses is found and adds all inside while is found
             * Next case pops all what is inside a parentheses 
             * default case considers what is inside the stack and compares the hierarchy to pop  or push element
             */
            switch (selectedCharacter.getValue())
            {
                case '(':
                    stack.Push(selectedCharacter);
                    break;

                case ')':
                    while (!stack.Peek().getValue().Equals('('))
                    {
                        result.Add(stack.Pop());
                    }
                    stack.Pop();
                    break;

                default:

                    while (stack.Count > 0)
                    {
                        CharDictionary nextSelected = stack.Peek();
                        int nextHierarchy = Hierarchy(nextSelected.getValue());
                        int actualHierarchy = Hierarchy(selectedCharacter.getValue());

                        if (nextHierarchy >= actualHierarchy)
                        {
                            result.Add(stack.Pop());
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
            result.Add(stack.Pop());
        }

        return result;
    }
}  

