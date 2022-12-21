using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bd_interface
{
    internal class Row
    {
        public List<string> Values { get; set; }=new List<string>();
        public int id;
        public Row(int id)
        {
         this.id = id;
        }
        public string this[int index]
        {  
            get=>Values[index];
            set=>Values[index]=value;
        }

    }
}


