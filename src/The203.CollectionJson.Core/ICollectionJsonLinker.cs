using System;
using System.Collections.Generic;
using System.Web.Routing;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core
{
	public interface ICollectionJsonLinker
	{
		Dictionary<Type, IRouteMapping> InternalMappings { get; }
		PluralValueDictionary<Type, IRouteMapping> AlternateMappings { get; }
		string SiteUrl { get; set; }
		CollectionJsonLinker StartPrimaryRoute();
		CollectionJsonLinker StartAlternateRoute();
		CollectionJsonLinker AddEmbeddedItem<TParent, TItem>(string route);
		CollectionJsonLinker AddEmbeddedCollection<TParent, TCollectionItem>(string route);
		CollectionJsonLinker AddFixedEndPoint<TItem>(String route);
		CollectionJsonLinker AddItemAndCollection<TItem>(String route, params Func<TItem, String>[] identifier);
		CollectionJsonLinker AddItemAndCollection<TParent, TItem>(string routeSegment, params Func<TItem, String>[] itemIdGetter);
		void MapRoutes(RouteCollection routeCollection);
	}
}