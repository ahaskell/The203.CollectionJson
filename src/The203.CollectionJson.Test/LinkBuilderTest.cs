using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core;
using The203.CollectionJson.Core.Links;
using The203.CollectionJson.Core.Model;
using The203.CollectionJson.Test.Domain;

namespace The203.CollectionJson.Test
{
    [TestClass]
    public class LinkBuilderTest : BaseTestClass
    {

        [TestMethod]
        public void EnsureLinkbuilderCanHaveAParent()
        {

            LinkBuilder<Room> linkBuilder = new LinkBuilder<Room>(defaultRouteBuilder);
            var actual = linkBuilder.AddParent<House>("House", sampleHouse);
            IList<ILink> links = new List<ILink>();
            linkBuilder.PopulateLinks(room3, links);
            Assert.AreSame(linkBuilder, actual);
            var actualLink = from link in links
                             where link.rel == "House"
                             select link;
            Assert.IsTrue(actualLink.Count() == 1);
            Assert.AreEqual("/Houses/123", actualLink.First().href);
        }

        [TestMethod]
        public void EnsureLinkbuilderCanHaveAChild()
        {
            LinkBuilder<Room> linkBuilder = new LinkBuilder<Room>(defaultRouteBuilder);
            room3.RoomDimension = new RoomDimension();
            room3.RoomDimension.AnswerId = Guid.NewGuid().ToString();
            room3.RoomDimension.Score = 10;
            var actual = linkBuilder.AddParent<House>("House", sampleHouse)
                                    .AddChildAlways<RoomDimension>("RoomDimension", d => d.RoomDimension);
            var links = new List<ILink>();
            linkBuilder.PopulateLinks(room3, links);
            Assert.AreSame(linkBuilder, actual);
            var actualLink = from link in links
                             where link.rel == "RoomDimension"
                             select link;
            Assert.IsTrue(actualLink.Count() == 1);
            Assert.AreEqual("/Houses/123/Rooms/CD/RoomDimension", actualLink.First().href);
        }

        [TestMethod]
        public void EnsureTopLevelItemCanHaveChildren()
        {
            List<Room> items = new List<Room>();
            sampleHouse.Rooms.Add(room3);
            sampleHouse.Rooms.Add(room1);

            LinkBuilder<House> linkBuilder = new LinkBuilder<House>(defaultRouteBuilder);
            var actual = linkBuilder.IsParent()
                                    .AddChildCollection<Room>("Rooms", sg => sg.Rooms);
            var links = new List<ILink>();
            linkBuilder.PopulateLinks(sampleHouse, links);
            Assert.AreSame(linkBuilder, actual);
            var actualLink = from link in links
                             where link.rel == "Rooms"
                             select link;
            Assert.IsTrue(actualLink.Count() == 1);
            Assert.AreEqual("/Houses/123/Rooms", actualLink.First().href);
        }

        [TestMethod]
        public void EnsureLinkbuilderCanHaveChildren()
        {
            LinkBuilder<Room> linkBuilder = new LinkBuilder<Room>(defaultRouteBuilder);
            room3.RoomDimension = new RoomDimension();
            room3.RoomDimension.AnswerId = Guid.NewGuid().ToString();
            room3.RoomDimension.Score = 10;
            var actual = linkBuilder.AddParent<House>("House", sampleHouse)
                                    .AddChildCollection<Furniture>("Furniture", d => d.Furniture);
            var links = new List<ILink>();
            linkBuilder.PopulateLinks(room3, links);
            Assert.AreSame(linkBuilder, actual);
            var actualLink = from link in links
                             where link.rel == "Furniture"
                             select link;
            Assert.IsTrue(actualLink.Count() == 1);
            Assert.AreEqual("/Houses/123/Rooms/CD/Furniture", actualLink.First().href);
        }

        /// <summary>
        /// Note that LinkBuilder always adds from the vantage point of the type that it was created for.  So in the test below,
        /// the sibling is added at the Room level.  See:  fluent interface ("new context is equivalent to last
        /// context")
        /// </summary>
        [TestMethod]
        public void EnsureLinkbuilderCanHaveSiblings()
        {
            LinkBuilder<Room> linkBuilder = new LinkBuilder<Room>(defaultRouteBuilder);
            room3.RoomDimension = new RoomDimension();
            room3.RoomDimension.AnswerId = Guid.NewGuid().ToString();
            room3.RoomDimension.Score = 10;
            var actual = linkBuilder.AddParent<House>("House", sampleHouse)
                                    .AddSibling<Room>("Next", room2)
                                    .AddSibling<Room>("Previous", room1);
            var links = new List<ILink>();
            linkBuilder.PopulateLinks(room3, links);

            Assert.AreSame(linkBuilder, actual);
            var actualLink = from link in links
                             where link.rel == "Next"
                             select link;
            Assert.IsTrue(actualLink.Count() == 1);
            Assert.AreEqual("/Houses/123/Rooms/EF", actualLink.First().href);

            actualLink = from link in links
                         where link.rel == "Previous"
                         select link;
            Assert.IsTrue(actualLink.Count() == 1);
            Assert.AreEqual("/Houses/123/Rooms/AB", actualLink.First().href);
        }

