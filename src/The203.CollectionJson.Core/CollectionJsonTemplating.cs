using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core
{
    public class CollectionJsonTemplating<T> : ICollectionJsonTemplating<T>
    {
	   private readonly Template template;

// ReSharper disable StaticFieldInGenericType - We want a static containing the property info for each type, so we aren't constantly reflecting to find it.
	   private static IEnumerable<PropertyInfo> PropertyInfos;
// ReSharper restore StaticFieldInGenericType

	   public CollectionJsonTemplating()
	   {
		  IList<Data> templateData = new List<Data>();
		  this.template = new Template(templateData);
	   }

	   public ITemplate GenerateTemplate()
	   {

		  PopulateDataTemplate(template.data, typeof(T));

		  return template;
	   }

	   private void PopulateDataTemplate(IList<Data> data, Type templatable)
	   {
		  PropertyInfo[] properties = templatable.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
		  foreach (PropertyInfo pi in properties)
		  {
			 if (pi.ExposedToClient())
			 {
				data.Add(new Data(pi.Name, ""));
			 }
		  }
	   }

	   public T CreateTemplate(string input)
	   {
		  T baseTemplate = (T)Activator.CreateInstance(typeof(T));
		  return HydrateInstance(input, baseTemplate);
	   }


	   public T HydrateInstance(string jsonInput, IDictionary<string, string> urlInput, T baseTemplate)
	   {
		  if (urlInput == null)
			 throw new ArgumentNullException("urlInput");


		  T objectFromJson = baseTemplate;
		  if (!String.IsNullOrWhiteSpace(jsonInput))
		  {
			 objectFromJson = HydrateInstance(jsonInput, baseTemplate);
		  }

		  foreach (KeyValuePair<string, string> data in urlInput)
		  {
			 PropertyInfo prop = GetCachedProperty(data.Key);
			 if (prop == null)
				throw new PropertyNotFoundHydrationException { ElementName = data.Key, ElementValue = data.Value };

			 object newValue = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromString(data.Value);
			 prop.SetValue(baseTemplate, newValue, null);
		  }
		  return objectFromJson;
	   }

	   public T HydrateInstance(string jsonInput, T baseTemplate)
	   {
		  if (jsonInput == null)
			 throw new ArgumentNullException("jsonInput");
		  if (string.IsNullOrWhiteSpace(jsonInput))
			 throw new ArgumentException("jsonInput string is null or empty");
		  if (baseTemplate == null)
			 throw new ArgumentNullException("baseTemplate");

		  JavaScriptSerializer serializer = new JavaScriptSerializer();
		  TemplateContainer tc = serializer.Deserialize<TemplateContainer>(jsonInput);
		  foreach (Data data in tc.template.data)
		  {
			 PropertyInfo prop = GetCachedProperty(data.name);
			 if (prop == null)
				throw new PropertyNotFoundHydrationException { ElementName = data.name, ElementValue = data.value };
			 object newValue = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromString(data.value);
			 prop.SetValue(baseTemplate, newValue, null);
		  }
		  return baseTemplate;
	   }

	   private static PropertyInfo GetCachedProperty(string p)
	   {
		  if (PropertyInfos == null)
		  {
			 PropertyInfos = typeof(T).GetProperties();
		  }

		  return PropertyInfos.FirstOrDefault(pi => pi.Name.ToLowerInvariant() == p.ToLowerInvariant());

	   }

	   public T HydrateInstance(string jsonInput, NameValueCollection queryString, T baseTemplate)
	   {
		  if (queryString == null)
			 throw new ArgumentNullException("queryString");


		  T objectFromJson = baseTemplate;
		  if (!String.IsNullOrWhiteSpace(jsonInput))
		  {
			 objectFromJson = HydrateInstance(jsonInput, baseTemplate);
		  }

		  foreach (string key in queryString.Keys)
		  {
			 //Becuase MS is awesome and likes to put crap in the "url" even though it is not really there we have to filter this stupid key out...
			 if (key == "uid") { continue; }
			 object value = queryString[key];
			 PropertyInfo prop = GetCachedProperty(key);
			 if (prop == null)
				throw new PropertyNotFoundHydrationException { ElementName = key, ElementValue = value.ToString() };

			 object newValue = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromString(value.ToString());
			 prop.SetValue(objectFromJson, newValue, null);
		  }
		  return objectFromJson;
	   }
    }
}
