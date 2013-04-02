using System.Collections.Generic;

namespace The203.CollectionJson.Core.Model
{
	public interface IItem
	{
		string href { get; set; }
		IList<ILink> links { get; }
		IList<IData> data { get; }
	}
}