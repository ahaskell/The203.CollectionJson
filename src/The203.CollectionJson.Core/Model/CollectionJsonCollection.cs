using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The203.CollectionJson.Core.Model
{
    public class CollectionJsonCollection
    {
        public String href { get; set; }
        public IList<Link> links {get; private set;}
        public IList<Item> items { get; private set; }
        public Template template { get; set;}
	   public Error error { get; set; }
        public CollectionJsonCollection()
        {
            this.links = new List<Link>();
            this.items = new List<Item>();
        }
    }
}
