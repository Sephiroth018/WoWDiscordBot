using System.Runtime.Serialization;

namespace WoWClient.Entities.Enums
{
    public enum Role
    {
        [EnumMember(Value = "DPS")] Dps,
        [EnumMember(Value = "HEALING")] Healing,
        [EnumMember(Value = "TANK")] Tank
    }
}