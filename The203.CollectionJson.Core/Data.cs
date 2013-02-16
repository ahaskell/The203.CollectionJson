using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pearson.StudyGuide.CollectionJson
{
    public class Data
    {
        public string Name {get; set;}
        public string Value { get; set; }
        public string Prompt { get; set; }


        public Data(String name, string value) : this(name, value, null){

        }
        public Data(String name, string value, string prompt)
        {
            this.Name = name;
            this.Value = value;
            this.Prompt = prompt;
        }
    }
}
