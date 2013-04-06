using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core.Links;
using The203.CollectionJson.Core.Model;
using The203.CollectionJson.Test.Domain;

namespace The203.CollectionJson.Test
{
    [TestClass]
    public class ItemLinkBuilderTest : BaseTestClass
    {
        [TestMethod]
        public void EnsureLinkBuilderItemFollowDirectionsForTheUrlToUse()
        {
            List<ILink> linkList = new List<ILink>();
            room1.RoomDimension = new RoomDimension() {AnswerId = "567", Confidence = 10, Score = 0};
            ILinkBuilderItem<Room> itemLinkBuilder = new ItemLinkBuilderItem<Room, RoomDimension>("test",
                                                                                                  ai => ai.RoomDimension,
                                                                                                  LinkBuilderUrlType
                                                                                                      .Parent);
            itemLinkBuilder.Resolve(room1
                                    ,
                                    new Dictionary<LinkBuilderUrlType, String>()
                                        {
                                            {LinkBuilderUrlType.Parent, "FIND_ME_LATER"}
                                        }
                                    , defaultRouteBuilder.GetRoutes(typeof(House)), linkList);

            Assert.IsTrue(linkList[0].href.StartsWith("FIND_ME_LATER"));
        }


        [TestMethod]
        public void EnsureLinkBuilderItemFollowDirectionsHandlesNoneDifferently()
        {
            List<ILink> linkList = new List<ILink>();
            room1.RoomDimension = new RoomDimension() {AnswerId = "567", Confidence = 10, Score = 0};
            ILinkBuilderItem<Room> itemLinkBuilder = new ItemLinkBuilderItem<Room, RoomDimension>("test",
                                                                                                  ai => ai.RoomDimension,
                                                                                                  LinkBuilderUrlType
                                                                                                      .None);
            itemLinkBuilder.Resolve(room1
                                    ,
                                    new Dictionary<LinkBuilderUrlType, String>()
                                        {
                                            {LinkBuilderUrlType.None, "IF_YOU_FIND_THIS_WE_FAIL"},
                                            {LinkBuilderUrlType.Parent, "DONT_FIND_THIS_EITHER"}
                                        }
                                    , defaultRouteBuilder.GetRoutes(typeof(House)), linkList);

            Assert.IsFalse(linkList[0].href.Contains("IF_YOU_FIND_THIS_WE_FAIL"));
            Assert.IsFalse(linkList[0].href.Contains("DONT_FIND_THIS_EITHER"));
        }

        [TestMethod]
        [ExpectedException(typeof (ApplicationException))]
        // Technically the default implementation sets up blank values for each key so this edge case shouldn't ever happen even in test code. 
        public void EnsureLinkBuilderItemThrowsAppropriateErrorWhenAKeyIsMissing()
        {
            List<ILink> linkList = new List<ILink>();
            room1.RoomDimension = new RoomDimension() {AnswerId = "567", Confidence = 10, Score = 0};
            ILinkBuilderItem<Room> itemLinkBuilder = new ItemLinkBuilderItem<Room, RoomDimension>( //NOTE Parent is not setup below...hence error
                "test", ai => ai.RoomDimension, LinkBuilderUrlType.Parent);
            itemLinkBuilder.Resolve(room1
                                    ,
                                    new Dictionary<LinkBuilderUrlType, String>()
                                        {
                                            {LinkBuilderUrlType.None, "IF_YOU_FIND_THIS_WE_FAIL"}
                                        }
                                    , defaultRouteBuilder.GetRoutes(typeof(House)), linkList);
        }

        [TestMethod]
        public void NullReferencesRoomDimensionInNullReturnsWhichShouldBeEquivilentToNoLinkBeingAdded()
        {
            List<ILink> linkList = new List<ILink>();
            room1.RoomDimension = null; //Just making sure it is Null for this test to work!
            ILinkBuilderItem<Room> itemLinkBuilder = new ItemLinkBuilderItem<Room, RoomDimension>("test",
                                                                                                  ai => ai.RoomDimension,
                                                                                                  LinkBuilderUrlType
                                                                                                      .Parent);
            itemLinkBuilder.Resolve(room1
                                    ,
                                    new Dictionary<LinkBuilderUrlType, String>()
                                        {
                                            {LinkBuilderUrlType.Parent, "usless since it will be null"}
                                        }
                                    , defaultRouteBuilder.GetRoutes(typeof(House)), linkList);

            Assert.AreEqual(0, linkList.Count);
        }

        [TestMethod]
        public void WhatShouldHappenIfMappingContainsNoReference()
        {
            List<ILink> linkList = new List<ILink>();
            room1.RoomDimension = new RoomDimension() {AnswerId = "567", Confidence = 10, Score = 0};
            defaultRouteBuilder.GetRoutes(typeof(House)).Remove(typeof (RoomDimension));
            ILinkBuilderItem<Room> itemLinkBuilder = new ItemLinkBuilderItem<Room, RoomDimension>("test",
                                                                                                  ai => ai.RoomDimension,
                                                                                                  LinkBuilderUrlType
                                                                                                      .Parent);
            itemLinkBuilder.Resolve(room1
                                    ,
                                    new Dictionary<LinkBuilderUrlType, String>()
                                        {
                                            {LinkBuilderUrlType.Parent, "exactly what the url will be"}
                                        }
                                    , defaultRouteBuilder.GetRoutes(typeof(House)), linkList);

            Assert.AreEqual("exactly what the url will be", linkList[0].href);
        }

        [TestMethod]
        public void AllowAlwaysToForceLinksToBeBuiltRegardlessOfNullValue()
        {
            List<ILink> linkList = new List<ILink>();
            room1.RoomDimension = null;
            ILinkBuilderItem<Room> itemLinkBuilder =
                new ItemLinkBuilderItem<Room, RoomDimension>("test", ai => ai.RoomDimension, LinkBuilderUrlType.Parent).Always();
            itemLinkBuilder.Resolve(room1
                                    ,
                                    new Dictionary<LinkBuilderUrlType, String>()
                                        {
                                            {LinkBuilderUrlType.Parent, "http://www.outthere.com"}
                                        }
                                    , defaultRouteBuilder.GetRoutes(typeof(House)), linkList);

            Assert.AreEqual("http://www.outthere.com/RoomDimension", linkList[0].href);
        }
    }
}