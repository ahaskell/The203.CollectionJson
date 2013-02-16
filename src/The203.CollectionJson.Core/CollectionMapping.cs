using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The203.CollectionJson.Core
{

    public class ItemMapping<TItem> : IRouteMapping<TItem>
    {
        public IEnumerable<Func<TItem, string>> RoutePropertyGetters { get; private set; }
        public string RouteTemplate { get; set; }
        public string Controller { get; set; }
        public Type Parent { get; set; }

        private RouteMappingScope scope;
        public virtual RouteMappingScope Scope
        {
            get { return this.scope; }
        }

        public ItemMapping(string routeTemplate, Func<TItem, string>[] routePropertyGetters, String controllerName)
            : this(routeTemplate, routePropertyGetters, controllerName, null)
        {
        }

        public ItemMapping(string routeTemplate, Func<TItem, string>[] routePropertyGetters, String controllerName, System.Type parent)
        {
            this.RouteTemplate = routeTemplate;
            this.RoutePropertyGetters = routePropertyGetters;
            this.Controller = controllerName;
            this.Parent = parent;
            SetScope(RouteMappingScope.Item);
        }

        protected void SetScope(RouteMappingScope scope)
        {
            this.scope = scope;
        }
    }


    public class CollectionMapping<TItem> : ItemMapping<TItem>
    {

        public CollectionMapping(string routeTemplate, Func<TItem, string>[] routePropertyGetters, String controllerName)
            : this(routeTemplate, routePropertyGetters, controllerName, null)
        {
        }

        public CollectionMapping(string routeTemplate, Func<TItem, string>[] routePropertyGetters, String controllerName, System.Type parent)
            : base(routeTemplate, routePropertyGetters, controllerName, parent)
        {
            SetScope(RouteMappingScope.Collection);
        }

    }

    public class CollectionItemMapping<TItem> : CollectionMapping<TItem>
    {

        public CollectionItemMapping(string routeTemplate, Func<TItem, string>[] routePropertyGetters, String controllerName) :
            this(routeTemplate, routePropertyGetters, controllerName, null)
        {
        }

        public CollectionItemMapping(string routeTemplate, Func<TItem, string>[] routePropertyGetters, String controllerName, System.Type parent) :
            base(routeTemplate, routePropertyGetters, controllerName, parent)
        {
            SetScope(RouteMappingScope.ItemAndCollection);
        }

    }

}