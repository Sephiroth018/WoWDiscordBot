using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using WoWClient.Entities;
using Character = WoWClient.Entities.Character.Character;
using Guild = WoWClient.Entities.Guild.Guild;

namespace WoWClient
{
    public class ApiClient : IApiClient
    {
        private static readonly Dictionary<Region, List<Locale>> RegionLocaleRestrictions = new Dictionary<Region, List<Locale>>
                                                                                            {
                                                                                                {
                                                                                                    Region.Us, new List<Locale>
                                                                                                               {
                                                                                                                   Locale.EnUs,
                                                                                                                   Locale.EsMx,
                                                                                                                   Locale.PtBr
                                                                                                               }
                                                                                                },
                                                                                                {
                                                                                                    Region.Eu, new List<Locale>
                                                                                                               {
                                                                                                                   Locale.EnGb,
                                                                                                                   Locale.DeDe,
                                                                                                                   Locale.EsEs,
                                                                                                                   Locale.FrFr,
                                                                                                                   Locale.ItIt,
                                                                                                                   Locale.PtPt,
                                                                                                                   Locale.RuRu
                                                                                                               }
                                                                                                },
                                                                                                {
                                                                                                    Region.Kr, new List<Locale>
                                                                                                               {
                                                                                                                   Locale.KoKr
                                                                                                               }
                                                                                                },
                                                                                                {
                                                                                                    Region.Tw, new List<Locale>
                                                                                                               {
                                                                                                                   Locale.ZhTw
                                                                                                               }
                                                                                                },
                                                                                                {
                                                                                                    Region.Cn, new List<Locale>
                                                                                                               {
                                                                                                                   Locale.ZhCn
                                                                                                               }
                                                                                                }
                                                                                            };

        private readonly string _baseUrl;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Locale _locale;
        private readonly Region _region;
        private readonly string _tokenUrl;

        public ApiClient(Region region, Locale locale, string clientId, string clientSecret)
        {
            _region = region;
            _locale = locale;
            _clientId = clientId;
            _clientSecret = clientSecret;

            ValidateLocale(_locale);

            switch (region)
            {
                case Region.Eu:
                case Region.Us:
                case Region.Kr:
                case Region.Tw:
                    _baseUrl = $"https://{region}.api.battle.net/";
                    _tokenUrl = $"https://{region}.battle.net/oauth/token";
                    break;
                case Region.Cn:
                    _baseUrl = "https://api.battlenet.com.cn/";
                    _tokenUrl = "https://www.battlenet.com.cn/oauth/token";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(region), region, null);
            }
        }

        #region OAuth endpoints

        public async Task<MythicChallengeMode> GetMythicChallengeModeIndex(Locale? locale = null)
        {
            ValidateLocale(locale ?? _locale);
            return await GetEndpoint("data/wow/mythic-challenge-mode/")
                         .SetQueryParam("namespace", $"dynamic-{_region}")
                         .SetQueryParam("locale", $"locale={locale ?? _locale}")
                         .WithOAuthBearerToken(await GetOAuthToken())
                         .GetJsonAsync<MythicChallengeMode>();
        }

        #endregion

        private async Task<T> DoApiKeyRequest<T>(Locale? locale, Url endpoint)
        {
            ValidateLocale(locale ?? _locale);
            return await $"{_baseUrl}{endpoint}"
                         .SetQueryParam("locale", locale ?? _locale)
                         .SetQueryParam("apikey", _clientId)
                         .GetJsonAsync<T>();
        }

        private Url GetEndpoint(string endpoint)
        {
            return $"{_baseUrl}{endpoint}";
        }

        private void ValidateLocale(Locale locale)
        {
            if (!RegionLocaleRestrictions[_region].Contains(locale))
                throw new ArgumentOutOfRangeException(nameof(locale), $"Locale {locale} not supported for region {_region}.");
        }

        private async Task<string> GetOAuthToken()
        {
            var result = await _tokenUrl.SetQueryParam("grant_type", "client_credentials")
                                        .SetQueryParam("client_id", _clientId)
                                        .SetQueryParam("client_secret", _clientSecret)
                                        .GetJsonAsync();
            return result.access_token;
        }

        #region ApiKey endpoints

        public async Task<ZoneData> GetZones(Locale? locale = null)
        {
            return await DoApiKeyRequest<ZoneData>(locale, GetEndpoint("wow/zone/"));
        }

        public async Task<Character> GetCharacter(string realm, string name, Locale? locale = null)
        {
            var endpoint = GetEndpoint($"wow/character/{realm}/{name}")
                .SetQueryParam("fields", "achievements+appearance+feed+guild+hunterPets+items+mounts+pets+petSlots+professions+progression+pvp+quests+reputation+statistics+stats+talents+titles+audit");
            return await DoApiKeyRequest<Character>(locale, endpoint);
        }

        public async Task<Guild> GetGuild(string realm, string name, Locale? locale = null)
        {
            var endpoint = GetEndpoint($"wow/guild/{realm}/{name}")
                .SetQueryParam("fields", "achievements+challenge+members+news");
            return await DoApiKeyRequest<Guild>(locale, endpoint);
        }

        public async Task<CharacterClassData> GetCharacterClasses(Locale? locale = null)
        {
            return await DoApiKeyRequest<CharacterClassData>(locale, GetEndpoint("wow/data/character/classes"));
        }

        #endregion
    }
}