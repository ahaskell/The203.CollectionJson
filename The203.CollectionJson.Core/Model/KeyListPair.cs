using System.Collections.Generic;
using System.Linq;

namespace The203.CollectionJson.Core.Model
{
	public class KeyListPair<TKey, TElement> : List<TElement>, IGrouping<TKey, TElement>
	{
		public TKey Key
		{
			get;
			set;
		}

 
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public KeyListPair(TKey key, IEnumerable<TElement> list)
		{
			this.Key = key;
			this.AddRange(list);
		}
	}
}