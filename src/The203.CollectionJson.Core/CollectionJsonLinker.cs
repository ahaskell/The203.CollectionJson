using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Text;
using System.Text.RegularExpressions;
using The203.CollectionJson.Core.Model;
using The203.CollectionJson.Mvc;

namespace The203.CollectionJson.Core
{


    public class CollectionJsonLinker : ICollectionJsonRoute
    {
	   public IDictionary<Type, IRouteMapping> Routes { get; private set; }
	   public string SiteUrl { get; set; }
	   public string Name { get; private set; }
	   public CollectionJsonLinker()
		  : this(Guid.NewGuid().ToString())
	   {

	   }

	   public CollectionJsonLinker(String name)
	   {
		  this.Name = name;
		  this.Routes = new Dictionary<Type, IRouteMapping>();
	   }



	   public ICollectionJsonRoute AddItemOnly<TParent, TItem>(string route)
	   {
		  Func<TItem, String>[] blank = new Func<TItem, String>[] { a => "" };
		  IRouteMapping mapping = new ItemMapping<TItem>(route, blank, GetControllerOfRoute(route), typeof(TParent));
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }
	   public ICollectionJsonRoute AddItemOnly<TItem>(String route)
	   {
		  IRouteMapping mapping = new ItemMapping<TItem>(route, new Func<TItem, String>[] { iterator => "" }, GetControllerOfRoute(route));
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }

	   public ICollectionJsonRoute AddCollectionOnly<TParent, TCollectionItem>(string route)
	   {
		  Func<TCollectionItem, String>[] blank = new Func<TCollectionItem, String>[] { a => "" };
		  IRouteMapping mapping = new CollectionMapping<TCollectionItem>(route, blank, GetControllerOfRoute(route), typeof(TParent));
		  AddMapping(typeof(TCollectionItem), mapping);
		  return this;
	   }

	   public ICollectionJsonRoute AddCollectionOnly<TItem>(String route)
	   {
		  return AddCollectionOnly<TItem>(null, route);
	   }

	   public ICollectionJsonRoute AddCollectionOnly<TItem>(Type controller, String route)
	   {
		  String controllerName = controller == null ? GetControllerOfRoute(route) : controller.Name;
		  IRouteMapping mapping = new CollectionMapping<TItem>(route, new Func<TItem, String>[] { iterator => "" }, controllerName);
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }

	   

	   public ICollectionJsonRoute AddItemAndCollection<TItem>(String route, params Func<TItem, String>[] identifier)
	   {
		  IRouteMapping mapping = new CollectionItemMapping<TItem>(route, identifier, GetControllerOfRoute(route));
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }


	   public ICollectionJsonRoute AddItemAndCollection<TParent, TItem>(string routeSegment, params Func<TItem, String>[] itemIdGetter)
	   {
		  IRouteMapping mapping = new CollectionItemMapping<TItem>(routeSegment, itemIdGetter, GetControllerOfRoute(routeSegment), typeof(TParent));
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }

	   public ICollectionJsonRoute AddItemAndCollection<TItem>(Type controller, String route, params Func<TItem, String>[] identifier)
	   {
		  IRouteMapping mapping = new CollectionItemMapping<TItem>(route, identifier, controller.Name);
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }


	   public ICollectionJsonRoute AddItemAndCollection<TParent, TItem>(Type controller, string routeSegment, params Func<TItem, String>[] itemIdGetter)
	   {
		  IRouteMapping mapping = new CollectionItemMapping<TItem>(routeSegment, itemIdGetter, controller.Name, typeof(TParent));
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }

	   public void MapRoutes<T>(IRouteCreation routeCreation, T routes)
	   {
		  routeCreation.MapRoutes(routes, this);
	   }

	   private String GetControllerOfRoute(string route)
	   {
		  var routePieces = route.Split('/');
		  string controllerName = routePieces.First();
		  if (controllerName.Contains("{"))
		  {
			 controllerName = routePieces[routePieces.Length - 2];
		  }
		  return controllerName;
	   }


	   private void AddMapping(Type type, IRouteMapping mapping)
	   {
		  this.Routes.Add(type, mapping);
	   }

    }
}