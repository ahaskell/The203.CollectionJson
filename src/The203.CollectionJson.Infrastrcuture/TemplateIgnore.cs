using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The203.CollectionJson.Core
{
    /// <summary>
    /// When used on a property that would otherwise be sent to the client, this attribute causes the
    /// CollectionJson framework to suppress it.  
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TemplateIgnoreAttribute : Attribute
    {
    }

    /// <summary>
    /// When used on a property that would otherwise not be sent to the client, this attribute causes the
    /// CollectionJson framework to add the property to a template it.  
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TemplateIncludeAttribute : Attribute
    {
    }
}
