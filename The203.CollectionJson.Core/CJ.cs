using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Script.Serialization;
using System.Reflection;
using The203.CollectionJson.Core.Model;
using The203.CollectionJson.Core.Links;


namespace The203.CollectionJson.Core
{
    public class CJ<T> : ICJ<T>
    {
        private IEnumerable<T> targetObjects;
        protected LinkBuilder<T> linkBuilder;
        protected CollectionJsonLinker collectionJsonLinker;
        protected string collectionUrl;

        public CollectionContainer Container { get; set; }
        

        protected CJ(CollectionJsonLinker collectionJsonLinker, string collectionUrl)
        {
            this.linkBuilder = new LinkBuilder<T>(collectionJsonLinker.InternalMappings);
            this.collectionJsonLinker = collectionJsonLinker;
            this.collectionUrl = collectionUrl;
            this.Container = new CollectionContainer();
        }

        public CJ(T targetObject, CollectionJsonLinker collectionJsonLinker, string collectionUrl)
            : this(collectionJsonLinker, collectionUrl)
        {
            var objCol = new List<T>();
            objCol.Add(targetObject);
            this.targetObjects = objCol;            
        }

        public CJ(IEnumerable<T> targetCollection, CollectionJsonLinker collectionJsonLinker, string collectionUrl)
            : this(collectionJsonLinker, collectionUrl)
        {
            this.targetObjects = targetCollection;
        }

        public LinkBuilder<T> BuildLinks()
        {
            return this.linkBuilder;
        }

        private void PopulateData(IList<Data> data, T obj)
        {
            //There is a possibility of nulls being passed in thanks to changes made elsewhere in linking. Thanks to Adam....
            if (obj == null)
            { 
                return;
            }
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            foreach (PropertyInfo pi in properties.Where(pi2 => pi2.ExposedToClient()))
            {
                var value = pi.GetValue(obj, null);
                if (value != null)
                {
                    data.Add(new Data(pi.Name, value.ToString()));
                }
            }
        }

        public virtual String GenerateJson()
        {
            CollectionContainer container = CreateCollectionContainer();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(container);
        }

        public CollectionContainer CreateCollectionContainer()
        {
            
            foreach (T item in this.targetObjects)
            {
                this.Container.collection.items.Add(CreateItems(item));
            }

            this.Container.collection.href = this.collectionUrl;

            return this.Container;
        }

        private Item CreateItems(T collectionItem)
        {
            Item item = new Item(linkBuilder.GetSelf(collectionItem));
            linkBuilder.PopulateLinks(collectionItem, item.links);
            PopulateData(item.data, collectionItem);
            return item;
        }

        public void AddTemplate()
        {
            this.Container.collection.template = (new CollectionJsonTemplating<T>()).GenerateTemplate();
        }
    }
}