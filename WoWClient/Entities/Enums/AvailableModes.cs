using System.Runtime.Serialization;

namespace WoWClient.Entities.Enums
{
    public enum AvailableModes
    {
        [EnumMember(Value = "DUNGEON_NORMAL")] DungeonNormal,
        [EnumMember(Value = "DUNGEON_HEROIC")] DungeonHeroic,
        [EnumMember(Value = "RAID_LFR")] RaidLfr,
        [EnumMember(Value = "RAID_10_NORMAL")] Raid10Normal,
        [EnumMember(Value = "RAID_25_NORMAL")] Raid25Normal,
        [EnumMember(Value = "RAID_10_HEROIC")] Raid10Heroic,
        [EnumMember(Value = "RAID_25_HEROIC")] Raid25Heroic,
        [EnumMember(Value = "RAID_FLEX_LFR")] RaidFlexLfr,

        [EnumMember(Value = "RAID_FLEX_NORMAL")]
        RaidFlexNormal,

        [EnumMember(Value = "RAID_FLEX_HEROIC")]
        RaidFlexHeroic,
        [EnumMember(Value = "RAID_MYTHIC")] RaidMythic,
        [EnumMember(Value = "DUNGEON_MYTHIC")] DungeonMythic
    }
}