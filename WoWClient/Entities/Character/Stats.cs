using Newtonsoft.Json;
using WoWClient.Converters;

namespace WoWClient.Entities.Character
{
    public class Stats
    {
        [JsonProperty("health")]
        public long Health { get; set; }

        [JsonProperty("powerType")]
        public string PowerType { get; set; }

        [JsonProperty("power")]
        public long Power { get; set; }

        [JsonProperty("str")]
        public long Str { get; set; }

        [JsonProperty("agi")]
        public long Agi { get; set; }

        [JsonProperty("int")]
        public long Int { get; set; }

        [JsonProperty("sta")]
        public long Sta { get; set; }

        [JsonProperty("speedRating")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? SpeedRating { get; set; }

        [JsonProperty("speedRatingBonus")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? SpeedRatingBonus { get; set; }

        [JsonProperty("crit")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Crit { get; set; }

        [JsonProperty("critRating")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? CritRating { get; set; }

        [JsonProperty("haste")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Haste { get; set; }

        [JsonProperty("hasteRating")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? HasteRating { get; set; }

        [JsonProperty("hasteRatingPercent")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? HasteRatingPercent { get; set; }

        [JsonProperty("mastery")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Mastery { get; set; }

        [JsonProperty("masteryRating")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? MasteryRating { get; set; }

        [JsonProperty("leech")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Leech { get; set; }

        [JsonProperty("leechRating")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? LeechRating { get; set; }

        [JsonProperty("leechRatingBonus")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? LeechRatingBonus { get; set; }

        [JsonProperty("versatility")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Versatility { get; set; }

        [JsonProperty("versatilityDamageDoneBonus")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? VersatilityDamageDoneBonus { get; set; }

        [JsonProperty("versatilityHealingDoneBonus")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? VersatilityHealingDoneBonus { get; set; }

        [JsonProperty("versatilityDamageTakenBonus")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? VersatilityDamageTakenBonus { get; set; }

        [JsonProperty("avoidanceRating")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? AvoidanceRating { get; set; }

        [JsonProperty("avoidanceRatingBonus")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? AvoidanceRatingBonus { get; set; }

        [JsonProperty("spellPen")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? SpellPen { get; set; }

        [JsonProperty("spellCrit")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? SpellCrit { get; set; }

        [JsonProperty("spellCritRating")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? SpellCritRating { get; set; }

        [JsonProperty("mana5")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Mana5 { get; set; }

        [JsonProperty("mana5Combat")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Mana5Combat { get; set; }

        [JsonProperty("armor")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Armor { get; set; }

        [JsonProperty("dodge")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Dodge { get; set; }

        [JsonProperty("dodgeRating")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? DodgeRating { get; set; }

        [JsonProperty("parry")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Parry { get; set; }

        [JsonProperty("parryRating")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? ParryRating { get; set; }

        [JsonProperty("block")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Block { get; set; }

        [JsonProperty("blockRating")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? BlockRating { get; set; }

        [JsonProperty("mainHandDmgMin")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? MainHandDmgMin { get; set; }

        [JsonProperty("mainHandDmgMax")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? MainHandDmgMax { get; set; }

        [JsonProperty("mainHandSpeed")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? MainHandSpeed { get; set; }

        [JsonProperty("mainHandDps")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? MainHandDps { get; set; }

        [JsonProperty("offHandDmgMin")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? OffHandDmgMin { get; set; }

        [JsonProperty("offHandDmgMax")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? OffHandDmgMax { get; set; }

        [JsonProperty("offHandSpeed")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? OffHandSpeed { get; set; }

        [JsonProperty("offHandDps")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? OffHandDps { get; set; }

        [JsonProperty("rangedDmgMin")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? RangedDmgMin { get; set; }

        [JsonProperty("rangedDmgMax")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? RangedDmgMax { get; set; }

        [JsonProperty("rangedSpeed")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? RangedSpeed { get; set; }

        [JsonProperty("rangedDps")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? RangedDps { get; set; }
    }
}