        [TestMethod]
        [ExpectedException(typeof (ApplicationException))]
        public void EnsureErrorThrownIfParentNull()
        {
            //  CollectionJsonRoomDimension<Room> usg = new CollectionJsonRoomDimension<Room>(room1);
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);
            builder.AddChild("Test", ai => ai.Furniture);
            builder.PopulateLinks(room1, new List<ILink>());
        }

        [TestMethod]
        [ExpectedException(typeof (ApplicationException))]
        public void EnsureErrorThrownIfParentNullForGetSelf()
        {
            CJ<Room> result = new CJ<Room>(room1, defaultRouteBuilder, "");
            var container = result.CreateCollectionContainer();
        }

        [TestMethod]
        public void EnsureIsParentSetsParentLink()
        {
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);

            // IsParent is supposed to return the ref to builder.  
            var result = builder.IsParent();
            Assert.AreSame(builder, result);

            // Populate links throws an exception if it is called when parent and parentLink are null.
            // So if it doesn't throw an exception, we know that one or both have been set.  And the only
            // thing that sets parent is AddParent, which we didn't call, so we can assume that
            // the parentLink has been set.  Kind of indirect and a usg of knowledge of the internals,
            // but there you are.
            List<ILink> target = new List<ILink>();
            builder.PopulateLinks(room1, target);
            Assert.AreEqual("", target.First().rel);
            Assert.AreEqual(1, target.Count());
        }

        [TestMethod]
        public void EnsureAddParentSetsParent()
        {
            LinkBuilder<Furniture> builder = new LinkBuilder<Furniture>(defaultRouteBuilder);

            // AddParent is supposed to return the ref to builder.  
            var result = builder.AddParent<Room>("Test", room3);
            Assert.AreSame(builder, result);

            List<ILink> targets = new List<ILink>();
            builder.PopulateLinks(room3.Furniture.First(), targets);
            Assert.AreEqual("Test", targets.First().rel);
        }

        [TestMethod]
        public void EnsureAddParentWorksWithId()
        {
            LinkBuilder<Furniture> builder = new LinkBuilder<Furniture>(defaultRouteBuilder);

            // AddParent is supposed to return the ref to builder.  
            var result = builder.AddParent<Room>("Test", room3.Id);
            Assert.AreSame(builder, result);

            List<ILink> targets = new List<ILink>();
            builder.PopulateLinks(room3.Furniture.First(), targets);
            Assert.AreEqual("Test", targets.First().rel);
            Assert.AreEqual("/Rooms/" + room3.Id, targets.First().href);

            LinkBuilder<RoomDimension> builder2 =
                new LinkBuilder<RoomDimension>(defaultRouteBuilder);

            RoomDimension result2 = new RoomDimension() {AnswerId = "AA"};
            RoomDimension result3 = new RoomDimension() {AnswerId = "XX"};
            builder2.IsParent();
            builder2.AddSibling<RoomDimension>("Sibling", result2.AnswerId);

            targets = new List<ILink>();
            builder2.PopulateLinks(result3, targets);
            Assert.AreEqual("Sibling", targets.Last().rel);
            Assert.AreEqual("/RoomDimension", targets.Last().href);
        }

        [TestMethod]
        public void EnsureAddChildrenCreatesAllChildren()
        {
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);

            var result = builder
                .IsParent()
                .AddChildCollection<Furniture>("Test", ar => ar.Furniture);

            List<ILink> targets = new List<ILink>();
            builder.PopulateLinks(room3, targets);

            Assert.AreEqual(2, targets.Count);
            Assert.AreEqual("", targets[0].rel);
            Assert.AreEqual("Test", targets[1].rel);
        }

        [TestMethod]
        public void EnsureAddChildrenDoesntCreateLinkIfCollectionEmpty()
        {
            room1.Furniture.Clear();
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);

            var result = builder
                .IsParent()
                .AddChildCollection<Furniture>("Test", ar => ar.Furniture);

            List<ILink> targets = new List<ILink>();
            builder.PopulateLinks(room1, targets);

            Assert.AreEqual(1, targets.Count);
            Assert.AreEqual("", targets[0].rel);
        }

        [TestMethod]
        public void EnsureAddChildrenHandlesNullCollection()
        {
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);

            var result = builder
                .IsParent()
                .AddChildCollection<Furniture>("Test", ai => null);

            List<ILink> targets = new List<ILink>();
            builder.PopulateLinks(room1, targets);

            Assert.AreEqual(1, targets.Count);
            Assert.AreEqual("", targets[0].rel);
        }

        [TestMethod]
        public void EnsureAddChildCreatesExactlyOneLink()
        {
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);

            var result = builder
                .IsParent()
                .AddChild<Furniture>("Test", ar => ar.Furniture.First());

            List<ILink> targets = new List<ILink>();
            builder.PopulateLinks(room3, targets);

            Assert.AreEqual(2, targets.Count);
            Assert.AreEqual("", targets[0].rel);
            Assert.AreEqual("Test", targets[1].rel);
            Assert.IsTrue(targets[1].href.EndsWith(room3.Furniture.First().Id.ToString()));
        }

        [TestMethod]
        public void EnsureAddChildHandlesNull()
        {
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);

            var result = builder
                .IsParent()
                .AddChild<Furniture>("Test", ar => null);

            List<ILink> targets = new List<ILink>();
            builder.PopulateLinks(room3, targets);
            Assert.AreEqual("", targets[0].rel);
            Assert.AreEqual(1, targets.Count);
        }

        [TestMethod]
        public void EnsureAddSiblingOverloadWorks()
        {
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);

            var result = builder
                .IsParent()
                .AddSibling<Room>("Next", room1.Id);


            List<ILink> targets = new List<ILink>();
            builder.PopulateLinks(room3, targets);

            Assert.AreEqual(2, targets.Count);
            Assert.AreEqual("Next", targets[1].rel);
            Assert.AreEqual("/Rooms/" + room1.Id, targets[1].href);
        }

        [TestMethod]
        public void SeeWhatCalculatePrependUrlDoes()
        {
            var mappings = defaultRouteBuilder;
            string searchedRouteTemplate = defaultRouteBuilder.GetRoutes(typeof(House))[typeof(Room)].RouteTemplate;
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);

            string randomUrl = @"https://plus.google.com/hangouts/_/7941eba9551c7d3090763b7d978950594a230209#/";
            string urlWithTemplateStuckInIt = randomUrl + searchedRouteTemplate + "/asdfaksdafsdfasdf";

            var result = builder.CalculatePrependUrl<House>(urlWithTemplateStuckInIt);
            result = result.AddParent("Guide", sampleHouse);

            List<ILink> links = new List<ILink>();
            result.PopulateLinks(room1, links);

            Assert.AreEqual(1, links.Count);

            var actual = links[0];
            Assert.AreEqual("Guide", actual.rel);
            Assert.AreEqual(randomUrl + "Houses/123", actual.href);
            Assert.IsFalse(actual.href.Contains("asdfaksdafsdfasdf"));

            // Apparently it lops off the variable portion of the parent's template, looks for that in the URL,
            // removes that and anything afterward, and uses the result of that for the prefix.  
        }


        [TestMethod]
        public void VerifyAddParentWithDelegateProducesExpectedLink()
        {
            var mappings = defaultRouteBuilder;
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);

            var result = builder
                .AddSibling<Room>("Next", room1.Id)
                .AddParent<House>("Parent", ai => sampleHouse);


            List<ILink> targets = new List<ILink>();
            builder.PopulateLinks(room1, targets);

            Assert.AreEqual(2, targets.Count);
            // Parents get processed before children
            Assert.AreEqual("/Houses/123", targets[0].href);
            Assert.AreEqual("/Houses/123/Rooms/AB", targets[1].href);
        }


        [TestMethod]
        public void VerifyAddSiblingWithDelegateProducesExpectedLink()
        {
            var mappings = defaultRouteBuilder;
            LinkBuilder<Room> builder = new LinkBuilder<Room>(defaultRouteBuilder);

            var result = builder
                .AddSibling<Room>("Next", room1.Id)
                .AddSibling<Room>("Previous", ai => room2)
                .AddParent<House>("Parent", ai => sampleHouse);


            List<ILink> targets = new List<ILink>();
            builder.PopulateLinks(room1, targets);

            Assert.AreEqual(3, targets.Count);
            // Parents get processed before children
            Assert.AreEqual("/Houses/123", targets[0].href);
            Assert.AreEqual("/Houses/123/Rooms/AB", targets[1].href);
            Assert.AreEqual("/Houses/123/Rooms/EF", targets[2].href);
        }
    }
}