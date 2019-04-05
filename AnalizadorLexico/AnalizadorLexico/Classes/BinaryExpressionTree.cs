using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        public List<List<int>> transiciones = new List<List<int>>(); 
        public List<FollowDictionary> Follow = new List<FollowDictionary>();
        private List<Node<T>> sets = new List<Node<T>>();
        public List<string> setsList = new List<string>();
        public List<string> nonRepeatedSets = new List<string>(); 
        public Dictionary<List<List<int>>, List<List<string>>> automatonTable = new Dictionary<List<List<int>>, List<List<string>>>();
        public List<List<string>> table = new List<List<string>>();
        private int LeafCount = 0;
      
        
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
            LeafCount = 1;

            for (int i = 0; i < elements.Count; i++)
            {
                string actual = elements.ElementAt(i).GetValue().getValue();
                if (isLeaf(elements.ElementAt(i)))
                {
                    elements.ElementAt(i).Nullable = false;
                    FollowDictionary follow = new FollowDictionary();
                    follow.setKey(LeafCount);
                    elements.ElementAt(i).leafNodeValue = LeafCount;
                    Follow.Add(follow);
                    Node<T> value = elements.ElementAt(i);
                    string set = value.Value.getValue();
                    // setsList.Add(set);
                   
                    if (!setsList.Contains(set))
                    {
                        sets.Add(value);
                        setsList.Add(set);                       
                    }
                    LeafCount++;
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
                    if (selected.Right.Nullable && selected.Right != null)
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

        public Node<T> FollowPos(Node<T> node)
        {
            Limpiar();
            List<Node<T>> elements = PostOrder(node);

            for (int i = 1; i < elements.Count; i++)
            {
                Node<T> selected = (elements.ElementAt(i));
                string value = selected.Value.getValue();

                if (value.Equals("."))
                {
                    List<int> Lc1 = selected.Left.Last;
                    List<int> Fc2 = selected.Right.First;

                    foreach  (int item in Lc1)
                    {
                        FollowDictionary newElement = new FollowDictionary();
                        newElement.setKey(item);
                        for (int j = 0; j < Fc2.Count; j++)
                        {
                            int singleValue = Fc2.ElementAt(j);
                            Follow.ElementAt(newElement.getKey() - 1).addValue(singleValue);
                        }
                    }
                }

                if(value.Equals("*") || value.Equals("+") || value.Equals("?"))
                {
                    List<int> Lc1, Fc1;

                    if (selected.Left != null)
                    {
                        Lc1 = selected.Left.Last;
                        Fc1 = selected.Left.First;

                        foreach (int item in Lc1)
                        {
                            FollowDictionary newElement = new FollowDictionary();
                            newElement.setKey(item);
                            for (int j = 0; j < Fc1.Count; j++)
                            {
                                Follow.ElementAt(newElement.getKey() - 1).addValue(Fc1.ElementAt(j));
                            }
                        }
                    }
                    else if(selected.Right != null)
                    {
                        Lc1 = selected.Right.Last;
                        Fc1 = selected.Right.First;

                        foreach (int item in Lc1)
                        {
                            FollowDictionary newElement = new FollowDictionary();
                            newElement.setKey(item);
                            for (int j = 0; j < Fc1.Count; j++)
                            {
                                Follow.ElementAt(newElement.getKey()).addValue(Fc1.ElementAt(j));
                            }
                        }
                    }
                }
            }
            return node;
        }

        public Node<T> makeAutomaton(Node<T> node)
        {           
            transiciones.Add(cabeza.First);

            for (int i = 0; i < transiciones.Count; i++)
            {
                List<int> actualTransition = transiciones.ElementAt(i);

                for (int j = 0; j < actualTransition.Count; j++)
                {
                    List<Node<T>> nodesLeading = new List<Node<T>>();
                    int actualTransitionElement = actualTransition.ElementAt(j);

                    for (int k = 0; k < sets.Count; k++)
                    {
                        if(sets.ElementAt(k).leafNodeValue.Equals(Follow.ElementAt(actualTransitionElement-1).getKey()))
                        {                          
                            List<int> nodeFollow = Follow.ElementAt(actualTransitionElement-1).getValues();                      

                            if (actualTransition.SequenceEqual(nodeFollow) && nodeFollow.Count> 0)
                            {
                                int name = getPosition(transiciones, nodeFollow);     
                            }
                            else
                            {
                                if (!isContained(transiciones, nodeFollow) && nodeFollow.Count > 0)
                                {
                                    transiciones.Add(nodeFollow);
                                }
                            }
                        }                       
                    }
                }
            }

            for (int i = 0; i < transiciones.Count; i++)
            {
                List<int> internalTransition = transiciones.ElementAt(i);
                List<string> result = new List<string>();
                int cont = 0; 

                for (int j = 0; j < sets.Count; j++)
                { 
                    if (internalTransition.Contains(j + 1))
                    {
                        int value = internalTransition.ElementAt(cont++);
                        List<int> resultFollow = Follow.ElementAt(value - 1).getValues();
                        int position = getPosition(transiciones, resultFollow);
                        result.Add(position.ToString());
                    }
                    else
                    {
                        result.Add("-");
                    }
                }
                table.Add(result);
                //automatonTable.Add(transiciones, table);
            }            
            return cabeza;
        }

        private int getPosition(List<List<int>> transition, List<int> follow)
        {
            int position = -1;
            for (int i = 0; i < transition.Count; i++)
            {
                if(transition.ElementAt(i).SequenceEqual(follow))
                {
                    position = i;
                }                
            }
            return position;
        }

        private bool isContained(List<List<int>> transition, List<int> follow)
        {
            bool result = false;

            for (int i = 0; i < transition.Count; i++)
            {
                if (transition.ElementAt(i).SequenceEqual(follow))
                {
                    result = true;
                    break;
                }
            }
            return result;
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

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
