using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The203.CollectionJson.Core.Model
{
    public class Data
    {
        public string name {get; set;}
        public string value { get; set; }
        public string prompt { get; set; }

        public Data() { }
        public Data(String name, string value) : this(name, value, null){

        }
        public Data(String name, string value, string prompt)
        {
            this.name = name;
            this.value = value;
            this.prompt = prompt;
        }
    }
}
