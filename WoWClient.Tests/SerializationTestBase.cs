using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JsonDiffPatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WoWClient.Entities.Character;

namespace WoWClient.Tests
{
    public abstract class SerializationTestBase : IEnumerable<TestCaseData>
    {
        public IEnumerator<TestCaseData> GetEnumerator()
        {
            var currentNamespace = GetType().Namespace;
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceNames()
                           .Where(resource => resource.Replace($"{currentNamespace}.TestData.", "").StartsWith(GetType().Namespace.Split('.').Last()))
                           .Select(resource =>
                                   {
                                       using (var stream = assembly.GetManifestResourceStream(resource))
                                       {
                                           using (var streamReader = new StreamReader(stream))
                                           {
                                               return new TestCaseData(streamReader.ReadToEnd()) { TestName = resource };
                                           }
                                       }
                                   })
                           .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void TestSerialization(string expectedJson)
        {
            var actualObject = JsonConvert.DeserializeObject<Character>(expectedJson);
            var actualJson = JsonConvert.SerializeObject(actualObject, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var expected = JsonConvert.DeserializeObject(expectedJson) as JObject;
            var actual = JsonConvert.DeserializeObject(actualJson) as JObject;
            var diff = new JsonDiffer().Diff(expected, actual, true);

            Assert.That(!diff.Operations.Any(), diff.ToString());
        }
    }
}