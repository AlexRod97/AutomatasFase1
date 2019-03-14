using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.Classes
{
    public class Node<T>
    {
        public Node<T> Left {get; set;}
        public Node<T> Right {get; set;}
        public Node<T> Parent {get; set;}
        public List<int> First { get; set; }
        public List<int> Last { get; set; }
        public List<int> Follow { get; set; }
        public bool Nullable { get; set; }
        public int leafNodeValue { get; set; }
        public T Value {get; set;}


        public Node()
        {
            Value = default(T);
        }

        public T GetValue()
        {
            return Value;
        }


    }
}
