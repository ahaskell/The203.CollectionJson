using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The203.CollectionJson.Core.Model
{
    public class TemplateContainer
    {
	   private Template _template;

	   public TemplateContainer()
	   { }

	   public Template template
	   {
		  get
		  {
			 if (this.collection != null)
			 {
				return (Template)this.collection.template;
			 }
			 return this._template;

		  }//Probably should output a warning about template being set using this....see note below for more details. 
		  set { this._template = value; }
	   }
	   //Alright so this is a bit wierd but I've seen confusion about the template and if it is sent inside collection or alone. So to make dure the serializer
	   //always works template is the root element and the collection element is also the Template container. The end result is a serializer working for either way. 
	   public TemplateContainer collection { get; set; }
    }
}
