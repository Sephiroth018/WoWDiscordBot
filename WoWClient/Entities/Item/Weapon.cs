using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class Weapon : Item
    {
        [JsonProperty("weaponInfo")]
        public WeaponInfo WeaponInfo { get; set; }
    }
}