using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.Classes
{
    public class BinaryExpressionTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private Node<T> head;

        public BinaryExpressionTree()
        {
            head = default(Node<T>);
        }

        public void Insert(T value)
        {
            Insert(value, head);
        }

        public void Insert(T value, Node<T> node)
        {
            if (head == null)
            {
                head = new Node<T>();
                head.Value = value;
            } 
            else
            {
                string word = value.ToString();

                for (int i = 0; i < word.Length; i++)
                {
                    string single = word.Substring(i, 1);

                    if (!single.Equals("(") || !single.Equals(")") || !single.Equals("|") || !single.Equals("*") || !single.Equals("+") || !single.Equals("?"))
                    {
                        
                    }
                }
            }
        }



        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
