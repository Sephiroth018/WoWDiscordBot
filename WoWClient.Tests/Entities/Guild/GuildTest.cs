using NUnit.Framework;

namespace WoWClient.Tests.Entities.Guild
{
    [TestFixture]
    public class GuildTest : SerializationTestBase
    {
        [Test]
        [TestCaseSource(typeof(GuildTest))]
        public void Test(string json)
        {
            TestSerialization(json);
        }
    }
}