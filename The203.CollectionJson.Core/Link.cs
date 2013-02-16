using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pearson.StudyGuide.CollectionJson
{
    public class Link
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