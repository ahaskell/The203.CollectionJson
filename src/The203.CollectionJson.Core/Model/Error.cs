using System;

namespace The203.CollectionJson.Core.Model
{
	public class Error
	{

	    public Error(Exception exception)
	    {
		    this.message = exception.Message;
		    ICJException cjException = exception as ICJException;
		    if (cjException != null)
		    {
			    this.code = cjException.Code;
			    this.title = cjException.Title;
			    this.message = cjException.UserMessage;
		    }
	    }
		public String title { get; set; }
		public String code { get; set; }
		public string message { get; set; }
	}
}