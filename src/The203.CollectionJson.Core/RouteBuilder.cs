using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using The203.CollectionJson.Mvc;

namespace The203.CollectionJson.Core
{
    public interface IRouteBuilder
    {
	   IDictionary<Type, IRouteMapping> GetRoutes(Type parentType);
	   ICollectionJsonRoute StartPrimaryRoute<TRoot>();
	   ICollectionJsonRoute StartPrimaryRoute<TRoot>(String routeName);
	   ICollectionJsonRoute StartAlternateRoute();
	   ICollectionJsonRoute StartAlternateRoute(String routeName);
    }

    public class RouteBuilder : IRouteBuilder
    {
	   private static RouteBuilder instance;

	   public static RouteBuilder Instance
	   {
		  get
		  {
			 return instance ?? (instance = new RouteBuilder());
		  }
	   }

	   public RouteBuilder()
	   {
		  this.PrimaryRoutes = new Dictionary<Type, ICollectionJsonRoute>();
		  this.AlternateRoutes = new List<ICollectionJsonRoute>();
	   }

	   protected IDictionary<Type, ICollectionJsonRoute> PrimaryRoutes { get; private set; }

	   protected List<ICollectionJsonRoute> AlternateRoutes { get; private set; }

	   public IDictionary<Type, IRouteMapping> GetRoutes(Type parentType)
	   {
		  return this.PrimaryRoutes[parentType].Routes;
	   }

	   public ICollectionJsonRoute StartPrimaryRoute<TRoot>()
	   {
		  return StartPrimaryRoute<TRoot>(Guid.NewGuid().ToString());
	   }
	   public ICollectionJsonRoute StartPrimaryRoute<TRoot>(String routeName)
	   {
		  ICollectionJsonRoute cjRoute = new CollectionJsonLinker(routeName);
		  this.PrimaryRoutes.Add(typeof (TRoot), cjRoute);
		  return cjRoute;
	   }

	   public ICollectionJsonRoute StartAlternateRoute()
	   {
		  return StartAlternateRoute(Guid.NewGuid().ToString());
	   }
	   public ICollectionJsonRoute StartAlternateRoute(String routeName)
	   {
		  ICollectionJsonRoute cjRoute = new CollectionJsonLinker(routeName);
		  this.AlternateRoutes.Add(cjRoute);
		  return cjRoute;
	   }
    }
}
