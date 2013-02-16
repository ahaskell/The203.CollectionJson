using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The203.CollectionJson.Core.Model
{
    public class Item
    {
        public string href { get; set; }
        public IList<Link> links{ get; private set; }
        public IList<Data> data { get; private set; }

        public Item(string href)
        {
            this.href = href;
            this.data = new List<Data>();
            this.links = new List<Link>();
        }

    }
}