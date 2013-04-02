using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The203.CollectionJson.Core.Model
{
	public class Link : ILink
	{
        public string href { get; set; }
        public string rel { get; set; }
        public Link(string relationship, string href)
        {
            this.rel = relationship;
            this.href = href;

        }

       
    }
}