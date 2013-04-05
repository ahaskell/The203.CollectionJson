using The203.CollectionJson.Core;

namespace The203.CollectionJson.Mvc
{
	public interface IRouteCreation
	{
		void MapRoutes<T>(T routeCollection, ICollectionJsonRoute cjRoutes);
	}
}