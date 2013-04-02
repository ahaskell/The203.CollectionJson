using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using The203.CollectionJson.Core.Model;
using System.Text.RegularExpressions;

namespace The203.CollectionJson.Core.Links
{
    public class ItemLinkBuilderItem<TItem, TRelated> : LinkBuilderItem<TItem, TRelated>
    {
        protected string rel;
        protected Func<TItem, TRelated> resolver;
        protected IEnumerable<Func<TRelated, string>> propertyResolution;
        private bool ignoreNullIfPossible = false;
        private string tackOnUrl;


        public ItemLinkBuilderItem(string rel, Func<TItem, TRelated> resolver, LinkBuilderUrlType prependUrlKey):this(rel, resolver, prependUrlKey, null)
        {
        }

        public ItemLinkBuilderItem(string rel, Func<TItem, TRelated> resolver, LinkBuilderUrlType prependUrlKey, String tackOnUrl)
        {
            this.rel = rel;
            this.resolver = resolver;
            this.prependUrlKey = prependUrlKey;
            this.tackOnUrl = tackOnUrl;
        }

        public override void Resolve(TItem baseObject, IDictionary<LinkBuilderUrlType, string> urlMap, IDictionary<Type, IRouteMapping> mappings, IList<ILink> links )
        {
            TRelated targetObject = resolver(baseObject);
            if (targetObject != null || (targetObject == null && this.ignoreNullIfPossible))
            {
                var url = CombinePath(CombinePath(GetPrependUrl(urlMap), CreateItemLink(targetObject, mappings)), this.tackOnUrl);
                links.Add(new Link(this.rel, url));
            }
        }


        protected string CreateItemLink(TRelated targetObject, IDictionary<Type, IRouteMapping> mappings)
        {
            Regex placeHolderFinder = new Regex("{.*?}");
            if (mappings.ContainsKey(typeof(TRelated)))
            {
                IRouteMapping<TRelated> routeMapping = (IRouteMapping<TRelated>)mappings[typeof(TRelated)];
                string urlTemplate = routeMapping.RouteTemplate;
                MatchCollection placeHolders = placeHolderFinder.Matches(urlTemplate);
                int iterationCount = 0;
                foreach (Match placeHolder in placeHolders)
                {
                    var getProperty = routeMapping.RoutePropertyGetters.ElementAt(iterationCount++);
                    urlTemplate = urlTemplate.Replace(placeHolder.Value, Convert.ToString(getProperty(targetObject)));
                }
                return "/" + urlTemplate;
            }
            return null;
        }

        public ILinkBuilderItem<TItem> Always()
        {
            this.ignoreNullIfPossible = true;
            return this;
        }
    }
}
