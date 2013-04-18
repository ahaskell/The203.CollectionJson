using System;
using System.Collections.Generic;
using System.IO;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core.Links
{
    public abstract class LinkBuilderItem<TItem, TRelated> : ILinkBuilderItem<TItem>
    {
	   protected LinkBuilderUrlType prependUrlKey;
	   protected bool ignoreNullIfPossible;

	   public Type LinkFor { get; private set; }
	   public LinkBuilderItem()
	   {
		  this.LinkFor = typeof(TRelated);
	   }

	   public ILinkBuilderItem<TItem> Always()
	   {
		  this.ignoreNullIfPossible = true;
		  return this;
	   }
	   protected virtual string GetPrependUrl(IDictionary<LinkBuilderUrlType, string> urlMap)
	   {
		  if (this.prependUrlKey == LinkBuilderUrlType.None)
		  {
			 return "";
		  }
		  else
		  {
			 try
			 {
				return urlMap[this.prependUrlKey];
			 }
			 catch (Exception)
			 {
				throw new ApplicationException("Desired Key not supplied!");
			 }
		  }
	   }
	   public abstract void Resolve(TItem baseObject, IDictionary<LinkBuilderUrlType, string> urlMap, IDictionary<Type, IRouteMapping> mappings, IList<ILink> links);


	   protected string CombinePath(string path1, string path2)
	   {
		  if (path2 == null)
			 return path1;

		  if (path2 != null && path2.StartsWith("/"))
			 path2 = path2.Substring(1);
		  if (path1 != null && !path1.EndsWith("/"))
			 path1 = string.Concat(path1, "/");

		  return Path.Combine(path1, path2);
	   }
    }




}
