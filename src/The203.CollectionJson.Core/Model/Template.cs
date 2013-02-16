using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The203.CollectionJson.Core.Model
{
    public class Template
    {   
        public Template()
        {

        }
        public Template(IList<Data> data)
        {
            this.data = data;
        }

        public IList<Data> data { get; set; }
    }
}
