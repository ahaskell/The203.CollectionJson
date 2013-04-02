using System;
using The203.CollectionJson.Core.Model;
namespace The203.CollectionJson.Core
{
    public interface ICJ
    {
        void AddTemplate();
        ICollectionContainer Container { get; set; }
        string GenerateJson();
    }
}
