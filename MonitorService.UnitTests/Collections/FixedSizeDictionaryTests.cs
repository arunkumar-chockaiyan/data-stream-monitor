using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorService.Collections;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorService.Collections.Tests
{
    [TestClass()]
    public class FixedSizeDictionaryTests
    {

        private FixedSizeDictionary<string, string> ValidFixedSizeDictionaryStub()
        {
            return new FixedSizeDictionary<string, string>(10, (value) => value);
        }

        [TestMethod()]
        public void FixedSizeDictionary_invalid_input1_Test()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() => new FixedSizeDictionary<int, string>(10, null));
        }

        [TestMethod()]
        public void FixedSizeDictionary_invalid_input2_Test()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => new FixedSizeDictionary<int, string>(-10, (_) => 0));
        }

        [TestMethod()]
        public void FixedSizeDictionary_valid_input_Test()
        {
            var fsd = new FixedSizeDictionary<int, string>(10, (_) => 0);
            Assert.IsNotNull(fsd);
            Assert.IsNotNull(fsd.getReadOnlyDictionary());
        }

        [TestMethod()]
        public void TryGetValue_null_key_Test()
        {
            var fsd = ValidFixedSizeDictionaryStub();

            string? value;
            _ = Assert.ThrowsException<ArgumentNullException>(() => fsd.TryGetValue(null, out value));

        }

        [TestMethod()]
        public void TryGetValue_missing_key_Test()
        {
            var fsd = ValidFixedSizeDictionaryStub();

            string? value;
            bool result = fsd.TryGetValue("test", out value);

            Assert.IsFalse(result);
            Assert.IsNull(value);
        }

        [TestMethod()]
        public void TryGetValue_valid_key_Test()
        {
            string inputValue = "test";
            var fsd = ValidFixedSizeDictionaryStub();
            fsd.TryAdd(inputValue);

            string? value;
            bool result = fsd.TryGetValue(inputValue, out value);

            Assert.IsTrue(result);
            Assert.AreEqual(inputValue, value);
        }

        [TestMethod()]
        public void Values_empty_Test()
        {
            var fsd = ValidFixedSizeDictionaryStub();

            ICollection<string> values = fsd.Values();

            Assert.IsNotNull(values);
            Assert.AreEqual(0, values.Count);   
            Assert.ThrowsException<NotSupportedException>(() => values.Add("item"));
        }

        [TestMethod()]
        public void Values_Test()
        {
            string inputValue = "test";
            var fsd = ValidFixedSizeDictionaryStub();
            fsd.TryAdd(inputValue);

            var values = fsd.Values();

            Assert.IsNotNull(values);
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(inputValue, values.ElementAt(0));
        }


        [TestMethod()]
        public void getReadOnlyDictionary_empty_Test()
        {
            var fsd = ValidFixedSizeDictionaryStub();

            var dictionary = fsd.getReadOnlyDictionary();

            Assert.IsNotNull(dictionary);
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod()]
        public void getReadOnlyDictionary_Test()
        {
            string inputValue = "test";
            var fsd = ValidFixedSizeDictionaryStub();
            fsd.TryAdd(inputValue);

            var dictionary = fsd.getReadOnlyDictionary();

            Assert.IsNotNull(dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual(inputValue, dictionary.ElementAt(0).Value);
        }

        [TestMethod()]
        public void TryAddByKey_key_null_Test()
        {
            var fsd = ValidFixedSizeDictionaryStub();

            _ = Assert.ThrowsException<ArgumentNullException>(() => fsd.TryAdd(null, (value) => value));
        }


        [TestMethod()]
        public void TryAddByKey_valueFactory_null_Test()
        {
            var fsd = ValidFixedSizeDictionaryStub();

            _ = Assert.ThrowsException<ArgumentNullException>(() => fsd.TryAdd("test", null));
        }

        [TestMethod()]
        public void TryAddByKey_above_maxSize_Test()
        {
            var fsd = new FixedSizeDictionary<string, string>(1, (value) => value);
            fsd.TryAdd("item1");

            bool result = fsd.TryAdd("item2", (value) => value);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void TryAddByKey_valid_Test()
        {
            var fsd = ValidFixedSizeDictionaryStub();
            string key = "item1";
            string value = "value:" + key;

            bool result = fsd.TryAdd(key, (key) => "value:" + key);
            Assert.IsTrue(result);
            Assert.AreEqual(value, fsd.Values().ElementAt(0));
        }

        [TestMethod()]
        public void TryAddByValue_null_Test()
        {
            var fsd = ValidFixedSizeDictionaryStub();

            _ = Assert.ThrowsException<ArgumentNullException>(() => fsd.TryAdd(null));
        }

        [TestMethod()]
        public void TryAddByValue_above_maxSize_Test()
        {
            var fsd = new FixedSizeDictionary<string, string>(1, (value) => value);
            fsd.TryAdd("item1");

            bool result = fsd.TryAdd("item2");
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void TryAddByValue_valid_Test()
        {
            var fsd = ValidFixedSizeDictionaryStub();
            string value = "item1";

            bool result = fsd.TryAdd(value);
            Assert.IsTrue(result);
            Assert.AreEqual(value, fsd.Values().ElementAt(0));
        }

        [TestMethod()]
        public void TryRemove_key_null_Test()
        {
            var fsd = ValidFixedSizeDictionaryStub();

            string? value;
            _ = Assert.ThrowsException<ArgumentNullException>(() => fsd.TryRemove(null, out value));
        }

        [TestMethod()]
        [DataRow("item1", true)]
        [DataRow("randomItem", false)]
        public void TryRemove_Test(string item, bool expectedResult)
        {
            var fsd = ValidFixedSizeDictionaryStub();
            fsd.TryAdd("item1");

            string? value;
            bool result = fsd.TryRemove(item, out value);

            Assert.AreEqual(expectedResult, result);
        }

    }
}