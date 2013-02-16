using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core.Links
{
    public class SimpleLinkBuilderItem<TItem, TRelated> : LinkBuilderItem<TItem, TRelated>
    {
        private string rel;
        private string theId;

        public SimpleLinkBuilderItem(string rel, string theId, LinkBuilderUrlType prependUrlKey)
        {
            this.rel = rel;
            this.theId = theId;
            this.prependUrlKey = prependUrlKey;
        }


        public override void Resolve(TItem baseObject, IDictionary<LinkBuilderUrlType, string> urlMap, IDictionary<Type, IRouteMapping> mappings, IList<Link> links )
        {
            var url = CombinePath(GetPrependUrl(urlMap), CreateItemLink(mappings));
            links.Add(new Link(this.rel, url));

        }
        protected string CreateItemLink(IDictionary<Type, IRouteMapping> mappings)
        {
            Regex placeHolderFinder = new Regex("{.*?}");
            if (mappings.ContainsKey(typeof(TRelated)))
            {
                IRouteMapping<TRelated> routeMapping = (IRouteMapping<TRelated>)mappings[typeof(TRelated)];
                string urlTemplate = routeMapping.RouteTemplate;
                MatchCollection placeHolders = placeHolderFinder.Matches(urlTemplate);

                foreach (Match placeHolder in placeHolders)
                {
                    // var getProperty = routeMapping.RoutePropertyGetters.ElementAt(iterationCount++);
                    urlTemplate = urlTemplate.Replace(placeHolder.Value, Convert.ToString(this.theId));
                }
                return "/" + urlTemplate;
            }
            return null;
        }
    }
}
