using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.Classes
{
   public class TreeDictionary: IComparable<TreeDictionary>
    {
        private int key { get; set; }
        string value { get; set; }

        //public int CompareTo(object obj)
        //{
        //    return key.CompareTo(obj);
        //}

        public int CompareTo(TreeDictionary other)
        {
            return key.CompareTo(other.key);
        }

        public int getKey()
        {
            return key;
        }

        public void setKey(int key)
        {
            this.key = key;
        }

        public string getValue()
        {
            return value;
        }

        public void setValue(string value)
        {
            this.value = value;
        }
    }
}
