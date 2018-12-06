using System.Runtime.Serialization;

namespace WoWClient.Entities.Enums
{
    public enum PowerTypes
    {
        [EnumMember(Value = "focus")] Focus,
        [EnumMember(Value = "rage")] Rage,
        [EnumMember(Value = "mana")] Mana,
        [EnumMember(Value = "energy")] Energy,
        [EnumMember(Value = "runic-power")] Runicpower,
        [EnumMember(Value = "fury")] Fury
    }
}