using System.Runtime.Serialization;

namespace WoWClient
{
    public enum Locale
    {
        [EnumMember(Value = "de_DE")] DeDe,
        [EnumMember(Value = "en_US")] EnUs,
        [EnumMember(Value = "es_MX")] EsMx,
        [EnumMember(Value = "pt_BR")] PtBr,
        [EnumMember(Value = "es_ES")] EsEs,
        [EnumMember(Value = "fr_FR")] FrFr,
        [EnumMember(Value = "en_GB")] EnGb,
        [EnumMember(Value = "ru_RU")] RuRu,
        [EnumMember(Value = "pt_PT")] PtPt,
        [EnumMember(Value = "it_IT")] ItIt,
        [EnumMember(Value = "ko_KR")] KoKr,
        [EnumMember(Value = "zh_TW")] ZhTw,
        [EnumMember(Value = "zh_CN")] ZhCn
    }
}