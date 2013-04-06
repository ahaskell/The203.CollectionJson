using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core;
using The203.CollectionJson.Test.Domain;

namespace The203.CollectionJson.Test
{
    [TestClass]
    public class CollectionJsonRoomDimensionTest : BaseTestClass
    {
        [TestMethod]
        public void VerifyHrefSetting()
        {
            var targetObject = room1;
            var cjLinker = defaultCollectionJsonLinker;
            CJ<Room> result = new CJ<Room>(targetObject, defaultRouteBuilder, "/Houses/123/Rooms");
            result.AddTemplate();
            result.BuildLinks().AddParent<House>("parent", sampleHouse);

            var container = result.CreateCollectionContainer();
            Assert.AreEqual("/Houses/123/Rooms", container.collection.href);
        }

        // TODO:  What is this actually testing?  ANything?
        [TestMethod]
        public void VerifyExtraPropertyOnTemplateCausesException()
        {
            var targetObject = room1;
            CJ<Room> result = new CJ<Room>(targetObject, defaultRouteBuilder, "");
            result.BuildLinks().IsParent();


            var container = result.CreateCollectionContainer();
        }

        [TestMethod]
        public void VerifyExposeToClientCausesPropertiesToBeSkipped()
        {
            var targetObject = room1;
            CJ<Room> result = new CJ<Room>(targetObject, defaultRouteBuilder, "");
            result.BuildLinks().IsParent();

            string[] hiddenFields = new string[] {"RawQtiData", "QtiIdent", "QtiAssessmentId"};
            var container = result.CreateCollectionContainer();

            Assert.IsTrue(container.collection.items.Count > 0);
            Assert.IsTrue(container.collection.items.All(item => item.data.Count() > 0));

            Assert.IsFalse(container.collection.items.Any(i => i.data.Any(d => hiddenFields.Contains(d.name))));

            CJ<Furniture> responseRoomDimension = new CJ<Furniture>(room1.Furniture.First(), defaultRouteBuilder, "");
            responseRoomDimension.BuildLinks().AddParent("parent", room1);

            hiddenFields = new string[] {"QtiIdentifier"};
            container = responseRoomDimension.CreateCollectionContainer();

            Assert.IsTrue(container.collection.items.Count > 0);
            Assert.IsTrue(container.collection.items.All(item => item.data.Count() > 0));

            Assert.IsFalse(container.collection.items.Any(i => i.data.Any(d => hiddenFields.Contains(d.name))));
        }
    }
}