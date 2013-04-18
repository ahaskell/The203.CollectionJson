using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core.Links
{
    public class CollectionLinkBuilderCollection<TItem, TRelated> : LinkBuilderItem<TItem, TRelated>
    {
	   private Func<TRelated, String> rel;
	   private Func<TItem, IEnumerable<TRelated>> resolver;
	   private string tackOnUrl;
	   private Func<TRelated, String> resolveId;
	   public CollectionLinkBuilderCollection(Func<TRelated, String> rel, Func<TItem, IEnumerable<TRelated>> resolver, Func<TRelated, String> resolveId, LinkBuilderUrlType prependUrlKey, String tackOnUrl)
	   {
		  this.rel = rel;
		  this.resolver = resolver;
		  this.prependUrlKey = prependUrlKey;
		  this.tackOnUrl = tackOnUrl;
		  this.resolveId = resolveId;
	   }


	   public override void Resolve(TItem baseObject, IDictionary<LinkBuilderUrlType, string> urlMap, IDictionary<Type, IRouteMapping> mappings, IList<ILink> links)
	   {
		  IEnumerable<TRelated> targetObject = resolver(baseObject);
		  if (targetObject != null)
		  {
			 foreach (var related in targetObject)
			 {
				ItemLinkBuilderItem<TRelated, TRelated> itemLink = new ItemLinkBuilderItem<TRelated, TRelated>(rel(related), item => item, prependUrlKey, tackOnUrl);
				itemLink.Resolve(related, urlMap, mappings, links);
			 }
		  }
	   }
    }
}
