using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.Classes
{
    public class CharDictionary
    {
        private int key { get; set; }
        char value { get; set; }       

        public int getKey()
        {
            return key;
        }

        public void setKey(int key)
        {
            this.key = key;
        }

        public char getValue()
        {
            return value;
        }

        public void setValue(char value)
        {
            this.value = value;
        }
    }
}
