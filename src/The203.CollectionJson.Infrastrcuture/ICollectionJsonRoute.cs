using System;
using System.Collections.Generic;
using The203.CollectionJson.Mvc;

namespace The203.CollectionJson.Core
{
    public interface ICollectionJsonRoute
    {
	   string Name { get; }
	   IDictionary<Type, IRouteMapping> Routes { get; }

	   ICollectionJsonRoute AddItemOnly<TItem>(String route);
	   ICollectionJsonRoute AddItemOnly<TParent, TItem>(string route);


	   ICollectionJsonRoute AddCollectionOnly<TItem>(string route);
	   ICollectionJsonRoute AddCollectionOnly<TItem>(Type controller, string route);
	   ICollectionJsonRoute AddCollectionOnly<TParent, TCollectionItem>(string route);

	   

	   ICollectionJsonRoute AddItemAndCollection<TItem>(String route, params Func<TItem, String>[] identifier);
	   ICollectionJsonRoute AddItemAndCollection<TItem>(Type controller, String route, params Func<TItem, String>[] identifier);

	   ICollectionJsonRoute AddItemAndCollection<TParent, TItem>(string routeSegment, params Func<TItem, String>[] itemIdGetter);
	   ICollectionJsonRoute AddItemAndCollection<TParent, TItem>(Type controller, string routeSegment, params Func<TItem, String>[] itemIdGetter);


	   void MapRoutes<T>(IRouteCreation routeCreation, T routes);
    }
}