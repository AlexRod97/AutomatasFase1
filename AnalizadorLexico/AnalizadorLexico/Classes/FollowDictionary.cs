using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.Classes
{
    class FollowDictionary
    {
        int key { get; set; }
        List<int> values = new List<int>(); 

        public void setKey(int key)
        {
            this.key = key;
        }

        public int getKey()
        {
            return key;
        }

        public void addValue(int value)
        {
            if(!values.Contains(value))
            {
                values.Add(value);
            }
        }

        public List<int> getValues()
        {
            return values;
        }
    }
}
