using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net.Mime;
using The203.CollectionJson.Core;
using The203.CollectionJson.Core.Links;
using System.Diagnostics.CodeAnalysis;

namespace The203.CollectionJson.Mvc
{
       [ExcludeFromCodeCoverage]
    public class CollectionJsonResult<T> : ActionResult
    {
        private ICJ<T> collectionJson;

        public CollectionJsonResult(ICJ<T> collectionJson)
        {
            this.collectionJson = collectionJson;
        }

        public LinkBuilder<T> BuildLinks()
        {
            return collectionJson.BuildLinks();
        }

        public void AddTemplate()
        {
            collectionJson.AddTemplate();
        }
        
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/vnd.collection+json";

            response.Write(collectionJson.GenerateJson());
        }
    }
}
