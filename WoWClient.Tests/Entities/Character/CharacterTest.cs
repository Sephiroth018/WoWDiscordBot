using NUnit.Framework;

namespace WoWClient.Tests.Entities.Character
{
    [TestFixture]
    public class CharacterTest : SerializationTestBase
    {
        [Test]
        [TestCaseSource(typeof(CharacterTest))]
        public void Test(string json)
        {
            TestSerialization(json);
        }
    }
}