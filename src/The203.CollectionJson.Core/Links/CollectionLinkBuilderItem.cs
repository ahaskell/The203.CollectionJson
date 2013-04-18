using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core.Links
{
    public class CollectionLinkBuilderItem<TItem, TRelated> : LinkBuilderItem<TItem, TRelated>
    {
	   private string rel;
	   private Func<TItem, IEnumerable<TRelated>> resolver;

	   public CollectionLinkBuilderItem(string rel, Func<TItem, IEnumerable<TRelated>> resolver, LinkBuilderUrlType prependUrlKey)
	   {
		  this.rel = rel;
		  this.resolver = resolver;
		  this.prependUrlKey = prependUrlKey;
	   }

	   public override void Resolve(TItem baseObject, IDictionary<LinkBuilderUrlType, string> urlMap, IDictionary<Type, IRouteMapping> mappings, IList<ILink> links)
	   {
		  IEnumerable<TRelated> targetObject = resolver(baseObject);
		  if (targetObject != null && targetObject.Count() > 0 || ignoreNullIfPossible)
		  {
			 var url = CombinePath(GetPrependUrl(urlMap), CreateCollectionLink(mappings));
			 links.Add(new Link(this.rel, url));
		  }
	   }

	   private string CreateCollectionLink(IDictionary<Type, IRouteMapping> mappings)
	   {
		  IRouteMapping<TRelated> routeMapping = (IRouteMapping<TRelated>)mappings[typeof(TRelated)];
		  string urlTemplate = routeMapping.RouteTemplate;
		  return "/" + urlTemplate.Remove(urlTemplate.LastIndexOf('{') - 1);
	   }
    }
}
