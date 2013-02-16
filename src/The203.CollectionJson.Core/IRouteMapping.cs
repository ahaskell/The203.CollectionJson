using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The203.CollectionJson.Core
{
    public interface IRouteMapping
    {
        string RouteTemplate { get; set; }
        string Controller { get; set; }
        Type Parent { get; set; }
        RouteMappingScope Scope { get; }
    }

    public interface IRouteMapping<TItem> : IRouteMapping
    {
        IEnumerable<Func<TItem, string>> RoutePropertyGetters { get; }
    }

    public enum RouteMappingScope
    {
        Item,
        Collection,
        ItemAndCollection
    }
 
}