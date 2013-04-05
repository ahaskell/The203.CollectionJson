using System;
using System.Collections.Generic;
using The203.CollectionJson.Mvc;

namespace The203.CollectionJson.Core
{
    public interface ICollectionJsonRoute
    {
	    string Name { get; }
	    IDictionary<Type, IRouteMapping> Routes { get; }
	    ICollectionJsonRoute AddEmbeddedItem<TParent, TItem>(string route);
	    ICollectionJsonRoute AddEmbeddedCollection<TParent, TCollectionItem>(string route);
	    ICollectionJsonRoute AddFixedEndPoint<TItem>(String route);
	    ICollectionJsonRoute AddItemAndCollection<TItem>(String route, params Func<TItem, String>[] identifier);
	    ICollectionJsonRoute AddItemAndCollection<TParent, TItem>(string routeSegment, params Func<TItem, String>[] itemIdGetter);
	    void MapRoutes<T>(IRouteCreation routeCreation,T routes);
    }
}