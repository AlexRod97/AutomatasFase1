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
        bool flag = false;

        public BinaryExpressionTree()
        {            
        }      
        
        public List<Node<T>> InOrder()
        {
            return InOrder(cabeza);
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
            //if (cabeza.Value.CompareTo(value) == 0)
            //{
            //    cabeza.Value = value;
            //    return cabeza;
            //}

            else if (node == null)
            {
                node = new Node<T>();
                node.Value = value;
                Char element = Convert.ToChar(node.Value.getValue());              
            }
            else
            {               
                if (node.Value.CompareTo(value) > 0)
                {                                    
                    node.Left = Insert(value, node.Left);
                    if(flag)
                    {
                        node.Left.Parent = node;                        
                    }
                    else
                    {
                        node.Left.Parent = node;
                    }
                    return node;
                }
                else
                if (node.Value.CompareTo(value) < 0)
                {                                    
                    node.Right = Insert(value, node.Right);
                    if(flag)
                    {
                        node.Right.Parent = node;                        
                    }
                    else
                    {
                        node.Right.Parent = node;
                    }
                    return node;
                }
                else
                {
                    throw new InvalidOperationException("Ya contiene una llave igual");
                }                
            }       
            return node;
        }
        
       

        public T Obtain(T llave)
        {
            throw new NotImplementedException();
        }

        public List<Node<T>> PostOrder()
        {
            return PostOrder(cabeza);
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
