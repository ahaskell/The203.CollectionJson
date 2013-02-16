using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net.Mime;
using System.Web.Script.Serialization;
using The203.CollectionJson.Core;
using The203.CollectionJson.Core.Links;
using System.Diagnostics.CodeAnalysis;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Mvc
{
	  [ExcludeFromCodeCoverage]
    public class CollectionJsonErrorResult: ActionResult
    {
	   private Exception exception;

	   public CollectionJsonErrorResult(Exception exception)
	   {
		   this.exception = exception;
	   }
	   
	   public override void ExecuteResult(ControllerContext context)
	   {
		  if (context == null)
		  {
			 throw new ArgumentNullException("context");
		  }

		  HttpResponseBase response = context.HttpContext.Response;
		  response.ContentType = "application/vnd.collection+json";
		  CollectionContainer container = new CollectionContainer();
		  container.collection = new CollectionJsonCollection(){error = new Error(exception)};
		  container.collection.href = context.HttpContext.Request.Url.AbsoluteUri;
		  JavaScriptSerializer serializer = new JavaScriptSerializer();
		  response.Write(serializer.Serialize(container));
	   }
    }
}
