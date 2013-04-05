using System;
using System.Collections.Generic;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Core.Links
{
	public interface ILinkBuilder<TAnchor>
	{
		ILinkBuilder<TAnchor> IsParent();
		ILinkBuilder<TAnchor> AddParent<TParent>(String rel, Func<TAnchor, TParent> getParent);
		ILinkBuilder<TAnchor> AddParent<TParent>(String rel, TParent theParent);
		ILinkBuilder<TAnchor> AddParent<TParent>(String rel, String theId);
		ILinkBuilder<TAnchor> AddChildCollection<TChild>(string rel, Func<TAnchor, IEnumerable<TChild>> getChildren);
		ILinkBuilder<TAnchor> AddChild<TChild>(string rel, Func<TAnchor, TChild> getChild);
		ILinkBuilder<TAnchor> AddChildAlways<TChild>(string rel, Func<TAnchor, TChild> getChild);
		ILinkBuilder<TAnchor> AddSibling<TSibling>(string rel, TSibling sibling);
		ILinkBuilder<TAnchor> AddSibling<TSibling>(string rel, string siblingId);
		ILinkBuilder<TAnchor> AddSibling<TSibling>(String rel, Func<TAnchor, TSibling> getSibling);
		ILinkBuilder<TAnchor> AddRelativeLink<TLinkRelativeTo>(string rel, Func<TAnchor, TLinkRelativeTo> getRelatedObject, String tackOnUrl);
		ILinkBuilder<TAnchor> AddAbsoluteLink(string rel, Func<TAnchor, string> resolveUrl);
		ILinkBuilder<TAnchor> AddRelativeGroup<TLinkRelativeTo, TGroupType>(Func<TGroupType, String> relationResolver, Func<TAnchor, IEnumerable<TGroupType>> getRelatedGroup, Func<TGroupType, String> resolveItemId, String tackOnUrl);
		void PopulateLinks(TAnchor source, IList<ILink> targetLinks);
		ILinkBuilder<TAnchor> PrependToUrl(string urlPart);
		ILinkBuilder<TAnchor> TryToCalculatePrependUrl<RootT>(string url);
		ILinkBuilder<TAnchor> CalculatePrependUrl<RootT>(string url);
	}
}