using System;
using System.Collections;
using System.Collections.Generic;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core.Links
{
    /// <summary>
    /// This Class is used to Build out the Links portion of the Collection Item
    /// TAnchor represents to the object all the links are anchored off. Generally relationships are relative to the TAnchor.
    /// So AddChild(jan) means jan is a child of TAnchor.
    /// 
    /// Link Builder offers a fluent interface which allows all the commands to be chained together. Since it can be used over and over to build links
    /// for all items on the collection it doesn't really have a terminating method. When you're done building links just stop calling it. 
    /// 
    /// </summary>
    /// <value>
    /// 
    /// </value>
    public class LinkBuilder<TAnchor> : ILinkBuilder<TAnchor>
    {
        private readonly IRouteBuilder routeBuilder;
        private readonly PluralValueDictionary<Type, IRouteMapping> alternateMappings;
        private readonly IDictionary<LinkBuilderUrlType, String> baseUrls;
        private readonly IList<ILinkBuilderItem<TAnchor>> linkBuilders;
        private ILinkBuilderItem<TAnchor> parent;
        private ILink parentLink;
        private Type rootType;

        public LinkBuilder(IRouteBuilder routeBuilder)
        {
            this.routeBuilder = routeBuilder;
            this.linkBuilders = new List<ILinkBuilderItem<TAnchor>>();
            this.baseUrls = new Dictionary<LinkBuilderUrlType, String> {
			  {LinkBuilderUrlType.Prepend, ""}
			 ,{LinkBuilderUrlType.None, ""}
			 ,{LinkBuilderUrlType.Parent, ""}
			 ,{LinkBuilderUrlType.OriginalItem, ""}
		  };
        }

        public LinkBuilder(IRouteBuilder routeBuilder, PluralValueDictionary<Type, IRouteMapping> alternateMappingData) :
            this(routeBuilder)
        {
            this.alternateMappings = alternateMappingData;
        }


        public ILinkBuilder<TAnchor> IsParent()
        {
            this.rootType = typeof(TAnchor);
            this.parentLink = new Link("", "");
            this.baseUrls[LinkBuilderUrlType.Parent] = this.baseUrls[LinkBuilderUrlType.Prepend];
            return this;
        }

        public ILinkBuilder<TAnchor> AddParent<TParent>(String rel, Func<TAnchor, TParent> getParent)
        {
            this.parent = new ItemLinkBuilderItem<TAnchor, TParent>(rel, getParent, LinkBuilderUrlType.Prepend);
            return this;
        }

        public ILinkBuilder<TAnchor> AddParent<TParent>(String rel, TParent theParent)
        {
            this.parent = new ItemLinkBuilderItem<TAnchor, TParent>(rel, useless => theParent, LinkBuilderUrlType.Prepend);
            return this;
        }

        public ILinkBuilder<TAnchor> AddParent<TParent>(String rel, String theId)
        {
            this.parent = new SimpleLinkBuilderItem<TAnchor, TParent>(rel, theId, LinkBuilderUrlType.Prepend);
            return this;
        }

        public ILinkBuilder<TAnchor> AddChildCollection<TChild>(string rel, Func<TAnchor, IEnumerable<TChild>> getChildren)
        {
            this.linkBuilders.Add(new CollectionLinkBuilderItem<TAnchor, TChild>(rel, getChildren, LinkBuilderUrlType.OriginalItem));
            return this;
        }

        public ILinkBuilder<TAnchor> AddChild<TChild>(string rel, Func<TAnchor, TChild> getChild)
        {
            this.linkBuilders.Add(new ItemLinkBuilderItem<TAnchor, TChild>(rel, getChild, LinkBuilderUrlType.OriginalItem));
            return this;
        }
        public ILinkBuilder<TAnchor> AddChildAlways<TChild>(string rel, Func<TAnchor, TChild> getChild)
        {
            this.linkBuilders.Add(new ItemLinkBuilderItem<TAnchor, TChild>(rel, getChild, LinkBuilderUrlType.OriginalItem).Always());
            return this;
        }

        public ILinkBuilder<TAnchor> AddSibling<TSibling>(string rel, TSibling sibling)
        {
            this.linkBuilders.Add(new ItemLinkBuilderItem<TAnchor, TSibling>(rel, thing => sibling, LinkBuilderUrlType.Parent));
            return this;
        }

        public ILinkBuilder<TAnchor> AddSibling<TSibling>(string rel, string siblingId)
        {
            this.linkBuilders.Add(new SimpleLinkBuilderItem<TAnchor, TSibling>(rel, siblingId, LinkBuilderUrlType.Parent));
            return this;
        }

        public ILinkBuilder<TAnchor> AddSibling<TSibling>(String rel, Func<TAnchor, TSibling> getSibling)
        {
            this.linkBuilders.Add(new ItemLinkBuilderItem<TAnchor, TSibling>(rel, getSibling, LinkBuilderUrlType.Parent));
            return this;
        }


        public ILinkBuilder<TAnchor> AddRelativeLink<TLinkRelativeTo>(string rel, Func<TAnchor, TLinkRelativeTo> getRelatedObject, String tackOnUrl)
        {
            LinkBuilderUrlType type = LinkBuilderUrlType.OriginalItem;
            if (typeof(TLinkRelativeTo) == typeof(TAnchor))
            {
                type = LinkBuilderUrlType.Prepend;
            }
            this.linkBuilders.Add(new ItemLinkBuilderItem<TAnchor, TLinkRelativeTo>(rel, getRelatedObject, type, tackOnUrl));
            return this;
        }
        public ILinkBuilder<TAnchor> AddAbsoluteLink(string rel, Func<TAnchor, string> resolveUrl)
        {
            this.linkBuilders.Add(new AbsoluteLinkBuilderItem<TAnchor>(rel, resolveUrl));
            return this;
        }

        public ILinkBuilder<TAnchor> AddRelativeGroup<TLinkRelativeTo, TGroupType>(Func<TGroupType, String> relationResolver, Func<TAnchor, IEnumerable<TGroupType>> getRelatedGroup, Func<TGroupType, String> resolveItemId, String tackOnUrl)
        {
            LinkBuilderUrlType type = LinkBuilderUrlType.OriginalItem;
            if (typeof(TLinkRelativeTo) == typeof(TAnchor))
            {
                type = LinkBuilderUrlType.Prepend;
            }
            if (typeof(TLinkRelativeTo) == this.parent.LinkFor)
            {
                type = LinkBuilderUrlType.Parent;
            }
            this.linkBuilders.Add(new CollectionLinkBuilderCollection<TAnchor, TGroupType>(relationResolver, getRelatedGroup, resolveItemId, type, tackOnUrl));
            return this;
        }

        public void PopulateLinks(TAnchor source, IList<ILink> targetLinks)
        {
            ResolveParent(source);
            if (parentLink.rel != "")
            {
                targetLinks.Add(this.parentLink);
            }
            this.baseUrls[LinkBuilderUrlType.OriginalItem] = GetSelf(source);
            foreach (var linkBuilderItem in this.linkBuilders)
            {
                linkBuilderItem.Resolve(source, this.baseUrls, routeBuilder.GetRoutes(this.rootType), targetLinks);
            }
        }

        private void ResolveParent(TAnchor source)
        {
            if (this.parentLink == null)
            {
                if (this.parent == null)
                {
                    throw new ApplicationException("Cannot resolve top-level URL.  Call IsParent or AddParent.");
                }
                IList<ILink> parentLinkList = new List<ILink>();
                if (this.rootType == null)
                {
                    this.rootType = this.parent.LinkFor;
                }
                this.parent.Resolve(source, this.baseUrls, routeBuilder.GetRoutes(this.rootType), parentLinkList);
                this.parentLink = parentLinkList[0];
                this.baseUrls[LinkBuilderUrlType.Parent] = this.parentLink.href;
            }
        }

        internal string GetSelf(TAnchor source)
        {
            ResolveParent(source);

            ItemLinkBuilderItem<TAnchor, TAnchor> self = new ItemLinkBuilderItem<TAnchor, TAnchor>("self", me => me, LinkBuilderUrlType.Parent);
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (source == null)
            // ReSharper restore CompareNonConstrainedGenericWithNull
            {
                self.Always();
            }
            IList<ILink> selfLinkList = new List<ILink>();
            self.Resolve(source, this.baseUrls, routeBuilder.GetRoutes(this.rootType), selfLinkList);
            return selfLinkList[0].href;
        }

        public ILinkBuilder<TAnchor> PrependToUrl(string urlPart)
        {
            this.baseUrls[LinkBuilderUrlType.Prepend] = urlPart;
            return this;
        }

        public ILinkBuilder<TAnchor> TryToCalculatePrependUrl<RootT>(string url)
        {
            IDictionary<Type, IRouteMapping> routes = routeBuilder.GetRoutes(typeof(RootT));
            if (routes.ContainsKey(typeof(TAnchor)) && routes.ContainsKey(routes[typeof(TAnchor)].Parent))
            {
                return CalculatePrependUrl<RootT>(url);
            }
            return PrependToUrl(url);
        }

        public ILinkBuilder<TAnchor> CalculatePrependUrl<RootT>(string url)
        {//This method will throw errors if it can not find a parent, but it won't hide odd bugs...use at own risk4
            this.rootType = typeof(RootT);
            IDictionary<Type, IRouteMapping> routes = routeBuilder.GetRoutes(this.rootType);
            String parentRouteTemplate = routes[routes[typeof(TAnchor)].Parent].RouteTemplate;
            String searchString = parentRouteTemplate.Remove(parentRouteTemplate.IndexOf("/", StringComparison.Ordinal));
            String urlPrefix = url.Remove(url.IndexOf("/" + searchString + "/", StringComparison.Ordinal));
            PrependToUrl(urlPrefix);
            return this;
        }
    }
}
