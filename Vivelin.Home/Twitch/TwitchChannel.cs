using System;
using System.Runtime.Serialization;

namespace Vivelin.Home.Twitch
{
    [DataContract]
    public class TwitchChannel
    {
        [DataMember(Name = "_id")]
        public ulong Id { get; set; }

        [DataMember(Name = "mature")]
        public bool Mature { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "broadcaster_language")]
        public string BroadcasterLanguage { get; set; }

        [DataMember(Name = "broadcaster_software")]
        public string BroadcasterSoftware { get; set; }

        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }

        [DataMember(Name = "game")]
        public string Game { get; set; }

        [DataMember(Name = "language")]
        public string Language { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [DataMember(Name = "partner")]
        public bool IsPartner { get; set; }

        [DataMember(Name = "logo")]
        public Uri Logo { get; set; }

        [DataMember(Name = "video_banner")]
        public Uri VideoBanner { get; set; }

        [DataMember(Name = "profile_banner")]
        public Uri ProfileBanner { get; set; }

        [DataMember(Name = "profile_banner_background_color")]
        public string ProfileBannerBackgroundColor { get; set; }

        [DataMember(Name = "url")]
        public Uri Url { get; set; }

        [DataMember(Name = "views")]
        public long Views { get; set; }

        [DataMember(Name = "followers")]
        public long Followers { get; set; }

        [DataMember(Name = "broadcaster_type")]
        public string BroadcasterType { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "private_video")]
        public bool IsPrivateVideo { get; set; }

        [DataMember(Name = "privacy_options_enabled")]
        public bool PrivateOptionsEnabled { get; set; }

        public override string ToString() => Status;
    }
}