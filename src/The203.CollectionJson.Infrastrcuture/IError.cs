using System;

namespace The203.CollectionJson.Core.Model
{
	public interface IError
	{
		String title { get; set; }
		String code { get; set; }
		string message { get; set; }
	}
}