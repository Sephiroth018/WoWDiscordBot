using System.Runtime.Serialization;

namespace WoWClient
{
    public enum Region
    {
        [EnumMember(Value = "EU")] Eu,
        [EnumMember(Value = "US")] Us,
        [EnumMember(Value = "KR")] Kr,
        [EnumMember(Value = "TW")] Tw,
        [EnumMember(Value = "CN")] Cn
    }
}