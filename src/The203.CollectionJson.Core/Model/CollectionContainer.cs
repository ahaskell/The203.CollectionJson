using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The203.CollectionJson.Core.Model
{
	public class CollectionContainer : ICollectionContainer
	{
        public ICollectionJsonCollection collection { get; set; }
        public CollectionContainer()
        {
            this.collection = new CollectionJsonCollection();
        }
    }
}
