using System;
using System.Collections.Generic;

namespace The203.CollectionJson.Core.Model
{
	public interface ICollectionJsonCollection
	{
		String href { get; set; }
		IList<ILink> links { get; }
		IList<IItem> items { get; }
		ITemplate template { get; set; }
		IError error { get; set; }
	}
}