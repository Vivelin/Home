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
using Vivelin.Home.Data;
using Vivelin.Home.Twitch;

namespace Vivelin.Home.ViewComponents
{
    public class StatusViewComponent : ViewComponent
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly HomeContext dbContext;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger logger;

        public StatusViewComponent(HomeContext dbContext, IConfiguration configuration, IMemoryCache memoryCache, ILoggerFactory loggerFactory)
        {
            this.dbContext = dbContext;
            this.memoryCache = memoryCache;
            this.logger = loggerFactory.CreateLogger<StatusViewComponent>();

            LastfmUserName = configuration["LastfmUser"];
            SteamId = configuration.GetValue<ulong>("SteamID");
            TwitchUserId = configuration.GetValue<ulong>("TwitchUserId");
            TwitchClientId = configuration["TwitchClientId"];

            var lastfmApiKey = configuration["LastfmApiKey"];
            var lastfmSecret = configuration["LastfmSharedSecret"];
            if (lastfmApiKey != null && lastfmSecret != null)
                Client = new LastfmClient(lastfmApiKey, lastfmSecret);

            var steamWebApiKey = configuration["SteamWebApiKey"];
            if (steamWebApiKey != null)
                SteamUser = new SteamUser(steamWebApiKey);
        }

        protected string LastfmUserName { get; }

        protected ulong SteamId { get; }

        protected ulong TwitchUserId { get; }

        protected string TwitchClientId { get; }

        protected LastfmClient Client { get; }

        protected SteamUser SteamUser { get; }

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

            var tagline = dbContext.Taglines.Sample();
            return View(tagline);
        }

        private async Task<TwitchStream> GetStreamAsync(bool cacheOnly = false)
        {
            const string cacheKey = "TwitchStream";

            try
            {
                if (cacheOnly)
                    return memoryCache.Get<TwitchStream>(cacheKey);

                return await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);

                    var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.twitch.tv/kraken/streams/{TwitchUserId}?stream_type=live");
                    request.Headers.Accept.Clear();
                    request.Headers.Accept.ParseAdd("application/vnd.twitchtv.v5+json");
                    request.Headers.Add("Client-ID", TwitchClientId);

                    var response = await httpClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                    var twitchResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TwitchStreamResponse>(content);
                    twitchResponse?.ThrowOnError();
                    return twitchResponse?.Stream;
                });
            }
            catch (TwitchException ex)
            {
                logger.LogError("There was a problem getting the current stream status from Twitch. {0}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError("There was a problem getting the current stream status from Twitch. Details: {0}", ex);
                return null;
            }
        }

        private async Task<LastTrack> GetNowPlayingAsync(bool cacheOnly = false)
        {
            const string cacheKey = "LastfmNowPlaying";

            try
            {
                if (cacheOnly)
                    return memoryCache.Get<LastTrack>(cacheKey);

                return await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    // "You will not make more than 5 requests per originating IP
                    // address per second, averaged over a 5 minute period"
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);

                    var recentTracks = await Client?.User.GetRecentScrobbles(LastfmUserName, count: 1);
                    return recentTracks?.FirstOrDefault(x => x.IsNowPlaying == true);
                });
            }
            catch (Exception ex)
            {
                logger.LogError("There was a problem getting the currently playing track from Last.fm. Details: {0}", ex);
                return null;
            }
        }

        private async Task<PlayerSummaryModel> GetSteamStatusAsync(bool cacheOnly = false)
        {
            const string cacheKey = "SteamStatus";
            try
            {
                if (cacheOnly) return memoryCache.Get<PlayerSummaryModel>(cacheKey);

                return await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    // "You are limited to one hundred thousand (100,000) calls to
                    // the Steam Web API per day. Valve may approve higher daily
                    // call limits if you adhere to these API Terms of Use."
                    //
                    // 100000/day ≈ 69/min ≈ 1.1/s
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);

                    var response = await SteamUser?.GetPlayerSummaryAsync(SteamId);
                    return response?.Data;
                });
            }
            catch (Exception ex)
            {
                logger.LogError("There was a problem getting the status from Steam. Details: {0}", ex);
                return null;
            }
        }
    }
}