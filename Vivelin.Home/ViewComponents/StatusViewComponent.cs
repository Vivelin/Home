using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Vivelin.Home.ViewComponents
{
    public class StatusViewComponent : ViewComponent
    {
        public StatusViewComponent(IConfiguration configuration)
        {
            var apiKey = configuration["LastfmApiKey"];
            var secret = configuration["LastfmSharedSecret"];
            UserName = configuration["LastfmUser"];
            Client = new LastfmClient(apiKey, secret);
        }

        protected string UserName { get; }

        protected LastfmClient Client { get; }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var recentTracks = await Client.User.GetRecentScrobbles(UserName, count: 1);
            var track = recentTracks.FirstOrDefault(x => x.IsNowPlaying == true);
            if (track != null)
                return View("Lastfm", track);

            return View();
        }
    }
}
