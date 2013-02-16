using System.Collections.Generic;
using System.Linq;

namespace The203.CollectionJson.Core.Model
{
	public class PluralValueDictionary<TKey, TElement> : ILookup<TKey, TElement>
    {
	   private Dictionary<TKey, List<TElement>> publicDictionary;

	   public PluralValueDictionary()
	   {
		  this.publicDictionary = new Dictionary<TKey, List<TElement>>();
	   }

	   public bool Contains(TKey key)
	   {
		  return this.publicDictionary.ContainsKey(key);
	   }

	   public int KeyCount
	   {
		  get
		  {
			 return this.publicDictionary.Count;
		  }
	   }

	   public int ValueCount
	   {
		  get
		  {
			 return this.publicDictionary.Sum(kvp => kvp.Value.Count);
		  }
	   }
	   public int Count
	   {
		  get
		  {
			 return this.KeyCount;
		  }
	   }

	   public IEnumerable<TElement> this[TKey key]
	   {
		  get { return this.publicDictionary[key]; }
	   }

	   public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
	   {
		  foreach (TKey key in this.publicDictionary.Keys)
		  {
			 // TODO:  Should this return a pointer to a clone rather than the actual contents of teh dictionary entry?
			 yield return new KeyListPair<TKey, TElement>(key, this.publicDictionary[key]);
		  }
	   }

	   System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	   {
		  return this.GetEnumerator();
	   }

	   public void Add(TKey key, TElement value)
	   {
		  List<TElement> list;
		  if (this.publicDictionary.TryGetValue(key, out list))
		  {
			 list.Add(value);
		  }
		  else
		  {
			 list = new List<TElement> {value};
			  this.publicDictionary.Add(key, list);
		  }
	   }
    }
}
