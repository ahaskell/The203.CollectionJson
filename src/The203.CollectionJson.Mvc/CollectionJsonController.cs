using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using The203.CollectionJson.Core;

namespace The203.CollectionJson.Mvc
{
   [ExcludeFromCodeCoverage]
    public abstract class CollectionJsonController : Controller
    {
	   protected virtual void HydrateModel<T>(T model) where T : class
	   {
		  var body = ExtractBody();
		   ICollectionJsonTemplating<T> collectionTemplate = new CollectionJsonTemplating<T>();
		  collectionTemplate.HydrateInstance(body, model);
	   }

	   private string ExtractBody()
	   {
		   var reader = new System.IO.StreamReader(Request.InputStream);
		   string body = reader.ReadToEnd();
		   Request.InputStream.Position = 0;
		   return body;
	   }

	   protected void HydrateModel<T>(T model, NameValueCollection nameValueCollection) where T : class
	   {
		  var body = ExtractBody();
		  ICollectionJsonTemplating<T> collectionTemplate = new CollectionJsonTemplating<T>();
		  collectionTemplate.HydrateInstance(body, nameValueCollection, model);
	   }

	   protected virtual void HydrateModel<T>(T model, Func<IDictionary<string, string>> func)
	   {
		  var body = ExtractBody();
		  ICollectionJsonTemplating<T> collectionTemplate = new CollectionJsonTemplating<T>();
		  collectionTemplate.HydrateInstance(body, func(), model);

	   }

	   protected CollectionJsonResult<T> CollectionJsonResult<T>(T targetObject)
	   {
		  var cj = new CJ<T>(targetObject, RouteBuilder.Instance, this.Request.Url.ToString());
		  cj.BuildLinks().PrependToUrl(this.BaseUrl);
		  return new CollectionJsonResult<T>(cj);
	   }

	   protected CollectionJsonResult<T> CollectionJsonResult<T>(IEnumerable<T> targetObjects)
	   {
		  var cj = new CJ<T>(targetObjects, RouteBuilder.Instance, this.Request.Url.ToString());
		  cj.BuildLinks().PrependToUrl(this.BaseUrl);
		  return new CollectionJsonResult<T>(cj);
	   }

	   protected string BaseUrl
	   {
		  get
		  {
			 return string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
		  }
	   }
    }
}