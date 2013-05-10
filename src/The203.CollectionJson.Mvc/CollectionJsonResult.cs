using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
	   private bool newItemCreated;

	   public CollectionJsonResult(ICJ<T> collectionJson)
	   {
		  this.collectionJson = collectionJson;
	   }

	   public ILinkBuilder<T> BuildLinks()
	   {
		  return collectionJson.BuildLinks();
	   }

	   public void AddTemplate()
	   {
		  collectionJson.AddTemplate();
	   }
	   public void AddTemplate<TT>()
	   {
		  collectionJson.AddTemplate<TT>();
	   }
	   public void NewItemCreated()
	   {
		  this.newItemCreated = true;
	   }

	   public override void ExecuteResult(ControllerContext context)
	   {
		  if (context == null)
		  {
			 throw new ArgumentNullException("context");
		  }
		  HttpResponseBase response = context.HttpContext.Response;
		  if (this.newItemCreated)
		  {

			 response.RedirectLocation = collectionJson.CreateCollectionContainer().collection.items[0].href;
			 response.StatusCode = 201;
			 return;
		  }
		  response.ContentType = "application/vnd.collection+json";
		  response.Write(collectionJson.GenerateJson());
	   }

	    public void IncludeField(Expression<Func<T, object>> field)
	    {
		    collectionJson.AddFieldToData(field);
	    }
    }
}
