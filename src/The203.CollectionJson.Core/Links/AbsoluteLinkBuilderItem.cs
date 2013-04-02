using System;
using System.Collections.Generic;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core.Links
{
    public class AbsoluteLinkBuilderItem<T> : ILinkBuilderItem<T>
    {
	   private readonly Func<T, string> resolver;
	   private readonly string rel;

	   public AbsoluteLinkBuilderItem(string rel, Func<T, string> resolver)
	   {
		  this.rel = rel;
		  this.resolver = resolver;
	   }
	   public void Resolve(T baseObject, IDictionary<LinkBuilderUrlType, string> urlMap, IDictionary<Type, IRouteMapping> mappings, IList<Model.ILink> links)
	   {
		  links.Add(new Link(this.rel, resolver(baseObject)));
	   }

	   public Type LinkFor { get; private set; }
    }
}
