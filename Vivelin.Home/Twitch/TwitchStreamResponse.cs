using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vivelin.Home.Twitch
{
    [DataContract]
    public class TwitchStreamResponse
    {
        [DataMember(Name = "streams")]
        public ICollection<TwitchStream> Streams { get; set; }

        [DataMember(Name = "stream")]
        public TwitchStream Stream { get; set; }

        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "message")]
        public string ErrorMessage { get; set; }

        public void ThrowOnError()
        {
            if (Error != null)
                throw new TwitchException(ErrorMessage, Error, Status);
        }
    }
}