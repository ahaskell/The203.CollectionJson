using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core;

namespace The203.CollectionJson.Test
{
    [TestClass]
    public class ExtensionTests : BaseTestClass
    {
        [TestMethod]
        public void VerifyIsScalarHandlesEnums()
        {
            Assert.IsTrue(typeof (TestEnum).IsScalar());

            Assert.IsTrue(typeof (TestClass).GetProperty("Value").ExposedToClient());

            Assert.IsTrue(typeof (TestStruct).GetProperty("Value").ExposedToClient());
        }

        [TestMethod]
        public void VerifyIsScalarHandlesNullables()
        {
            int? nullInt = 23;
            bool? nullBool = true;
            TestStruct? nullableStruct = new TestStruct() {Value = TestEnum.Value26};

            Assert.IsTrue(nullInt.GetType().IsScalar());
            Assert.IsTrue(nullBool.GetType().IsScalar());
            Assert.IsFalse(nullableStruct.GetType().IsScalar());
        }

        [TestMethod]
        public void VerifyIsScalarHandlesAllPrimitiveTypes()
        {
            Assert.IsTrue(typeof (bool).IsScalar());
            Assert.IsTrue(typeof (Byte).IsScalar());
            Assert.IsTrue(typeof (SByte).IsScalar());
            Assert.IsTrue(typeof (Int16).IsScalar());
            Assert.IsTrue(typeof (Int32).IsScalar());
            Assert.IsTrue(typeof (UInt32).IsScalar());
            Assert.IsTrue(typeof (Int64).IsScalar());
            Assert.IsTrue(typeof (UInt64).IsScalar());
            Assert.IsTrue(typeof (Char).IsScalar());
            Assert.IsTrue(typeof (Double).IsScalar());
            Assert.IsTrue(typeof (Single).IsScalar());
        }

        [TestMethod]
        public void VerifyIsScalarHandlesStrings()
        {
            Assert.IsTrue(typeof (string).IsScalar());
        }

        [TestMethod]
        public void VerifyIsScalarHandlesOtherGenerics()
        {
            List<int> testList = new List<int>();
            Assert.IsFalse(testList.GetType().IsScalar());
        }

        [TestMethod]
        public void VerifyIsScalarHandlesPlainNullable()
        {
            Assert.IsFalse(typeof (Nullable).IsScalar());
        }

        [TestMethod]
        public void VerifyIsScalarHandlesDates()
        {
            Assert.IsTrue(typeof (DateTime).IsScalar());
        }

        [TestMethod]
        public void VerifyIsScalarHandlesGuids()
        {
            Assert.IsTrue(typeof (Guid).IsScalar());
        }

        [TestMethod]
        public void VerifyIsScalarHandlesDecimals()
        {
            Assert.IsTrue(typeof (Decimal).IsScalar());
        }

        [TestMethod]
        public void VerifyIsScalarHandlesComplexTypes()
        {
            Assert.IsFalse(typeof (TestClass).IsScalar());
            Assert.IsFalse(typeof (TestStruct).IsScalar());
        }

        [TestMethod]
        public void VerifyIsExposedToClientRecognizesHiddenProperties()
        {
            var pi = typeof (TestClass).GetProperty("Value");
            Assert.IsTrue(pi.ExposedToClient());
            pi = typeof (TestClass).GetProperty("HiddenValue");
            Assert.IsFalse(pi.ExposedToClient());
        }

        private class TestClass
        {
            public TestEnum Value { get; set; }

            [HideFromClient()]
            public TestEnum HiddenValue { get; set; }
        }

        private enum TestEnum
        {
            Value1,
            Value2,
            Value26 = 26
        }

        private struct TestStruct
        {
            public TestEnum Value { get; set; }
        }
    }
}