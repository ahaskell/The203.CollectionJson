using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core.Links
{

    public interface ILinkBuilderItem<TItem>
    {
	    Type LinkFor { get; }
	    void Resolve(TItem baseObject, IDictionary<LinkBuilderUrlType, string> urlMap, IDictionary<Type, IRouteMapping> mappings, IList<ILink> links );
    }
}
