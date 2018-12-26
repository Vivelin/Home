using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vivelin.Home.Twitch
{
    [DataContract]
    public class TwitchStream
    {
        [DataMember(Name = "_id")]
        public ulong Id { get; set; }

        [DataMember(Name = "game")]
        public string Game { get; set; }

        [DataMember(Name = "broadcast_platform")]
        public string BroadcastPlatform { get; set; }

        [DataMember(Name = "community_id")]
        public string CommunityId { get; set; }

        [DataMember(Name = "community_ids")]
        public ICollection<string> CommunityIds { get; set; }

        [DataMember(Name = "viewers")]
        public int Viewers { get; set; }

        [DataMember(Name = "video_height")]
        public int VideoHeight { get; set; }

        [DataMember(Name = "average_fps")]
        public double AverageFps { get; set; }

        [DataMember(Name = "delay")]
        public int Delay { get; set; }

        [DataMember(Name = "created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [DataMember(Name = "is_playlist")]
        public bool IsPlaylist { get; set; }

        [DataMember(Name = "stream_type")]
        public StreamType StreamType { get; set; }

        [DataMember(Name = "preview")]
        public TwitchPreview Preview { get; set; }

        [DataMember(Name = "channel")]
        public TwitchChannel Channel { get; set; }

        public override string ToString()
        {
            if (Channel != null)
                return $"{Channel.DisplayName ?? Channel.Name} - {Game} - {Viewers} viewer(s)";

            return $"{Game} - {Viewers} viewer(s)";
        }
    }
}
