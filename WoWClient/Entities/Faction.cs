using System.Runtime.Serialization;

namespace WoWClient.Entities
{
    public enum Faction
    {
        [EnumMember(Value = "alliance")] Alliance = 0,
        [EnumMember(Value = "horde")] Horde = 1
    }
}