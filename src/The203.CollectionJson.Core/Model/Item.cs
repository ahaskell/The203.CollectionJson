using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The203.CollectionJson.Core.Model
{
    public class Item : IItem
    {
        public string href { get; set; }
        public IList<ILink> links{ get; private set; }
        public IList<IData> data { get; private set; }

        public Item(string href)
        {
            this.href = href;
            this.data = new List<IData>();
            this.links = new List<ILink>();
        }

    }
}