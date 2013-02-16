using System;
using The203.CollectionJson.Core.Model;
namespace The203.CollectionJson.Core
{
    public interface ICJ
    {
        void AddTemplate();
        CollectionContainer Container { get; set; }
        string GenerateJson();
    }
}
