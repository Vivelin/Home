using System;
using System.Runtime.Serialization;

namespace Vivelin.Home.Twitch
{
    [DataContract]
    public class TwitchPreview
    {
        [DataMember(Name = "small")]
        public Uri Small { get; set; }

        [DataMember(Name = "medium")]
        public Uri Medium { get; set; }

        [DataMember(Name = "large")]
        public Uri Large { get; set; }

        [DataMember(Name = "template")]
        public string Template { get; set; }

        public Uri GetUri(int width, int height)
        {
            var uri = Template
                .Replace("{width}", width.ToString())
                .Replace("{height}", height.ToString());
            return new Uri(uri);
        }

        public override string ToString() => Template;
    }
}