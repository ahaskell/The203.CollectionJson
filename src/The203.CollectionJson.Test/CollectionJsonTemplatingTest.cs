using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core;
using The203.CollectionJson.Core.Model;
using The203.CollectionJson.Test.Domain;

namespace The203.CollectionJson.Test
{
    [TestClass]
    public class CollectionJsonTemplatingTest
    {
        public string jsonInput = @"{""template"" : {
""data"": [ 
{""name"" : ""A"", ""value"" : 23},
{""name"" : ""B"", ""value"" : true},
{""name"" : ""C"", ""value"" : ""ABC""},
{""name"" : ""D"", ""value"" : ""264.342""},
{""name"" : ""E"", ""value"" : ""47E31D14-3B91-466B-B262-CD593C614A66""},
{""name"" : ""F"", ""value"" : ""234123.2342312312343""},
{""name"" : ""G"", ""value"" : ""123.23""}
]}}";

        [TestMethod]
        public void EnsureTemplatesAreGeneratedOffClassesCorrectly()
        {
            //  HouseTemplate usgTemplate = new HouseTemplate();
            CollectionJsonTemplating<HouseTemplate> template = new CollectionJsonTemplating<HouseTemplate>();
            var actual = template.GenerateTemplate();

            String[] names = {"FurnitureId", "OwnerId", "HasPets"};
            Assert.AreEqual(3, actual.data.Count);
            Assert.IsTrue(names.Contains(actual.data[0].name));
            Assert.IsTrue(names.Contains(actual.data[1].name));
            Assert.IsTrue(names.Contains(actual.data[2].name));
        }

        [TestMethod]
        public void EnsureTemplateGenerationSkipsPropertiesMarkedAsHidden()
        {
            CollectionJsonTemplating<Room> template = new CollectionJsonTemplating<Room>();
            var actual = template.GenerateTemplate();
            string[] hiddenFields = new string[] {"ClientShallntSeeThis"};

            foreach (Data d in actual.data)
                Assert.IsFalse(hiddenFields.Contains(d.name));

            CollectionJsonTemplating<Furniture> template2 = new CollectionJsonTemplating<Furniture>();
            actual = template2.GenerateTemplate();
            hiddenFields = new string[] {"ClientShallntSeeThis"};

            foreach (Data d in actual.data)
                Assert.IsFalse(hiddenFields.Contains(d.name));
        }

        [TestMethod]
        public void EnsureTemplateGenerationProvidesVisibleProperties()
        {
            CollectionJsonTemplating<Room> template = new CollectionJsonTemplating<Room>();
            var actual = template.GenerateTemplate();
            string[] visibleFields = new string[] {"Id", "Title", "ClientShallntSeeThis", "RoomDimension", "Id", "RoomDimension"};

            foreach (Data d in actual.data)
                Assert.IsTrue(visibleFields.Contains(d.name));

            CollectionJsonTemplating<Furniture> template2 = new CollectionJsonTemplating<Furniture>();
            actual = template2.GenerateTemplate();
            visibleFields = new string[] {"Id", "Weight"};

            foreach (Data d in actual.data)
                Assert.IsTrue(visibleFields.Contains(d.name));
        }

        [TestMethod]
        public void EnsureTemplatesGenerateForAllPropertyTypes()
        {
            CollectionJsonTemplating<DummyClass> template = new CollectionJsonTemplating<DummyClass>();
            var actual = template.GenerateTemplate();

            String[] names = {"A", "B", "C", "D", "E", "F", "G"};

            // EventArgs (H) isn't a scalar and shouldn't be serialized. And things not
            // serializable shouldn't appear in the template.  
            Assert.AreEqual(7, actual.data.Count);
            for (int i = 0; i < 7; i++)
            {
                Assert.IsTrue(names.Contains(actual.data[i].name));
            }
            Assert.IsFalse(actual.data.Select(d => d.name).Any(n => n.Equals("H")));
        }

        [TestMethod]
        public void EnsureTemplatesCanBeConvertedToObjects()
        {
            var ownerid = "023D";
            var furnitureid = "ogiaes";
            var haspets = true;
            var input = "{\"template\" : {\"data\" : [{\"name\" : \"OwnerId\", \"value\" : \"" + ownerid +
                        "\"},{\"name\" : \"FurnitureId\", \"value\" : \"" + furnitureid +
                        "\"},{\"name\" : \"HasPets\", \"value\" : \"" + haspets + "\"}]}}";
            CollectionJsonTemplating<HouseTemplate> template = new CollectionJsonTemplating<HouseTemplate>();
            HouseTemplate usgt = template.CreateTemplate(input);
            Assert.AreEqual(ownerid, usgt.OwnerId);
            Assert.AreEqual(furnitureid, usgt.FurnitureId);
            Assert.AreEqual(haspets, usgt.HasPets);
        }

        [TestMethod]
        public void EnsureHydrateInstanceFromJsonWorks()
        {
            var ownerid = "023D";
            var furnitureid = "ogiaes";
            var haspets = false;
            var input = "{\"template\" : {\"data\" : [{\"name\" : \"OwnerId\", \"value\" : \"" + ownerid +
                        "\"},{\"name\" : \"FurnitureId\", \"value\" : \"" + furnitureid + "\"}]}}";
            CollectionJsonTemplating<HouseTemplate> template = new CollectionJsonTemplating<HouseTemplate>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("HasPets", haspets.ToString());
            HouseTemplate usgt = template.HydrateInstance(input, dictionary, new HouseTemplate());

            Assert.AreEqual(ownerid, usgt.OwnerId);
            Assert.AreEqual(furnitureid, usgt.FurnitureId);
            Assert.AreEqual(haspets, usgt.HasPets);
        }

        [TestMethod]
        [ExpectedException(typeof (PropertyNotFoundHydrationException))]
        public void EnsureHydrateInstanceFromJsonThrowsExceptionOnBadProperty()
        {
            var ownerid = "023D";
            var furnitureid = "ogiaes";

            var input = "{\"template\" : {\"data\" : [{\"name\" : \"ownerid\", \"value\" : \"" + ownerid +
                        "\"},{\"name\" : \"FurnitureId\", \"value\" : \"" + furnitureid +
                        "\"},{\"name\" : \"extra\", \"value\" : \"something\"}]}}";
            CollectionJsonTemplating<HouseTemplate> template = new CollectionJsonTemplating<HouseTemplate>();

            HouseTemplate usgt = template.HydrateInstance(input, new HouseTemplate());
        }

        [TestMethod]
        [ExpectedException(typeof (PropertyNotFoundHydrationException))]
        public void EnsureHydrateInstanceFromJsonThrowsExceptionOnBadProperty2()
        {
            var ownerid = "023D";
            var furnitureid = "ogiaes";
            var haspets = true;;
            var input = "{\"template\" : {\"data\" : [{\"name\" : \"ownerid\", \"value\" : \"" + ownerid +
                        "\"},{\"name\" : \"FurnitureId\", \"value\" : \"" + furnitureid + "\"}]}}";
            CollectionJsonTemplating<HouseTemplate> template = new CollectionJsonTemplating<HouseTemplate>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("misspelledpropname", haspets.ToString());
            HouseTemplate usgt = template.HydrateInstance(input, dictionary, new HouseTemplate());
        }

        [TestMethod]
        public void EnsureHydrateInstanceFromJsonHandlesTypeConversion()
        {
            var input = jsonInput;
            CollectionJsonTemplating<DummyClass> template = new CollectionJsonTemplating<DummyClass>();
            DummyClass dummy = template.HydrateInstance(input, new DummyClass());

            Assert.AreEqual(dummy.A, 23);
            Assert.AreEqual(dummy.B, true);
            Assert.AreEqual(dummy.C, "ABC");
            Assert.AreEqual(dummy.D, 264.342f);
            Assert.AreEqual(dummy.E, new Guid("47E31D14-3B91-466B-B262-CD593C614A66"));
            Assert.AreEqual(dummy.F, 234123.2342312312343M);
            Assert.AreEqual(dummy.G, 123.23f);

            var dictionary = new Dictionary<string, string>()
                {
                    {"A", "23"},
                    {"B", "true"},
                    {"C", "ABC"},
                    {"D", "264.342"},
                    {"E", "47E31D14-3B91-466B-B262-CD593C614A66"},
                    {"F", "234123.2342312312343"},
                    {"G", "123.23"}
                };

            var emptyTemplate =
                @"{template : { data:[]}}";

            dummy = template.HydrateInstance(emptyTemplate, dictionary, new DummyClass());

            Assert.AreEqual(dummy.A, 23);
            Assert.AreEqual(dummy.B, true);
            Assert.AreEqual(dummy.C, "ABC");
            Assert.AreEqual(dummy.D, 264.342f);
            Assert.AreEqual(dummy.E, new Guid("47E31D14-3B91-466B-B262-CD593C614A66"));
            Assert.AreEqual(dummy.F, 234123.2342312312343M);
            Assert.AreEqual(dummy.G, 123.23f);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void EnsureHydrateInstanceHandlesNullString()
        {
            string input = null;
            CollectionJsonTemplating<DummyClass> template = new CollectionJsonTemplating<DummyClass>();
            DummyClass dummy = template.HydrateInstance(input, new DummyClass());
        }

        [TestMethod]
        public void HydrateInstanceCanTakeANullIfAdictionaryIsSupplied()
        {
            string input = null;
            CollectionJsonTemplating<DummyClass> template = new CollectionJsonTemplating<DummyClass>();
            DummyClass dummy = template.HydrateInstance(input, new Dictionary<string, string>(), new DummyClass());
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void EnsureHydrateInstanceHandlesEmptyString()
        {
            string input = string.Empty;
            CollectionJsonTemplating<DummyClass> template = new CollectionJsonTemplating<DummyClass>();
            DummyClass dummy = template.HydrateInstance(input, new DummyClass());
        }

        [TestMethod]
        public void HydrateInstanceCanTakeAnEmptyStringifAdictionaryIsSupplied()
        {
            string input = string.Empty;
            CollectionJsonTemplating<DummyClass> template = new CollectionJsonTemplating<DummyClass>();
            DummyClass dummy = template.HydrateInstance(input, new Dictionary<string, string>(), new DummyClass());
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void EnsureHydrateInstanceHandlesNullDictionary()
        {
            string input = jsonInput;
            CollectionJsonTemplating<DummyClass> template = new CollectionJsonTemplating<DummyClass>();
            IDictionary<string, string> meh = null;
            DummyClass dummy = template.HydrateInstance(input, meh, new DummyClass());
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void EnsureHydrateInstanceHandlesNullBaseTemplate()
        {
            string input = jsonInput;
            CollectionJsonTemplating<DummyClass> template = new CollectionJsonTemplating<DummyClass>();
            DummyClass dummy = template.HydrateInstance(input, null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void EnsureHydrateInstanceHandlesNullBaseTemplate2()
        {
            string input = jsonInput;
            CollectionJsonTemplating<DummyClass> template = new CollectionJsonTemplating<DummyClass>();
            DummyClass dummy = template.HydrateInstance(input, new Dictionary<string, string>(), null);
        }

        public class DummyClass
        {
            public int A { get; set; }
            public bool B { get; set; }
            public string C { get; set; }
            public Single D { get; set; }
            public Guid E { get; set; }
            public Decimal F { get; set; }
            public Single? G { get; set; }
            public EventArgs H { get; set; } // just need a non-scalar property
        }
    }
}