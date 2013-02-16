using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The203.CollectionJson.Core
{
    public class PropertyNotFoundHydrationException : ApplicationException
    {
        public string ElementName { get; set; }
        public string ElementValue { get; set; }
    }
}
