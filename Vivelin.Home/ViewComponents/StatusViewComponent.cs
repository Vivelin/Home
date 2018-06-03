using System;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using Vivelin.Home.Data;

namespace Vivelin.Home.ViewComponents
{
    public class StatusViewComponent : ViewComponent
    {
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

        protected LastfmClient Client { get; }

        protected SteamUser SteamUser { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var steamStatus = await GetSteamStatusAsync();
            if (steamStatus?.PlayingGameName != null)
                return View("Steam", steamStatus);

            var track = await GetNowPlayingAsync();
            if (track != null)
                return View("Lastfm", track);

            var tagline = dbContext.Taglines.Sample();
            return View(tagline);
        }

        private Task<IF.Lastfm.Core.Objects.LastTrack> GetNowPlayingAsync()
        {
            try
            {
                return memoryCache.GetOrCreateAsync("LastfmNowPlaying", async entry =>
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

        private Task<PlayerSummaryModel> GetSteamStatusAsync()
        {
            try
            {
                return memoryCache.GetOrCreateAsync("SteamStatus", async entry =>
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