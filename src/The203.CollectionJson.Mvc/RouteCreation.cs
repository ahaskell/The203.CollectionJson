using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using The203.CollectionJson.Core;

namespace The203.CollectionJson.Mvc
{
    public class RouteCreation : IRouteCreation
    {
	   
	   public void MapRoutes<T>(T routeCollection, ICollectionJsonRoute cjRoutes)
	   {
		  RouteMappings(routeCollection as RouteCollection, cjRoutes);
	   }

	   private void RouteMappings(RouteCollection routeCollection, ICollectionJsonRoute cjRoutes)
	   {
		  if (routeCollection == null)
		  {
			  throw new NotImplementedException("RouteCollection not found!");
		  }
		  IDictionary<Type, IRouteMapping> mappings = cjRoutes.Routes;
		  foreach (var routeEntry in mappings)
		  {
			 IRouteMapping route = routeEntry.Value;
			 string routeName = CalculateRouteName(route, cjRoutes);
			 string urlRoute = CalculateRoute(route, cjRoutes);

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


	   private string CalculateRoute(IRouteMapping route, ICollectionJsonRoute cjRoutes)
	   {

		  string finalRoute = route.RouteTemplate;

		  if (route.Parent != null)
		  {
			 if (cjRoutes.Routes.ContainsKey(route.Parent))
				return CalculateRoute(cjRoutes.Routes[route.Parent], cjRoutes) + "/" + finalRoute;
		  }
		  return finalRoute;
	   }

	   private string CalculateRouteName(IRouteMapping route, ICollectionJsonRoute cjRoutes)
	   {
		  //string routeName = routeSegment.GetType().GetGenericArguments()[0].Name;
		  return String.Format("{0}-{1}", cjRoutes.Name, route.Controller);
		  //return routeName;
	   }
    }
}
