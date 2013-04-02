using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The203.CollectionJson.Core.Model
{
    public class CollectionJsonCollection : ICollectionJsonCollection
    {
        public String href { get; set; }
        public IList<ILink> links {get; private set;}
        public IList<IItem> items { get; private set; }
        public ITemplate template { get; set;}
	   public IError error { get; set; }
        public CollectionJsonCollection()
        {
            this.links = new List<ILink>();
            this.items = new List<IItem>();
        }
    }
}
