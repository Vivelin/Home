using System;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Vivelin.Home.ViewComponents
{
    public class StatusViewComponent : ViewComponent
    {
        private readonly IMemoryCache memoryCache;

        public StatusViewComponent(IConfiguration configuration, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;

            var apiKey = configuration["LastfmApiKey"];
            var secret = configuration["LastfmSharedSecret"];
            UserName = configuration["LastfmUser"];
            Client = new LastfmClient(apiKey, secret);
        }

        protected string UserName { get; }

        protected LastfmClient Client { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var track = await GetNowPlayingAsync();
            if (track != null)
                return View("Lastfm", track);

            return View();
        }

        private Task<IF.Lastfm.Core.Objects.LastTrack> GetNowPlayingAsync()
        {
            return memoryCache.GetOrCreateAsync("LastfmNowPlaying", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);

                var recentTracks = await Client.User.GetRecentScrobbles(UserName, count: 1);
                return recentTracks.FirstOrDefault(x => x.IsNowPlaying == true);
            });
        }
    }
}