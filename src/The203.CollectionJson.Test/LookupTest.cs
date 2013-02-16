using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core.Model;

namespace The203.CollectionJson.Test
{
    [TestClass]
    public class LookupTest
    {
        [TestMethod]
        public void VerifyLookupAddHandlesRepeatedAdds()
        {
            PluralValueDictionary<int, string> actual = new PluralValueDictionary<int, string>();
            actual.Add(1, "string");
            actual.Add(1, "string2");
            actual.Add(2, "string3");
            Assert.AreEqual(2, actual.KeyCount);
            Assert.AreEqual(3, actual.ValueCount);
            Assert.AreEqual(2, actual[1].Count());
            Assert.AreEqual(1, actual[2].Count());
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void VerifyLookupThrowsCorrectErrorOnNullKey()
        {
            PluralValueDictionary<string, string> actual = new PluralValueDictionary<string, string>();
            actual.Add(null, "something");
        }

        [TestMethod]
        public void VerifyCanWorkWithForLoop()
        {
            PluralValueDictionary<int, string> actual = new PluralValueDictionary<int, string>();
            actual.Add(1, "string");
            actual.Add(1, "string2");
            actual.Add(2, "string3");

            int i = 0;
            foreach (KeyListPair<int, string> grouping in actual)
            {
                i++;
                Assert.IsNotNull(grouping);
                Assert.AreNotEqual(0, grouping.Count);
                Assert.IsNotNull(grouping.Key);

                Assert.IsNotNull(grouping[0]);
            }
            Assert.AreEqual(2, i);
        }
    }
}