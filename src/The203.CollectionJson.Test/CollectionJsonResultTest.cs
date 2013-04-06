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

        [TestMethod]
        public void VerifyExposeToClientCausesPropertiesToBeSkipped()
        {
            var targetObject = room1;
            CJ<Room> result = new CJ<Room>(targetObject, defaultRouteBuilder, "");
            result.BuildLinks().AddParent<House>("parent", "2");

            string[] hiddenFields = new string[] {"ClientShallntSeeThis"};
            var container = result.CreateCollectionContainer();

            Assert.IsTrue(container.collection.items.Count > 0);
            Assert.IsTrue(container.collection.items.All(item => item.data.Count() > 0));

            Assert.IsFalse(container.collection.items.Any(i => i.data.Any(d => hiddenFields.Contains(d.name))));
        }
    }
}