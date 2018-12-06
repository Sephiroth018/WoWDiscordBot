using System.Runtime.Serialization;

namespace WoWClient.Entities.Enums
{
    public enum FeedType
    {
        [EnumMember(Value = "ACHIEVEMENT")] Achievement,
        [EnumMember(Value = "BOSSKILL")] Bosskill,
        [EnumMember(Value = "CRITERIA")] Criteria,
        [EnumMember(Value = "LOOT")] Loot
    }
}