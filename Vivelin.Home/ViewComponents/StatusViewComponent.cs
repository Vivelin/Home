using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using Vivelin.Home.Data;
using Vivelin.Home.Twitch;

namespace Vivelin.Home.ViewComponents
{
    public class StatusViewComponent : ViewComponent
    {
        private static readonly HttpClient s_httpClient = new HttpClient();
        private readonly HomeContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;

        public StatusViewComponent(HomeContext dbContext, IConfiguration configuration, IMemoryCache memoryCache, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _logger = loggerFactory.CreateLogger<StatusViewComponent>();

            LastfmUserName = configuration["LastfmUser"];
            SteamId = configuration.GetValue<ulong>("SteamID");
            TwitchUserId = configuration.GetValue<ulong>("TwitchUserId");
            TwitchClientId = configuration["TwitchClientId"];

            var lastfmApiKey = configuration["LastfmApiKey"];
            var lastfmSecret = configuration["LastfmSharedSecret"];
            if (lastfmApiKey != null && lastfmSecret != null)
                Client = new LastfmClient(lastfmApiKey, lastfmSecret, s_httpClient);

            var steamWebApiKey = configuration["SteamWebApiKey"];
            if (steamWebApiKey != null)
                SteamFactory = new SteamWebInterfaceFactory(steamWebApiKey);
        }

        protected string LastfmUserName { get; }

        protected ulong SteamId { get; }

        protected ulong TwitchUserId { get; }

        protected string TwitchClientId { get; }

        protected LastfmClient Client { get; }

        protected SteamWebInterfaceFactory SteamFactory { get; }

        public async Task<IViewComponentResult> InvokeAsync(bool cacheOnly = false)
        {
            var streamStatus = await GetStreamAsync(cacheOnly);
            if (streamStatus != null)
                return View("Twitch", streamStatus);

            var steamStatus = await GetSteamStatusAsync(cacheOnly);
            if (steamStatus?.PlayingGameName != null)
                return View("Steam", steamStatus);

            var track = await GetNowPlayingAsync(cacheOnly);
            if (track != null)
                return View("Lastfm", track);

            var tagline = _dbContext.Taglines.Sample();
            return View(tagline);
        }

        private async Task<TwitchStream> GetStreamAsync(bool cacheOnly = false)
        {
            const string CacheKey = "TwitchStream";

            try
            {
                if (cacheOnly)
                    return _memoryCache.Get<TwitchStream>(CacheKey);

                return await _memoryCache.GetOrCreateAsync(CacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);

                    var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.twitch.tv/kraken/streams/{TwitchUserId}?stream_type=live");
                    request.Headers.Accept.Clear();
                    request.Headers.Accept.ParseAdd("application/vnd.twitchtv.v5+json");
                    request.Headers.Add("Client-ID", TwitchClientId);

                    var response = await s_httpClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                    var twitchResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TwitchStreamResponse>(content);
                    twitchResponse?.ThrowOnError();
                    return twitchResponse?.Stream;
                });
            }
            catch (TwitchException ex)
            {
                _logger.LogError("There was a problem getting the current stream status from Twitch. {0}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("There was a problem getting the current stream status from Twitch. Details: {0}", ex);
                return null;
            }
        }

        private async Task<LastTrack> GetNowPlayingAsync(bool cacheOnly = false)
        {
            if (Client == null)
                return null;

            const string CacheKey = "LastfmNowPlaying";

            try
            {
                if (cacheOnly)
                    return _memoryCache.Get<LastTrack>(CacheKey);

                return await _memoryCache.GetOrCreateAsync(CacheKey, async entry =>
                {
                    // "You will not make more than 5 requests per originating IP
                    // address per second, averaged over a 5 minute period"
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);

                    var recentTracks = await Client.User.GetRecentScrobbles(LastfmUserName, count: 1);
                    return recentTracks?.FirstOrDefault(x => x.IsNowPlaying == true);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("There was a problem getting the currently playing track from Last.fm. Details: {0}", ex);
                return null;
            }
        }

        private async Task<PlayerSummaryModel> GetSteamStatusAsync(bool cacheOnly = false)
        {
            if (SteamFactory == null)
                return null;

            const string CacheKey = "SteamStatus";
            try
            {
                if (cacheOnly) return _memoryCache.Get<PlayerSummaryModel>(CacheKey);

                return await _memoryCache.GetOrCreateAsync(CacheKey, async entry =>
                {
                    // "You are limited to one hundred thousand (100,000) calls to
                    // the Steam Web API per day. Valve may approve higher daily
                    // call limits if you adhere to these API Terms of Use."
                    //
                    // 100000/day ≈ 69/min ≈ 1.1/s
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);

                    var steamUser = SteamFactory.CreateSteamWebInterface<SteamUser>(s_httpClient);
                    var response = await steamUser.GetPlayerSummaryAsync(SteamId);
                    return response?.Data;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("There was a problem getting the status from Steam. Details: {0}", ex);
                return null;
            }
        }
    }
}