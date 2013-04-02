using System;
using The203.CollectionJson.Core.Links;

namespace The203.CollectionJson.Core
{
   public  interface ICJ<T> : ICJ
    {
        ILinkBuilder<T> BuildLinks();
    }
}
