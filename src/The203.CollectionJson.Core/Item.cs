using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pearson.StudyGuide.CollectionJson
{
    public class Item
    {
        private string href;
        private IList<Link> links;
        private IList<Data> data;

        public Item(string href, IList<Data> data, IList<Link> links)
        {
            this.href = href;
            this.data = data;
            this.links = links;
        }

    }
}