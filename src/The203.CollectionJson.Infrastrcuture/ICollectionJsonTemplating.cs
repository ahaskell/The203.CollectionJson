using System.Collections.Generic;
using System.Collections.Specialized;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core
{
	public interface ICollectionJsonTemplating<T>
	{
		ITemplate GenerateTemplate();
		T CreateTemplate(string input);
		T HydrateInstance(string jsonInput, IDictionary<string, string> urlInput, T baseTemplate);
		T HydrateInstance(string jsonInput, T baseTemplate);
		T HydrateInstance(string jsonInput, NameValueCollection queryString, T baseTemplate);
	}
}