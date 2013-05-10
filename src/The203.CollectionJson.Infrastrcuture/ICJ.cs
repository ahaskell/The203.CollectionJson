using System;
using The203.CollectionJson.Core.Model;
namespace The203.CollectionJson.Core
{
    public interface ICJ
    {
	   void AddTemplate();
	   void AddTemplate<TT>();
	   ICollectionContainer Container { get; set; }
	   string GenerateJson();
	   ICollectionContainer CreateCollectionContainer();
    }
}
