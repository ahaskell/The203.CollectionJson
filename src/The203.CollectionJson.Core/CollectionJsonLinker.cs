using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core
{
	

	public class CollectionJsonLinker 
	{
	   private enum LinkerState
	   {
		  None,
		  BuildingPrimary,
		  BuildingAlternate
	   }

	   public Dictionary<Type, IRouteMapping> InternalMappings { get; private set; }
	   public PluralValueDictionary<Type, IRouteMapping> AlternateMappings { get; private set; }
	   private static CollectionJsonLinker _instance;
	   public string SiteUrl { get; set; }
	   private LinkerState linkerState;

	   public static CollectionJsonLinker Instance
	   {
		  get
		  {
			 if (_instance == null)
			 {
				_instance = new CollectionJsonLinker();
			 }
			 return _instance;
		  }

	   }


	   public CollectionJsonLinker()
	   {
		  InternalMappings = new Dictionary<Type, IRouteMapping>();
		  AlternateMappings = new PluralValueDictionary<Type, IRouteMapping>();
		  this.linkerState = LinkerState.None;
	   }

	   public CollectionJsonLinker StartPrimaryRoute()
	   {
		  if (this.linkerState != LinkerState.None)
			 throw new InvalidOperationException("Previous routing was not terminated with MapRoute.");
		  this.linkerState = LinkerState.BuildingPrimary;
		  return this;
	   }

	   public CollectionJsonLinker StartAlternateRoute()
	   {
		  if (this.linkerState != LinkerState.None)
			 throw new InvalidOperationException("Previous routing was not terminated with MapRoute.");

		  this.linkerState = LinkerState.BuildingAlternate;
		  return this;
	   }


	   public CollectionJsonLinker AddEmbeddedItem<TParent, TItem>(string route)
	   {
		  Func<TItem, String>[] blank = new Func<TItem, String>[] { a => "" };
		  IRouteMapping mapping = new ItemMapping<TItem>(route, blank, GetControllerOfRoute(route), typeof(TParent));
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }

	   public CollectionJsonLinker AddEmbeddedCollection<TParent, TCollectionItem>(string route)
	   {
		  Func<TCollectionItem, String>[] blank = new Func<TCollectionItem, String>[] { a => "" };
		  IRouteMapping mapping = new CollectionMapping<TCollectionItem>(route, blank, GetControllerOfRoute(route), typeof(TParent));
		  AddMapping(typeof(TCollectionItem), mapping);
		  return this;
	   }

	   public CollectionJsonLinker AddFixedEndPoint<TItem>(String route)
	   {
		  IRouteMapping mapping = new ItemMapping<TItem>(route, new Func<TItem, String>[] { iterator => "" }, GetControllerOfRoute(route));
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }

	   public CollectionJsonLinker AddItemAndCollection<TItem>(String route, params Func<TItem, String>[] identifier)
	   {
		  IRouteMapping mapping = new CollectionItemMapping<TItem>(route, identifier, GetControllerOfRoute(route));
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }


	   public CollectionJsonLinker AddItemAndCollection<TParent, TItem>(string routeSegment, params Func<TItem, String>[] itemIdGetter)
	   {
		  IRouteMapping mapping = new CollectionItemMapping<TItem>(routeSegment, itemIdGetter, GetControllerOfRoute(routeSegment), typeof(TParent));
		  AddMapping(typeof(TItem), mapping);
		  return this;
	   }

	   public void MapRoutes(RouteCollection routeCollection)
	   {
		  switch (this.linkerState)
		  {
			 case LinkerState.None:
				throw new InvalidOperationException("Expected state to be BuildingAlternate or BuildingPrimary, but it was None.");
			 case LinkerState.BuildingPrimary:
				RouteMappings(routeCollection, InternalMappings);
				this.linkerState = LinkerState.None;
				break;
			 case LinkerState.BuildingAlternate:
				RouteMappings(routeCollection, AlternateMappings);
				this.linkerState = LinkerState.None;
				break;
			 default:
				throw new InvalidOperationException("linkerState has invalid state");
		  }

	   }

	   private void RouteMappings(RouteCollection routeCollection, IEnumerable<KeyValuePair<Type, IRouteMapping>> mappings)
	   {
		  foreach (var routeEntry in mappings)
		  {
			 IRouteMapping route = routeEntry.Value;
			 string routeName = CalculateRouteName(route);
			 string urlRoute = CalculateRoute(route);

			 if (route.Scope == RouteMappingScope.Item || route.Scope == RouteMappingScope.ItemAndCollection)
			 {
				routeCollection.MapRoute(routeName + ".Item", urlRoute, new { controller = route.Controller, action = "Item" });
			 }
			 if (route.Scope == RouteMappingScope.Collection || route.Scope == RouteMappingScope.ItemAndCollection)
			 {
				Match match = Regex.Match(urlRoute, "^(.*){\\w+}$");
				if (match.Success)
				{
				    urlRoute = match.Groups[1].Value;
				}
				routeCollection.MapRoute(routeName + ".Collection", urlRoute, new { controller = route.Controller, action = "Collection" });
			 }
		  }
	   }

	   private void RouteMappings(RouteCollection routeCollection, PluralValueDictionary<Type, IRouteMapping> mappings)
	   {
		  IEnumerable<KeyValuePair<Type, IRouteMapping>> routeEntries =
			 mappings.SelectMany(klp => klp.Select(entry => new KeyValuePair<Type, IRouteMapping>(klp.Key, entry)));

		  RouteMappings(routeCollection, routeEntries);

	   }

	   private string CalculateRoute(IRouteMapping route)
	   {

		  string finalRoute = route.RouteTemplate;
		  if (route.Parent != null)
		  {
			 if (InternalMappings.ContainsKey(route.Parent))
				return CalculateRoute(InternalMappings[route.Parent]) + "/" + finalRoute;
			 else
				// TODO:  ???? Does this make sense?  
				return AlternateMappings[route.Parent].Select(m => CalculateRoute(m)).First() + "/" + finalRoute;
		  }
		  return finalRoute;
	   }

	   private string CalculateRouteName(IRouteMapping route)
	   {
		  //string routeName = routeSegment.GetType().GetGenericArguments()[0].Name;
		  return route.Controller;
		  //return routeName;
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
		  switch (this.linkerState)
		  {
			 case LinkerState.None:
				throw new InvalidOperationException("Building a route must be started by calling StartPrimaryRoute or StartAlternateRoute.");
			 case LinkerState.BuildingAlternate:
				AlternateMappings.Add(type, mapping);
				break;
			 case LinkerState.BuildingPrimary:
				InternalMappings.Add(type, mapping);
				break;
			 default:
				throw new InvalidOperationException("Invalid linker state");
		  }
	   }

    }
}