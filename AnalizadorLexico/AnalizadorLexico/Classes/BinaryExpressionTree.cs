using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalizadorLexico.Classes
{
    public class BinaryExpressionTree<T> where T : TreeDictionary
    {
        public Node<T> cabeza;
        private List<Node<T>> inOrden = new List<Node<T>>();
        private List<Node<T>> preOrden = new List<Node<T>>();
        private List<Node<T>> postOrden = new List<Node<T>>();

        public BinaryExpressionTree()
        {            
        }
        
        public void Insert(T value)
        {         
            Insert(value, cabeza);
        }

        public Node<T> Insert(T value, Node<T> node)
        {
            if (cabeza == null)
            {
                cabeza = new Node<T>();
                cabeza.Value = value;              
                return cabeza;
            }
            else if (node == null)
            {
                node = new Node<T>();
                node.Value = value;                         
            }
            else
            {               
                if (node.Value.CompareTo(value) > 0)
                {                                    
                    node.Left = Insert(value, node.Left);
                    node.Left.Parent = node;                   
                    return node;
                }
                else
                if (node.Value.CompareTo(value) < 0)
                {                                    
                    node.Right = Insert(value, node.Right);
                    node.Right.Parent = node;                   
                    return node;
                }
                else
                {
                    throw new InvalidOperationException("Ya contiene una llave igual");
                }                
            }       
            return node;
        }

        private bool isLeaf(Node<T> node)
        {
            if (node.Left == null && node.Right == null)
            {
                return true;
            }
            return false;
        }

        public T Obtain(T llave)
        {
            throw new NotImplementedException();
        }

        public List<Node<T>> PreOrder()
        {
            return PreOrder(cabeza);
        }             

        public List<Node<T>> PreOrder(Node<T> node)
        {
            if (node != null)
            {
                preOrden.Add(node);
                PreOrder(node.Left);
                PreOrder(node.Right);
            }
            return preOrden;
        }

        public List<Node<T>> InOrder()
        {
            return InOrder(cabeza);
        }

        public List<Node<T>> InOrder(Node<T> node)
        {
            if (node != null)
            {
                InOrder(node.Left);
                inOrden.Add(node);
                InOrder(node.Right);
            }
            return inOrden;
        }

        public List<Node<T>> PostOrder()
        {
            return PostOrder(cabeza);
        }

        public List<Node<T>> PostOrder(Node<T> node)
        {
            if (node != null)
            {
                PostOrder(node.Left);
                PostOrder(node.Right);
                postOrden.Add(node);
            }
            return postOrden;
        }

        public Node<T> Nullable(Node<T> node)
        {
            Limpiar();
            List<Node<T>> elements = PostOrder(node);
            int cont = 1;

            for (int i = 0; i < elements.Count; i++)
            {
                if(isLeaf(elements.ElementAt(i)))
                {
                    elements.ElementAt(i).Nullable = false;
                    elements.ElementAt(i).leafNodeValue = cont++;
                }
            }

            for (int i = 0; i < elements.Count; i++)
            {
                Node<T> selected = elements.ElementAt(i);
                string value = selected.Value.getValue();

                if (value.Equals("|"))
                {
                    bool result = selected.Left.Nullable || selected.Right.Nullable;
                    selected.Nullable = result; 
                }

                if (value.Equals("."))
                {
                    bool result = selected.Left.Nullable && selected.Right.Nullable;
                    selected.Nullable = result;
                }

                if (value.Equals("*"))
                {
                    selected.Nullable = true;
                }

                if(value.Equals("+"))
                {
                    if (selected.Left != null)
                    {
                        selected.Nullable = selected.Left.Nullable;
                    }
                    else if (selected.Right != null)
                    {
                        selected.Nullable = selected.Right.Nullable;
                    }
                }

                if (value.Equals("?"))
                {
                    selected.Nullable = true;
                }
            }
            return node;
        }

        public Node<T> FirstPos(Node<T> node)
        {
            Limpiar();
            List<Node<T>> elements = PostOrder(node);

            for (int i = 0; i < elements.Count; i++)
            {
                Node<T> selected = (elements.ElementAt(i)); 

                if (isLeaf(selected))
                {
                    selected.First = new List<int>();
                    selected.First.Add(selected.leafNodeValue);
                }
            }

            for (int i = 0; i < elements.Count; i++)
            {
                Node<T> selected = elements.ElementAt(i);
                string value = selected.Value.getValue();

                if(selected.First == null)
                {
                    selected.First = new List<int>();
                }

                if (value.Equals("|"))
                {
                    for (int j = 0; j < selected.Left.First.Count; j++)
                    {
                        selected.First.Add(selected.Left.First.ElementAt(j));
                    }

                    for (int j = 0; j < selected.Right.First.Count; j++)
                    {
                        int number = selected.Right.First.ElementAt(j);
                        if (!selected.First.Contains(number))
                        {
                            selected.First.Add(number);
                        }
                    }
                }

                if (value.Equals("."))
                {
                    if(selected.Left.Nullable)
                    {
                        for (int j = 0; j < selected.Left.First.Count; j++)
                        {
                            selected.First.Add(selected.Left.First.ElementAt(j));
                        }

                        for (int j = 0; j < selected.Right.First.Count; j++)
                        {
                            int number = selected.Right.First.ElementAt(j);
                            if (!selected.First.Contains(number))
                            {
                                selected.First.Add(number);
                            }
                        }
                    }
                    else
                    {
                        selected.First = selected.Left.First;
                    }
                }

                if (value.Equals("*"))
                {
                    if (selected.Left != null)
                    {
                        selected.First = selected.Left.First;
                    }
                    else if (selected.Right != null)
                    {
                        selected.First = selected.Right.First;
                    }
                }

                if (value.Equals("+"))
                {
                    if (selected.Left != null)
                    {
                        selected.First = selected.Left.First;
                    }
                    else if (selected.Right != null)
                    {
                        selected.First = selected.Right.First;
                    }
                }

                if (value.Equals("?"))
                {
                    if (selected.Left != null)
                    {
                        selected.First = selected.Left.First;
                    }
                    else if (selected.Right != null)
                    {
                        selected.First = selected.Right.First;
                    }

                }
            }
            return node;
        }

        public Node<T> LastPos(Node<T> node)
        {
            Limpiar();
            List<Node<T>> elements = PostOrder(node);

            for (int i = 0; i < elements.Count; i++)
            {
                Node<T> selected = (elements.ElementAt(i));

                if (isLeaf(selected))
                {
                    selected.Last = new List<int>();
                    selected.Last.Add(selected.leafNodeValue);
                }
            }

            for (int i = 0; i < elements.Count; i++)
            {
                Node<T> selected = elements.ElementAt(i);
                string value = selected.Value.getValue();

                if (selected.Last == null)
                {
                    selected.Last = new List<int>();
                }

                if (value.Equals("|"))
                {
                    for (int j = 0; j < selected.Left.Last.Count; j++)
                    {
                        selected.Last.Add(selected.Left.Last.ElementAt(j));
                    }

                    for (int j = 0; j < selected.Right.Last.Count; j++)
                    {
                        int number = selected.Right.Last.ElementAt(j);
                        if (!selected.Last.Contains(number))
                        {
                            selected.Last.Add(number);
                        }
                    }
                }

                if (value.Equals("."))
                {
                    if (selected.Right.Nullable)
                    {
                        for (int j = 0; j < selected.Left.Last.Count; j++)
                        {
                            selected.Last.Add(selected.Left.Last.ElementAt(j));
                        }

                        for (int j = 0; j < selected.Right.Last.Count; j++)
                        {
                            int number = selected.Right.Last.ElementAt(j);
                            if (!selected.Last.Contains(number))
                            {
                                selected.Last.Add(number);
                            }
                        }
                    }
                    else
                    {
                        selected.Last = selected.Right.Last;
                    }
                }

                if (value.Equals("*"))
                {
                    if (selected.Left != null)
                    {
                        selected.Last = selected.Left.Last;
                    }
                    else if (selected.Right != null)
                    {
                        selected.Last = selected.Right.Last;
                    }
                }

                if (value.Equals("+"))
                {
                    if (selected.Left != null)
                    {
                        selected.Last = selected.Left.Last;
                    }
                    else if (selected.Right != null)
                    {
                        selected.Last = selected.Right.Last;
                    }
                }

                if (value.Equals("?"))
                {
                    if (selected.Left != null)
                    {
                        selected.Last = selected.Left.Last;
                    }
                    else if (selected.Right != null)
                    {
                        selected.Last = selected.Right.Last;
                    }

                }
            }
            return node;
        }

        public bool Limpiar()
        {
            try
            {
                inOrden.Clear();
                preOrden.Clear();
                postOrden.Clear();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Node<T> Search(Node<T> nodo, T value)
        {
            if (nodo == null)
                return null;
            else if (nodo.Value.CompareTo(value) == 0)
                return nodo;

            else if (nodo.Value.CompareTo(value) > 0)
                Search(nodo.Left, value);
            else if (nodo.Value.CompareTo(value) < 0)
                Search(nodo.Right, value);
            return nodo;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
